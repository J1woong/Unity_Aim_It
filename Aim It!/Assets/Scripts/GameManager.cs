using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text gameOverText;             // 게임 종료 메시지 UI 텍스트 (간단히 “게임 종료” 등)
    public Text accuracyText;             // 정확도 표시 UI 텍스트
    public Text timerText;                // 진행 시간 표시 UI 텍스트
    public Text realTargetCountText;      // 제거한 진짜 타겟 수 UI 텍스트
    public Text fakeTargetCountText;      // 제거한 가짜 타겟 수 UI 텍스트
    public Text totalClicksText;          // 전체 클릭 횟수 UI 텍스트

    private int realTargetRemoved = 0;    // 제거한 진짜 타겟 수
    private int fakeTargetRemoved = 0;    // 제거한 가짜 타겟 수
    private int totalClicks = 0;           // 전체 클릭 횟수

    private int totalRealTargets = 50;    // 목표 진짜 타겟 수

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

    // 전체 클릭 카운트 증가용 (모든 클릭에서 호출)
    public void OnClick()
    {
        if (isGameOver) return;

        totalClicks++;
        UpdateUI();
    }

    // 진짜 타겟이 제거될 때 호출
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

    // 가짜 타겟이 제거될 때 호출
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
        // 정확도 계산 및 표시
        float accuracy = totalClicks > 0 ? ((totalRealTargets - fakeTargetRemoved) / (float)totalClicks) * 100f : 0f;
        accuracyText.text = $"정확도: {accuracy:F2}%";
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
