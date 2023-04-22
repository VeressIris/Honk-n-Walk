using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool gameOver = false;
    
    [Header("Obstacles")]
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private float spawnCooldown = 3f;
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject[] airObstacles;
    [SerializeField] private float airSpawnCooldown = 6.5f;
    [SerializeField] private Transform airSpawner;

    private float score = 0f;
    
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;
    private bool paused = false;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject shootButton;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private GameObject[] hearts = new GameObject[3];
    [SerializeField] private GameObject healthBar;

    [Header("Difficulty")]
    [SerializeField] private MoveCamera camScript;
    [SerializeField] private AnimationCurve difficultyCurve;

    void Start()
    {
        SetUIElements();

        StartCoroutine(Spawn(obstacles, spawner, spawnCooldown, 0)); //start spawning ground osbtacles
        StartCoroutine(Spawn(airObstacles, airSpawner, airSpawnCooldown, 2)); //start spawning air osbtacles
    }

    void Update()
    {
        if (!gameOver)
        {
            UpdateScore();
        }
        else
        {
            ShowGameOverScreen();
        }

        //increase difficulty
        camScript.speed = difficultyCurve.Evaluate((int)score);
    }

    private IEnumerator Spawn(GameObject[] obstacles, Transform spawner, float spawnCooldown, int initCooldown)
    {
        while (!gameOver)
        {
            int i = Random.Range(0, obstacles.Length); //pick random obstacle from obstacle list

            yield return new WaitForSeconds(initCooldown);

            Instantiate(obstacles[i], spawner.position, spawner.rotation); //spawn obstacle at position of spawner

            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    private void UpdateScore()
    {
        score += Time.deltaTime * 10;
        scoreText.text = score.ToString("0");
    }

    public void PauseResume()
    {
        if (!paused)
        {
            Time.timeScale = 0;

            pauseButton.SetActive(false);
            pauseScreen.SetActive(true);
            shootButton.SetActive(false);
            healthBar.SetActive(false);

            scoreText.gameObject.SetActive(false);
        }
        if (paused)
        {
            Time.timeScale = 1;

            pauseButton.SetActive(true);
            pauseScreen.SetActive(false);
            shootButton.SetActive(true);
            healthBar.SetActive(false);

            scoreText.gameObject.SetActive(true);
        }

        paused = !paused;
    }

    private void SetUIElements()
    {
        finalScoreText.text = ""; //basically hides final score text
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        shootButton.SetActive(true);
        scoreText.gameObject.SetActive(true);
        pauseButton.SetActive(true);
        healthBar.SetActive(true);
    }

    private void ShowGameOverScreen()
    {
        finalScoreText.text = "Score: " + scoreText.text; //change final score text
        scoreText.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        shootButton.SetActive(false);
        pauseButton.SetActive(false);
        healthBar.SetActive(false);
    }

    public void HideHearts(int index)
    {
        if (index >= 0)
        {
            hearts[index].gameObject.SetActive(false);
        }
    }
}
