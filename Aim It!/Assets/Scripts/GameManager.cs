using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text gameOverText;             // ���� ���� �޽��� UI �ؽ�Ʈ (������ ������ ���ᡱ ��)
    public Text accuracyText;             // ��Ȯ�� ǥ�� UI �ؽ�Ʈ
    public Text timerText;                // ���� �ð� ǥ�� UI �ؽ�Ʈ
    public Text realTargetCountText;      // ������ ��¥ Ÿ�� �� UI �ؽ�Ʈ
    public Text fakeTargetCountText;      // ������ ��¥ Ÿ�� �� UI �ؽ�Ʈ
    public Text totalClicksText;          // ��ü Ŭ�� Ƚ�� UI �ؽ�Ʈ

    private int realTargetRemoved = 0;    // ������ ��¥ Ÿ�� ��
    private int fakeTargetRemoved = 0;    // ������ ��¥ Ÿ�� ��
    private int totalClicks = 0;           // ��ü Ŭ�� Ƚ��

    private int totalRealTargets = 50;    // ��ǥ ��¥ Ÿ�� ��

    private bool isGameOver = false;
    private float elapsedTime = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameOverText.gameObject.SetActive(false);
        accuracyText.gameObject.SetActive(false);

        timerText.gameObject.SetActive(true);
        realTargetCountText.gameObject.SetActive(true);
        fakeTargetCountText.gameObject.SetActive(true);

        isGameOver = false;
        realTargetRemoved = 0;
        fakeTargetRemoved = 0;
        elapsedTime = 0f;

        UpdateUI();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            UpdateUI();
        }
        else if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // ��ü Ŭ�� ī��Ʈ ������ (��� Ŭ������ ȣ��)
    public void OnClick()
    {
        if (isGameOver) return;

        totalClicks++;
        UpdateUI();
    }

    // ��¥ Ÿ���� ���ŵ� �� ȣ��
    public void OnRealTargetRemoved()
    {
        if (isGameOver) return;

        realTargetRemoved++;
        UpdateUI();

        if (realTargetRemoved >= totalRealTargets)
        {
            EndGame();
        }
    }

    // ��¥ Ÿ���� ���ŵ� �� ȣ��
    public void OnFakeTargetRemoved()
    {
        if (isGameOver) return;

        fakeTargetRemoved++;
        UpdateUI();
    }

    private void EndGame()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        // ��Ȯ�� ��� �� ǥ��
        float accuracy = totalClicks > 0 ? ((totalRealTargets - fakeTargetRemoved) / (float)totalClicks) * 100f : 0f;
        accuracyText.text = $"��Ȯ��: {accuracy:F2}%";
        accuracyText.gameObject.SetActive(true);
    }

    private void UpdateUI()
    {
        timerText.text = $"Time : {elapsedTime:F2} sec";
        realTargetCountText.text = $"Real Target : {realTargetRemoved} / {totalRealTargets}";
        fakeTargetCountText.text = $"Fake Target : {fakeTargetRemoved}";
        totalClicksText.text = $"Click : {totalClicks}";
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
