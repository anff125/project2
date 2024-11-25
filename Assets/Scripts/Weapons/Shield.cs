using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public event EventHandler OnBlockSuccess;

    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask interactionBlockLayerMask;
    private void OnTriggerEnter(Collider collision)
    {
        //check if is not in interactionBlockLayerMask
        if ((interactionBlockLayerMask.value & 1 << collision.gameObject.layer) == 0)
        {
            Debug.Log("Interaction block: " + collision.name);
            return;
        }
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null)
        {
            //push object with rigidbody away from player
            Debug.Log("Pushing object away: " + collision.name);
            Rigidbody rb = collision.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = collision.transform.position - transform.position;
                rb.AddForce(direction.normalized * 10, ForceMode.Impulse);
            }

            return;
        }
        if (bullet.shooterLayerMask == playerLayerMask)
        {
            Debug.Log("Bullet from player");
            return;
        }
        OnBlockSuccess?.Invoke(this, EventArgs.Empty);
        bullet.SetShooterLayerMask(playerLayerMask);
        bullet.SetTextureForPlayer();
        if (bullet is BulletRazorLeaf razorLeafBullet)
        {
            var shooter = razorLeafBullet.GetShooter();
            if (shooter != null)
            {
                razorLeafBullet.SetTarget(shooter.transform);
                razorLeafBullet.SetDamage(5);
            }
        }
        else
        {
            if (bullet.canBeReflected)
                bullet.SetBulletProperty(-bullet.MovingDirection, 10, 5f, 5);
        }
    }


}