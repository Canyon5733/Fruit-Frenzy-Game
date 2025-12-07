using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthToAdd;
    public GameObject pickupEffect;

    public bool giveFullHealth;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
            {
                if (giveFullHealth)
                {
                    PlayerHealthController.instance.AddHealth(PlayerHealthController.instance.maxHealth);
                }
                else
                {
                    PlayerHealthController.instance.AddHealth(healthToAdd);
                }
                AudioManager.instance.PlaySFX(10);
                Destroy(gameObject);
                Instantiate(pickupEffect, transform.position, transform.rotation);
            }
        }
    }
}
