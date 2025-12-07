using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public static CollectiblesManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int collectibleCount;
    public int extraLifeThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectibleCount = InfoTracker.instance.currentFruits;

        if (UIController.instance != null)
        {
            UIController.instance.UpdateFruitsDisplay(collectibleCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCollectible(int amount)
    {
        collectibleCount += amount;

        if (collectibleCount >= extraLifeThreshold )
        {
            collectibleCount -= extraLifeThreshold;
            if (LifeController.instance != null)
            {
                LifeController.instance.AddLife();
            }
        }
        UpdateDisplay(collectibleCount);
    }

    public void UpdateDisplay(int currentFruits)
    {
        if (UIController.instance != null)
        {
            UIController.instance.UpdateFruitsDisplay(currentFruits);
        }
    }
}
