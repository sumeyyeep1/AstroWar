using UnityEngine;

// Bu script can iksirinin:
// - Aşağı doğru hareketini
// - Level'a göre çok hafif hızlanmasını
// - Toplanmazsa sahneden silinmesini
// kontrol eder
public class CanIksiri : MonoBehaviour
{
    // -------------------- HAREKET HIZI --------------------
    // Can iksiri bilerek YAVAŞ başlar
    // (Oyuncu kolayca toplayamasın diye)
    private float hiz = 1.2f;

    // İksir toplandığında çalacak ses
    // (PlayerController içinden tetikleniyor)
    public AudioClip toplamaSesi;

    void Start()
    {
        // Sahnedeki GameManager'ı bul
        // Level bilgisini almak için
        GameManager yonetici = FindObjectOfType<GameManager>();

        if (yonetici != null)
        {
            // -------------------- LEVEL'A GÖRE HIZ AYARI --------------------
            // Her level için sadece 0.2 hız ekliyoruz
            // Amaç:
            // - İksir tamamen sabit kalmasın
            // - Ama asla düşman gibi hızlı olmasın
            //
            // Örnek:
            // Level 1 → 1.4f
            // Level 3 → 1.8f
            // Level 5 → 2.2f (hala yavaş)
            hiz += (yonetici.suankiLevel * 0.2f);
        }
    }

    void Update()
    {
        // Can iksirini aşağı doğru hareket ettir
        // Time.deltaTime sayesinde FPS'ten bağımsız
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        // Eğer ekranın altına düşerse:
        // Oyuncu alamadı demektir → sahneden sil
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }
}
