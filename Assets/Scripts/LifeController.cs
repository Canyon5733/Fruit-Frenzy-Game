using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public static LifeController instance;

    private void Awake()
    {
        instance = this;
    }

    private PlayerController thePlayer;

    public float respawnDelay = 2f;

    public int currentLives = 3;

    public GameObject deathEffect, respawnEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();

        currentLives = InfoTracker.instance.currentLives;

        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        thePlayer.gameObject.SetActive(false);
        thePlayer.theRB.linearVelocity = Vector2.zero;
        currentLives--;
        
        // Kiểm tra checkpoint có bị chìm trong lava không
        bool checkpointSubmerged = false;
        if (RisingLava.instance != null)
        {
            checkpointSubmerged = RisingLava.instance.IsCheckpointSubmerged();
        }
        
        if (currentLives > 0 && !checkpointSubmerged)
        {
            // Còn mạng VÀ checkpoint chưa bị chìm → Respawn
            StartCoroutine(RespawnCo());
        }
        else
        {
            // Hết mạng HOẶC checkpoint bị chìm → Game Over
            currentLives = 0;
            StartCoroutine(GameOverCo());
        }
        UpdateDisplay();
        Instantiate(deathEffect, thePlayer.transform.position, deathEffect.transform.rotation);
        AudioManager.instance.PlaySFX(4);
    }

    public IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(respawnDelay);
        thePlayer.transform.position = FindFirstObjectByType<CheckpointManager>().respawnPosition;
        PlayerHealthController.instance.AddHealth(PlayerHealthController.instance.maxHealth);
        thePlayer.gameObject.SetActive(true);
        Instantiate(respawnEffect, thePlayer.transform.position, Quaternion.identity);
    }

    public IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (UIController.instance != null)
        {
            UIController.instance.ShowGameOver();
        }
    }

    public void AddLife()
    {
        currentLives++;
        UpdateDisplay();
        AudioManager.instance.PlaySFX(3);
    }

    public void UpdateDisplay()
    {
        if (UIController.instance != null)
        {
            UIController.instance.UpdateLivesDisplay(currentLives);
        }
    }
}
