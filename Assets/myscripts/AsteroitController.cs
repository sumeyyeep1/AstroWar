using UnityEngine;

public class AsteroitController : MonoBehaviour
{
    public float speed = 3f;

    // --- YENÝ: Patlama Sesi ---
    public AudioClip patlamaSesi;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. MERMÝ ÇARPARSA (Puan kazan ve yok et)
        if (other.gameObject.CompareTag("mermi"))
        {
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.PuanKazan(10);

            // --- YENÝ: Patlama Sesini Çal ---
            if (patlamaSesi != null)
            {
                // Obje yok olsa bile sesi o noktada çalar
                AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);
            }
            // --------------------------------

            Destroy(other.gameObject); // Mermiyi sil
            Destroy(gameObject);       // Taþý sil
        }

        // 2. GEMÝ ÇARPARSA (Sadece yok ol, sesi gemi çýkarýyor zaten)
        else if (other.gameObject.CompareTag("Player"))
        {
            // Buraya da istersen patlama sesi ekleyebilirsin ama
            // Gemi zaten "Çarpýþma Sesi" çýkaracaðý için gürültü olmasýn.
            Destroy(gameObject);
        }
    }
}