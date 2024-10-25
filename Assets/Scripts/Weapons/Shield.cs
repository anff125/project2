using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //make enemy bullet bounce back when it hits the shield and change shooterLayerMask to playerLayerMask
    [SerializeField] private LayerMask playerLayerMask;
    private void OnTriggerEnter(Collider collision)
    {
        //make the bullet bounce back
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet == null) return;
        //set bullet Texture to the player texture
        bullet.SetTextureForPlayer();
        //set bullet direction to the shield forward direction
        bullet.SetBulletProperty(-bullet.MovingDirection, 10, 5f, 10);
        bullet.SetShooterLayerMask(playerLayerMask);
    }
}