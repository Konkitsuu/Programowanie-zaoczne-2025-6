using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdLancher : MonoBehaviour, BirsActions.ILancherActions
{
    [SerializeField]
    private float launchSpeed = 5;

    [SerializeField]
    private int trajectoryPositions = 20;

    [SerializeField]
    private float trajectoryTimeStep = 0.1f;

    [SerializeField]
    private float minLaunchDelta = 0.1f;

    [SerializeField]
    private LineRenderer trajectoryLine;

    [SerializeField]
    private LayerMask quadLayer;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private List<Bird> birds;
    private BirsActions inputActions;
    private BirsActions.LancherActions lancherActions;
    private Vector3 mousePosition;
    private Vector3 mousePositionWorld;
    private Vector3 mouseDelta;
    private Vector3 startAimMousePosition;

    public bool isAiming;
    public bool isShooting;
    private Bird loadedBird;

    void Start()
    {
        inputActions = new BirsActions();
        lancherActions = inputActions.Lancher;
        lancherActions.AddCallbacks(this);
        lancherActions.Enable();
        LoadNextBird();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            DrawTrajectory();
        }
        if (isShooting)
        {
            if (loadedBird != null && loadedBird.rigidbody != null)
            {
                if (loadedBird.rigidbody.linearVelocity.magnitude < 0.1f)
                {
                    //bird stopped
                    isShooting = false;
                    Destroy(loadedBird.gameObject);
                    LoadNextBird();
                }
            }
        }
    }

    public void LoadNextBird()
    {
        if (birds.Count == 0)
        {
            print("Game over");
            gameOverScreen.SetActive(true);
            return;
        }

        Bird birdPrefab = birds[0];
        Bird spawnedBird = Instantiate(birdPrefab, transform.position, transform.rotation);
        loadedBird = spawnedBird;
        spawnedBird.rigidbody.isKinematic = true;
        spawnedBird.rigidbody.position = transform.position;
        birds.RemoveAt(0);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        mousePositionWorld = GetMousePositionWorld();
        mouseDelta = startAimMousePosition - mousePositionWorld;

        if (isAiming)
        {
            loadedBird.rigidbody.position = startAimMousePosition - mouseDelta;
        }
    }

    public void OnMousePress(InputAction.CallbackContext context)
    {
        if (isShooting)
        {
            return;
        }

        if (context.started && loadedBird != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<Bird>() == loadedBird)
                {
                    isAiming = true;
                    trajectoryLine.enabled = true;
                    startAimMousePosition = mousePositionWorld;
                }
            }
        }

        if (isAiming && context.canceled)
        {
            if (mouseDelta.magnitude > minLaunchDelta)
            {
                loadedBird.rigidbody.isKinematic = false;
                loadedBird.rigidbody.linearVelocity = mouseDelta * launchSpeed;
                isShooting = true;
            }
            else
            {
                loadedBird.rigidbody.position = transform.position;
            }
            isAiming = false;
            trajectoryLine.enabled = false;
        }
    }

    private Vector3 GetMousePositionWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, quadLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void DrawTrajectory()
    {
        if (mouseDelta.magnitude < minLaunchDelta)
        {
            trajectoryLine.enabled = false;
            return;
        }
        trajectoryLine.enabled = true;
        Vector3 velocity = mouseDelta * launchSpeed;

        trajectoryLine.positionCount = trajectoryPositions;
        for (int i = 0; i < trajectoryPositions; i++)
        {
            //pos(t) = startPos + v*t + gravity * t * t/2
            float t = trajectoryTimeStep * i;
            Vector3 position =
                loadedBird.rigidbody.position + velocity * t + Physics.gravity * t * t / 2;
            trajectoryLine.SetPosition(i, position);
        }
    }
}
