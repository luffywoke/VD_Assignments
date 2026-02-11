using UnityEngine;

public class Collisions : MonoBehaviour
{
    private GameObject[] allSpheres;
    private GameObject[] allWalls;
    float direction = 1f;
    public float speed = 3;
    
    void Start()
    {
        // Finding all objects with tag Sphere and Wall
        allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        allWalls = GameObject.FindGameObjectsWithTag("Wall");
        
    }
    void Update()
    {
        foreach (var sphere in allSpheres)
        {
            transform.position += Vector3.right * direction * speed * Time.deltaTime;
        }

        // Looping through allSpheres and allWalls
        foreach (var sphere in allSpheres)
        {
            foreach (var wall in allWalls)
            {
                if (Vector3.Distance(sphere.transform.position, wall.transform.position) < 0.5)
                {
                    direction = -direction;

                    // allWalls[0].transform.position += new Vector3(0f, 0f, 0f) * -direction * speed * Time.deltaTime;
                    // allWalls[1].transform.position += new Vector3(0f, 0f, 0f) * -direction * speed * Time.deltaTime;
                }
                
            }
        }
    }
}
