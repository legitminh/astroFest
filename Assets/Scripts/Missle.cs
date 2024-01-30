using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public GameObject gfx;
    public float speed;
    public float rotateSpeed;
    public float duration;

    GameObject players;
    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(Vector3.right * 100f);
        players = GameObject.Find("Players").gameObject;
    }

    // Update is called once per frame
    void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            //Destroy(gameObject);
        }
        //rb.MovePosition(transform.position + Vector3.Normalize(target.position - transform.position));
        Locate();
        Vector3 delta = Vector3.Normalize(target.position - transform.position);
        //transform.rotation = Quaternion.Euler( delta);
        float rotationAmount = Vector3.Cross(delta, transform.up).z;
        rb.angularVelocity = -rotationAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
        //transform.eulerAngles = new Vector3(0f, 0f, rotation_amount);
        //rb.MovePosition(transform.position + delta * Time.deltaTime * speed);
        //rb.AddForce( delta * Time.deltaTime * speed);
        //gfx.transform.LookAt(target, transform.up);
    }
    void Locate()
    {
        GameObject closestPlayer = null;
        GameObject currentPlayer = null;
        float minDist = Mathf.Infinity;
        float currentDist = 0f;
        for (int num = 0; num < players.transform.childCount; num++)
        {
            currentPlayer = players.transform.GetChild(num).gameObject;
            currentDist = Vector3.Distance(currentPlayer.transform.position, transform.position);
            if ( currentPlayer.name != transform.name && currentDist < minDist) // if another player is clostest
            {
                
                closestPlayer = currentPlayer;
                minDist = currentDist;
            }
        }
        target = closestPlayer.transform;
    }
    public void Bulleted()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == name && collision.transform.parent.name == "Players")//if is originating player
        {

        }
        else
        {
            if (collision.transform.parent.name == "Players")
            {
                collision.gameObject.transform.GetComponent<ShipMain>().Bulleted();
                Destroy(gameObject);
            }
            if (collision.transform.name == "Tilemap")
            {
                Destroy(gameObject);
            }
            else
            {
                
            }
            

        }

    }
}
