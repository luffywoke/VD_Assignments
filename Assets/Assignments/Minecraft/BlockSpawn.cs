using UnityEngine;

public class BlockSpawn : MonoBehaviour
{
    public GameObject blockPrefab;
    private int gridWidth = 5;
    private int gridHeight = 5;
    private float gridCellSize = 1f;

    void Start()
    {
        blockSpawn();
    }

    void blockSpawn()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 spawnPos = new Vector3(x * gridCellSize, 0, y * gridCellSize);

                Instantiate(blockPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
