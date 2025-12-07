using UnityEngine;

public class CollectiblePickup : MonoBehaviour
{
    public int amount = 1;

    //public GameObject pickupEffect;

    private Animator anim;
    private bool isCollected = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("isCollected");
            if (CollectiblesManager.instance != null)
            {
                CollectiblesManager.instance.GetCollectible(amount);
                Destroy(gameObject, 0.5f);
                AudioManager.instance.PlaySFX(2);
                //Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
