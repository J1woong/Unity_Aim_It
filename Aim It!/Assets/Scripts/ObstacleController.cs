using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 mouseWorldPos;

    private int hitCount = 0;
    private float collisionRadius = 0.5f;

    private float lastHitTime = -Mathf.Infinity; // 마지막 히트 시간 초기화
    private float invincibleDuration = 1.5f;     // 무적 시간 1.5초

    private SpriteRenderer spriteRenderer;
    private Color normalColor = Color.white;    // 기본 색상
    private Color transparentColor = new Color(1f, 1f, 1f, 0.3f); // 반투명

    private float blinkInterval = 0.2f;  // 깜빡임 간격
    private float blinkTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer 컴포넌트를 찾을 수 없습니다!");
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

        // 무적 시간 내에 깜빡임 처리
        if (Time.time - lastHitTime <= invincibleDuration)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                // 색상 토글
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
                Debug.Log($"장애물이 마우스에 닿음! 누적 횟수: {hitCount}");

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
