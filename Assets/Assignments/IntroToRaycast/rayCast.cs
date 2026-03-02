using UnityEngine;

public class rayCast : MonoBehaviour
{
    public LayerMask pickupLayer;
    private Camera cam;
    private SimplePickupSystem pickupSystem;
    private bool isHolding = false;
    private RaycastHit currentHit;

    void Start()
    {
        cam = Camera.main;
        pickupSystem = GetComponent<SimplePickupSystem>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (isHolding)
        {
            if (Physics.Raycast(ray, out RaycastHit groundHit))
            {
                Vector3 holdPosition = groundHit.point + Vector3.up * 1.5f;
                pickupSystem.UpdatePickupPosition(holdPosition);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                pickupSystem.Drop();
                isHolding = false;
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, pickupLayer))
            {
                currentHit = hit;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (IsInLayerMask(hit.collider.gameObject, pickupLayer))
                    {
                        pickupSystem.Pickup(hit.collider.gameObject);
                        isHolding = true;
                    }
                }
            }
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((1 << obj.layer) & mask) != 0;
    }
}