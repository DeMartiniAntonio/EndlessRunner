using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isRestart = false;
    private bool isPaused = false;
    public static bool resetGameObjects = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject optionsPanel2;

    [Header("Timer & Score")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text highScoreText;

    private float currentTime = 0f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource SFXaudioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip menuMusic;

    [SerializeField] private List<Slider> musicSlider;
    [SerializeField] private List<Slider> sfxSlider;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (isRestart)
        {
            StartGame();
            isRestart = false;
        }
        else
        {
            ShowMainMenu();
        }

        ShowHighScore();


        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SfxVolume", 1f);

        audioSource.volume = musicVol;
        SFXaudioSource.volume = sfxVol;

        foreach (var slider in musicSlider)
        {
            if (slider != null)
            {
                slider.value = musicVol;
                slider.onValueChanged.AddListener(SetMusicVolume);
            }
        }

        foreach (var slider in sfxSlider)
        {
            if (slider != null)
            {
                slider.value = sfxVol;
                slider.onValueChanged.AddListener(SetSfxVolume);
            }
        }
    }

    public void SetMusicVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        foreach (var slider in musicSlider)
            if (slider != null && slider.value != value)
                slider.value = value;
    }

    public void SetSfxVolume(float value)
    {
        SFXaudioSource.volume = value;
        PlayerPrefs.SetFloat("SfxVolume", value);
        PlayerPrefs.Save();

        foreach (var slider in sfxSlider)
            if (slider != null && slider.value != value)
                slider.value = value;
    }

    public void SyncSliders()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SfxVolume", 1f);


    }

    private void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gamePanel.SetActive(false);
        gameOverScreen.SetActive(false);
        Time.timeScale = 0f;
        PlayMusic(menuMusic, true);
        resetGameObjects=true;
    }

    private void StartGame()
    {
        mainMenu.SetActive(false);
        gamePanel.SetActive(true);
        gameOverScreen.SetActive(false);
        Time.timeScale = 1f;
        currentTime = 0f;
        PlayMusic(backgroundMusic, true);
        resetGameObjects=false;
    }

    public void PlayGame()
    {
        StartGame();
    }

    public void RestartGame()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        float savedHighScore = PlayerPrefs.GetFloat("HighScore", 0f);

        if (currentTime > savedHighScore)
        {
            PlayerPrefs.SetFloat("HighScore", currentTime);
            PlayerPrefs.Save();
        }

        ShowHighScore();
        PlayMusic(gameOverMusic, false);
        gameOverScreen.SetActive(true);
        gamePanel.SetActive(false);
        Time.timeScale = 0f;
    }

    private void ShowHighScore()
    {
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        int minute = Mathf.FloorToInt(highScore / 60);
        int sekunde = Mathf.FloorToInt(highScore % 60);
        int stotinke = Mathf.FloorToInt((highScore - Mathf.Floor(highScore)) * 100);

        if (highScoreText != null)
            highScoreText.text = "HIGHSCORE: " + minute.ToString("00") + ":" + sekunde.ToString("00") + ":" + stotinke.ToString("00");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        optionsPanel.SetActive(true);
        mainMenu.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        optionsPanel2.SetActive(false);
    }

    public void OptionsButton2()
    {
        optionsPanel.SetActive(false);
        mainMenu.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        optionsPanel2.SetActive(true);
    }

    public void ControlButton()
    {
        optionsPanel.SetActive(false);
        mainMenu.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
        optionsPanel2.SetActive(false);
    }

    public void MainMenuButton()
    {
        optionsPanel.SetActive(false);
        mainMenu.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        optionsPanel2.SetActive(false);

    }

    public void PauseGame()
    {
        optionsPanel2.SetActive(false);
        mainMenu.SetActive(false);
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
        controlsPanel.SetActive(false);


        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        mainMenu.SetActive(false);
        optionsPanel.SetActive(false);
        gamePanel.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GameOverPanel() {
        Debug.Log("GameOverPanel called");
        gamePanel.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }



    private void Update()
    {
        if (Time.timeScale == 0f) return;

        currentTime += Time.deltaTime;

        int minute = Mathf.FloorToInt(currentTime / 60);
        int sekunde = Mathf.FloorToInt(currentTime % 60);
        int stotinke = Mathf.FloorToInt((currentTime - Mathf.Floor(currentTime)) * 100);

        if (timerText != null)
            timerText.text = minute.ToString("00") + ":" + sekunde.ToString("00") + ":" + stotinke.ToString("00");



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public float GetScore()
    {
        return currentTime;
    }
    // Add this method to fix CS0103: The name 'RestartGameObjects' does not exist in the current context

    private void RestartGameObjects()
    {
        // Implement logic to reset or restart game objects as needed.
        // If you have specific objects to reset, add that logic here.
        // For now, this is a placeholder to resolve the error.
    }

}