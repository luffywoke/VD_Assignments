using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    [Header("World Size")]
    public int gridWidth = 20;
    public int gridDepth = 20;

    [Header("Height Settings")]
    public int baseHeight = 4;
    public int maxHeightVariation = 6;

    [Header("Perlin Noise")]
    public int seed = 42;
    public float noiseScale = 0.3f;

    [Header("Materials")]
    public Material blockMaterial;

    [Header("Player")]
    public Transform player;

    private float gridCellSize = 1f;

    void Start()
    {
        blockSpawn();
        SpawnPlayerOnSurface();
    }

    void blockSpawn()
    {
        Mesh grassMesh = MeshGenerate.CreateGrassMesh();
        Mesh dirtMesh = MeshGenerate.CreateDirtMesh();
        Mesh stoneMesh = MeshGenerate.CreateStoneMesh();

        // Offset Perlin sampling based on seed so each seed gives a different world
        float offsetX = seed * 0.1f;
        float offsetZ = seed * 0.1f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridDepth; z++)
            {
                // Sample Perlin noise to get the surface height for this column
                float noiseValue = Mathf.PerlinNoise(
                    (x * noiseScale) + offsetX,
                    (z * noiseScale) + offsetZ
                );

                int surfaceHeight = baseHeight + Mathf.RoundToInt(noiseValue * maxHeightVariation);

                // Spawn a full column of blocks from y=0 up to the surface
                for (int y = 0; y <= surfaceHeight; y++)
                {
                    Vector3 spawnPos = new Vector3(x * gridCellSize, y * gridCellSize, z * gridCellSize);

                    Mesh meshToUse;

                    if (y == surfaceHeight)
                        meshToUse = grassMesh;         // Top block = grass
                    else if (y >= surfaceHeight - 3)
                        meshToUse = dirtMesh;          // 3 layers of dirt below grass
                    else
                        meshToUse = stoneMesh;         // Everything below = stone

                    GameObject cube = new GameObject("Block_" + x + "_" + y + "_" + z);
                    cube.transform.position = spawnPos;
                    cube.AddComponent<BoxCollider>();

                    MeshFilter mf = cube.AddComponent<MeshFilter>();
                    mf.mesh = meshToUse;

                    MeshRenderer mr = cube.AddComponent<MeshRenderer>();
                    mr.material = blockMaterial;

                    Vector3Int gridKey = Worldregistry.ToGrid(spawnPos);
                    Worldregistry.Instance.RegisterBlock(gridKey, cube);
                }
            }
        }
    }

    // Moves the player above the surface so they never spawn inside a block
    void SpawnPlayerOnSurface()
    {
        if (player == null) return;

        int px = Mathf.RoundToInt(player.position.x);
        int pz = Mathf.RoundToInt(player.position.z);

        // Scan downward from the highest possible point to find the surface
        for (int y = baseHeight + maxHeightVariation + 5; y >= 0; y--)
        {
            Vector3Int checkPos = new Vector3Int(px, y, pz);
            if (Worldregistry.Instance.IsBlockSolid(checkPos))
            {
                player.position = new Vector3(player.position.x, y + 1.5f, player.position.z);
                return;
            }
        }
    }

    void Update() { }
}
