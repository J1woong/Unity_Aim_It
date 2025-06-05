using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartTextBlink : MonoBehaviour
{
    public Text startText;         // StartText UI 텍스트
    public float blinkSpeed = 0.5f; // 깜빡이는 속도 조절

    private void Start()
    {
        // 텍스트가 깜빡이도록 하는 코루틴 시작
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        Color originalColor = startText.color;  // 원래 색상 저장

        while (true)  // 무한 반복
        {
            startText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.PingPong(Time.time * blinkSpeed, 1f));  // 알파 값 깜빡이기
            yield return null;  // 한 프레임 대기
        }
    }
}
