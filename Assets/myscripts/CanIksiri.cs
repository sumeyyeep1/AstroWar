using UnityEngine;

public class CanIksiri : MonoBehaviour
{
    public float hiz = 3f;

    // --- SES DEÐÝÞKENÝ ---
    public AudioClip toplamaSesi;
    // ---------------------

    void Update()
    {
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null)
            {
                yonetici.CanKazan();
            }

            // --- SESÝ ÇAL (Toplama) ---
            if (toplamaSesi != null)
            {
                // Sesi 1.0 þiddetinde çal
                AudioSource.PlayClipAtPoint(toplamaSesi, transform.position, 1.0f);
            }
            // --------------------------

            Destroy(gameObject);
        }
    }
}