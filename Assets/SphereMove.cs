using UnityEngine;
using System;

public class SphereMove : MonoBehaviour
{
    public float maxX;
    public float minX;
    public float speed;
    public int waitTime;

    //If direction is negative, it will go opposite direction
    public float direction = 1f;

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        //Once position reaches Max X, go left but wait first
        if (transform.position.x > maxX)
        {
            System.Threading.Thread.Sleep(waitTime);
            direction = -1f;
        }
        //Once position reaches Min X, go right but wait first
        else if (transform.position.x < minX)
        {
            System.Threading.Thread.Sleep(waitTime);
            direction = 1f;
        }

    }
}
