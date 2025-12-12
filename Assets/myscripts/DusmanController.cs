using UnityEngine;

public class DusmanController : MonoBehaviour
{
    public float hiz = 3f;
    public GameObject mermiPrefab;
    public GameObject patlamaEfekti;

    // --- SES DEÐÝÞKENÝ ---
    public AudioClip patlamaSesi;
    // ---------------------

    public float atesSikligi = 2f;

    void Start()
    {
        InvokeRepeating("AtesEt", 1f, atesSikligi);
    }

    void Update()
    {
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    void AtesEt()
    {
        if (mermiPrefab != null)
        {
            Instantiate(mermiPrefab, transform.position, transform.rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("mermi"))
        {
            // Puan Ver
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.PuanKazan(20);

            // Efekt Yarat
            if (patlamaEfekti != null)
            {
                Instantiate(patlamaEfekti, transform.position, Quaternion.identity);
            }

            // --- SESÝ ÇAL (Patlama) ---
            // PlayClipAtPoint kullanýyoruz çünkü obje yok olsa bile ses devam etmeli
            if (patlamaSesi != null)
            {
                AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);
            }
            // --------------------------

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}