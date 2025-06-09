using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text gameOverText;             // 게임 종료 메시지 UI 텍스트
    public Text accuracyText;             // 정확도 표시 UI 텍스트
    public Text timerText;                // 진행 시간 표시 UI 텍스트
    public Text realTargetCountText;      // 제거한 진짜 타겟 수 UI 텍스트
    public Text fakeTargetCountText;      // 제거한 가짜 타겟 수 UI 텍스트
    public Text totalClicksText;          // 전체 클릭 횟수 UI 텍스트
    public Text obstacleHitCountText;     // 장애물 충돌 횟수 UI 텍스트
    public Text countdownText;            // 카운트다운 텍스트 UI (3초 카운트다운)

    private int realTargetRemoved = 0;    // 제거한 진짜 타겟 수
    private int fakeTargetRemoved = 0;    // 제거한 가짜 타겟 수
    private int totalClicks = 0;          // 전체 클릭 횟수
    private int obstacleHitCount = 0;     // 장애물 충돌 횟수

    private int totalRealTargets = 50;    // 목표 진짜 타겟 수

    public bool isGameOver = false;
    public bool isCountingDown = false; // 카운트다운 진행 여부
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
        countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 숨김
        obstacleHitCountText.gameObject.SetActive(true); // 장애물 충돌 UI 활성화

        isGameOver = false;
        realTargetRemoved = 0;
        fakeTargetRemoved = 0;
        obstacleHitCount = 0;
        elapsedTime = 0f;

        UpdateUI();

        // 게임이 시작되면 카운트다운 시작
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        // ESC 키 눌렀을 때 Menu 씬으로 이동
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu"); // Menu 씬으로 전환
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

    // 3초 카운트다운 시작
    IEnumerator StartCountdown()
    {
        isCountingDown = true;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"{i}";
            countdownText.gameObject.SetActive(true);  // 카운트다운 텍스트 보이기
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 숨기기
        StartGame();  // 게임 시작
    }

    private void StartGame()
    {
        Debug.Log("게임 시작!");
        isCountingDown = false;
        elapsedTime = 0f;  // 타이머 초기화
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

    // 장애물 충돌 횟수 증가 함수
    public void IncreaseObstacleHitCount()
    {
        if (isGameOver || isCountingDown) return;

        obstacleHitCount++;
        UpdateUI();
    }

    private void EndGame()
    {
        isGameOver = true;

        // 정확도 계산
        float accuracy = totalClicks > 0 ? ((totalRealTargets - fakeTargetRemoved) / (float)totalClicks) * 100f : 0f;


        // 다이아몬드 분포로 랭크 계산
        string rank = CalculateRank(accuracy, elapsedTime, obstacleHitCount);
        accuracyText.text = $"정확도: {accuracy:F2}%\n랭크: {rank}";

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
        obstacleHitCountText.text = $"Hits : {obstacleHitCount}";  // 장애물 충돌 횟수 UI 갱신
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
