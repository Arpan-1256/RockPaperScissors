using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RPSManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI Result;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BotScoreText;
    public TextMeshProUGUI CountdownText;
    public Image Choice;
    public Image YourChoice;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject congratsPanel;

    [Header("Game Choices")]
    public string[] Choices = { "Rock", "Paper", "Scissor" };
    public Sprite Rock, Paper, Scissor;

    private int score = 0;
    private int botScore = 0;
    private bool gameIsOver = false;
    private bool inputLocked = false;
    private float countdown = 3f;
    private bool countdownRunning = false;

    private void Awake()
    {
        StartCountdown();
    }

    private void Update()
    {
        if (gameIsOver) return;
        if (!countdownRunning)
        {
            CountdownText.text = "";
            return;
        }

        countdown -= Time.deltaTime;

        if (countdown <= 0f)
        {
            countdown = 0f;
            CountdownText.text = "";
            countdownRunning = false;
            if (!inputLocked)
            {
                inputLocked = true;
                StartCoroutine(AutoLoseRound());
            }
        }
        else
        {
            CountdownText.text = Mathf.Ceil(countdown).ToString();
        }
    }

    public void Play(string myChoice)
    {
        if (gameIsOver || inputLocked) return;

        inputLocked = true;
        countdownRunning = false;
        CountdownText.text = "";

        UpdateChoiceImage(YourChoice, myChoice);
        YourChoice.enabled = true;

        Choice.enabled = false;
        Result.text = "";

        StartCoroutine(HandleRound(myChoice));
    }

    public void StartCountdown()
    {
        countdown = 3f;
        countdownRunning = true;
        inputLocked = false;
        Choice.enabled = false;
        YourChoice.enabled = false;
        Result.text = "";
    }

    private System.Collections.IEnumerator HandleRound(string myChoice)
    {
        yield return new WaitForSeconds(1f);

        string randomChoice = Choices[Random.Range(0, Choices.Length)];
        UpdateChoiceImage(Choice, randomChoice);
        Choice.enabled = true;

        if (myChoice == randomChoice)
        {
            Result.text = "It's a tie!";
        }
        else if (IsPlayerWinner(myChoice, randomChoice))
        {
            Result.text = "You win!";
            score++;
        }
        else
        {
            Result.text = "You lose!";
            botScore++;
        }

        UpdateScoreUI();
        CheckGameOver();

        yield return new WaitForSeconds(1.2f);

        Result.text = "";
        YourChoice.enabled = false;
        Choice.enabled = false;
        inputLocked = false;
        StartCountdown();
    }

    private System.Collections.IEnumerator AutoLoseRound()
    {
        yield return new WaitForSeconds(0.5f);

        string randomChoice = Choices[Random.Range(0, Choices.Length)];
        UpdateChoiceImage(Choice, randomChoice);
        Choice.enabled = true;

        Result.text = "You lost!";
        botScore++;

        UpdateScoreUI();
        CheckGameOver();

        yield return new WaitForSeconds(1.2f);

        Result.text = "";
        Choice.enabled = false;
        YourChoice.enabled = false;
        inputLocked = false;
        StartCountdown();
    }

    private void CheckGameOver()
    {
        if (score >= 5)
        {
            congratsPanel.SetActive(true);
            gameIsOver = true;
            Time.timeScale = 0f;
        }
        else if (botScore >= 5)
        {
            gameOverPanel.SetActive(true);
            gameIsOver = true;
            Time.timeScale = 0f;
        }
    }

    private void UpdateChoiceImage(Image imageComponent, string choice)
    {
        switch (choice)
        {
            case "Rock": imageComponent.sprite = Rock; break;
            case "Paper": imageComponent.sprite = Paper; break;
            case "Scissor": imageComponent.sprite = Scissor; break;
        }
    }

    private bool IsPlayerWinner(string player, string computer)
    {
        return (player == "Rock" && computer == "Scissor") ||
               (player == "Paper" && computer == "Rock") ||
               (player == "Scissor" && computer == "Paper");
    }

    private void UpdateScoreUI()
    {
        ScoreText.text = $"{score}";
        BotScoreText.text = $"{botScore}";
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

