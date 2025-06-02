using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 mouseWorldPos;

    private int hitCount = 0;
    private float collisionRadius = 0.5f;

    private float lastHitTime = -Mathf.Infinity; // 마지막 히트 시간 초기화
    private float invincibleDuration = 1.5f;     // 무적 시간 1.5초

    void Update()
    {
        if (GameManager.Instance == null) return;

        // 카운트다운 중이거나 게임 오버면 아무 것도 하지 않음
        if (GameManager.Instance.isCountingDown || GameManager.Instance.isGameOver)
            return;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        CheckMouseCollision();
    }

    private void CheckMouseCollision()
    {
        float distance = Vector3.Distance(transform.position, mouseWorldPos);

        if (distance <= collisionRadius)
        {
            // 무적시간 체크
            if (Time.time - lastHitTime > invincibleDuration)
            {
                hitCount++;
                lastHitTime = Time.time;
                Debug.Log($"장애물이 마우스에 닿음! 누적 횟수: {hitCount}");
            }
        }
    }

    public int GetHitCount()
    {
        return hitCount;
    }
}
