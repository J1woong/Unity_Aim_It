using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isFake = false;

    // Ÿ�� ���̾ �˻��ϱ� ���� ���̾� ����ũ
    public LayerMask targetLayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            // ���̾� ����ũ�� �̿��� Ư�� ���̾ �˻�
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, targetLayerMask);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Ŭ�� Ƚ�� ī��Ʈ (�� ���� ȣ���)
                GameManager.Instance.OnClick();

                // ��¥/��¥ Ÿ�� ó��
                if (isFake)
                    GameManager.Instance.OnFakeTargetRemoved();
                else
                    GameManager.Instance.OnRealTargetRemoved();

                Destroy(gameObject);
            }
        }
    }
}
