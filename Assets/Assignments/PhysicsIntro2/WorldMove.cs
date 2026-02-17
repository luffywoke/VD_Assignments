using UnityEngine;

public class WorldMove : MonoBehaviour
{
    float speed = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            
            transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        }

       
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
        }

        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            
            transform.Rotate(Vector3.left * speed * Time.deltaTime);
        }

        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
