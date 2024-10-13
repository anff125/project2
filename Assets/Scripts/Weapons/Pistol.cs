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