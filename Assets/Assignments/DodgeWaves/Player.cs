using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 5;
    private GameObject player;
    float MaxX = 4.5F;
    float MinX = -4.5F;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
       
    }

    void Update()
    { 
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        float clamped = Mathf.Clamp(transform.position.x, MinX, MaxX);
        transform.position = new Vector3(clamped, transform.position.y, transform.position.z);
    }
}
