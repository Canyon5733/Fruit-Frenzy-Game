using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator anim;

    [HideInInspector]
    public bool isDefeated;

    public float waitToDestroy;
    void Start()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (isDefeated == true)
        {
            waitToDestroy -= Time.deltaTime;

            if (waitToDestroy <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isDefeated == false)
            {
                PlayerHealthController.instance.DamagePlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(9);
            FindFirstObjectByType<PlayerController>().Jump();
            anim.SetTrigger("defeated");
            isDefeated = true;
        }
    }
}
