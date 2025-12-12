using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 8f;

    public GameObject lazerPrefab;
    public Transform atesNoktasi;

    // Ses Ayarlarý
    public AudioClip atesSesi;
    private AudioSource audioSource;

    void Start()
    {
        // Hoparlörü bul (Eðer Player objesinde AudioSource yoksa hata vermesin diye kontrol edebiliriz ama þimdilik gerek yok)
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- Hareket ---
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveInput) < 0.1f)
        {
            moveInput = 0f;
        }

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

        // --- Sesi Çal ---
        if (audioSource != null && atesSesi != null)
        {
            audioSource.PlayOneShot(atesSesi);
        }
    }

    // --- Çarpýþma Algýlama ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // Kural: Düþman mermisi VEYA Düþman VEYA Asteroit çarparsa...
        if (other.gameObject.CompareTag("dusmanmermisi") ||
            other.gameObject.CompareTag("dusman") ||
            other.gameObject.CompareTag("astreoit")) // <-- YENÝ EKLENEN KISIM
        {
            Debug.Log("Vurulduk!");

            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.CanAzalt();

            // Çarpan þeyi (Taþ, Mermi veya Düþman) yok et
            Destroy(other.gameObject);
        }
    }
}