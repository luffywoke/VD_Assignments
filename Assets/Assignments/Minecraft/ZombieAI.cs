using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AstarPath))]
public class ZombieAI : MonoBehaviour
{
    // Inspector fields

    [Header("References")]
    [Tooltip("The player the zombie will chase.")]
    public Transform player;

    [Header("Movement")]
    [Tooltip("How fast the zombie moves (units per second).")]
    public float moveSpeed = 3f;

    [Tooltip("How close the zombie must get to a waypoint before moving to the next.")]
    public float waypointReachedDistance = 0.3f;

    [Header("Detection")]
    [Tooltip("Maximum distance at which the zombie can see the player.")]
    public float sightRange = 15f;

    [Tooltip("How many seconds of lost sight before the zombie gives up chasing.")]
    public float lostSightTimeout = 3f;

    [Header("Pathfinding")]
    [Tooltip("How often (seconds) the zombie recalculates its path while chasing.")]
    public float pathRecalculateInterval = 1f;

    [Tooltip("How often (seconds) the zombie picks a new wander target.")]
    public float wanderInterval = 4f;

    [Tooltip("How far away (in blocks) the zombie can pick a wander target.")]
    public int wanderRadius = 8;

    // State machine

    private enum State { Wandering, Chasing }
    private State _currentState = State.Wandering;

    // Runtime data

    private AstarPath _pathfinder;
    private List<Vector3> _path = new List<Vector3>();
    private int _pathIndex = 0;

    private float _pathRecalcTimer = 0f;
    private float _wanderTimer = 0f;
    private float _lostSightTimer = 0f;

    // Gizmo data
    private List<Vector3> _gizmoPath = new List<Vector3>();
    private Vector3 _gizmoNextTarget = Vector3.zero;
    private bool _hasGizmoTarget = false;

    private void Awake()
    {
        _pathfinder = GetComponent<AstarPath>();
    }

    private void Start()
    {
        // Subscribe to world-change events so the path is refreshed if blocks change
        if (Worldregistry.Instance != null)
            Worldregistry.Instance.OnWorldChanged += OnWorldChanged;

        // Start in wandering state — immediately pick a wander destination
        EnterWandering();
    }

    private void OnDestroy()
    {
        if (Worldregistry.Instance != null)
            Worldregistry.Instance.OnWorldChanged -= OnWorldChanged;
    }

    // Main update loop

    private void Update()
    {
        switch (_currentState)
        {
            case State.Wandering: UpdateWandering(); break;
            case State.Chasing: UpdateChasing(); break;
        }

        FollowPath();
    }

    // Wandering state

    private void EnterWandering()
    {
        _currentState = State.Wandering;
        _wanderTimer = 0f;
        PickNewWanderTarget();
    }

    private void UpdateWandering()
    {
        // Check for line-of-sight — if spotted, switch to chasing
        if (CanSeePlayer())
        {
            EnterChasing();
            return;
        }

        // Periodically pick a new random wander target
        _wanderTimer += Time.deltaTime;
        if (_wanderTimer >= wanderInterval || _path.Count == 0)
        {
            _wanderTimer = 0f;
            PickNewWanderTarget();
        }
    }

    private void PickNewWanderTarget()
    {
        // Try a handful of random positions until we find a walkable one
        for (int attempt = 0; attempt < 20; attempt++)
        {
            Vector3Int zombieGrid = Worldregistry.ToGrid(transform.position);

            Vector3Int candidate = new Vector3Int(
                zombieGrid.x + Random.Range(-wanderRadius, wanderRadius + 1),
                zombieGrid.y,
                zombieGrid.z + Random.Range(-wanderRadius, wanderRadius + 1)
            );

            if (Worldregistry.Instance != null && Worldregistry.Instance.IsWalkable(candidate))
            {
                SetPath(_pathfinder.FindPath(transform.position, (Vector3)candidate));
                return;
            }
        }

        // Could not find a valid wander target this tick — try again next interval
    }

    // Chasing state

    private void EnterChasing()
    {
        _currentState = State.Chasing;
        _lostSightTimer = 0f;
        _pathRecalcTimer = pathRecalculateInterval; // Force immediate recalc
    }

    private void UpdateChasing()
    {
        if (CanSeePlayer())
        {
            _lostSightTimer = 0f;
        }
        else
        {
            _lostSightTimer += Time.deltaTime;
            if (_lostSightTimer >= lostSightTimeout)
            {
                EnterWandering();
                return;
            }
        }

        // Periodically recalculate path to player
        _pathRecalcTimer += Time.deltaTime;
        if (_pathRecalcTimer >= pathRecalculateInterval)
        {
            _pathRecalcTimer = 0f;
            SetPath(_pathfinder.FindPath(transform.position, player.position));
        }
    }

    // Path following

    private void FollowPath()
    {
        if (_path == null || _pathIndex >= _path.Count) return;

        Vector3 target = _path[_pathIndex];

        // Move horizontally toward the waypoint; snap Y to avoid floating/sinking
        Vector3 targetFlat = new Vector3(target.x, transform.position.y, target.z);
        float distFlat = Vector3.Distance(transform.position, targetFlat);

        if (distFlat < waypointReachedDistance)
        {
            // Snap vertical position when close enough, then advance to next waypoint
            transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
            _pathIndex++;
        }
        else
        {
            // Move toward current waypoint
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Update gizmo target
        if (_pathIndex < _path.Count)
        {
            _gizmoNextTarget = _path[_pathIndex];
            _hasGizmoTarget = true;
        }
        else
        {
            _hasGizmoTarget = false;
        }
    }

    // Set a new path

    private void SetPath(List<Vector3> newPath)
    {
        _path = newPath ?? new List<Vector3>();
        _pathIndex = 0;

        // Mirror into gizmo list
        _gizmoPath.Clear();
        _gizmoPath.AddRange(_path);
        _hasGizmoTarget = _path.Count > 0;
    }

    // Called by WorldRegistry when a block is placed or broken
    private void OnWorldChanged()
    {
        if (_path == null || _path.Count == 0) return;

        // Recalculate based on current state destination
        if (_currentState == State.Chasing && player != null)
            SetPath(_pathfinder.FindPath(transform.position, player.position));
        else
            PickNewWanderTarget();
    }

    // Returns true if the zombie has line-of-sight to the player
    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 zombieEye = transform.position + Vector3.up * 1.5f;
        Vector3 playerChest = player.position + Vector3.up * 1f;

        float dist = Vector3.Distance(zombieEye, playerChest);
        if (dist > sightRange) return false;

        Vector3 direction = (playerChest - zombieEye).normalized;

        // Raycast — if we hit something that isn't the player it means a block is in the way
        if (Physics.Raycast(zombieEye, direction, out RaycastHit hit, dist))
        {
            return hit.transform == player;
        }

        // Nothing between zombie and player
        return true;
    }

    // Draws the A* path in the Scene view for debugging
    private void OnDrawGizmos()
    {
        // Draw the full path
        if (_gizmoPath != null && _gizmoPath.Count > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _gizmoPath.Count; i++)
            {
                Gizmos.DrawSphere(_gizmoPath[i] + Vector3.up * 0.5f, 0.15f);

                if (i > 0)
                    Gizmos.DrawLine(
                        _gizmoPath[i - 1] + Vector3.up * 0.5f,
                        _gizmoPath[i] + Vector3.up * 0.5f
                    );
            }
        }

        // Highlight the current target waypoint in a distinct colour
        if (_hasGizmoTarget)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_gizmoNextTarget + Vector3.up * 0.5f, 0.25f);
        }

        // Draw sight range
        Gizmos.color = (_currentState == State.Chasing) ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}