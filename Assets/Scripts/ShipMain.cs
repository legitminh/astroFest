using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMain : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;
    public KeyCode middleKeyCode;
    public GameObject projectile;
    public float bulletSpeed;
    public string weapon; //normal attack
    public int abilityTimes = 0;
    public float whiteTimeMax;


    float whiteTime = 0;

    float clickLeft = 1;
    float clickRight = 1;
    public float cooldown = 0.9f; // mana current
    public float maxManaTime = 1; // how long does it take to load to full
    float sideDashTimeMax = 0.1f;
    float sideDashTimeCount = 0f;
    string dashMode = "none";
    public float maxHp;
    public float hp;
    public Rigidbody2D rb;
    public GameObject gfx;
    Color standardColor;//color without effect | true color
    public GameObject trail;
    //Abilities
    public GameObject lazer;
    public GameObject bomb;
    public GameObject shield;
    public GameObject missle;
    GameObject mainCamera;
    // Start is called before the first frame update
    IDictionary<string, System.Action> weapons;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        print("hello");
        standardColor = gfx.GetComponent<SpriteRenderer>().color;
        rb.AddForce(transform.up * speed /2);
        hp = maxHp;
        weapons = new Dictionary<string, System.Action>{ //set up all weapon and corr func dictionary
            {"normal", Normal},
            {"triGun", TriGun },
            {"lazer", Lazer },
            {"bomb", Bomb },
            {"scatter", Scatter },
            {"missle", Missle }
        };
    }

    // Update is called once per frame
    void Update()
    {

        standardColor = new Color(standardColor.r, standardColor.g, standardColor.b, hp / maxHp * 2);
        gfx = transform.Find("GFX").gameObject;
        if (whiteTime > 0)
        {
            whiteTime -= Time.deltaTime;

            gfx.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            gfx.GetComponent<SpriteRenderer>().color = standardColor;
        }
        
        if (abilityTimes < 1)
        {
            weapon = "normal";
        }
        if (cooldown < 3)
        {
            cooldown += Time.deltaTime;
        }
        trail.GetComponent<TrailRenderer>().time = cooldown / 3;
        //GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up * Time.deltaTime * speed);
        transform.Find("Hp").GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, hp / maxHp, 0f));
        rb.AddForce(transform.up * speed * Time.deltaTime);
        KeyInput();
        

        Shoot();
    }
    void KeyInput()
    {
        clickLeft += Time.deltaTime;
        clickRight += Time.deltaTime;
        if (Input.GetKeyDown(leftKeyCode)) //check downclick for double click
        {

            if (clickLeft < 0.2) //if double click (recent left key)
            {
                if (cooldown > 1f)
                {
                    sideDashTimeCount = sideDashTimeMax;
                    dashMode = "dashLeft"; //double clicked
                }
            }
            else
            {
                clickLeft = 0;
            }
        }
        else if (Input.GetKeyDown(rightKeyCode))
        {
            if (clickRight < 0.2)
            {
                if (cooldown > 1f)
                {
                    sideDashTimeCount = sideDashTimeMax;
                    dashMode = "dashRight";
                }
            }
            else
            {
                clickRight = 0;
            }
        }
        else if (Input.GetKey(leftKeyCode))
        {
            //transform.RotateAround(transform.position + new Vector3(0f, 5f, 0f), Vector3.forward,rotationSpeed * Time.deltaTime); //also works
            transform.RotateAround(transform.Find("Axis").position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(rightKeyCode))
        {
            transform.RotateAround(transform.Find("Axis").position, Vector3.forward, -rotationSpeed * Time.deltaTime);

        }
        if (sideDashTimeCount > 0 && dashMode != "none") // if dashing
        {
            if (dashMode == "dashRight")
            {
                transform.Rotate(new Vector3(0f, 0f, -90 / sideDashTimeMax * Time.deltaTime));
            }
            else
            {
                transform.Rotate(new Vector3(0f, 0f, 90 / sideDashTimeMax * Time.deltaTime));
            }
            sideDashTimeCount -= Time.deltaTime;
            if (sideDashTimeCount <= 0)
            {
                dashMode = "finished";
            }

        }
        else if (dashMode == "finished")
        {
            rb.AddForce(transform.up * speed);
            cooldown -= 1f;
            dashMode = "none";
        }
    }
    void Shoot()
    {
        if (Input.GetKeyDown(middleKeyCode))
        {
            if (cooldown >= 0)
            {
                weapons[weapon]();
            }
        }
    }
    void Normal()
    {
        GameObject projectileCurrent = Instantiate(projectile, transform.position, transform.rotation, GameObject.Find("Bullets").transform);
        projectileCurrent.name = name;
        projectileCurrent.GetComponent<Bullet>().speed = bulletSpeed;
        projectileCurrent.GetComponent<SpriteRenderer>().color = standardColor;
        rb.AddForce(transform.up * -speed / 2);
        cooldown -= 1f;
    }
    void TriGun()
    {
        for (int num = -1; num <= 1; num++)
        {
            GameObject projectileCurrent = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + num * 10), GameObject.Find("Bullets").transform);
            projectileCurrent.name = name;
            projectileCurrent.GetComponent<Bullet>().speed = bulletSpeed;
            projectileCurrent.GetComponent<SpriteRenderer>().color = standardColor;

        }
        rb.AddForce(transform.up * -speed / 2);
        abilityTimes -= 1;

    }
    void Scatter()
    {
        for (int num = 0; num < 8; num++)
        {
            GameObject projectileCurrent = Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + num * 45), GameObject.Find("Bullets").transform);
            projectileCurrent.name = name;
            projectileCurrent.GetComponent<Bullet>().speed = bulletSpeed;
            projectileCurrent.GetComponent<SpriteRenderer>().color = standardColor;

        }
        abilityTimes -= 1;

    }
    void Missle()
    {
        GameObject missleCurrent = Instantiate(missle, transform.position, transform.rotation, GameObject.Find("Missles").transform);
        missleCurrent.transform.Find("GFX").GetComponent<SpriteRenderer>().color = standardColor;
        missleCurrent.name = name;
        abilityTimes -= 1;
    }
    void Lazer()
    {
        mainCamera.GetComponent<CameraMain>().shakeDuration = 0.1f;
        GameObject lazerCurrent = Instantiate(lazer, transform.position, transform.rotation, transform);
        lazerCurrent.name = name;
        lazerCurrent.GetComponent<LineRenderer>().startColor = standardColor;
        lazerCurrent.GetComponent<LineRenderer>().endColor = standardColor;
        rb.AddForce(transform.up * -speed / 2);
        abilityTimes -= 1;
    }
    void Bomb()
    {
        GameObject bombCurrent = Instantiate(bomb, transform.position, transform.rotation, GameObject.Find("Bombs").transform);
        bombCurrent.name = name;
        bombCurrent.transform.Find("Timer").gameObject.GetComponent<SpriteRenderer>().color = standardColor;
        bombCurrent.transform.Find("Timer").gameObject.GetComponent<LineRenderer>().startColor = standardColor;
        bombCurrent.transform.Find("Timer").gameObject.GetComponent<LineRenderer>().endColor = standardColor;
        abilityTimes -= 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name != name) // for collecting weapons
        {
            //if (int.TryParse(collision.gameObject.name, out int n))
            //{
            //    GotHit();[=-
            //}
            if (collision.gameObject.tag == "powerUp")
            {
                weapon = collision.gameObject.name;
            }
        }

    }
    public void GotHit() //effect of losing 1 hp count
    {
        if (whiteTime <= 0) // if available for damage
        {
            hp -= 1;
            if (hp == 0)// dies
            {
                Destroy(transform.gameObject);
            }

        }

        whiteTime = whiteTimeMax;




    }
    public void Bulleted()
    {
        GotHit();

    }
    public void Immortaled(float tim)
    {
        whiteTime = tim;
    }
    public void Bombed()
    {
        GotHit();
    }

}
