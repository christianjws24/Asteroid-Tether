using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPoint;
    public float bulletSpeed;
    public float bulletLifeTime;

    public InputActionReference attack;

    public float fireRate;
    private float nextFireTime;
    private bool isHoldingAttack;

    private void OnEnable()
    {
        if (attack != null && attack.action != null)
        {
            attack.action.started += OnAttackStarted;
            attack.action.canceled += OnAttackCanceled;

            attack.action.actionMap.Enable();
            attack.action.Enable();
        }
    }

    private void Update()
    {
        // Si está dejando presionado el botón y ya pasó el tiempo de cooldown
        if (isHoldingAttack && Time.time >= nextFireTime)
        {
            Shoot();
            // Calcula cuándo será el próximo disparo permitido
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnDisable()
    {
        if (attack != null && attack.action != null)
        {
            attack.action.started -= OnAttackStarted;
            attack.action.canceled -= OnAttackCanceled;

            attack.action.Disable();
        }
    }

    private void OnAttackStarted(InputAction.CallbackContext obj)
    {
        if (GameStateManager.IsGameOver) return;
        isHoldingAttack = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext obj)
    {
        isHoldingAttack = false;
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bulletPoint.up * bulletSpeed;
        }

        Destroy(newBullet, bulletLifeTime);
    }

}
