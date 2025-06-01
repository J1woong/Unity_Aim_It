using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Update()
    {
        // 클릭 시 Main 씬으로 이동
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            SceneManager.LoadScene("Main"); // Main 씬으로 전환
        }
    }
}
