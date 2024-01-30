using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMain : MonoBehaviour
{
    public Vector2[] spawnArea = new Vector2[2];
    public string[] abilities = new string[1];
    public float spawnRate; //spawn per second
    public int maxSpawn;
    
    public float countdown = 0;
    public GameObject buff;
    Dictionary<string, Color> colors = new Dictionary<string, Color>(){
            { "bomb", Color.red },
            { "heal", Color.green},
            { "lazer", Color.blue },
            { "triGun", new Color(255f, 255f, 0f)},
            { "scatter", Color.cyan },
            { "missle", Color.magenta}
            
            //Color.white,
    };
    //first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.childCount < maxSpawn) {
            if (countdown > spawnRate) // if rate is available
            {
                float buffX = Random.Range(spawnArea[0].x, spawnArea[1].x); //first area, first point (two point to define rect)
                float buffY = Random.Range(spawnArea[0].y, spawnArea[1].y);

                string buffName = abilities[Random.Range(0, abilities.Length)];
                GameObject buffObj = Instantiate(buff, new Vector3(buffX, buffY, 0f), Quaternion.identity, transform);
                buffObj.name = buffName;
                buffObj.GetComponent<SpriteRenderer>().color = colors[buffName];
                countdown = 0;
            }
            else
            {
                countdown += Time.deltaTime;
            }
            
        }
        

    }
}
