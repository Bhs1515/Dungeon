using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    [SerializeField]
    Player player;
    public void EndAttack()
    {
        player.AttackEnd();
    }

}
