using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text gameOverText;             // ���� ���� �޽��� UI �ؽ�Ʈ
    public Text accuracyText;             // ��Ȯ�� ǥ�� UI �ؽ�Ʈ
    public Text timerText;                // ���� �ð� ǥ�� UI �ؽ�Ʈ
    public Text realTargetCountText;      // ������ ��¥ Ÿ�� �� UI �ؽ�Ʈ
    public Text fakeTargetCountText;      // ������ ��¥ Ÿ�� �� UI �ؽ�Ʈ
    public Text totalClicksText;          // ��ü Ŭ�� Ƚ�� UI �ؽ�Ʈ
    public Text obstacleHitCountText;     // ��ֹ� �浹 Ƚ�� UI �ؽ�Ʈ
    public Text countdownText;            // ī��Ʈ�ٿ� �ؽ�Ʈ UI (3�� ī��Ʈ�ٿ�)

    private int realTargetRemoved = 0;    // ������ ��¥ Ÿ�� ��
    private int fakeTargetRemoved = 0;    // ������ ��¥ Ÿ�� ��
    private int totalClicks = 0;          // ��ü Ŭ�� Ƚ��
    private int obstacleHitCount = 0;     // ��ֹ� �浹 Ƚ��

    private int totalRealTargets = 50;    // ��ǥ ��¥ Ÿ�� ��

    public bool isGameOver = false;
    public bool isCountingDown = false; // ī��Ʈ�ٿ� ���� ����
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
        totalClicksText.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(false);  // ī��Ʈ�ٿ� �ؽ�Ʈ ����
        obstacleHitCountText.gameObject.SetActive(true); // ��ֹ� �浹 UI Ȱ��ȭ

        isGameOver = false;
        realTargetRemoved = 0;
        fakeTargetRemoved = 0;
        obstacleHitCount = 0;
        elapsedTime = 0f;

        UpdateUI();

        // ������ ���۵Ǹ� ī��Ʈ�ٿ� ����
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        // ESC Ű ������ �� Menu ������ �̵�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu"); // Menu ������ ��ȯ
        }

        if (!isGameOver && !isCountingDown)
        {
            elapsedTime += Time.deltaTime;
            UpdateUI();
        }
        else if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // 3�� ī��Ʈ�ٿ� ����
    IEnumerator StartCountdown()
    {
        isCountingDown = true;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"{i}";
            countdownText.gameObject.SetActive(true);  // ī��Ʈ�ٿ� �ؽ�Ʈ ���̱�
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);  // ī��Ʈ�ٿ� �ؽ�Ʈ �����
        StartGame();  // ���� ����
    }

    private void StartGame()
    {
        Debug.Log("���� ����!");
        isCountingDown = false;
        elapsedTime = 0f;  // Ÿ�̸� �ʱ�ȭ
    }

    public void OnClick()
    {
        if (isGameOver || isCountingDown) return;

        totalClicks++;
        UpdateUI();
    }

    public void OnRealTargetRemoved()
    {
        if (isGameOver || isCountingDown) return;

        realTargetRemoved++;
        UpdateUI();

        if (realTargetRemoved >= totalRealTargets)
        {
            EndGame();
        }
    }

    public void OnFakeTargetRemoved()
    {
        if (isGameOver || isCountingDown) return;

        fakeTargetRemoved++;
        UpdateUI();
    }

    // ��ֹ� �浹 Ƚ�� ���� �Լ�
    public void IncreaseObstacleHitCount()
    {
        if (isGameOver || isCountingDown) return;

        obstacleHitCount++;
        UpdateUI();
    }

    private void EndGame()
    {
        isGameOver = true;

        // ��Ȯ�� ���
        float accuracy = totalClicks > 0 ? ((totalRealTargets - fakeTargetRemoved) / (float)totalClicks) * 100f : 0f;


        // ���̾Ƹ�� ������ ��ũ ���
        string rank = CalculateRank(accuracy, elapsedTime, obstacleHitCount);
        accuracyText.text = $"��Ȯ��: {accuracy:F2}%\n��ũ: {rank}";

        gameOverText.gameObject.SetActive(true);
        accuracyText.gameObject.SetActive(true);
    }

    private string CalculateRank(float accuracy,float elapsedTime, int obstacleHits)
    {
        float finalScore = accuracy;

        finalScore += Mathf.Max(0, (50f - elapsedTime));
        finalScore -= Mathf.Min(50f, obstacleHits);

        if (finalScore >= 90f) return "S";
        else if (finalScore >= 80f) return "A";
        else if (finalScore >= 55f) return "B";
        else if (finalScore >= 45f) return "C";
        else if (finalScore >= 20f) return "D";
        else return "E";
    }

    private void UpdateUI()
    {
        timerText.text = $"Time : {elapsedTime:F2} sec";
        realTargetCountText.text = $"Real Target : {realTargetRemoved} / {totalRealTargets}";
        fakeTargetCountText.text = $"Fake Target : {fakeTargetRemoved}";
        totalClicksText.text = $"Click : {totalClicks}";
        obstacleHitCountText.text = $"Hits : {obstacleHitCount}";  // ��ֹ� �浹 Ƚ�� UI ����
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
