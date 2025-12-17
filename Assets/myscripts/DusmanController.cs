using UnityEngine;

public class DusmanController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float hiz = 3f;
    public float atesSikligi = 2f; // Varsayýlan ateþ hýzý

    [Header("Gerekli Objeler")]
    public GameObject mermiPrefab;
    public GameObject patlamaEfekti;

    [Header("Ses Ayarlarý")]
    public AudioClip patlamaSesi;

    // Yöneticiye oyunun baþýnda eriþip saklayacaðýz
    private GameManager yonetici;

    void Start()
    {
        // 1. Yöneticiyi bul ve hafýzaya al
        yonetici = FindObjectOfType<GameManager>();

        // 2. LEVEL ZORLUK AYARI (Senin istediðin kýsým)
        if (yonetici != null)
        {
            // Level 3 ise: Yavaþ ateþ et (Kolay)
            if (yonetici.suankiLevel == 3)
            {
                atesSikligi = 2.5f;
            }
            // Level 3'ten büyükse: Makineli tüfek gibi ateþ et (Zor)
            else if (yonetici.suankiLevel > 3)
            {
                atesSikligi = 0.8f; // Hýzý arttýrdýk
            }
        }

        // 3. Ateþ etmeye baþla (Belirlenen sýklýkta)
        InvokeRepeating("AtesEt", 1f, atesSikligi);
    }

    void Update()
    {
        // Aþaðý doðru hareket
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        // Ekrandan çýkýnca yok ol (Performans için)
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
        // Senin mermi etiketin "mermi" (küçük harf) idi, onu koruduk.
        if (other.gameObject.CompareTag("mermi") || other.gameObject.CompareTag("lazer"))
        {
            // Puan Ver (Start'ta bulduðumuz yöneticiyi kullanýyoruz)
            if (yonetici != null) yonetici.PuanKazan(20);

            // Efekt Yarat
            if (patlamaEfekti != null)
            {
                Instantiate(patlamaEfekti, transform.position, Quaternion.identity);
            }

            // Ses Çal
            if (patlamaSesi != null)
            {
                AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);
            }

            // Yok Et
            Destroy(other.gameObject); // Mermiyi sil
            Destroy(gameObject);       // Gemiyi sil
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncuya çarparsa direkt gemiyi yok et
            // (Burada istersen yonetici.CanAzalt() da diyebilirsin)
            Destroy(gameObject);
        }
    }
}