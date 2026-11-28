using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Start Screen")]
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject playButton;

    [Header("Score")]
    [SerializeField] private TMP_Text score;

    [Header("Game ready section")]
    [SerializeField] private GameObject gameReadyPannel;

    [Header("Game Over Section")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverScore;
    [SerializeField] private TMP_Text gameOverBestScore;
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PipeSpawner pipeSpawner;
    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeAmount = 0.05f;

    private const string Best_Score_Key = "Best Score";

    private GameState gameState = GameState.Home;
    public GameState GameState => gameState;

    private Camera mainCamera;

    private int currentScore;
    private int CurrentScore

    {
        get => currentScore;
        set
        {
            currentScore = value;
            score.text = currentScore.ToString();
        }

    }
    public int BestScore
    {
        get => PlayerPrefs.GetInt(Best_Score_Key, 0);
        set
        {
            PlayerPrefs.SetInt(Best_Score_Key, value);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        gameState = GameState.Home;
        logo.SetActive(true);
        playButton.SetActive(true);

        gameOverPanel.SetActive(false);
        gameReadyPannel.SetActive(false);
        score.gameObject.SetActive(false);

    }
    public void PlayButton()
    {
        gameState = GameState.GetReady;
        CurrentScore = 0;

        logo.SetActive(false);
        playButton.SetActive(false);

        gameOverPanel.SetActive(false);
        gameReadyPannel.SetActive(true);
        score.gameObject.SetActive(true);

        ResetGame();

    }
    private void ResetGame()
    {
        playerController.ResetPlayer();
        pipeSpawner.ResetSpawners();

    } 
    public void GamePlay()
    {
        gameState = GameState.Playing;
        gameReadyPannel.SetActive(false);
    }

    public void GameOver()
    {

        gameState = GameState.GameOver;
        StartCoroutine(ShakeCamera());
        gameOverPanel.SetActive(true);
        score.gameObject.SetActive(false);
        playButton.SetActive(true);

        gameOverScore.text = currentScore.ToString();
        if(CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
        gameOverScore.text = BestScore.ToString();

    }
    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float x = Random.Range(-shakeAmount,shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount); 
            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);

            yield return null;
        }
        mainCamera.transform.position = originalPos;


    }

    public void AddScore()
    {
        if (gameState != GameState.Playing) return;

        CurrentScore++;
        AudioManager.Instance.Score();

    }

}
