using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class destroyAndPlace : MonoBehaviour
{
    public Material blockMaterial;
    public float gridCellSize = 1f;

    public enum BlockType { Dirt, Grass } // *
    public BlockType selectedBlock = BlockType.Dirt; // *

    private Mesh dirtMesh; // *
    

    void Start() // *
    { // *
        dirtMesh = MeshGenerate.CreateDirtMesh(); // *
    } // *

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
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
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
        mf.mesh = dirtMesh;

        MeshRenderer mr = cube.AddComponent<MeshRenderer>();
        mr.material = blockMaterial;

        Vector3Int gridKey = Worldregistry.ToGrid(spawnPos);
        Worldregistry.Instance.RegisterBlock(gridKey, cube);
    }
}
