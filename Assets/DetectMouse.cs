using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock.LowLevel;

public class DetectMouse : MonoBehaviour
{
    private Vector2 mousePosition;
    [SerializeField] float rayRange = 5;
    [SerializeField] LayerMask layer;
    private bool clicked;
    private Vector3 targetPosition;
    [SerializeField] private float speed = 2;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, layer))
        {
            if (clicked)
            {
                targetPosition = hit.point;
                transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
        clicked = false;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void OnMousePosition(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }

    void OnAttack()
    {
        clicked = true;
    }

    private void OnDrawGizmos()
    {
        return;
        Gizmos.color = Color.yellow;
        Ray ray = new Ray(transform.position, transform.forward);


        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayRange, layer))
        {
            Gizmos.DrawSphere(hit.point, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hit.point, hit.normal);
        }
        Gizmos.DrawRay(transform.position, transform.forward * rayRange);
    }
}
