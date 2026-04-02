using System.Collections.Generic;
using UnityEngine;

public class AstarPath : MonoBehaviour
{
    // Extra gCost added when the zombie moves up or down one block.
    // Keeps flat paths preferred over unnecessary climbing.
    private const float VERTICAL_COST_PENALTY = 2f;

    /// <summary>
    /// Finds a path from startWorld to targetWorld.
    /// Returns a list of world-space Vector3 positions to walk through,
    /// or an empty list if no path exists.
    /// </summary>
    public List<Vector3> FindPath(Vector3 startWorld, Vector3 targetWorld)
    {
        Vector3Int start = Worldregistry.ToGrid(startWorld);
        Vector3Int target = Worldregistry.ToGrid(targetWorld);

        // A* data structures
        var openSet = new PriorityQueue<Node>();
        var closedSet = new HashSet<Vector3Int>();

        // gCost lookup so we can check if we found a cheaper path to a node
        var gCostMap = new Dictionary<Vector3Int, float>();

        // Seed the open set with the start node
        float startH = Heuristic(start, target);
        openSet.Enqueue(new Node(start, 0f, startH, null));
        gCostMap[start] = 0f;

        // Main A* loop
        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            // Reached the target — reconstruct and return the path
            if (current.position == target)
                return ReconstructPath(current);

            // Already evaluated this position via a cheaper route
            if (closedSet.Contains(current.position))
                continue;

            closedSet.Add(current.position);

            // Evaluate each neighbour
            foreach (Vector3Int neighbourPos in GetNeighbours(current.position))
            {
                if (closedSet.Contains(neighbourPos)) continue;

                // Determine movement cost (vertical movement costs more)
                float moveCost = 1f;
                if (neighbourPos.y != current.position.y)
                    moveCost += VERTICAL_COST_PENALTY;

                float tentativeG = current.gCost + moveCost;

                // Skip if we already have a cheaper path to this neighbour
                if (gCostMap.TryGetValue(neighbourPos, out float existingG) && tentativeG >= existingG)
                    continue;

                gCostMap[neighbourPos] = tentativeG;

                float h = Heuristic(neighbourPos, target);
                openSet.Enqueue(new Node(neighbourPos, tentativeG, h, current));
            }
        }

        // No path found
        return new List<Vector3>();
    }

    // Heuristic: Euclidean distance between two grid positions
    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Vector3Int.Distance(a, b);
    }

    // Returns all valid walkable neighbours of a given grid position
    private IEnumerable<Vector3Int> GetNeighbours(Vector3Int pos)
    {
        // The 4 flat cardinal directions
        Vector3Int[] flatDirections = new Vector3Int[]
        {
            new Vector3Int( 1, 0,  0),
            new Vector3Int(-1, 0,  0),
            new Vector3Int( 0, 0,  1),
            new Vector3Int( 0, 0, -1),
        };

        foreach (var dir in flatDirections)
        {
            // Flat movement
            Vector3Int flat = pos + dir;
            if (Worldregistry.Instance.IsWalkable(flat))
                yield return flat;

            // Step UP one block
            Vector3Int stepUp = pos + dir + Vector3Int.up;
            bool headRoomAboveCurrent = !Worldregistry.Instance.IsBlockSolid(pos + Vector3Int.up);
            if (headRoomAboveCurrent && Worldregistry.Instance.IsWalkable(stepUp))
                yield return stepUp;

            // Step DOWN one block
            Vector3Int stepDown = pos + dir + Vector3Int.down;
            if (Worldregistry.Instance.IsWalkable(stepDown))
                yield return stepDown;
        }
    }

    // Walks the parent chain back to the start and returns the path in order
    private List<Vector3> ReconstructPath(Node endNode)
    {
        var path = new List<Vector3>();
        Node current = endNode;

        while (current != null)
        {
            // Offset by (0.5, 0, 0.5) so the zombie walks through block centres
            path.Add(new Vector3(
                current.position.x + 0.5f,
                current.position.y,
                current.position.z + 0.5f
            ));
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
