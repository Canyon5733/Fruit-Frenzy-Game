using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public string tutorial;

    public int startingLives = 3, startingFruits = 0;

    public GameObject continueButton;
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();

        if (PlayerPrefs.HasKey("currentLevel"))
        {
            continueButton.SetActive(true);
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
    public void Tutorial()
    {
        SceneManager.LoadScene(tutorial);
    }
}
