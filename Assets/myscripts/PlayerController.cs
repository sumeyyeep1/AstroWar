using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 8f;

    public GameObject lazerPrefab;
    public Transform atesNoktasi;

    // --- SES AYARLARI ---
    public AudioClip atesSesi;      // Mermi sesi
    public AudioClip carpismaSesi;  // YENÝ: Çarpma sesi (Bunu editörden ekleyeceksin)
    private AudioSource audioSource;
    // --- HASAR KORUMASI ---
    private bool hasarAlabilirMi = true; // Baþlangýçta hasar alabiliriz
    // Görsel efekt
    private SpriteRenderer gemiGrafigi;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gemiGrafigi = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- Hareket ---
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveInput) < 0.1f) moveInput = 0f;

        Vector2 movement = new Vector2(moveInput * moveSpeed * Time.deltaTime, 0f);
        transform.Translate(movement);

        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector2(clampedX, transform.position.y);

        // --- Ateþ Etme ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AtesEt();
        }
    }

    void AtesEt()
    {
        Vector3 cikisYeri = (atesNoktasi != null) ? atesNoktasi.position : transform.position;
        Instantiate(lazerPrefab, cikisYeri, Quaternion.identity);

        if (audioSource != null && atesSesi != null)
        {
            audioSource.PlayOneShot(atesSesi);
        }
    }

    // --- Çarpýþma Algýlama ---
    // --- Çarpýþma Algýlama ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // ÖNEMLÝ: Eðer þu an koruma altýndaysak (hasarAlabilirMi = false ise) hiçbir þey yapma!
        if (hasarAlabilirMi == false) return;

        if (other.gameObject.CompareTag("dusmanmermisi") ||
            other.gameObject.CompareTag("dusman") ||
            other.gameObject.CompareTag("astreoit"))
        {
            Debug.Log("Vurulduk!");

            // 1. GÖRSEL EFEKT (Kýzarma)
            if (gemiGrafigi != null) StartCoroutine(HasarEfekti());

            // 2. SES EFEKTÝ
            if (audioSource != null && carpismaSesi != null)
            {
                audioSource.PlayOneShot(carpismaSesi);
            }

            // 3. CAN AZALTMA
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.CanAzalt();

            // 4. HASAR KORUMASINI AÇ (Artýk 1.5 saniye hasar almayacaksýn)
            hasarAlabilirMi = false;
            Invoke("HasarKorumasiniKaldir", 1.5f); // 1.5 saniye sonra fonksiyonu çalýþtýr

            Destroy(other.gameObject); // Çarpan þeyi yok et
        }
        else if (other.gameObject.CompareTag("can"))
        {
            Debug.Log("Can toplandý ");
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.CanKazan();
            Destroy(other.gameObject);
        }
    }

    // --- YENÝ FONKSÝYON: Koruma Süresi Bitince Çalýþýr ---
    void HasarKorumasiniKaldir()
    {
        hasarAlabilirMi = true; // Tekrar hasar alabilir hale gel
        gemiGrafigi.color = Color.white; // Rengi normale döndür (Garanti olsun)
    }

    IEnumerator HasarEfekti()
    {
        gemiGrafigi.color = Color.red;
        yield return new WaitForSeconds(0.2f); // Kýzarma süresi
        gemiGrafigi.color = Color.white;

        // Yanýp sönme efekti (Opsiyonel: Daha havalý durur)
        yield return new WaitForSeconds(0.2f);
        gemiGrafigi.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gemiGrafigi.color = Color.white;
    }
}