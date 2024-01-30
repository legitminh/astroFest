using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float duration;
    public float countdown;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        countdown = duration;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown > 0)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, 100f, mask);
            foreach (RaycastHit2D hit in hits)
            {
                print(hit.collider.transform.parent.name);
                if (hit.collider.gameObject.name == name && hit.collider.transform.parent.name == "Players")//if is originating player
                {
                    
                }
                else if (hit.collider.gameObject.name == name) //if from player
                {
                    if (hit.collider.transform.parent.name == "Bombs")
                    {
                        hit.collider.gameObject.transform.GetComponent<Bomb>().Bulleted();
                    }
                }
                else
                {
                    if (hit.collider.transform.parent.name == "Players")
                    {
                        hit.collider.gameObject.transform.GetComponent<ShipMain>().Bulleted();
                    }
                    else if (hit.collider.transform.parent.name == "Bombs")
                    {
                        hit.collider.gameObject.transform.GetComponent<Bomb>().Bulleted();
                    }
                    else if (hit.collider.transform.parent.name == "Missles")
                    {
                        hit.collider.gameObject.transform.GetComponent<Missle>().Bulleted();
                    }
                    else if (hit.collider.transform.parent.name == "Bullets")
                    {
                        print("zap");
                        hit.collider.gameObject.transform.GetComponent<Bullet>().Destroyed();
                    }
                }
            }

        }
        else
        {
            countdown = duration;
            Destroy(gameObject);
        }
        
    }
}
