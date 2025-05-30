using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AreaCircleRenderer : MonoBehaviour
{
    public int segments = 60;
    public float radius = 5f;
    public Color lineColor = Color.white;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.loop = true;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        line.material = new Material(Shader.Find("Unlit/Color"));
        line.material.color = lineColor;

        CreatePoints();
    }

    void CreatePoints()
    {
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Z 위치를 -0.1f 등 카메라 앞쪽으로 약간 띄움
            line.SetPosition(i, new Vector3(x, y, -0.1f));
        }
    }
}
