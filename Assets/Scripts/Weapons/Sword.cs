using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private Transform shield;
    public override void MainAttack()
    {
        if (!CanShoot) return;
        SetShootCooldown(.5f);
        Vector3 direction = GetDirection();
        // Check a sector in the direction and deal 30 damage
        Collider[] hitColliders = Physics.OverlapSphere(firePoint.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 toCollider = (hitCollider.transform.position - firePoint.position).normalized;
            if (Vector3.Dot(direction, toCollider) > 0.5f)
            {
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(30);
                }
            }
        }
    }

    public override void SecondaryAttack()
    {
        HideVisualSupport();
    }

    public override void StartSecondaryAttack()
    {
        ShowVisualSupport();
    }

    public override void DrawVisualSupport()
    {
        //make the visual support rotate around the player(use player as Origin) according to the mouse position
        Vector3 direction = GetDirection();
        Vector3 position = Player.Instance.transform.position + direction * .6f;
        VisualSupport.position = position;
        VisualSupport.rotation = Quaternion.LookRotation(direction);
    }

}