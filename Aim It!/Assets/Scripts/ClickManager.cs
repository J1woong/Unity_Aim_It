using UnityEngine;

public class ClickManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.OnClick();
        }
    }
}
