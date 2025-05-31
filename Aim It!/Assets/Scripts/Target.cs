using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isFake = false;  // 인스펙터에서 반드시 가짜 타겟은 true로 설정해야 함

    private void OnMouseDown()
    {
        GameManager.Instance.OnClick(); // 클릭 횟수 증가

        if (isFake)
            GameManager.Instance.OnFakeTargetRemoved();
        else
            GameManager.Instance.OnRealTargetRemoved();

        Destroy(gameObject);
    }
}
