using UnityEngine;

public class FallingSpheres : MonoBehaviour
{
    private GameObject[] allSpheres;
    float speed;
    void Start()
    {
        speed = Random.Range(3.5f, 4.5f);
        allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        foreach (var spheres in allSpheres)
        {
            transform.position = new Vector3(Random.Range(-35f,35f), 35f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var spheres in allSpheres)
        {
            transform.position += Vector3.down * speed * Time.deltaTime; 
        }
    }
}
