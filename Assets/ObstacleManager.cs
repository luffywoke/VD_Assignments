using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private GameObject[] obstacles;
    float MaxX = 4.5f;
    float MinX = -4.5f;
    float speed = 1f; 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles)
        {
            transform.position = new Vector3(Random.Range(MinX, MaxX), 13f, 0);
            transform.localScale = new Vector3(Random.Range(1, 3), Random.Range(1, 3), 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;


        foreach (var obstacle in obstacles)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            /*
            if (obstacle.transform.position.y < -3)
            {
                obstacle.transform.position = new Vector3(x, y, z);
            }
            */
            
        }
    }
}
