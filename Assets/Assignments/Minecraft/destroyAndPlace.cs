using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class destroyAndPlace : MonoBehaviour
{
    public Material blockMaterial;
    public float gridCellSize = 1f;

    public void OnBreak(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (performRayCast(out RaycastHit hit))
            {
                breakBlock(hit.transform.gameObject);
            }
        }
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (performRayCast(out RaycastHit hit))
            {
                placeBlock(hit);
            }
        }
    }

    bool performRayCast(out RaycastHit hit)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        return Physics.Raycast(ray, out hit, 100f);
    }

    void breakBlock(GameObject target)
    {
        Destroy(target);
    }

    void placeBlock(RaycastHit hit)
    {
        Vector3 spawnPos = hit.transform.position + hit.normal * gridCellSize;

        spawnPos.x = Mathf.Round(spawnPos.x / gridCellSize) * gridCellSize;
        spawnPos.y = Mathf.Round(spawnPos.y / gridCellSize) * gridCellSize;
        spawnPos.z = Mathf.Round(spawnPos.z / gridCellSize) * gridCellSize;

        GameObject cube = new GameObject("Block");
        cube.transform.position = spawnPos;

        cube.AddComponent<BoxCollider>();

        MeshFilter mf = cube.AddComponent<MeshFilter>();
        mf.mesh = MeshGenerate.CreateCubeMesh();

        MeshRenderer mr = cube.AddComponent<MeshRenderer>();
        mr.material = blockMaterial;
    }
}
