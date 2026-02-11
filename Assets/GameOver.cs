using UnityEngine;

public class GameOver : MonoBehaviour
{
    private GameObject[] allSpheres;
    private GameObject cube;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        cube = GameObject.FindGameObjectWithTag("Cube");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var sphere in allSpheres)
        {
            if (Vector3.Distance(sphere.transform.position, cube.transform.position) < 0.5)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
