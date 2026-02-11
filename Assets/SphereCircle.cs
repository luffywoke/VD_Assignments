using UnityEngine;

public class SphereCircle : MonoBehaviour
{
    
    public float radius;
    

    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Moves the sphere object following a circular path
    void Update()
    {
        transform.position = new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time), 0) * radius;
    }
}
