using UnityEngine;

public class BouncePlayer : MonoBehaviour
{
    public float bounceAmount;

    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("bounce");

            collision.GetComponent<PlayerController>().BouncePlayer(bounceAmount);
        }    
    }
}
