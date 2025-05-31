using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isFake = false;  // �ν����Ϳ��� �ݵ�� ��¥ Ÿ���� true�� �����ؾ� ��

    private void OnMouseDown()
    {
        GameManager.Instance.OnClick(); // Ŭ�� Ƚ�� ����

        if (isFake)
            GameManager.Instance.OnFakeTargetRemoved();
        else
            GameManager.Instance.OnRealTargetRemoved();

        Destroy(gameObject);
    }
}
