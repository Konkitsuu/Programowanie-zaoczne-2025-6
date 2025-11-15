using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    public bool IsKnockedDown;
    public float angle;
    [SerializeField] private float knockDownAngle = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        angle = Vector3.Angle(Vector3.up, transform.up);
        //IsKnockedDown = angle > knockDownAngle; Inny sposób
        if (angle > knockDownAngle)
        {
            IsKnockedDown = true;
        }
        else
        {
            IsKnockedDown = false;
        }
    }
}
