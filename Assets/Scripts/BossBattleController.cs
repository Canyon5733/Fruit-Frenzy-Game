using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    private bool bossActive;

    public GameObject blockers;

    public Transform camPoint;
    private CameraController camController;
    public float cameraMoveSpeed;

    public Transform theBoss;
    public float bossGrowSpeed = 2f;

    public Transform projectileLauncher;
    public float launcherGrowSpeed = 2f;

    public float launcherRotateSpeed = 90f;
    private float launcherRotation;

    public GameObject projectileToFire;
    public Transform[] projectilePoints;

    public float waitToStartShooting, timeBetweenShots;
    private float shootStartCounter, shotCounter;
    private int currenShot;

    public Animator bossAnim;
    private bool isWeak;

    public Transform[] bossMovePoints;
    private int currentMovePoint;
    public float bossMoveSpeed;

    private int currentPhase;

    public GameObject deathEffect;

    public float weakDuration = 2f;  
    private float weakCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camController = FindFirstObjectByType<CameraController>();

        shootStartCounter = waitToStartShooting;

        blockers.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossActive == true)
        {
            camController.transform.position = Vector3.MoveTowards(camController.transform.position,
                camPoint.position,
                cameraMoveSpeed * Time.deltaTime);

            if (theBoss.localScale != Vector3.one)
            {
                theBoss.localScale = Vector3.MoveTowards(
                    theBoss.localScale,
                    Vector3.one,
                    bossGrowSpeed * Time.deltaTime);
            }

            if (projectileLauncher.transform.localScale != Vector3.one)
            {
                projectileLauncher.localScale = Vector3.MoveTowards(
                    projectileLauncher.localScale,
                    Vector3.one,
                    bossGrowSpeed * Time.deltaTime);
            }

            launcherRotation += launcherRotateSpeed * Time.deltaTime;
            if (launcherRotation > 360)
            {
                launcherRotation -= 360;
            }
            projectileLauncher.transform.localRotation = Quaternion.Euler(0f, 0f, launcherRotation);

            // Start Shooting
            if (shootStartCounter > 0f)
            {
                shootStartCounter -= Time.deltaTime;
                if (shootStartCounter <= 0f)
                {
                    shotCounter = timeBetweenShots;
                    FireShot();
                }    
            }    

            if (shotCounter > 0f)
            {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0f)
                {
                    shotCounter = timeBetweenShots;
                    FireShot();
                }    
            }    

            if (isWeak == false)
            {
                theBoss.transform.position = Vector3.MoveTowards(
                    theBoss.transform.position,
                    bossMovePoints[currentMovePoint].position,
                    bossMoveSpeed * Time.deltaTime);

                if (theBoss.transform.position == bossMovePoints[currentMovePoint].position)
                {
                    currentMovePoint++;

                    if (currentMovePoint >= bossMovePoints.Length)
                    {
                        currentMovePoint = 0;
                    }
                }
            }

            // Handle weak state timer
            if (isWeak)
            {
                weakCounter -= Time.deltaTime;

                if (weakCounter <= 0f)
                {
                    isWeak = false;
                    bossAnim.SetTrigger("recover");

                    shotCounter = timeBetweenShots;
                    shootStartCounter = 0f;

                    foreach (Transform point in projectilePoints)
                    {
                        point.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void ActiveBattle()
    {
        bossActive = true;

        blockers.SetActive(true);

        camController.enabled = false;

        AudioManager.instance.PlayBossMusic();
    }

    public void FireShot()
    {
        Instantiate(projectileToFire, projectilePoints[currenShot].position, projectilePoints[currenShot].rotation);

        projectilePoints[currenShot].gameObject.SetActive(false);

        currenShot++;

        if (currenShot >= projectilePoints.Length)
        {
            shotCounter = 0f;
            MakeWeak();
            currenShot = 0;
        }

        AudioManager.instance.PlaySFX(12);
    }    

    void MakeWeak()
    {
        bossAnim.SetTrigger("isWeak");
        isWeak = true;

        weakCounter = weakDuration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isWeak == false)
            {
                PlayerHealthController.instance.DamagePlayer();
            }
            else
            {
                if (collision.transform.position.y > theBoss.position.y)
                {
                    bossAnim.SetTrigger("hit");
                    AudioManager.instance.PlaySFX(11);
                    FindFirstObjectByType<PlayerController>().Jump();

                    MoveToNextPhase();
                }
            }
        }
    }

    void MoveToNextPhase()
    {
        currentPhase++;

        if (currentPhase < 3)
        {
            isWeak = false;

            waitToStartShooting *= .5f;
            timeBetweenShots *= .75f;
            bossMoveSpeed *= 1.5f;

            shootStartCounter = waitToStartShooting;

            projectileLauncher.localScale = Vector3.zero;

            foreach(Transform point in projectilePoints)
            {
                point.gameObject.SetActive(true);
            }

            currenShot = 0;
            AudioManager.instance.PlaySFX(13);
        }
        else
        {
            gameObject.SetActive(false);
            blockers.SetActive(false);

            camController.enabled = true;

            Instantiate(deathEffect, theBoss.position, Quaternion.identity);

            AudioManager.instance.PlaySFX(14);
            AudioManager.instance.PlayLevelMusic(FindFirstObjectByType<LevelMusicPlayer>().trackToPlay);
        }
    }
}
