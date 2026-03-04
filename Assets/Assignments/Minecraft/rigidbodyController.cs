using UnityEngine;
using UnityEngine.InputSystem;

public class rigidbodyController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    private Vector2 move, look;
    private bool grounded;
    private Camera MainCamera;
    public float sensitivity = 0.35f;
    public float smoothSpeed = 10f;
    


    private float targetRotation; 
    private float currentRotation;


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void onJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void onSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Sprint();
        }
        else if (context.canceled)
        {
            speed = 5f;
        }
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

    void Sprint()
    {
        speed = speed * 1.5f;
    }

    

    void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if (grounded)
        {
            jumpForces.y = 2f;
            
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }
    
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MainCamera = Camera.main;
        
        //Hiding cursor and locking to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void LateUpdate()
    {
        //Turn player based on mouse movement
        transform.Rotate(Vector3.up * look.x * sensitivity);

        targetRotation += (-look.y * sensitivity);
        targetRotation = Mathf.Clamp(targetRotation, -90, 90);

        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * smoothSpeed);
        MainCamera.transform.localEulerAngles = new Vector3(currentRotation, 0f, 0f);
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
