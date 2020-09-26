using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator generator;
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh meshInstance, int res, Vector3 up)
    {
        generator = shapeGenerator;
        mesh = meshInstance;
        resolution = res;
        localUp = up;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
        //axisA = -Vector3.Cross(localUp, axisB);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
        int triIndex = 0;
        Vector2[] uvs = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1f);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2f * axisA + (percent.y - 0.5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = generator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex + 0] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uvs = new Vector2[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1f);
                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2f * axisA + (percent.y - 0.5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uvs[i] = new Vector2(colorGenerator.BiomePercentageFromPoint(pointOnUnitSphere), 0f);
            }
        }

        mesh.uv = uvs;
    }
}
