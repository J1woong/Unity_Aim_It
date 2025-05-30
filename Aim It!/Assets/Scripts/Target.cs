using UnityEngine;

public class Target : MonoBehaviour
{
    // 타겟 클릭 시 제거

    private void OnMouseDown()
    {
        Destroy(gameObject);
        // 필요 시 점수 증가, 효과음 재생 등 추가 구현 가능
    }
}
