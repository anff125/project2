using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public event EventHandler OnBlockSuccess;

    [SerializeField] private LayerMask playerLayerMask;
    private void OnTriggerEnter(Collider collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null) return;
        //if is razor leaf, destroy it
        OnBlockSuccess?.Invoke(this, EventArgs.Empty);
        bullet.SetShooterLayerMask(playerLayerMask);
        bullet.SetTextureForPlayer();
        if (bullet is BulletRazorLeaf razorLeafBullet)
        {
            razorLeafBullet.SetTarget(razorLeafBullet.GetShooter().transform);
            razorLeafBullet.SetDamage(5);
        }
        else
        {
            bullet.SetBulletProperty(-bullet.MovingDirection, 10, 5f, 5);
        }
    }


}