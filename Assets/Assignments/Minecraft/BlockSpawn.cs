using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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

    [Header("Trees")]
    [UnityEngine.Range(0f, 1f)]
    public float treeChance = 0.15f;
    public int treeDistance = 3;
    public int trunkHeightMin = 3;
    public int trunkHeightMax = 6;
    public int leafRadius = 2;

    [Header("Materials")]
    public Material blockMaterial;

    [Header("Player")]
    public Transform player;

    private float gridCellSize = 1f;

    private List<Vector2Int> treePosns = new List<Vector2Int>();

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

        float offsetX = seed * 0.1f;
        float offsetZ = seed * 0.1f;

        // Store surface heights so tree pass can look them up
        int[,] surfaceHeights = new int[gridWidth, gridDepth]; // *

        // First pass: terrain blocks
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridDepth; z++)
            {
                float noiseValue = Mathf.PerlinNoise(
                    (x * noiseScale) + offsetX,
                    (z * noiseScale) + offsetZ
                );

                int surfaceHeight = baseHeight + Mathf.RoundToInt(noiseValue * maxHeightVariation);
                surfaceHeights[x, z] = surfaceHeight; // *

                for (int y = 0; y <= surfaceHeight; y++)
                {
                    Vector3 spawnPos = new Vector3(x, y, z);

                    Mesh meshToUse;
                    if (y == surfaceHeight)
                        meshToUse = grassMesh;
                    else if (y >= surfaceHeight - 3)
                        meshToUse = dirtMesh;
                    else
                        meshToUse = stoneMesh;

                    SpawnBlock(spawnPos, meshToUse, "Block_" + x + "_" + y + "_" + z);
                }
            }
        }

        // Second pass: trees // *
        Mesh logMesh = MeshGenerate.CreateLogMesh();  // *
        Mesh leafMesh = MeshGenerate.CreateLeafMesh(); // *

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridDepth; z++)
            {
                // Use a seeded random value per column — fully deterministic
                float treeNoise = SeededRandom(x, z, seed);
                if (treeNoise > treeChance) continue;

                // Check minimum distance from all already-placed trees
                if (!IsFarEnoughFromTrees(x, z)) continue;

                int surface = surfaceHeights[x, z];

                // Deterministic trunk height based on position and seed
                int trunkHeight = trunkHeightMin + Mathf.Abs((SeededRandomInt(x, z, seed + 1)) % (trunkHeightMax - trunkHeightMin + 1));

                // Spawn trunk
                for (int y = surface + 1; y <= surface + trunkHeight; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (!Worldregistry.Instance.IsBlockSolid(Worldregistry.ToGrid(pos)))
                        SpawnBlock(pos, logMesh, "Log_" + x + "_" + y + "_" + z);
                }

                // Spawn flat square of leaves on top of trunk
                int leafY = surface + trunkHeight + 1;
                for (int lx = x - leafRadius; lx <= x + leafRadius; lx++)
                {
                    for (int lz = z - leafRadius; lz <= z + leafRadius; lz++)
                    {
                        // Stay within world bounds
                        if (lx < 0 || lx >= gridWidth || lz < 0 || lz >= gridDepth) continue;

                        Vector3 leafPos = new Vector3(lx, leafY, lz);
                        if (!Worldregistry.Instance.IsBlockSolid(Worldregistry.ToGrid(leafPos)))
                            SpawnBlock(leafPos, leafMesh, "Leaf_" + lx + "_" + leafY + "_" + lz);
                    }
                }

                treePosns.Add(new Vector2Int(x, z));
            }
        }
    }

    // Spawns a single block and registers it // *
    void SpawnBlock(Vector3 pos, Mesh mesh, string name) // *
    {
        GameObject cube = new GameObject(name);
        cube.transform.position = pos;
        cube.layer = LayerMask.NameToLayer("Block");
        cube.AddComponent<BoxCollider>();

        MeshFilter mf = cube.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = cube.AddComponent<MeshRenderer>();
        mr.material = blockMaterial;

        Worldregistry.Instance.RegisterBlock(Worldregistry.ToGrid(pos), cube);
    }

    // Returns true if (x,z) is at least treeDistance blocks away from all existing trees // *
    bool IsFarEnoughFromTrees(int x, int z) // *
    {
        foreach (Vector2Int t in treePosns)
        {
            int dx = x - t.x;
            int dz = z - t.y;
            if (Mathf.Sqrt(dx * dx + dz * dz) < treeDistance)
                return false;
        }
        return true;
    }

    // Deterministic float 0-1 based on position and seed // *
    float SeededRandom(int x, int z, int s) // *
    {
        int hash = x * 73856093 ^ z * 19349663 ^ s * 83492791;
        return (Mathf.Abs(hash) % 1000) / 1000f;
    }

    // Deterministic int based on position and seed // *
    int SeededRandomInt(int x, int z, int s) // *
    {
        return x * 73856093 ^ z * 19349663 ^ s * 83492791;
    }

    void SpawnPlayerOnSurface()
    {
        if (player == null) return;

        int px = Mathf.RoundToInt(player.position.x);
        int pz = Mathf.RoundToInt(player.position.z);

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