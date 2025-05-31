using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject Area;          // ���� ���� ������Ʈ (SphereCollider �Ǵ� CircleCollider2D ����)
    public GameObject TargetPrefab;  // ���� ǥ�� ������
    public GameObject FakePrefab;    // ��¥ ǥ�� ������

    public float SpawnInterval = 0.5f; // ���� ���� (��)
    public int MaxTargets = 10;        // �ִ� ������ ǥ�� ���� (��¥ + ��¥ �ջ�)
    public float TargetLifetime = 2f;  // Ÿ�� ���� �ð� (��)

    private CircleCollider2D areaCollider2D;
    private SphereCollider areaCollider3D;
    private bool is3D = false;

    private Queue<GameObject> activeTargets = new Queue<GameObject>(); // ��� Ÿ���� ���� ����

    void Start()
    {
        areaCollider2D = Area.GetComponent<CircleCollider2D>();
        areaCollider3D = Area.GetComponent<SphereCollider>();
        is3D = areaCollider3D != null;

        InvokeRepeating(nameof(SpawnTarget), 0f, SpawnInterval);
    }

    void SpawnTarget()
    {
        // �ִ� ���� �ʰ� �� ���� ������ Ÿ�� ����
        if (activeTargets.Count >= MaxTargets)
        {
            GameObject oldest = activeTargets.Dequeue();
            if (oldest != null) Destroy(oldest);
        }

        Vector3 spawnPos = is3D ? GetRandomPointInArea3D() : GetRandomPointInArea2D();

        // 1/4 Ȯ���� ��¥ Ÿ�� ����, �ƴϸ� ���� Ÿ�� ����
        GameObject prefabToSpawn = (Random.value < 0.25f) ? FakePrefab : TargetPrefab;

        GameObject newTarget = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        activeTargets.Enqueue(newTarget);

        Destroy(newTarget, TargetLifetime);
    }

    Vector3 GetRandomPointInArea3D()
    {
        Vector3 center = areaCollider3D.bounds.center;
        float radius = areaCollider3D.radius * Area.transform.lossyScale.x;
        Vector3 randomDirection = Random.onUnitSphere;
        float randomDistance = Random.Range(0f, radius);
        Vector3 randomPoint = center + randomDirection * randomDistance;
        randomPoint.z = Area.transform.position.z; // 2D �����̸� Z ����
        return randomPoint;
    }

    Vector3 GetRandomPointInArea2D()
    {
        Vector2 center = areaCollider2D.bounds.center;
        float radius = areaCollider2D.radius * Area.transform.lossyScale.x;
        Vector2 randomPoint2D = Random.insideUnitCircle * radius;
        Vector3 randomPoint = new Vector3(center.x + randomPoint2D.x, center.y + randomPoint2D.y, Area.transform.position.z);
        return randomPoint;
    }
}
