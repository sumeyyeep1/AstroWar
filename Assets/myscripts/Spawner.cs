using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefablar")]
    public GameObject[] tehlikeler; // 0: Asteroit, 1: Düþman Gemisi
    public GameObject canIksiriPrefab;

    [Header("Zorluk Durumu")]
    public float saniyeAraligi = 2.5f;
    public float hizCarpani = 0.8f;
    public bool gemilerGelsinMi = false;
    public bool iksirGelsinMi = false;

    private GameManager yonetici;

    void Start()
    {
        yonetici = FindObjectOfType<GameManager>();
        ZamanlayiciyiKur();
    }

    public void ZamanlayiciyiKur()
    {
        CancelInvoke("TehlikeYarat");
        InvokeRepeating("TehlikeYarat", 0f, saniyeAraligi);
    }

    public void ZorlukGuncelle(float yeniSaniye, float yeniHiz, bool gemiIzni, bool iksirIzni)
    {
        saniyeAraligi = yeniSaniye;
        hizCarpani = yeniHiz;
        gemilerGelsinMi = gemiIzni;
        iksirGelsinMi = iksirIzni;
        ZamanlayiciyiKur();
    }

    void TehlikeYarat()
    {
        float[] seritler = { -6f, -3f, 0f, 3f, 6f };
        int rastgeleSerit = Random.Range(0, seritler.Length);
        Vector2 dogmaYeri = new Vector2(seritler[rastgeleSerit], transform.position.y);

        // --- ÝKSÝR ALGORÝTMASI (NORMAL MOD) ---
        if (iksirGelsinMi)
        {
            // 1. Rastgele bir þans tut (0 ile 100 arasý)
            int sans = Random.Range(0, 100);

            // 2. Acil durum kontrolü (Level 2 ve Can 1 ise ihtimali arttýr)
            bool acilDurum = (yonetici.suankiLevel == 2 && yonetici.kalanCan <= 1);
            int iksirSiniri = acilDurum ? 50 : 10; // Acilse %50, deðilse %10 þans

            // 3. Eðer þans tutarsa iksir üret
            if (sans < iksirSiniri)
            {
                if (canIksiriPrefab != null)
                {
                    Instantiate(canIksiriPrefab, dogmaYeri, Quaternion.identity);
                    return; // Ýksir çýktýysa bu seferlik taþ çýkmasýn (Üst üste binmesin)
                }
            }
        }

        // --- DÜÞMAN VE TAÞ ALGORÝTMASI ---

        // Güvenlik Kontrolü: Eðer Düþman Gemisi prefabý atamadýysan oyun çökmesin diye limit koyuyoruz.
        int limit = 1;
        if (gemilerGelsinMi && tehlikeler.Length >= 2 && tehlikeler[1] != null)
        {
            limit = tehlikeler.Length; // Düþman gemisi varsa listeyi tam kullan
        }

        if (tehlikeler.Length > 0)
        {
            int secilen = Random.Range(0, limit);

            // Seçilen kutu boþ deðilse üret
            if (tehlikeler[secilen] != null)
            {
                GameObject yeniObje = Instantiate(tehlikeler[secilen], dogmaYeri, Quaternion.identity);

                // Hýz Ayarlarý
                if (yeniObje.GetComponent<AsteroitController>())
                    yeniObje.GetComponent<AsteroitController>().speed *= hizCarpani;

                else if (yeniObje.GetComponent<DusmanController>())
                    yeniObje.GetComponent<DusmanController>().hiz *= hizCarpani;
            }
        }
    }
}