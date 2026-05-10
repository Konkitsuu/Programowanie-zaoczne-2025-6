using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdLancher : MonoBehaviour, BirsActions.ILancherActions
{
    [SerializeField]
    private float launchSpeed = 5;

    [SerializeField]
    private float minLaunchDelta = 0.1f;

    [SerializeField]
    private LayerMask quadLayer;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private List<Bird> birds;

    [SerializeField]
    private TrajectoryDrawer trajectoryDrawer;

    private BirsActions inputActions;
    private BirsActions.LancherActions lancherActions;
    private Vector3 mousePosition;
    private Vector3 mousePositionWorld;
    private Vector3 mouseDelta;
    private Vector3 startAimMousePosition;
    private BirdState state;
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
        if (state == BirdState.Aiming && mouseDelta.magnitude >= minLaunchDelta)
        {
            trajectoryDrawer.SetLineEnabled(true);
            Vector3 velocity = mouseDelta * launchSpeed;
            trajectoryDrawer.DrawTrajectory(startAimMousePosition, velocity);
        }
        else
        {
            trajectoryDrawer.SetLineEnabled(false);
        }

        if (state == BirdState.Shooting)
        {
            if (loadedBird != null && loadedBird.rigidbody != null)
            {
                if (loadedBird.rigidbody.linearVelocity.magnitude < 0.1f || loadedBird.transform.position.y < -20)
                {
                    //bird stopped
                    state = BirdState.Idle;
                    Destroy(loadedBird.gameObject);
                    LoadNextBird();
                }
            }
            else
            {
                state = BirdState.Idle;
                LoadNextBird();
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

        if (state == BirdState.Aiming)
        {
            loadedBird.rigidbody.position = startAimMousePosition - mouseDelta;
        }
    }

    public void OnMousePress(InputAction.CallbackContext context)
    {
        if (state == BirdState.Shooting && loadedBird != null)
        {
            loadedBird.Activate();
            return;
        }

        if (context.started && loadedBird != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.GetComponent<Bird>() == loadedBird)
                {
                    state = BirdState.Aiming;
                    trajectoryDrawer.SetLineEnabled(true);
                    startAimMousePosition = mousePositionWorld;
                }
            }
        }

        if (state == BirdState.Aiming && context.canceled)
        {
            if (mouseDelta.magnitude > minLaunchDelta)
            {
                loadedBird.rigidbody.isKinematic = false;
                loadedBird.rigidbody.linearVelocity = mouseDelta * launchSpeed;
                state = BirdState.Shooting;
            }
            else
            {
                loadedBird.rigidbody.position = transform.position;
                state = BirdState.Idle;
            }

            trajectoryDrawer.SetLineEnabled(false);
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
}
