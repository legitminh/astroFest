using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameBoss : MonoBehaviour
{
    public GameObject player; //default player object
    public GameObject players;
    public GameObject mainCamera;
    public int numberPlayer;
    KeyCode[][] keys =
        {
            new KeyCode[]{
                KeyCode.Z,
                KeyCode.X,
                KeyCode.C,
            },
            new KeyCode[]
            {
                KeyCode.Minus,
                KeyCode.Equals,
                KeyCode.Backspace,
            },
            new KeyCode[]
            {
                KeyCode.BackQuote,
                KeyCode.Alpha1,
                KeyCode.Alpha2,
            },
            new KeyCode[]
            {
                KeyCode.LeftArrow,
                KeyCode.UpArrow,
                KeyCode.RightArrow,
            },
            new KeyCode[]
            {
                KeyCode.V,
                KeyCode.B,
                KeyCode.N,
            },
            new KeyCode[]
            {
                KeyCode.Semicolon,
                KeyCode.Quote,
                KeyCode.Return,
            },
            new KeyCode[]
            {
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
            },
        };
    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlayerGenerator", 0.1f);
        Invoke("StartCamera", 0.1f);
    }
    void StartCamera()
    {
        mainCamera.GetComponent<CameraMain>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            exitGame();
        }
        if (players.transform.childCount == 1)
        {
            print("Player" + players.transform.GetChild(0).name + "won");
            //exitGame();
        }
    }
    public void PlayerGenerator() // Generate player based on current mode selection option
    {
        foreach (Transform child in players.transform)
        {
            Destroy(child.gameObject);
        }

        Vector2[] locations = { new Vector2(-15, -15), new Vector2(15, 15), new Vector2(-15, 15), new Vector2(15, -15), new Vector2(0, 1), new Vector2(15, 0), new Vector2(-15, 0), }; //bottomLeft, topRight, topLeft, bottomRight, center, centerRight, centerLeft,

        Color[] colors =
        {
            Color.red,
            Color.green,
            Color.blue,
            new Color(255f,255f,0f),
            Color.cyan,
            Color.magenta,
            new Color(1f, 0.5f, 0f),
            Color.white,
        };
        //int numberPlayer = chooseNumPlayer.GetComponent<Dropdown>().value;
        for (int a = 0; a < numberPlayer; a++)
        {

            Vector3 newPos = new Vector3(locations[a].x, locations[a].y, 0f);
            GameObject newPlayer = Instantiate(player, newPos, Quaternion.Euler(0f, 0f, -90f), players.transform);
            newPlayer.transform.Rotate(new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(newPlayer.transform.position.y, newPlayer.transform.position.x)));
            newPlayer.name = a.ToString();
            newPlayer.transform.Find("GFX").GetComponent<SpriteRenderer>().color = colors[a];
            newPlayer.transform.Find("Trail").GetComponent<TrailRenderer>().endColor = colors[a];
            newPlayer.transform.Find("Trail").GetComponent<TrailRenderer>().startColor = colors[a];
            ShipMain controlScript = newPlayer.GetComponent<ShipMain>();
            controlScript.leftKeyCode = keys[a][0];
            controlScript.middleKeyCode = keys[a][1];
            controlScript.rightKeyCode = keys[a][2];
            newPlayer = null;



        }
    }
    void exitGame()
    {
        
        Time.timeScale = 0;
        SceneManager.LoadScene("Intro");
    }
}
