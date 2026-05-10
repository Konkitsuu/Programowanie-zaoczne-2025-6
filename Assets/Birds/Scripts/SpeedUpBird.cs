using UnityEngine;

public class SpeedUpBird : Bird
{
    public override void Activate()
    {
        rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * activationSpeed;
    }
}
