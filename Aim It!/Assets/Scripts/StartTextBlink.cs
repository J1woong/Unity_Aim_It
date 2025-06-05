using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartTextBlink : MonoBehaviour
{
    public Text startText;         // StartText UI �ؽ�Ʈ
    public float blinkSpeed = 0.5f; // �����̴� �ӵ� ����

    private void Start()
    {
        // �ؽ�Ʈ�� �����̵��� �ϴ� �ڷ�ƾ ����
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        Color originalColor = startText.color;  // ���� ���� ����

        while (true)  // ���� �ݺ�
        {
            startText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.PingPong(Time.time * blinkSpeed, 1f));  // ���� �� �����̱�
            yield return null;  // �� ������ ���
        }
    }
}
