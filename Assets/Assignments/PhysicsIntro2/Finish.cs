using UnityEngine;

public class Finish : MonoBehaviour
{
    private Rigidbody rb;
    public Transform respawnPoint;


   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("You win!");
            // Move the player back to the respawn point
            rb.isKinematic = true;
            transform.position = respawnPoint.position;
            rb.isKinematic = false;

        }
    }


}
