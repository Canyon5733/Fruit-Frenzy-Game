using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 8f;
    private Vector3 direction;

    public float lifetime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = (PlayerHealthController.instance.transform.position - transform.position).normalized;

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * (Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController.instance.DamagePlayer();

            Destroy(gameObject);
        }    
    }
}
