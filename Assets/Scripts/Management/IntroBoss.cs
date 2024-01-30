using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroBoss : MonoBehaviour
{
    //public GameObject mainCamera;
    //public Transform maps;
    // Start is called before the first frame update
    public void enterLocal()
    {
        SceneManager.LoadScene("LocalLobby");
    }
    public void enterOnline()
    {
        SceneManager.LoadScene("Loading");
    }
}
