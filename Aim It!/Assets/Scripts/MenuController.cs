using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Update()
    {
        // Ŭ�� �� Main ������ �̵�
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ��
        {
            SceneManager.LoadScene("Main"); // Main ������ ��ȯ
        }
    }
}
