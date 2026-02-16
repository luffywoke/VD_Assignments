using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter(Collision collisionInfo)
    {
        // Log a message to the console
        Debug.Log("Collision detected with: " + collisionInfo.gameObject.name);
    }
}
