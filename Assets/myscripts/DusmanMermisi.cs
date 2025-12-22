using UnityEngine;

// Bu script düşmanların attığı mermilerin
// - Hızını
// - Hareket yönünü
// - Level'a göre zorluk ayarını
// - Oyuncuya çarpınca can azaltmasını
// kontrol eder
public class DusmanMermisi : MonoBehaviour
{
    // -------------------- MERMİ HIZI --------------------
    // Varsayılan mermi hızı
    public float hiz = 10f;

    // GameManager referansı
    // Level bilgisi ve CanAzalt fonksiyonu için gerekli
    private GameManager yonetici;

    void Start()
    {
        // Sahnedeki GameManager'ı bul
        yonetici = FindObjectOfType<GameManager>();

        // -------------------- LEVEL'A GÖRE HIZ AYARI --------------------
        // Amaç: Oyun ilerledikçe mermilerin davranışı değişsin
        if (yonetici != null)
        {
            // Eğer Level 3 ise:
            // Oyuncu daha yeni düşman mermilerine alışıyor
            // O yüzden mermiler daha YAVAŞ
            if (yonetici.suankiLevel == 3)
            {
                hiz = 5f; // Normal hızın yaklaşık yarısı
            }
            // Eğer Level 3'ten büyükse:
            // Oyun zorlaştı → mermiler daha HIZLI
            else if (yonetici.suankiLevel > 3)
            {
                hiz = 12f; // Normalden daha hızlı
            }
        }

        // Performans için:
        // Mermi 3 saniye sonra otomatik yok edilir
        // Böylece sahnede gereksiz obje birikmez
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Mermiyi aşağı doğru hareket ettiriyoruz
        // NOT: Sprite zaten aşağı baktığı için Vector2.down kullanıyoruz
        // Time.deltaTime sayesinde FPS'ten bağımsız hareket eder
        transform.Translate(Vector2.down * hiz * Time.deltaTime);
    }

    // -------------------- ÇARPIŞMA KONTROLÜ --------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer çarptığımız obje Player (oyuncu gemisi) ise
        if (other.gameObject.CompareTag("Player"))
        {
            // GameManager'a haber ver:
            // Oyuncunun canını azalt
            if (yonetici != null)
                yonetici.CanAzalt();

            // Mermi görevini yaptı → yok et
            Destroy(gameObject);
        }
    }
}
