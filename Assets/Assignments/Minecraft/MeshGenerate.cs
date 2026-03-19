using UnityEngine;

public class MeshGenerate : MonoBehaviour
{
    public static Mesh CreateCubeMesh(Vector2 textureOffset)
    {
        Mesh mesh = new Mesh();

        float x = textureOffset.x;
        float y = textureOffset.y;
        float t = 0.0625f; // one texture size in UV space

        Vector2 bottomLeft = new Vector2(x, y);
        Vector2 bottomRight = new Vector2(x + t, y);
        Vector2 topLeft = new Vector2(x, y + t);
        Vector2 topRight = new Vector2(x + t, y + t);

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

        // Same UV layout applied to every face
        Vector2[] uvs = new Vector2[]
        {
            // Bottom
            bottomLeft, bottomRight, topRight, topLeft,

            // Top
            bottomLeft, bottomRight, topRight, topLeft,

            // Front
            bottomLeft, bottomRight, topRight, topLeft,

            // Back
            bottomLeft, bottomRight, topRight, topLeft,

            // Left
            bottomLeft, bottomRight, topRight, topLeft,

            // Right
            bottomLeft, bottomRight, topRight, topLeft,
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh CreateDirtMesh()
    {
        // column 2, row 15 (flipped from top row 0)
        return CreateCubeMesh(new Vector2(2 * 0.0625f, 15 * 0.0625f));
    }

    public static Mesh CreateGrassMesh()
    {
        // column 0, row 15 (flipped from top row 0)
        return CreateCubeMesh(new Vector2(0 * 0.0625f, 15 * 0.0625f));
    }
}
