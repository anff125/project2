using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : Weapon
{
    private Vector3 _gizmoCenter;
    private Vector3 _gizmoExtents;
    private Quaternion _gizmoRotation;
    private Vector3 landPoint;
    [SerializeField] private Transform VisualSupportPrefab;
    [SerializeField] private int _projectilePerShoot = 2;
    [SerializeField] private float _skillDuration = 5f;
    [SerializeField] private float _dagmagePerHit = 8f;
    [SerializeField] private float _damageInterval = 0.5f;
    [SerializeField] private float _dragSpeed = 2f;

    public override void MainAttack()
    {
        if (!CanShoot) return;
        SetShootCooldown(0.3f);
        Vector3 direction = GetDirection();
        float theta = 360f / _projectilePerShoot;

        for (int i = 0; i < _projectilePerShoot; i++)
        {
            Transform bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            //set bullet direction
            bullet.GetComponent<BulletSpiral>().SetBulletProperty(direction, 10, 5f, 10);
            bullet.GetComponent<BulletSpiral>().SetSpiralProperty(firePoint.position, (theta * i + firePoint.rotation.eulerAngles.z) % 360f,
                rotationSpeed: 360);
        }

    }

    public override void SecondaryAttack()
    {
        GameObject handler = new GameObject("SpiralSkill");
        SpiralSkillHandler spiralSkillHandler = handler.AddComponent<SpiralSkillHandler>();

        VisualSupport.gameObject.SetActive(false);
        spiralSkillHandler.Initialize(
            VisualSupportPrefab, landPoint, _dagmagePerHit, _damageInterval, _skillDuration, _dragSpeed, LayerMask.GetMask("Enemy"));
    }

    public override void StartSecondaryAttack()
    {
        VisualSupport.gameObject.SetActive(true);
    }

    public override void DrawVisualSupport()
    {
        landPoint = GetCursorPointOnGround();
        VisualSupport.transform.position = landPoint;
    }
}