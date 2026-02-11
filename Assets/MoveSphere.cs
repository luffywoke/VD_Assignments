using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    public float speed;
    float direction;


    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * direction * Time.deltaTime * speed;
    }
}
