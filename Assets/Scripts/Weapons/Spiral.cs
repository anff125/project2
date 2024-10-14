using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : Weapon
{
    private Vector3 _gizmoCenter;
    private Vector3 _gizmoExtents;
    private Quaternion _gizmoRotation;
    private Vector3 landPoint;
    [SerializeField] private int _projectilePerShoot = 2;
    [SerializeField] private float _skillDuration = 5f;
    [SerializeField] private float _dagmagePerHit = 8f;
    [SerializeField] private float _damageInterval = 0.5f;
    [SerializeField] private float _dragSpeed = 2f;

    public override void MainAttack()
    {
        if (!CanShoot) return;
        SetShootCooldown(0.1f);
        Vector3 direction = GetDirection();
        float theta = 360f / _projectilePerShoot;
        
        for (int i=0; i<_projectilePerShoot; i++)
        {
            Transform bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            //set bullet direction
            bullet.GetComponent<BulletSpiral>().SetBulletProperty(direction, 15, 5f, 10);
            bullet.GetComponent<BulletSpiral>().SetSpiralProperty(firePoint.position, (theta * i + firePoint.rotation.eulerAngles.z) % 360f);
        }
        
    }

    public override void SecondaryAttack()
    {
        GameObject handler = new GameObject("SpiralSkill");
        SpiralSkillHandler spiralSkillHandler = handler.AddComponent<SpiralSkillHandler>();

        Destroy(VisualSupport.gameObject);
        spiralSkillHandler.Initialize(
            visualSupportPrefab, landPoint, _dagmagePerHit, _damageInterval, _skillDuration, _dragSpeed, LayerMask.GetMask("Enemy"));
    }

    public override void StartSecondaryAttack()
    {
        VisualSupport = Instantiate(visualSupportPrefab, GetCursorPointOnGround(), Quaternion.identity);
    }

    public override void DrawVisualSupport()
    {
        landPoint = GetCursorPointOnGround();
        VisualSupport.transform.position = landPoint;
    }
}