using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isFake = false;

    // 타겟 레이어만 검사하기 위한 레이어 마스크
    public LayerMask targetLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // 레이어 마스크를 이용해 특정 레이어만 검사
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, targetLayerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 클릭 횟수 카운트 (한 번만 호출됨)
                GameManager.Instance.OnClick();

                // 가짜/진짜 타겟 처리
                if (isFake)
                    GameManager.Instance.OnFakeTargetRemoved();
                else
                    GameManager.Instance.OnRealTargetRemoved();

                Destroy(gameObject);
            }
        }
    }
}
