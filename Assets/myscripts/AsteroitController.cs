using UnityEngine;

// Bu script asteroitlerin:
// - Aşağı doğru hareketini
// - Mermiyle vurulunca yok olmasını
// - Oyuncuya çarparsa kaybolmasını
// - Patlama sesini
// kontrol eder
public class AsteroitController : MonoBehaviour
{
    // -------------------- HAREKET --------------------
    // Asteroitin aşağı doğru düşme hızı
    public float speed = 3f;

    // -------------------- SES --------------------
    // Asteroit yok olurken çalacak patlama sesi
    public AudioClip patlamaSesi;

    void Update()
    {
        // Asteroiti sürekli aşağı doğru hareket ettir
        // Time.deltaTime → FPS'e bağlı olmasın diye
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Eğer ekranın altına düşerse
        // Oyuncu kaçırdı demektir → yok et
        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    // -------------------- ÇARPIŞMA KONTROLÜ --------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // -------- OYUNCU MERMİSİ ÇARPARSA --------
        if (other.gameObject.CompareTag("mermi"))
        {
            // GameManager üzerinden puan kazandır
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null)
                yonetici.PuanKazan(10);

            // Patlama sesi çal
            // Obje yok olsa bile ses devam etsin diye
            if (patlamaSesi != null)
            {
                AudioSource.PlayClipAtPoint(
                    patlamaSesi,
                    transform.position
                );
            }

            // Önce mermiyi sil
            Destroy(other.gameObject);

            // Sonra asteroiti sil
            Destroy(gameObject);
        }

        // -------- OYUNCUYA ÇARPARSA --------
        else if (other.gameObject.CompareTag("Player"))
        {
            // Burada ekstra ses çalmıyoruz
            // Çünkü oyuncu gemisi zaten çarpışma sesi çıkarıyor
            Destroy(gameObject);
        }
    }
}
