using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // BU SATIR ÞART! (Sahne yönetimi için)

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI skorYazisi;
    public TextMeshProUGUI canYazisi;
    public GameObject gameOverPaneli;
    public TextMeshProUGUI levelYazisi;

   public int toplamPuan = 0;
    int kalanCan = 3;
    int suankiLevel = 1;

    void Start()
    {
        GuncelleCanYazisi();
        if (levelYazisi != null) levelYazisi.text = "LEVEL " + suankiLevel;
        if (gameOverPaneli != null) gameOverPaneli.SetActive(false);
    }

    public void PuanKazan(int gelenPuan)
    {
        toplamPuan += gelenPuan;
        if (skorYazisi != null) skorYazisi.text = "SKOR: " + toplamPuan.ToString();

        // --- LEVEL 2 KONTROLÜ (300 Puan) ---
        if (toplamPuan >= 300 && suankiLevel == 1)
        {
            suankiLevel = 2;
            if (levelYazisi != null)
            {
                levelYazisi.text = "LEVEL " + suankiLevel;
                levelYazisi.color = Color.yellow; // Sarý Renk
            }
        }

        // --- YENÝ: LEVEL 3 KONTROLÜ (700 Puan) ---
        if (toplamPuan >= 700 && suankiLevel == 2)
        {
            suankiLevel = 3;

            // 1. Yazýyý Güncelle
            if (levelYazisi != null)
            {
                levelYazisi.text = "LEVEL " + suankiLevel + " (MAX)";
                levelYazisi.color = Color.red; // Kýrmýzý Renk (Tehlike!)
            }

            // 2. Spawner'ý Bul ve Hýzlandýr
            Spawner fabrika = FindObjectOfType<Spawner>();
            if (fabrika != null)
            {
                fabrika.UretimiHizlandir();
            }
        }
    }

    public void CanAzalt()
    {
        kalanCan--;
        GuncelleCanYazisi();

        if (kalanCan <= 0)
        {
            OyunBitti();
        }
    }

    void GuncelleCanYazisi()
    {
        if (canYazisi != null) canYazisi.text = "CAN: " + kalanCan.ToString();
    }

    void OyunBitti()
    {
        Time.timeScale = 0f; // Oyunu dondur
        if (gameOverPaneli != null) gameOverPaneli.SetActive(true); // Paneli aç
    }

    // --- ÝÞTE BUTONUN ÇALIÞTIRACAÐI KOD BURASI ---
    public void YenidenBasla()
    {
        Time.timeScale = 1f; // Zamaný tekrar akýt (Yoksa oyun donuk baþlar)
        // Mevcut sahneyi (bölümü) baþtan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // YENÝ: Can Kazanma Fonksiyonu
    public void CanKazan()
    {
        // Canýmýz 3'ten azsa artýr (Maksimum can 3 olsun)
        if (kalanCan < 3)
        {
            kalanCan++;
            GuncelleCanYazisi();
        }
    }
}