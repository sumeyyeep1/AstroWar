using UnityEngine;

public class DusmanController : MonoBehaviour
{
    [Header("Kolaylaþtýrýlmýþ Ayarlar")]
    public float hiz = 2f;         // Varsayýlan hýz düþtü
    public float atesSikligi = 3f; // Sayý büyüdükçe daha AZ ateþ eder (3 saniyede 1 mermi)

    [Header("Baðlantýlar")]
    public GameObject mermiPrefab;
    public GameObject patlamaEfekti;
    public AudioClip patlamaSesi;

    private GameManager yonetici;

    void Start()
    {
        yonetici = FindObjectOfType<GameManager>();

        // Levellere göre hýzý ayarladým ama eskisinden çok daha YAVAÞ
        if (yonetici != null)
        {
            if (yonetici.suankiLevel == 1)
                hiz = 1.5f; // Kaplumbaða hýzý (Hoca rahat görsün)

            else if (yonetici.suankiLevel == 2)
                hiz = 2.5f; // Biraz hýzlandý ama kaçýlabilir

            else if (yonetici.suankiLevel >= 3)
                hiz = 3.5f; // Hýzlý ama refleks gerektirmez
        }

        // Oyun baþlar baþlamaz ateþ etmesin, 2 saniye beklesin. Sonra yavaþ yavaþ ateþ etsin.
        InvokeRepeating("AtesEt", 2f, atesSikligi);
    }

    void Update()
    {
        transform.Translate(Vector2.down * hiz * Time.deltaTime);
        if (transform.position.y < -7f) Destroy(gameObject);
    }

    void AtesEt()
    {
        if (mermiPrefab != null)
        {
            Instantiate(mermiPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Mermi Çarparsa
        if (other.gameObject.CompareTag("mermi"))
        {
            CancelInvoke("AtesEt");

            if (yonetici != null) yonetici.PuanKazan(20);

            // Efekt ve Sesler
            if (patlamaEfekti != null)
                Instantiate(patlamaEfekti, transform.position, Quaternion.identity);

            if (patlamaSesi != null)
                AudioSource.PlayClipAtPoint(patlamaSesi, Camera.main.transform.position);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        // Oyuncuya Çarparsa
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}