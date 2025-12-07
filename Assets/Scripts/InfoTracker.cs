using UnityEngine;

public class InfoTracker : MonoBehaviour
{
    public static InfoTracker instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey("lives"))
            {
                currentLives = PlayerPrefs.GetInt("lives");
                currentFruits = PlayerPrefs.GetInt("fruits");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int currentLives, currentFruits;

    public void GetInfo()
    {
        if (LifeController.instance != null)
        {
            currentLives = LifeController.instance.currentLives;
        }

        if (CollectiblesManager.instance != null)
        {
            currentFruits = CollectiblesManager.instance.collectibleCount;
        }
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetInt("lives", currentLives);
        PlayerPrefs.SetInt("fruits", currentFruits);
    }
}
