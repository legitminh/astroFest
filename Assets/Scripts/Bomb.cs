using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius;
    public ContactFilter2D mask;
    float countdownMax = 0.5f;
    float countdown; //decreases from countdownMax
    bool triggered = false;
    GameObject timer;
    // Start is called before the first frame update
    void Start()
    {

        countdown = countdownMax;
        timer = transform.Find("Timer").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = new Collider2D[10];
        Physics2D.OverlapCircle(transform.position, radius, mask, hits);
        foreach (Collider2D hit in hits)
        {
            if (hit != null)
            {
                if ((hit.transform.parent.name == "Players") && (hit.gameObject.name != name))
                {
                    triggered = true;

                }
            }
        }
        if (triggered)
        {
            countdown -= Time.deltaTime;
            if (countdown < 0)
            {
                BlowUp();

            }
        }
        timer.GetComponent<LineRenderer>().SetPosition(1, new Vector3(Mathf.Cos(countdown / countdownMax * 2 * Mathf.PI) * radius, Mathf.Sin(countdown / countdownMax * 2 * Mathf.PI) * radius, 0f));
        
    }
    void BlowUp()
    {
        Collider2D[] hits = new Collider2D[10];
        Physics2D.OverlapCircle(transform.position, radius, mask, hits);
        foreach (Collider2D hit in hits)
        {
            if (hit != null)
            {
                if (hit.transform.parent.name == "Players")
                {
                    hit.gameObject.transform.GetComponent<ShipMain>().Bombed();
                }
                else if (hit.transform.parent.name == "Bombs")
                {
                    print("b2");
                    hit.gameObject.transform.GetComponent<Bomb>().Bombed();
                }
                else if (hit.transform.parent.name == "Bullets")
                {
                }
                //else
                //{
                //    print(hit.gameObject.name);
                //    hit.gameObject.transform.GetComponent<ObjMain>().Bombed();
                //}
            }
        }
        Destroy(gameObject);

    }
    public void Bulleted()
    {
        triggered = true;
    }
    public void Bombed()
    {

        triggered = true;
        countdown = 0.1f;
    }
}
