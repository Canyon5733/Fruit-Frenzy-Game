using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    public int startingLives = 3, startingFruits = 0;

    public GameObject continueButton;
    
    [Header("Level Select Panel")]
    public GameObject levelSelectPanel;
    
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();

        if (PlayerPrefs.HasKey("currentLevel"))
        {
            continueButton.SetActive(true);
        }
        
        // Ẩn Level Select panel lúc đầu
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }

    public void StartGame()
    {
        InfoTracker.instance.currentLives = startingLives;
        InfoTracker.instance.currentFruits = startingFruits;

        InfoTracker.instance.SaveInfo();
        SceneManager.LoadScene(firstLevel);
    }   
    
    public void QuitGame()
    {
        Application.Quit(); 
    }    

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("currentLevel"));
    }
    
    // ========== LEVEL SELECT ==========
    
    public void ShowLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }
    
    public void HideLevelSelect()
    {
        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }
    
    public void LoadLevel1()
    {
        StartNewGameAndLoadLevel("Level1");
    }
    
    public void LoadLevel2()
    {
        StartNewGameAndLoadLevel("Level2");
    }
    
    public void LoadLevel3()
    {
        StartNewGameAndLoadLevel("Level3");
    }
    
    public void LoadLevel4()
    {
        StartNewGameAndLoadLevel("Level4");
    }
    
    public void LoadBossLevel()
    {
        StartNewGameAndLoadLevel("BossLevel");
    }
    
    private void StartNewGameAndLoadLevel(string levelName)
    {
        InfoTracker.instance.currentLives = startingLives;
        InfoTracker.instance.currentFruits = startingFruits;
        InfoTracker.instance.SaveInfo();
        SceneManager.LoadScene(levelName);
    }
}
