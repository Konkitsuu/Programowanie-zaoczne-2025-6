using UnityEngine;

public class GrowingBird : Bird
{
    private bool juzUrosl = false;

    public override void Activate()
    {
        if (juzUrosl) return;

        juzUrosl = true;


        transform.localScale = transform.localScale * 2f;

        if (rigidbody != null)
        {
            rigidbody.mass = rigidbody.mass * 4f;
        }

    }
}