using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject Area;          // 원형 범위 오브젝트 (SphereCollider 또는 CircleCollider2D 포함)
    public GameObject TargetPrefab;  // 정상 표적 프리팹
    public GameObject FakePrefab;    // 가짜 표적 프리팹

    public float SpawnInterval = 0.5f; // 생성 간격 (초)
    public int MaxTargets = 10;        // 최대 생성할 표적 개수 (진짜 + 가짜 합산)
    public float TargetLifetime = 2f;  // 타겟 존재 시간 (초)

    private CircleCollider2D areaCollider2D;
    private SphereCollider areaCollider3D;
    private bool is3D = false;

    private Queue<GameObject> activeTargets = new Queue<GameObject>(); // 모든 타겟을 같이 관리

    void Start()
    {
        areaCollider2D = Area.GetComponent<CircleCollider2D>();
        areaCollider3D = Area.GetComponent<SphereCollider>();
        is3D = areaCollider3D != null;

        InvokeRepeating(nameof(SpawnTarget), 0f, SpawnInterval);
    }

    void SpawnTarget()
    {
        // 최대 개수 초과 시 가장 오래된 타겟 제거
        if (activeTargets.Count >= MaxTargets)
        {
            GameObject oldest = activeTargets.Dequeue();
            if (oldest != null) Destroy(oldest);
        }

        Vector3 spawnPos = is3D ? GetRandomPointInArea3D() : GetRandomPointInArea2D();

        // 1/4 확률로 가짜 타겟 생성, 아니면 정상 타겟 생성
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
        randomPoint.z = Area.transform.position.z; // 2D 게임이면 Z 고정
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
