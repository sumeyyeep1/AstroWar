using UnityEngine;

// Bu script oyunda yukarýdan gelen tüm tehlikeleri (asteroit, düþman gemisi)
// ve can iksirlerini üretmekten (spawnlamak) sorumlu.
public class Spawner : MonoBehaviour
{
    // -------------------- PREFABLAR --------------------
    [Header("Prefablar")]
    // Tehlikeler dizisi:
    // 0. index = Asteroit
    // 1. index = Düþman Gemisi
    public GameObject[] tehlikeler;

    // Can iksiri prefabý (oyuncunun canýný artýran obje)
    public GameObject canIksiriPrefab;

    // -------------------- ZORLUK AYARLARI --------------------
    [Header("Zorluk Durumu")]
    // Tehlikelerin kaç saniyede bir geleceðini belirler
    public float saniyeAraligi = 2.5f;

    // Zorluk arttýkça objelerin hýzýný çarptýðýmýz deðer
    public float hizCarpani = 0.8f;

    // Düþman gemileri oyuna girsin mi?
    public bool gemilerGelsinMi = false;

    // Can iksiri oyuna girsin mi?
    public bool iksirGelsinMi = false;

    // -------------------- YARDIMCI DEÐÝÞKENLER --------------------
    // GameManager referansý (level, can bilgisi vs. için)
    private GameManager yonetici;

    // Ayný þeritten arka arkaya spawn olmasýný engellemek için
    private int sonSecilenSerit = -1;

    // Son can iksirinin çýktýðý zamaný tutar
    // Böylece sürekli iksir yaðmasýný engelleriz
    private float sonIksirZamani = 0f;

    void Start()
    {
        // Sahnedeki GameManager'ý buluyoruz
        yonetici = FindObjectOfType<GameManager>();

        // Spawn sistemini baþlatýyoruz
        ZamanlayiciyiKur();

        // Oyunun baþýnda iksir hemen gelmesin diye
        // mevcut zamaný referans alýyoruz
        sonIksirZamani = Time.time;
    }

    // TehlikeYarat fonksiyonunu belli aralýklarla çaðýran sistem
    public void ZamanlayiciyiKur()
    {
        // Önceden çalýþan varsa iptal ediyoruz
        CancelInvoke("TehlikeYarat");

        // Belirlenen saniye aralýðýnda sürekli çaðýr
        InvokeRepeating("TehlikeYarat", 0f, saniyeAraligi);
    }

    // Zorluk arttýðýnda GameManager tarafýndan çaðrýlýr
    public void ZorlukGuncelle(float yeniSaniye, float yeniHiz, bool gemiIzni, bool iksirIzni)
    {
        // Yeni zorluk deðerlerini al
        saniyeAraligi = yeniSaniye;
        hizCarpani = yeniHiz;
        gemilerGelsinMi = gemiIzni;
        iksirGelsinMi = iksirIzni;

        // Zamanlayýcýyý yeni deðerlere göre yeniden kur
        ZamanlayiciyiKur();
    }

    // Asýl spawn iþleminin yapýldýðý fonksiyon
    void TehlikeYarat()
    {
        // -------------------- ÞERÝT SEÇÝMÝ --------------------
        // Objelerin düþebileceði X pozisyonlarý
        float[] seritler = { -6f, -3f, 0f, 3f, 6f };

        // Rastgele bir þerit seç
        int rastgeleSerit = Random.Range(0, seritler.Length);

        // Eðer bir öncekiyle aynýysa tekrar seçtir
        // Amaç: Üst üste ayný yerden obje gelmesin
        if (rastgeleSerit == sonSecilenSerit)
        {
            rastgeleSerit = Random.Range(0, seritler.Length);
        }

        // Seçilen þeridi hafýzaya al
        sonSecilenSerit = rastgeleSerit;

        // Objelerin doðacaðý pozisyon
        Vector2 dogmaYeri = new Vector2(seritler[rastgeleSerit], transform.position.y);

        // -------------------- CAN ÝKSÝRÝ SÝSTEMÝ --------------------
        if (iksirGelsinMi)
        {
            // Varsayýlan olarak aþýrý uzun bir süre
            // (yani gelmesin gibi düþünebiliriz)
            float gerekenSure = 9999f;

            // Level'e göre iksir cooldown süresi
            switch (yonetici.suankiLevel)
            {
                case 1: gerekenSure = 9999f; break; // Level 1: Hiç gelmesin
                case 2: gerekenSure = 20f; break;   // 20 saniyede bir
                case 3: gerekenSure = 10f; break;
                case 4: gerekenSure = 7f; break;
                case 5: gerekenSure = 5f; break;    // Çok sýk
                default: gerekenSure = 10f; break;
            }

            // Eðer oyuncunun caný 1 veya daha azsa
            // yardým daha hýzlý gelsin diye süreyi yarýya indiriyoruz
            if (yonetici.kalanCan <= 1)
                gerekenSure /= 2f;

            // Son iksirden sonra yeterli süre geçmiþ mi?
            if (Time.time > sonIksirZamani + gerekenSure)
            {
                // Eðer can zaten full ise iksir üretmeyelim
                // Ama süreyi de sýfýrlamýyoruz
                // Böylece can azalýnca hemen iksir çýkabiliyor
                if (yonetici.kalanCan < 10)
                {
                    if (canIksiriPrefab != null)
                    {
                        Instantiate(canIksiriPrefab, dogmaYeri, Quaternion.identity);

                        // Son iksir zamaný güncellenir
                        sonIksirZamani = Time.time;

                        // Ýksir çýktýysa bu turda taþ veya düþman çýkmasýn
                        return;
                    }
                }
            }
        }

        // -------------------- TEHLÝKE (TAÞ / DÜÞMAN) SÝSTEMÝ --------------------

        // Varsayýlan olarak sadece asteroit gelsin
        int limit = 1;

        // Eðer düþman gemilerine izin varsa
        // dizinin tamamýný kullanabiliriz
        if (gemilerGelsinMi && tehlikeler.Length >= 2 && tehlikeler[1] != null)
        {
            limit = tehlikeler.Length;
        }

        // Dizide en az bir tehlike varsa
        if (tehlikeler.Length > 0)
        {
            // Rastgele bir tehlike seç
            int secilen = Random.Range(0, limit);

            if (tehlikeler[secilen] != null)
            {
                // Seçilen tehlikeyi sahneye üret
                GameObject yeniObje =
                    Instantiate(tehlikeler[secilen], dogmaYeri, Quaternion.identity);

                // -------------------- HIZ AYARLARI --------------------
                // Eðer asteroitse speed deðiþkenini çarp
                if (yeniObje.GetComponent<AsteroitController>())
                {
                    yeniObje.GetComponent<AsteroitController>().speed *= hizCarpani;
                }
                // Eðer düþman gemisiyse onun hýzýný çarp
                else if (yeniObje.GetComponent<DusmanController>())
                {
                    yeniObje.GetComponent<DusmanController>().hiz *= hizCarpani;
                }
            }
        }
    }
}
