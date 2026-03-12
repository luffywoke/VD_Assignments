using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerate : MonoBehaviour
{
    public static Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // Bottom face (looking up from below, clockwise)
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),

            // Top face (looking down from above, clockwise)
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),

            // Front face (looking from z = 0, clockwise)
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),

            // Back face (looking from z = 1, clockwise)
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),

            // Left face (looking from x = 0, clockwise)
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),

            // Right face (looking from x = 1, clockwise)
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
        };

        int[] triangles = new int[]
        {
            // Bottom
            0, 1, 2,
            0, 2, 3,

            // Top
            4, 5, 6,
            4, 6, 7,

            // Front
            8, 9, 10,
            8, 10, 11,

            // Back
            12, 13, 14,
            12, 14, 15,

            // Left
            16, 17, 18,
            16, 18, 19,

            // Right
            20, 21, 22,
            20, 22, 23,
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }


}
