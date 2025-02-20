using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Unity.Services.Multiplay.Authoring.Editor")]
[assembly: InternalsVisibleTo("Unity.Services.Multiplay.Authoring")]
[assembly: InternalsVisibleTo("Unity.Services.Cli.Multiplay")]
[assembly: InternalsVisibleTo("Unity.Services.Cli.Multiplay.UnitTest")]
[assembly: InternalsVisibleTo("Unity.Services.Cli.GameServerHosting")]
[assembly: InternalsVisibleTo("Unity.Services.Cli.GameServerHosting.UnitTest")]
[assembly: InternalsVisibleTo("InternalsVisible.DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Unity.DedicatedServer.DeploymentOrchestrator.Editor.MultiplayNode")]

#if UNITY_INCLUDE_TESTS
[assembly: InternalsVisibleTo("Unity.Services.Multiplay.Authoring.Tests.Editor")]
[assembly: InternalsVisibleTo("Unity.Services.Multiplay.Authoring.IntegrationTests.EditMode")]
[assembly: InternalsVisibleTo("Unity.Services.Multiplay.Authoring.Tests")]
#endif

// Needed to enable record types
namespace System.Runtime.CompilerServices
{
    static class IsExternalInit {}
}
