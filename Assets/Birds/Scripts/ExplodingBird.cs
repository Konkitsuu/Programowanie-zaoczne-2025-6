using UnityEngine;

public class ExplodingBird : Bird
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionStrength;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionDuration;

    public override void Activate()
    {
        GameObject explosition = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in collidersInRange)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionStrength, transform.position, explosionRadius);
            }
        }
        Destroy(explosition, explosionDuration);
        Destroy(gameObject);
    }
}
