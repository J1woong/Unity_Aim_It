using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 mouseWorldPos;

    private int hitCount = 0;
    private float collisionRadius = 0.5f;

    private float lastHitTime = -Mathf.Infinity; // ������ ��Ʈ �ð� �ʱ�ȭ
    private float invincibleDuration = 1.5f;     // ���� �ð� 1.5��

    private SpriteRenderer spriteRenderer;
    private Color normalColor = Color.white;    // �⺻ ����
    private Color transparentColor = new Color(1f, 1f, 1f, 0.3f); // ������

    private float blinkInterval = 0.2f;  // ������ ����
    private float blinkTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.isCountingDown || GameManager.Instance.isGameOver)
        {
            SetColor(normalColor);
            return;
        }

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        CheckMouseCollision();

        // ���� �ð� ���� ������ ó��
        if (Time.time - lastHitTime <= invincibleDuration)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                // ���� ���
                if (spriteRenderer.color.a == normalColor.a)
                    SetColor(transparentColor);
                else
                    SetColor(normalColor);
            }
        }
        else
        {
            SetColor(normalColor);
        }
    }

    private void CheckMouseCollision()
    {
        float distance = Vector3.Distance(transform.position, mouseWorldPos);

        if (distance <= collisionRadius)
        {
            if (Time.time - lastHitTime > invincibleDuration)
            {
                hitCount++;
                lastHitTime = Time.time;
                Debug.Log($"��ֹ��� ���콺�� ����! ���� Ƚ��: {hitCount}");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.IncreaseObstacleHitCount();
                }
            }
        }
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    public int GetHitCount()
    {
        return hitCount;
    }
}
