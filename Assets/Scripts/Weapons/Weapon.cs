using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform bulletPrefab;
    [SerializeField] protected Transform VisualSupport;

    public int index;
    protected float _mainAttackCooldownTimer = 0f; // Cooldown timer
    protected bool CanShoot => _mainAttackCooldownTimer <= 0;
    //create two virtual for main attack and secondary attack
    public virtual void MainAttack()
    {
        //Debug.Log("Main Attack" + gameObject.name);
    }
    public virtual void SecondaryAttack()
    {
        // Debug.Log("Secondary Attack"+ gameObject.name);
    }
    public virtual void StartSecondaryAttack() { }
    public virtual void DrawVisualSupport() { }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void ShowVisualSupport()
    {
        VisualSupport.gameObject.SetActive(true);
    }
    public void HideVisualSupport()
    {
        VisualSupport.gameObject.SetActive(false);
    }
    public virtual void Update()
    {
        if (_mainAttackCooldownTimer > 0)
        {
            _mainAttackCooldownTimer -= Time.deltaTime;
        }
    }
    protected void SetShootCooldown(float duration)
    {
        _mainAttackCooldownTimer = duration;
    }
    protected Vector3 GetDirection()
    {
        //calculate direction from firepoint to mouse position
        Vector3 firePointPosition = firePoint.position;
        System.Diagnostics.Debug.Assert(Camera.main != null, "Camera.main != null");
        var mousePosition = GetCursorPointOnGround();
        Vector3 direction = (mousePosition - firePointPosition).normalized;
        direction.y = 0;
        return direction;
    }

    protected Vector3 GetCursorPointOnGround()
    {
        System.Diagnostics.Debug.Assert(Camera.main != null, "Camera.main != null");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //return point of ray hit layer ground
        if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
        {
            Vector3 mousePosition = hit.point;
            return mousePosition;
        }
        return Vector3.zero;
    }
}