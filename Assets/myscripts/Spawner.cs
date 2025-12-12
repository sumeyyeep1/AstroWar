using UnityEngine;

public class Spawner : MonoBehaviour
{
    // "Size" kutusu için gerekli olan liste (Düþmanlar)
    public GameObject[] tehlikeler;

    // --- ÝÞTE EKSÝK OLAN SATIR BU ---
    public GameObject canIksiriPrefab; // Can iksiri kutusu buraya gelecek
    // --------------------------------

    public float saniyeAraligi = 1.2f;

    void Start()
    {
        InvokeRepeating("TehlikeYarat", 0f, saniyeAraligi);
    }

    void TehlikeYarat()
    {
        // 1. Þeritleri Belirle
        float[] seritler = { -6f, -3f, 0f, 3f, 6f };
        int rastgeleSerit = Random.Range(0, seritler.Length);
        float secilenX = seritler[rastgeleSerit];
        Vector2 dogmaYeri = new Vector2(secilenX, transform.position.y);

        // 2. GameManager'a Ulaþ
        GameManager yonetici = FindObjectOfType<GameManager>();

        // --- YENÝ EKLENEN KISIM: Ýksir Çýkma Þansý ---
        // Eðer puan 300'den büyükse (Level 2 ve üstü)
        if (yonetici != null && yonetici.toplamPuan >= 300)
        {
            // %10 ihtimalle (0 ile 100 arasýnda sayý tut, 10'dan küçükse)
            if (Random.Range(0, 100) < 10)
            {
                // Sadece Ýksiri yarat
                if (canIksiriPrefab != null)
                {
                    Instantiate(canIksiriPrefab, dogmaYeri, Quaternion.identity);
                }
                return; // Burasý çok önemli: Düþman üretmeden fonksiyondan çýk!
            }
        }
        // ---------------------------------------------

        // 3. Limit Ayarý (Level Zorluðu)
        int limit = 1; // Baþlangýçta sadece Asteroit

        if (yonetici != null)
        {
            if (yonetici.toplamPuan >= 300)
            {
                limit = tehlikeler.Length; // Düþmanlarý da dahil et
            }
        }

        // 4. Düþman veya Taþ Yarat
        if (tehlikeler.Length > 0)
        {
            int secilenTehlike = Random.Range(0, limit);
            Instantiate(tehlikeler[secilenTehlike], dogmaYeri, Quaternion.identity);
        }
    }

    // Bu fonksiyon çaðrýldýðýnda üretim hýzý artacak (Level 3 için)
    public void UretimiHizlandir()
    {
        CancelInvoke("TehlikeYarat"); // Eski yavaþ üretimi durdur
        InvokeRepeating("TehlikeYarat", 0.5f, 0.8f); // Hýzlý baþlat
    }
}