using UnityEngine;

public class SkeletonAnime : MonoBehaviour
{
    [SerializeField]
    Enemy enemy;

    public void Attack()
    {
        enemy.AttackEnd();
    }

    public void Hit()
    {
        enemy.HitEnd();
    }

    public void AttackAnimationEnd()
    {
        enemy.AttackAnimationEnd();
    }
}
