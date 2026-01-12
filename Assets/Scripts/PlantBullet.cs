using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController.instance.DamagePlayer();
            Destroy(gameObject);
        }
    }
}
