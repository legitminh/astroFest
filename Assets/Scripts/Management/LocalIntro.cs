using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LocalIntro : MonoBehaviour
{
    public GameObject player; //default player object
    public GameObject players;
    //public GameObject mainCamera;
    //public Transform maps;
    public Transform playerControlTexts;
    public Transform chooseNumPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        PlayerGenerator();
    }

    // Update is called once per frame
    void Update()
    {



    }
    public void enterGame()
    {
        SceneManager.LoadScene(chooseNumPlayer.Find("Label").GetComponent<TMPro.TextMeshProUGUI>().text);
        Time.timeScale = 1;
    }

    public void PlayerGenerator() // Generate player based on current mode selection option
    {
        foreach (Transform child in players.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerControlTexts)
        {
            child.gameObject.SetActive(false);
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

        int numberPlayer = int.Parse(chooseNumPlayer.Find("Label").GetComponent<TMPro.TextMeshProUGUI>().text);
        //int numberPlayer = chooseNumPlayer.GetComponent<Dropdown>().value;
        for (int a = 0; a < numberPlayer; a++)
        {
            playerControlTexts.GetChild(a).transform.gameObject.SetActive(true);

            Vector3 newPos = new Vector3(locations[a].x, locations[a].y, 0f);
            GameObject newPlayer = Instantiate(player, newPos, Quaternion.Euler(0f, 0f, -90f), players.transform);
            newPlayer.transform.Rotate(new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(newPlayer.transform.position.y, newPlayer.transform.position.x)));
            newPlayer.name = a.ToString();
            newPlayer.transform.Find("GFX").GetComponent<SpriteRenderer>().color = colors[a];
            newPlayer.transform.Find("Trail").GetComponent<TrailRenderer>().endColor = colors[a];
            newPlayer.transform.Find("Trail").GetComponent<TrailRenderer>().startColor = colors[a];
            ShipMain controlScript = newPlayer.GetComponent<ShipMain>();
            //controlScript.leftKeyCode = keys[a][0];
            //controlScript.middleKeyCode = keys[a][1];
            //controlScript.rightKeyCode = keys[a][2];
            newPlayer = null;



        }

    }
}
