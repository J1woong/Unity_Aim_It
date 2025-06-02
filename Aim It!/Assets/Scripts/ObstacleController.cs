using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 mouseWorldPos;

    private int hitCount = 0;
    private float collisionRadius = 0.5f;

    private float lastHitTime = -Mathf.Infinity; // ������ ��Ʈ �ð� �ʱ�ȭ
    private float invincibleDuration = 1.5f;     // ���� �ð� 1.5��

    void Update()
    {
        if (GameManager.Instance == null) return;

        // ī��Ʈ�ٿ� ���̰ų� ���� ������ �ƹ� �͵� ���� ����
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
            // �����ð� üũ
            if (Time.time - lastHitTime > invincibleDuration)
            {
                hitCount++;
                lastHitTime = Time.time;
                Debug.Log($"��ֹ��� ���콺�� ����! ���� Ƚ��: {hitCount}");
            }
        }
    }

    public int GetHitCount()
    {
        return hitCount;
    }
}
