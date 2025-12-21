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
    private int sonSecilenSerit = -1; // Arka arkaya ayný yerden gelmesin diye hafýza

    // --- YENÝ: Zamanlayýcý Deðiþkeni ---
    private float sonIksirZamani = 0f;

    void Start()
    {
        yonetici = FindObjectOfType<GameManager>();
        ZamanlayiciyiKur();
        // Oyun baþladýðýnda sayaç mevcut zamaný alsýn ki hemen ilk saniyede iksir atmasýn
        sonIksirZamani = Time.time;
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
        // 1. ÞERÝT SEÇÝMÝ (Çakýþma Önleyici Sistem)
        float[] seritler = { -6f, -3f, 0f, 3f, 6f };
        int rastgeleSerit = Random.Range(0, seritler.Length);

        // Eðer seçilen þerit bir öncekiyle aynýysa deðiþtir (Üst üste binmeyi engeller)
        if (rastgeleSerit == sonSecilenSerit)
        {
            rastgeleSerit = Random.Range(0, seritler.Length);
        }
        sonSecilenSerit = rastgeleSerit; // Hafýzaya al

        Vector2 dogmaYeri = new Vector2(seritler[rastgeleSerit], transform.position.y);


        // --- YENÝ ÝKSÝR SÝSTEMÝ (SÜREYE DAYALI) ---
        if (iksirGelsinMi)
        {
            float gerekenSure = 9999f; // Varsayýlan (Çok uzun süre)

            // Levele göre bekleme süresini (Cooldown) belirle
            switch (yonetici.suankiLevel)
            {
                case 1: gerekenSure = 9999f; break; // Level 1'de gelmesin
                case 2: gerekenSure = 20f; break; // 20 saniyede bir
                case 3: gerekenSure = 10f; break; // 10 saniyede bir
                case 4: gerekenSure = 7f; break; // 7 saniyede bir
                case 5: gerekenSure = 5f; break; // 5 saniyede bir (Çok sýk)
                default: gerekenSure = 10f; break;
            }

            // Acil Durum: Eðer can 1 ise süreleri yarýya indir (Daha hýzlý yardým gelsin)
            if (yonetici.kalanCan <= 1) gerekenSure /= 2f;

            // ZAMAN KONTROLÜ: Son iksirden bu yana yeterli süre geçti mi?
            if (Time.time > sonIksirZamani + gerekenSure)
            {
                // EK KONTROL: Canýn zaten 10 (Full) ise iksir çýkartýp harcama
                // Ama süreyi de sýfýrlama, canýn azaldýðý an hemen gelsin.
                if (yonetici.kalanCan < 10)
                {
                    if (canIksiriPrefab != null)
                    {
                        Instantiate(canIksiriPrefab, dogmaYeri, Quaternion.identity);
                        sonIksirZamani = Time.time; // Sayacý sýfýrla
                        return; // ÖNEMLÝ: Ýksir çýktýysa taþ çýkmasýn (Çakýþmayý önler)
                    }
                }
            }
        }

        // --- DÜÞMAN VE TAÞ ALGORÝTMASI ---

        // Eðer iksir çýkmadýysa burasý çalýþýr
        int limit = 1;
        if (gemilerGelsinMi && tehlikeler.Length >= 2 && tehlikeler[1] != null)
        {
            limit = tehlikeler.Length;
        }

        if (tehlikeler.Length > 0)
        {
            int secilen = Random.Range(0, limit);

            if (tehlikeler[secilen] != null)
            {
                GameObject yeniObje = Instantiate(tehlikeler[secilen], dogmaYeri, Quaternion.identity);

                // Hýz Ayarlarýný Uygula
                if (yeniObje.GetComponent<AsteroitController>())
                    yeniObje.GetComponent<AsteroitController>().speed *= hizCarpani;

                else if (yeniObje.GetComponent<DusmanController>())
                    yeniObje.GetComponent<DusmanController>().hiz *= hizCarpani;
            }
        }
    }
}