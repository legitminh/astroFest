using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public Vector2 velocity;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.up * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == name && collision.transform.parent.name == "Players")//if is originating player
        {

        }
        else if (collision.gameObject.name == name) //if from player
        {
            if (collision.transform.parent.name == "Bombs")
            {
                collision.gameObject.transform.GetComponent<Bomb>().Bulleted();
                Destroy(gameObject);
            }
            
        }
        else 
        {
            if (collision.transform.parent.name == "Players")
            {
                collision.gameObject.transform.GetComponent<ShipMain>().Bulleted();
                Destroy(gameObject);
            }
            else if(collision.transform.parent.name == "Bombs")
            {
                collision.gameObject.transform.GetComponent<Bomb>().Bulleted();
                Destroy(gameObject);
            }
            else if (collision.transform.parent.name == "Missles")
            {
                collision.gameObject.transform.GetComponent<Missle>().Bulleted();
                Destroy(gameObject);
            }
            else if (collision.transform.parent.name == "Grid")
            {
                Invoke("Destroyed", 2f);
                print("pass");
            }
            
            
        }
        
    }
    public void Destroyed()
    {
        Destroy(gameObject);
    }

}
