using UnityEngine;

public class LazerController : MonoBehaviour
{
    public float speed = 10f; // Merminin hýzý
    public float lifeTime = 3f; // Kaç saniye yaþayacak?

    void Start()
    {
        // Doðduktan 3 saniye sonra kendini yok et (Sahne çöplüðe dönmesin)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mermiyi yukarý doðru uçur
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}