#if UNITY_EDITOR || BURST_INTERNAL
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Burst.Editor
{
    internal partial class BurstDisassembler
    {
        /// <summary>
        /// Base class for providing extended information of an identifier
        /// </summary>
        private abstract class AsmTokenKindProvider
        {
            // Internally using string slice instead of string
            // to support faster lookup from AsmToken
            private readonly Dictionary<StringSlice, AsmTokenKind> _tokenKinds;
            private int _maximumLength;

            protected AsmTokenKindProvider(int capacity)
            {
                _tokenKinds = new Dictionary<StringSlice, AsmTokenKind>(capacity);
            }

            protected void AddTokenKind(string text, AsmTokenKind kind)
            {
                _tokenKinds.Add(new StringSlice(text), kind);
                if (text.Length > _maximumLength) _maximumLength = text.Length;
            }

            public AsmTokenKind FindTokenKind(StringSlice slice)
            {
                return slice.Length <= _maximumLength && _tokenKinds.TryGetValue(slice, out var tokenKind) ? tokenKind : AsmTokenKind.Identifier;
            }

            public virtual bool AcceptsCharAsIdentifierOrRegisterEnd(char c)
            {
                return false;
            }
        }

        /// <summary>
        /// The ASM tokenizer
        /// </summary>
        private struct AsmTokenizer
        {
            private readonly string _text;
            private readonly AsmKind _asmKind;
            private readonly AsmTokenKindProvider _tokenKindProvider;
            private int _position;
            private int _nextPosition;
            private char _c;
            private readonly char _commentStartChar;

            public AsmTokenizer(string text, AsmKind asmKind, AsmTokenKindProvider tokenKindProvider)
            {
                _text = text;
                _asmKind = asmKind;
                _tokenKindProvider = tokenKindProvider;
                _position = 0;
                _nextPosition = 0;
                _commentStartChar = (asmKind == AsmKind.Intel || asmKind == AsmKind.Wasm) ? '#' : ';';
                _c = (char)0;
                NextChar();
            }

            public bool TryGetNextToken(out AsmToken token)
            {
                token = new AsmToken();
                while (true)
                {
                    var startPosition = _position;

                    if (_c == 0)
                    {
                        return false;
                    }

                    if (_c == '.')
                    {
                        token = ParseDirective(startPosition);
                        return true;
                    }

                    // Like everywhere else in this file, we are inlining the matching characters instead
                    // of using helper functions, as Mono might not be enough good at inlining by itself
                    if (_c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z' || _c == '_' || _c == '@')
                    {
                        token = ParseInstructionOrIdentifierOrRegister(startPosition);
                        return true;
                    }

                    if (_c >= '0' && _c <= '9' || _c == '-')
                    {
                        token = ParseNumber(startPosition);
                        return true;
                    }

                    if (_c == '"')
                    {
                        token = ParseString(startPosition);
                        return true;
                    }

                    if (_c == _commentStartChar)
                    {
                        token = ParseComment(startPosition);
                        return true;
                    }

                    if (_c == '\r')
                    {
                        if (PreviewChar() == '\n')
                        {
                            NextChar(); // skip \r
                        }
                        token = ParseNewLine(startPosition);
                        return true;
                    }

                    if (_c == '\n')
                    {
                        token = ParseNewLine(startPosition);
                        return true;
                    }

                    token = ParseMisc(startPosition);
                    return true;
                }
            }

            private AsmToken ParseNewLine(int startPosition)
            {
                var endPosition = _position;
                NextChar(); // Skip newline
                return new AsmToken(AsmTokenKind.NewLine, startPosition, endPosition - startPosition + 1);
            }

            private AsmToken ParseMisc(int startPosition)
            {
                var endPosition = _position;
                // Parse anything that is not a directive, instruction, number, string or comment
                while (!((_c == (char)0) || (_c == '\r') || (_c == '\n') || (_c == '.') || (_c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z' || _c == '_' || _c == '@') || (_c >= '0' && _c <= '9' || _c == '-') || (_c == '"') || (_c == _commentStartChar)))
                {
                    endPosition = _position;
                    NextChar();
                }
                return new AsmToken(AsmTokenKind.Misc, startPosition, endPosition - startPosition + 1);
            }

            private static readonly string[] DataDirectiveStrings = new[]
            {
                AssertDataDirectiveLength(".long"),
                AssertDataDirectiveLength(".byte"),
                AssertDataDirectiveLength(".short"),
                AssertDataDirectiveLength(".ascii"),
                AssertDataDirectiveLength(".asciz"),
            };

            private static string AssertDataDirectiveLength(string text)
            {
                var length = text.Length;
                Debug.Assert(length == 5 || length == 6, $"Invalid length {length} for string {text}. Expecting 5 or 6");
                return text;
            }

            private AsmToken ParseDirective(int startPosition)
            {
                var endPosition = _position;
                NextChar(); // skip .
                bool isLabel = _c == 'L'; // A label starts with a capital `L` like .Lthis_is_a_jump_label
                while (_c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z' || _c >= '0' && _c <= '9' || _c == '.' || _c == '_' || _c == '@')
                {
                    endPosition = _position;
                    NextChar();
                }

                // Refine the kind of directive:
                //
                // .Lfunc_begin => FunctionBegin
                // .Lfunc_end => FunctionEnd
                // .L????????? => Label
                // data directive (.byte, .long, .short...) => DataDirective
                // anything else => Directive
                const string MatchFunc = ".Lfunc_";
                const int MatchFuncLength = 7;
                Debug.Assert(MatchFunc.Length == MatchFuncLength);
                var kind = isLabel ? AsmTokenKind.Label : AsmTokenKind.Directive;
                // Fast early check 
                if (isLabel && string.CompareOrdinal(_text, startPosition, MatchFunc, 0, MatchFuncLength) == 0)
                {
                    if (string.CompareOrdinal(_text, startPosition, ".Lfunc_begin", 0, ".Lfunc_begin".Length) == 0)
                    {
                        kind = AsmTokenKind.FunctionBegin;
                    }
                    else if (string.CompareOrdinal(_text, startPosition, ".Lfunc_end", 0, ".Lfunc_end".Length) == 0)
                    {
                        kind = AsmTokenKind.FunctionEnd;
                    }
                }

                // Adjust directive to mark data directives, source location directives...etc.
                int length = endPosition - startPosition + 1;

                // Use length to early exit
                if (!isLabel && length >= 4 && length <= 8)
                {
                    if ((length == 5 || length == 6))
                    {
                        foreach (var dataDirectiveStr in DataDirectiveStrings)
                        {
                            if (string.CompareOrdinal(_text, startPosition, dataDirectiveStr, 0, dataDirectiveStr.Length) == 0)
                            {
                                kind = AsmTokenKind.DataDirective;
                                break;
                            }
                        }

                        // .file => SourceFile
                        if (kind == AsmTokenKind.Directive && string.CompareOrdinal(_text, startPosition, ".file", 0, 5) == 0)
                        {
                            kind = AsmTokenKind.SourceFile;
                        }
                    }
                    // .loc => SourceLocation
                    // .cv_loc => SourceLocation
                    else if ((length == 4 && string.CompareOrdinal(_text, startPosition, ".loc", 0, 4) == 0) ||
                             (length == 7 && string.CompareOrdinal(_text, startPosition, ".cv_loc", 0, 7) == 0))
                    {
                        kind = AsmTokenKind.SourceLocation;
                    }
                    // .file .cv_file => SourceFile
                    else if (length == 8 && string.CompareOrdinal(_text, startPosition, ".cv_file", 0, 8) == 0)
                    {
                        kind = AsmTokenKind.SourceFile;
                    }
                }

                return new AsmToken(kind, startPosition, length);
            }

            private AsmToken ParseInstructionOrIdentifierOrRegister(int startPosition)
            {
                var endPosition = _position;
                // we include . because we have instructions like `b.le` or `f32.const`
                while (_c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z' || _c >= '0' && _c <= '9' || _c == '_' || _c == '@' || _c == '.')
                {
                    endPosition = _position;
                    NextChar();
                }

                if (_tokenKindProvider.AcceptsCharAsIdentifierOrRegisterEnd(_c))
                {
                    endPosition = _position;
                    NextChar();
                }

                // Resolve token kind for identifier
                int length = endPosition - startPosition + 1;
                var tokenKind = _tokenKindProvider.FindTokenKind(new StringSlice(_text, startPosition, length));

                if (tokenKind == AsmTokenKind.Identifier)
                {
                    // If we have `:` right after an identifier, change from identifier to label declaration to help the semantic pass later
                    tokenKind = _c == ':' ? AsmTokenKind.Label : tokenKind;
                }

                return new AsmToken(tokenKind, startPosition, endPosition - startPosition + 1);
            }

            private AsmToken ParseNumber(int startPosition)
            {
                var endPosition = _position;
                if (_c == '-')
                {
                    NextChar();
                }
                while (_c >= '0' && _c <= '9' || _c >= 'a' && _c <= 'f' || _c >= 'A' && _c <= 'F' || _c == 'x' || _c == '.')
                {
                    endPosition = _position;
                    NextChar();
                }

                // If we have `:` right after a number, change from number to label declaration to help the semantic pass later
                var numberKind = _c == ':' ? AsmTokenKind.Label : AsmTokenKind.Number;
                return new AsmToken(numberKind, startPosition, endPosition - startPosition + 1);
            }
            private AsmToken ParseString(int startPosition)
            {
                var endPosition = _position;
                // Skip first "
                NextChar();
                while (_c != (char)0 && _c != '"')
                {
                    // Skip escape \"
                    if (_c == '\\' && PreviewChar() == '"')
                    {
                        NextChar();
                    }
                    endPosition = _position;
                    NextChar();
                }

                endPosition = _position;
                NextChar(); // Skip trailing 0

                // If we have `:` right after a string, change from string to label declaration to help the semantic pass later
                var stringKind = _c == ':' ? AsmTokenKind.Label : AsmTokenKind.String;
                return new AsmToken(stringKind, startPosition, endPosition - startPosition + 1);
            }

            private AsmToken ParseComment(int startPosition)
            {
                var endPosition = _position;
                while (_c != (char)0 && (_c != '\n' && _c != '\r'))
                {
                    endPosition = _position;
                    NextChar();
                }

                return new AsmToken(AsmTokenKind.Comment, startPosition, endPosition - startPosition + 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void NextChar()
            {
                if (_nextPosition < _text.Length)
                {
                    _position = _nextPosition;
                    _c = _text[_position];
                    _nextPosition = _position + 1;
                }
                else
                {
                    _c = (char)0;
                }
            }

            private char PreviewChar()
            {
                return _nextPosition < _text.Length ? _text[_nextPosition] : (char)0;
            }

        }

        /// <summary>
        /// A slice of a string from an original string.
        /// </summary>
        public readonly struct StringSlice : IEquatable<StringSlice>
        {
            private readonly string _text;

            public readonly int Position;

            public readonly int Length;

            public StringSlice(string text)
            {
                _text = text ?? throw new ArgumentNullException(nameof(text));
                Position = 0;
                Length = text.Length;
            }

            public StringSlice(string text, int position, int length)
            {
                _text = text ?? throw new ArgumentNullException(nameof(text));
                Position = position;
                Length = length;
            }

            public char this[int index] => _text[Position + index];

            public bool Equals(StringSlice other)
            {
                if (Length != other.Length) return false;

                for (int i = 0; i < Length; i++)
                {
                    if (this[i] != other[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj is StringSlice other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Length;
                    for (int i = 0; i < Length; i++)
                    {
                        hashCode = (hashCode * 397) ^ this[i];
                    }
                    return hashCode;
                }
            }

            public static bool operator ==(StringSlice left, StringSlice right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(StringSlice left, StringSlice right)
            {
                return !left.Equals(right);
            }

            public override string ToString()
            {
                return _text.Substring(Position, Length);
            }

            public bool StartsWith(string text)
            {
                if (text == null) throw new ArgumentNullException(nameof(text));
                if (Length < text.Length) return false;
                for (var i = 0; i < text.Length; i++)
                {
                    var c = text[i];
                    if (_text[Position + i] != c) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// An ASM token. The token doesn't contain the string of the token, but provides method <see cref="Slice"/> and <see cref="ToString"/> to extract it.
        /// </summary>
        internal readonly struct AsmToken
        {
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // CAUTION: It is important to not put *any managed objects*
            // into this struct for GC efficiency
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            public AsmToken(AsmTokenKind kind, int position, int length)
            {
                Kind = kind;
                Position = position;
                Length = length;
            }

            public readonly AsmTokenKind Kind;

            public readonly int Position;

            public readonly int Length;

            public StringSlice Slice(string text) => new StringSlice(text, Position, Length);

            public string ToString(string text) => text.Substring(Position, Length);

            public string ToFriendlyText(string text)
            {
                return $"{text.Substring(Position, Length)} : {Kind}";
            }
        }

        /// <summary>
        /// Kind of an ASM token.
        /// </summary>
        internal enum AsmTokenKind
        {
            Eof,
            Directive,
            DataDirective,
            SourceFile,
            SourceLocation,
            Label,
            FunctionBegin,
            FunctionEnd,
            Identifier,
            Qualifier,
            Instruction,
            CallInstruction,
            BranchInstruction,
            JumpInstruction,
            ReturnInstruction,
            InstructionSIMD,
            Register,
            Number,
            String,
            Comment,
            NewLine,
            Misc
        }
    }
}
#endif