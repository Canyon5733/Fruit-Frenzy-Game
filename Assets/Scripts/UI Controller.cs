using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }

    public Image[] HeartIcons;
    public Sprite heartFull, heartEmpty;
    public TMP_Text livesText;
    public TMP_Text fruitsText;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public string mainMenuScene;

    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadingToBlack, fadingFromBlack;
    void Start()
    {
        FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
        if (fadingFromBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r,
                fadeScreen.color.g,
                fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime)
                );
        }    

        if (fadingToBlack)
        {
            fadeScreen.color = new Color(
                fadeScreen.color.r,
                fadeScreen.color.g,
                fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime)
                );
        }    
    }

    public void UpdateHealthDisplay(int health, int maxHealth)
    {
        for (int i = 0; i < HeartIcons.Length; i++)
        {
            HeartIcons[i].enabled = true;

            if (health > i)
            {
                HeartIcons[i].sprite = heartFull;
            }
            else
            {
                HeartIcons[i].sprite = heartEmpty;

                if (maxHealth <= i)
                {
                    HeartIcons[i].enabled = false;
                }    
            }    
        }    
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        livesText.text = currentLives.ToString();
    }

    public void UpdateFruitsDisplay(int currentFruits)
    {
        fruitsText.text = currentFruits.ToString();
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void PauseUnPause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.instance.PlaySFX(5);
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            AudioManager.instance.PlaySFX(6);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void FadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
    }    

    public void FadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }    
}
