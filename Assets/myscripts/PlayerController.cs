using System.Collections;
using UnityEngine;

// Bu script oyuncu gemisinin:
// - Saða sola hareketini
// - Ateþ etmesini
// - Hasar almasýný
// - Can iksiri toplamasýný
// kontrol eder
public class PlayerController : MonoBehaviour
{
    // -------------------- HAREKET --------------------
    // Oyuncunun saða sola ne kadar hýzlý gideceði
    public float moveSpeed = 5f;

    // X ekseninde gidebileceði maksimum sýnýr
    public float xLimit = 8f;

    // -------------------- ATIÞ --------------------
    // Lazer mermisinin prefabý
    public GameObject lazerPrefab;

    // Lazerin çýkacaðý nokta (geminin ucu gibi)
    public Transform atesNoktasi;

    // -------------------- SESLER --------------------
    // Ateþ ederken çalacak ses
    public AudioClip atesSesi;

    // Çarpýþma olduðunda çalacak ses
    public AudioClip carpismaSesi;

    // Sesleri çalmak için AudioSource
    private AudioSource audioSource;

    // Gemi sprite’ýna eriþmek için
    private SpriteRenderer gemiGrafigi;

    // -------------------- HASAR KORUMASI --------------------
    // Oyuncu hasar alabilir mi?
    // false olursa kýsa süreli ölümsüzlük olur
    private bool hasarAlabilirMi = true;

    void Start()
    {
        // Ayný objenin üzerindeki AudioSource'u alýyoruz
        audioSource = GetComponent<AudioSource>();

        // SpriteRenderer ile renk deðiþimi yapabilmek için
        gemiGrafigi = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Klavyeden sað-sol input al
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Çok küçük deðerleri sýfýrla (titreme olmasýn diye)
        if (Mathf.Abs(moveInput) < 0.1f)
            moveInput = 0f;

        // Hareket vektörü (sadece X ekseni)
        Vector2 movement = new Vector2(moveInput * moveSpeed * Time.deltaTime, 0f);

        // Objeyi hareket ettir
        transform.Translate(movement);

        // Oyuncunun ekrandan çýkmasýný engelle
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector2(clampedX, transform.position.y);

        // Space tuþuna basýldýysa ateþ et
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AtesEt();
        }
    }

    // -------------------- ATIÞ FONKSÝYONU --------------------
    void AtesEt()
    {
        // Eðer atesNoktasi varsa oradan,
        // yoksa direkt geminin pozisyonundan ateþ et
        Vector3 cikisYeri =
            (atesNoktasi != null) ? atesNoktasi.position : transform.position;

        // Lazer mermisini üret
        Instantiate(lazerPrefab, cikisYeri, Quaternion.identity);

        // Ateþ sesi çal
        if (audioSource != null && atesSesi != null)
            audioSource.PlayOneShot(atesSesi);
    }

    // -------------------- ÇARPIÞMA KONTROLÜ --------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer hasar korumasý aktifse
        // hiçbir þeye tepki verme
        if (hasarAlabilirMi == false)
            return;

        // -------- DÜÞMAN / MERMÝ / ASTEROÝT --------
        if (other.gameObject.CompareTag("dusmanmermisi") ||
            other.gameObject.CompareTag("dusman") ||
            other.gameObject.CompareTag("astreoit"))
        {
            // Hasar alýndý, korumayý kapat
            hasarAlabilirMi = false;

            // 1.5 saniye sonra tekrar hasar alabilsin
            Invoke("KorumayiKaldir", 1.5f);

            // Görsel hasar efekti (kýrmýzý-beyaz yanýp sönme)
            if (gemiGrafigi != null)
                StartCoroutine(HasarEfekti());

            // Çarpýþma sesi
            if (audioSource != null && carpismaSesi != null)
                audioSource.PlayOneShot(carpismaSesi);

            // GameManager üzerinden can azalt
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null)
                yonetici.CanAzalt();

            // Çarpan objeyi yok et
            Destroy(other.gameObject);
        }

        // -------- CAN ÝKSÝRÝ TOPLAMA --------
        else if (other.gameObject.CompareTag("can"))
        {
            // GameManager üzerinden can artýr
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null)
                yonetici.CanKazan();

            // Ýksirin kendi scriptinden toplama sesini al
            CanIksiri iksirScripti =
                other.gameObject.GetComponent<CanIksiri>();

            if (iksirScripti != null && iksirScripti.toplamaSesi != null)
            {
                // Obje yok olsa bile ses devam etsin diye
                // PlayClipAtPoint kullanýyoruz
                AudioSource.PlayClipAtPoint(
                    iksirScripti.toplamaSesi,
                    Camera.main.transform.position
                );
            }

            // Ýksiri sahneden sil
            Destroy(other.gameObject);
        }
    }

    // -------------------- KORUMAYI GERÝ AÇ --------------------
    void KorumayiKaldir()
    {
        hasarAlabilirMi = true;

        // Rengi normale döndür
        if (gemiGrafigi != null)
            gemiGrafigi.color = Color.white;
    }

    // -------------------- HASAR GÖRSEL EFEKTÝ --------------------
    IEnumerator HasarEfekti()
    {
        // Gemi 3 kez kýrmýzý-beyaz yanýp söner
        for (int i = 0; i < 3; i++)
        {
            gemiGrafigi.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            gemiGrafigi.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
