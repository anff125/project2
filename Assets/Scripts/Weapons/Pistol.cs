using UnityEngine;

public class Pistol : Weapon
{
    public override void MainAttack()
    {
        if (!CanShoot) return;
        SetShootCooldown(.5f);
        Vector3 direction = GetDirection();

        Transform bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        //set bullet direction
        bullet.GetComponent<Bullet>().SetBulletProperty(direction, 10, 3f, 10);
    }
    public override void SecondaryAttack()
    {
    }

}