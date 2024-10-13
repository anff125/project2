using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private Vector3 _gizmoCenter;
    private Vector3 _gizmoExtents;
    private Quaternion _gizmoRotation;

    public override void MainAttack()
    {
        if (!CanShoot) return;
        SetShootCooldown(0.1f);
        Vector3 direction = GetDirection();

        Transform bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //set bullet direction
        bullet.GetComponent<Bullet>().SetBulletProperty(direction, 15, 6f, 12);
    }

    public override void SecondaryAttack()
    {
        //move the player to endpoint of VisualSupport
        Player.Instance.Moveto(VisualSupport.position + VisualSupport.forward * VisualSupport.localScale.z);

        // Deal 100 damage to all enemies colliding with VisualSupport
        Collider[] hitColliders = Physics.OverlapBox(VisualSupport.position, VisualSupport.localScale / 2, VisualSupport.rotation);

        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(30);
            }
        }

        VisualSupport.gameObject.SetActive(false);
    }

    public override void StartSecondaryAttack()
    {
        VisualSupport.gameObject.SetActive(true);
    }

    public override void DrawVisualSupport()
    {
        Vector3 direction = GetDirection();
        VisualSupport.position = Player.Instance.transform.position + direction * VisualSupport.transform.localScale.z / 2;
        VisualSupport.rotation = Quaternion.LookRotation(direction);
    }



}