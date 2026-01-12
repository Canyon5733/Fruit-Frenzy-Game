using UnityEngine;

public class PlantShooter : MonoBehaviour
{
    public float detectionRange = 5f;

    public float startShootDelay = 0.8f;
    private float delayCounter;

    public float fireRate = 1.5f;
    private float fireCounter;

    public GameObject plantBullet;
    public Transform firePoint;

    private Transform player;
    private Animator anim;

    private bool playerInRange;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Player vào vùng phát hiện
        if (distance <= detectionRange)
        {
            if (!playerInRange)
            {
                // Player vừa mới bước vào
                playerInRange = true;
                delayCounter = startShootDelay;
            }

            anim.SetBool("hasPlayer", true);

            // Đang đếm delay trước khi bắn
            if (delayCounter > 0)
            {
                delayCounter -= Time.deltaTime;
                return; // chưa bắn vội
            }

            // Sau delay thì bắn theo fireRate
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                fireCounter = fireRate;
                Fire();
            }
        }
        else
        {
            // Player ra khỏi vùng
            playerInRange = false;
            anim.SetBool("hasPlayer", false);
            fireCounter = 0;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(plantBullet, firePoint.position, Quaternion.identity);

        // bắn từ phải sang trái
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f, 0f);
        AudioManager.instance.PlaySFX(15);

    }
}
