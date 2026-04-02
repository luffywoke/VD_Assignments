using UnityEngine;
using System.Collections.Generic;

public class Worldregistry : MonoBehaviour
{
    // Singleton 
    public static Worldregistry Instance { get; private set; }

    // Block storage 
    // Key   = block position rounded to the nearest integer grid cell
    // Value = the actual GameObject
    private Dictionary<Vector3Int, GameObject> _blocks = new Dictionary<Vector3Int, GameObject>();

    //Observer pattern 
    // Any listener (e.g. ZombieAI) can subscribe to be notified when the world changes.
    public event System.Action OnWorldChanged;

   

    private void Awake()
    {
        // Classic singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Public API 

    
    // Call this whenever a block is created (from BlockSpawn or destroyAndPlace).
    public void RegisterBlock(Vector3Int position, GameObject block)
    {
        _blocks[position] = block;
        OnWorldChanged?.Invoke();
    }

 
    // Call this whenever a block is destroyed (from destroyAndPlace).
    public void UnregisterBlock(Vector3Int position)
    {
        if (_blocks.ContainsKey(position))
        {
            _blocks.Remove(position);
            OnWorldChanged?.Invoke();
        }
    }

    
    // Converts a world-space Vector3 to the Vector3Int grid key used by the registry.
    public static Vector3Int ToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    
    // Returns true if there is a solid block at the given grid position.
    // Used by the pathfinder to determine which cells are walls.
    
    public bool IsBlockSolid(Vector3Int position)
    {
        return _blocks.ContainsKey(position);
    }

   
    // Returns true if the given grid position is walkable:
    // - The position itself must be AIR (no block).
    // - There must be a solid block directly below to stand on.
    public bool IsWalkable(Vector3Int position)
    {
        bool positionIsClear = !IsBlockSolid(position);
        bool headIsClear = !IsBlockSolid(position + Vector3Int.up); 
        bool hasFloor = IsBlockSolid(position + Vector3Int.down);
        return positionIsClear && headIsClear && hasFloor;
    }

    
    // Exposes a read-only snapshot of all registered block positions.
    // Useful for debugging.
    public IEnumerable<Vector3Int> AllBlockPositions => _blocks.Keys;
}
