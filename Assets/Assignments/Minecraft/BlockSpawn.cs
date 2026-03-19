using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    public Material blockMaterial;
    private int gridWidth = 5;
    private int gridHeight = 5;
    private float gridCellSize = 1f;

    void Start()
    {
        blockSpawn();
    }

    void blockSpawn()
    {
        Mesh dirtMesh = MeshGenerate.CreateDirtMesh(); // *
        Mesh grassMesh = MeshGenerate.CreateGrassMesh(); // *

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 spawnPos = new Vector3(x * gridCellSize, 0, y * gridCellSize);

                Mesh meshToUse = (x + y) % 2 == 0 ? dirtMesh : grassMesh;

                GameObject cube = new GameObject("Block_" + x + "_" + y);
                cube.transform.position = spawnPos;

                cube.AddComponent<BoxCollider>();

                MeshFilter mf = cube.AddComponent<MeshFilter>();
                mf.mesh = meshToUse;

                MeshRenderer mr = cube.AddComponent<MeshRenderer>();
                mr.material = blockMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
