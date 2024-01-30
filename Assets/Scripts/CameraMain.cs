using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    // Start is called before the first frame update
    public float shakeDuration;
    public float shakeDurationMax;
    public float shakeMagnitude;
    public Transform players;
    float minZoom = 5f;
    Vector3 velocity;
    void Start()
    {
        //GetComponent<Camera>().orthographicSize = screenUnitSize * 8;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Camera>().rect = new Rect(0f, 0f, (maxScreen.y - maxScreen.y) * screenUnitSize * 2, (maxScreen.y - maxScreen.y) * screenUnitSize);

    }
    private void LateUpdate()
    {
        Move();
        Zoom();
    }
    private void Move()
    {
        if (shakeDuration > 0) //if shaking
        {
            Vector2 circle = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition += new Vector3(circle.x, circle.y, 0f);
            shakeDuration -= Time.deltaTime;
        }
        else
        {

            Vector3 newPosition = new Vector3(GetCenterPoint().x, GetCenterPoint().y, -10f);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.2f);
        }
    }
    private void Zoom()
    {
         GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, GetGreatestDist()/2 + 4f, Time.deltaTime);
        
    }
    Vector3 GetCenterPoint()
    {
        Bounds bound = new Bounds(players.GetChild(0).position, Vector3.zero);
        for (int player = 0; player < players.childCount; player++)
        {
            bound.Encapsulate(players.GetChild(player).position);
        }
        return bound.center;
    }
    float GetGreatestDist()
    {
        Bounds bound = new Bounds(players.GetChild(0).position, Vector3.zero);
        for (int player = 0; player < players.childCount; player++)
        {
            bound.Encapsulate(players.GetChild(player).position);
        }
        if (bound.size.x > bound.size.y)
        {
            return bound.size.x;
        }
        return bound.size.y;
    }
}
