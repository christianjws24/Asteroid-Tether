using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputActionReference lookAction;

    private Vector2 mousePosition;
    [SerializeField] private float angleOffset = -90f;
    [SerializeField] private bool invertAngle = false;

    private void OnEnable()
    {
        if (lookAction != null && lookAction.action != null)
        {
            lookAction.action.performed += OnLookAction;
            lookAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (lookAction != null && lookAction.action != null)
        {
            lookAction.action.performed -= OnLookAction;
            lookAction.action.Disable();
        }
    }

    public void OnLook(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    private void OnLookAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            mousePosition = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        // CORRECCIÓN 1: Si el juego está pausado, no procesar la rotación de la nave
        if (Time.timeScale == 0f)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        Vector2 screenPos;

        // CORRECCIÓN 2: Eliminado el 'Input.mousePosition' clásico.
        // Si la acción está asignada, lee su valor; si no, lee directamente el hardware del Mouse
        if (lookAction != null && lookAction.action != null)
        {
            screenPos = lookAction.action.ReadValue<Vector2>();
        }
        else if (Mouse.current != null)
        {
            screenPos = Mouse.current.position.ReadValue();
        }
        else
        {
            return; // Evita continuar si no hay un mouse conectado
        }

        float zDistance = transform.position.z - mainCamera.transform.position.z;
        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, zDistance);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);

        Vector3 rotateDirection = (worldPosition - transform.position);
        rotateDirection.z = 0f;
        rotateDirection.Normalize();

        float angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
        if (invertAngle)
            angle = -angle;
        angle += angleOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
