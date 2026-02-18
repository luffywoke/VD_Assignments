using UnityEngine;

public class Finish : MonoBehaviour
{
    private GameObject[] finishLine;
    private GameObject[] spheres;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishLine = GameObject.FindGameObjectsWithTag("Finish");
        spheres = GameObject.FindGameObjectsWithTag("Sphere");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
