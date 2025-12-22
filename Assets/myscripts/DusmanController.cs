using UnityEngine;

// Bu script düþman gemisinin:
// - Aþaðý doðru hareketini
// - Belirli aralýklarla ateþ etmesini
// - Level'a göre hýz ayarýný
// - Vurulunca patlayýp yok olmasýný
// kontrol eder
public class DusmanController : MonoBehaviour
{
    // -------------------- AYARLAR --------------------
    [Header("Kolaylaþtýrýlmýþ Ayarlar")]
    // Düþman gemisinin aþaðý doðru hareket hýzý
    // Varsayýlan düþük tutuldu (oyun adil olsun diye)
    public float hiz = 2f;

    // Düþmanýn kaç saniyede bir ateþ edeceði
    // Deðer büyüdükçe ateþ etme daha seyrek olur
    public float atesSikligi = 3f;

    // -------------------- BAÐLANTILAR --------------------
    [Header("Baðlantýlar")]
    // Düþman mermisi prefabý
    public GameObject mermiPrefab;

    // Patlama efekti (Particle / animasyon)
    public GameObject patlamaEfekti;

    // Patlama sesi
    public AudioClip patlamaSesi;

    // GameManager referansý (puan ve level bilgisi için)
    private GameManager yonetici;

    void Start()
    {
        // Sahnedeki GameManager'ý bul
        yonetici = FindObjectOfType<GameManager>();

        // -------------------- LEVEL'A GÖRE HIZ AYARI --------------------
        // Amaç: Level arttýkça düþman biraz daha tehditkâr olsun
        // Ama aþýrý hýzlanýp oyunu sinir bozucu yapmasýn
        if (yonetici != null)
        {
            if (yonetici.suankiLevel == 1)
            {
                // Yeni baþlayan oyuncu için çok yavaþ
                hiz = 1.5f;
            }
            else if (yonetici.suankiLevel == 2)
            {
                // Orta seviye, kaçýlabilir
                hiz = 2.5f;
            }
            else if (yonetici.suankiLevel >= 3)
            {
                // Biraz daha hýzlý ama hâlâ kontrol edilebilir
                hiz = 3.5f;
            }
        }

        // -------------------- ATEÞ ETME SÝSTEMÝ --------------------
        // Düþman sahneye girer girmez ateþ etmesin diye
        // 2 saniye gecikme verdik
        // Sonrasýnda atesSikligi süresince düzenli ateþ eder
        InvokeRepeating("AtesEt", 2f, atesSikligi);
    }

    void Update()
    {
        // Düþman gemisini aþaðý doðru hareket ettir
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        // Ekranýn altýna düþerse yok et
        // Performans + gereksiz objeleri temizlemek için
        if (transform.position.y < -7f)
            Destroy(gameObject);
    }

    // -------------------- ATEÞ ETME --------------------
    void AtesEt()
    {
        if (mermiPrefab != null)
        {
            // Düþman mermisini bulunduðu yerden üret
            Instantiate(mermiPrefab, transform.position, Quaternion.identity);
        }
    }

    // -------------------- ÇARPIÞMA KONTROLÜ --------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // -------- OYUNCU MERMÝSÝ ÇARPARSA --------
        if (other.gameObject.CompareTag("mermi"))
        {
            // Düþman ölüyorsa artýk ateþ etmemeli
            // Bu yüzden InvokeRepeating'i iptal ediyoruz
            CancelInvoke("AtesEt");

            // Oyuncuya puan ver
            if (yonetici != null)
                yonetici.PuanKazan(20);

            // Patlama efekti
            if (patlamaEfekti != null)
                Instantiate(patlamaEfekti, transform.position, Quaternion.identity);

            // Patlama sesi
            if (patlamaSesi != null)
                AudioSource.PlayClipAtPoint(patlamaSesi, Camera.main.transform.position);

            // Oyuncu mermisini yok et
            Destroy(other.gameObject);

            // Düþman gemisini yok et
            Destroy(gameObject);
        }
        // -------- OYUNCUYA ÇARPARSA --------
        else if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncuya çarpýnca direkt yok olur
            // Hasarý PlayerController halleder
            Destroy(gameObject);
        }
    }
}
