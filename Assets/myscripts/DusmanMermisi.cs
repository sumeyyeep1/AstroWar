using UnityEngine;

public class DusmanMermisi : MonoBehaviour
{
    public float hiz = 10f; // Varsayýlan hýz
    private GameManager yonetici;

    void Start()
    {
        // GameManager'ý bul (Level bilgisini ve Can Azaltma komutunu buradan alacaðýz)
        yonetici = FindObjectOfType<GameManager>();

        // --- LEVEL HIZ AYARI ---
        if (yonetici != null)
        {
            // Level 3 ise: Mermiler YAVAÞ ve Tembel (Kolay)
            if (yonetici.suankiLevel == 3)
            {
                hiz = 5f; // Hýzý yarýya düþürdük (Senin 10f ayarýna göre)
            }
            // Level 3'ten büyükse: Mermiler HIZLI (Zor)
            else if (yonetici.suankiLevel > 3)
            {
                hiz = 12f; // Hýzý arttýrdýk
            }
        }

        // 3 saniye sonra sahneden silinsin (Performans için)
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // DÝKKAT: Resim zaten aþaðý baktýðý için "Down" kullanýyoruz.
        // Hýz deðiþkeni yukarýda Level'a göre ayarlandýðý için doðru hýzda gidecektir.
        transform.Translate(Vector2.down * hiz * Time.deltaTime);
    }

    // --- ÇARPIÞMA KONTROLÜ (YENÝ EKLENEN KISIM) ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer çarptýðýmýz þey "Player" (Bizim gemimiz) ise:
        if (other.gameObject.CompareTag("Player"))
        {
            // 1. Yöneticiye söyle: Can azalt
            if (yonetici != null) yonetici.CanAzalt();

            // 2. Mermiyi yok et (Çarptý çünkü)
            Destroy(gameObject);
        }
    }
}