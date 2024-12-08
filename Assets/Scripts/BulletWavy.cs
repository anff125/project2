using UnityEngine;

public class BulletWavy : Bullet
{
    private float _waveFrequency = 6f; // Frequency of the wave
    private float _waveAmplitude = 0.04f; // Amplitude of the wave
    private float _waveOffset; // Offset to track wave progression
    public void SetBulletWavyProperty(Vector3 bulletDirection, float speed = 10, float destroyTime = 0.5f, int damage = 25)
    {
        SetSpeed(speed);
        SetDirection(bulletDirection);
        SetDestroyTime(destroyTime);
        SetDamage(damage);
    }
    protected override void Update()
    {
        // Apply forward movement in the XZ plane toward the player
        Vector3 movement = new Vector3(_movingDirection.x, 0, _movingDirection.z) * (Time.deltaTime * 5);
        transform.position += movement;

        // Update wave offset
        _waveOffset += Time.deltaTime * _waveFrequency;
        float waveMovement = Mathf.Sin(_waveOffset) * _waveAmplitude;

        // Add wave movement perpendicular to the bullet's direction in the XZ plane
        Vector3 waveDirection = Vector3.Cross(_movingDirection, Vector3.up).normalized; // Use Vector3.up for the Y-axis to stay in the XZ plane
        transform.position += waveDirection * waveMovement;
    }
    

}
