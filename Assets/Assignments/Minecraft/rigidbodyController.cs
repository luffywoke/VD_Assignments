using UnityEngine;
using UnityEngine.InputSystem;

public class rigidbodyController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    private Vector2 move, look;
    private float lookRotation;
    private Camera MainCamera;

    
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    

    private void FixedUpdate()
    {
        Vector3 currVelocity = rb.linearVelocity;
        Vector3 targVelocity = new Vector3(move.x, 0, move.y);
        targVelocity *= speed;

        //Align direction
        targVelocity = transform.TransformDirection(targVelocity);

        // Calculate forces
        Vector3 velocityChange = (targVelocity - currVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        // Limit force applied to player
        Vector3.ClampMagnitude(velocityChange, speed);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MainCamera = Camera.main;
        
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Turn player based on mouse movement
        transform.Rotate(Vector3.up * look.x * Time.deltaTime * 50);

        lookRotation += (-look.y * Time.deltaTime * 50);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        MainCamera.transform.eulerAngles = new Vector3(lookRotation, MainCamera.transform.eulerAngles.y, MainCamera.transform.eulerAngles.z);
    }
}
