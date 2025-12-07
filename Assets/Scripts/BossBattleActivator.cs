using UnityEngine;

public class BossBattleActivator : MonoBehaviour
{
    public BossBattleController theBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            theBoss.ActiveBattle();

            gameObject.SetActive(false);
        }
    }
}
