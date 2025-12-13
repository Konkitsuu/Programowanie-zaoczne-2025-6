using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // int - liczba ca³kowita (1, 214324, -32434)
    // float - liczba z przecinkiem 213.4545f, 23.0f
    // string - tekst "tekst" 
    // bool - zmienna logiczna true/false

    [SerializeField] private float speed = 2;
    [SerializeField] private float lookSpeed = 100;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float interactionRange = 10;
    public int health = 5;
    [SerializeField]
    private bool isAlive;
    //ctrl + r + r - replace wszêdzie
    [SerializeField] private bool isOnTheRight;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject hitParticles;
    [SerializeField] private ParticleSystem shootParticles;
    private float timer = 1;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float someFloat = 2;
        someFloat += health;
        string text1 = "tekst 1";
        string text2 = "tekst 2";
        Debug.Log(text1 + health);

        isAlive = !isAlive;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveInput * speed * Time.deltaTime;

        transform.Rotate(new Vector3(0, lookInput.x * lookSpeed * Time.deltaTime, 0));
        cameraTransform.transform.Rotate(new Vector3(-lookInput.y * lookSpeed * Time.deltaTime, 0, 0));
    }

    private void FixedUpdate()
    {
        Vector2 moveVector = moveInput * speed;
        rb.linearVelocity = transform.forward * moveVector.y + transform.right * moveVector.x;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void OnAttack()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        shootParticles.Emit(1);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            else
            {
                GameObject spawnedParticle = Instantiate(hitParticles, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(spawnedParticle, 2);
            }
            Debug.Log(hit.transform.gameObject.name);
        }
    }

    private void OnInteract()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit,interactionRange))
        {
            if(hit.collider.TryGetComponent(out Interactable interactable))
            {
                interactable.Interact();
            }
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            print("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void DoSomething()
    {

    }
}


