using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour // just a shell that carries the name 
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.name == "Players")
        {
            if (name == "heal")
            {
                if (collision.gameObject.GetComponent<ShipMain>().hp < collision.gameObject.GetComponent<ShipMain>().maxHp)
                {
                    collision.gameObject.GetComponent<ShipMain>().hp += 1;
                }
                else
                {
                    collision.gameObject.GetComponent<ShipMain>().Immortaled(4f);
                }
                
            }
            else // if is normal weapon ability
            {
                collision.gameObject.GetComponent<ShipMain>().weapon = name;
                collision.gameObject.GetComponent<ShipMain>().abilityTimes = 3;
            }
            
            Destroy(gameObject);

            
        }
    }
}
