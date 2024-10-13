using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private float skillRange = 10;
    private Vector3 landPoint;
    public override void MainAttack()
    {
        Vector3 direction = GetDirection();
        //Shoot 5 bullets in a Circular sector with 120 degrees
        for (int i = 0; i < 5; i++)
        {
            Transform bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            // Set bullet direction
            Vector3 bulletDirection = Quaternion.Euler(0, -30 + i * 15, 0) * direction;
            bullet.GetComponent<Bullet>().SetBulletProperty(bulletDirection, 20, 0.2f, 7);
        }
    }

    public override void SecondaryAttack()
    {
        //move the player to the cursor position
        Vector3 targetPosition = landPoint;
        Player.Instance.Moveto(targetPosition);
        //Deal 10 damage to all enemies in 5 units radius
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, 5, LayerMask.GetMask("Enemy"));
        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<Enemy>().TakeDamage(50);
        }
        VisualSupport.gameObject.SetActive(false);

    }

    public override void StartSecondaryAttack()
    {
        VisualSupport.gameObject.SetActive(true);
    }

    public override void DrawVisualSupport()
    {
        // Ensure the position radius is less than 5 units from the player
        Vector3 cursorPointOnGround = GetCursorPointOnGround() - Player.Instance.transform.position;
        landPoint = cursorPointOnGround.magnitude > skillRange
            ? Player.Instance.transform.position + cursorPointOnGround.normalized * skillRange
            : GetCursorPointOnGround();

        VisualSupport.transform.position = landPoint;
    }
}