using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 8f;
    public GameObject lazerPrefab;
    public Transform atesNoktasi;

    public AudioClip atesSesi;
    public AudioClip carpismaSesi;
    private AudioSource audioSource;
    private SpriteRenderer gemiGrafigi;

    // Hasar Korumasý
    private bool hasarAlabilirMi = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gemiGrafigi = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) < 0.1f) moveInput = 0f;

        Vector2 movement = new Vector2(moveInput * moveSpeed * Time.deltaTime, 0f);
        transform.Translate(movement);

        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector2(clampedX, transform.position.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AtesEt();
        }
    }

    void AtesEt()
    {
        Vector3 cikisYeri = (atesNoktasi != null) ? atesNoktasi.position : transform.position;
        Instantiate(lazerPrefab, cikisYeri, Quaternion.identity);

        if (audioSource != null && atesSesi != null) audioSource.PlayOneShot(atesSesi);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasarAlabilirMi == false) return; // Koruma varsa iþlem yapma

        // DÜÞMAN VEYA MERMÝ ÇARPARSA
        if (other.gameObject.CompareTag("dusmanmermisi") ||
            other.gameObject.CompareTag("dusman") ||
            other.gameObject.CompareTag("astreoit"))
        {
            hasarAlabilirMi = false;
            Invoke("KorumayiKaldir", 1.5f);

            if (gemiGrafigi != null) StartCoroutine(HasarEfekti());
            if (audioSource != null && carpismaSesi != null) audioSource.PlayOneShot(carpismaSesi);

            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.CanAzalt();

            Destroy(other.gameObject);
        }
        // --- CAN ÝKSÝRÝ TOPLAMA KISMI (GÜNCELLENDÝ) ---
        else if (other.gameObject.CompareTag("can"))
        {
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.CanKazan();

            // YENÝ: Ýksirin üzerindeki sesi bul ve çal
            CanIksiri iksirScripti = other.gameObject.GetComponent<CanIksiri>();
            if (iksirScripti != null && iksirScripti.toplamaSesi != null)
            {
                // PlayClipAtPoint kullanýyoruz ki obje yok olunca ses kesilmesin
                AudioSource.PlayClipAtPoint(iksirScripti.toplamaSesi, Camera.main.transform.position);
            }

            Destroy(other.gameObject);
        }
    }

    void KorumayiKaldir()
    {
        hasarAlabilirMi = true;
        if (gemiGrafigi != null) gemiGrafigi.color = Color.white;
    }

    IEnumerator HasarEfekti()
    {
        for (int i = 0; i < 3; i++)
        {
            gemiGrafigi.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            gemiGrafigi.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
}