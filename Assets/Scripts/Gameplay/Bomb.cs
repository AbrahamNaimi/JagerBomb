using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject[] explosionPrefabs;  // Assign explosion, light, smoke prefabs here
    public float explosionForce, effectiveRadius;
    public float explosionScale = 1f;

    // Trigger
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        foreach (GameObject prefab in explosionPrefabs)
        {
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab, transform.position, transform.rotation);
                instance.transform.localScale *= explosionScale;
                Destroy(instance, 10);
            }
        }

        KnockBack();
        Destroy(gameObject);
        GameSceneManager.Instance.Explode();
    }

    void KnockBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectiveRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rigidbody = nearby.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, effectiveRadius);
            }
        }
    }
}
