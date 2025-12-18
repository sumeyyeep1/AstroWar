using UnityEngine;

public class DusmanController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float hiz = 3f; // Bu deðer Start fonksiyonunda level'a göre deðiþecek
    public float atesSikligi = 2f;

    [Header("Gerekli Objeler")]
    public GameObject mermiPrefab;
    public GameObject patlamaEfekti;
    public AudioClip patlamaSesi;

    private GameManager yonetici;

    void Start()
    {
        yonetici = FindObjectOfType<GameManager>();

        // -----------------------------------------------------------
        // BURASI YENÝ: HIZI LEVEL'A GÖRE AYARLIYORUZ (Kesin Çözüm)
        // -----------------------------------------------------------
        if (yonetici != null)
        {
            if (yonetici.suankiLevel == 1)
            {
                hiz = 2f; // Level 1 ise YAVAÞ
            }
            else if (yonetici.suankiLevel == 2)
            {
                hiz = 3.5f; // Level 2 ise ORTA
            }
            else if (yonetici.suankiLevel == 3)
            {
                hiz = 5f; // Level 3 ise HIZLI
            }
            else if (yonetici.suankiLevel > 3)
            {
                hiz = 7f; // Level 4+ ise ÇOK HIZLI
            }

            // ATEÞ SIKLIÐI AYARI (Bunu zaten yapmýþtýk)
            if (yonetici.suankiLevel == 3) atesSikligi = 2.5f;
            else if (yonetici.suankiLevel > 3) atesSikligi = 0.8f;
        }
        // -----------------------------------------------------------

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
        // Sadece Mermi Çarparsa
        if (other.gameObject.CompareTag("mermi"))
        {
            // 1. ÖNEMLÝ: Ateþ etmeyi ve her þeyi durdur!
            CancelInvoke();

            // Puan Ver
            if (yonetici != null) yonetici.PuanKazan(20);

            // --- PATLAMA KODUNU ÝPTAL ETTÝK (Sorun kalmadý) ---
            // if (patlamaEfekti != null) Instantiate(patlamaEfekti, ...); 
            // ---------------------------------------------------

            // Ses Çal
            if (patlamaSesi != null) AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);

            Destroy(other.gameObject); // Mermiyi yok et
            Destroy(gameObject);       // Kendini yok et
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}