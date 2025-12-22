using System.Collections;
using System.Collections.Generic; // Ýleride List kullanýrsak lazým olabilir
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Bu script oyunun BEYNÝ
// - Skoru tutar
// - Caný kontrol eder
// - Level sistemini yönetir
// - Game Over / Win ekranlarýný açar
// - Zorluðu Spawner üzerinden ayarlar
public class GameManager : MonoBehaviour
{
    // -------------------- UI ELEMANLARI --------------------
    [Header("UI Elemanlarý")]
    public TextMeshProUGUI skorYazisi;
    public TextMeshProUGUI canYazisi;
    public TextMeshProUGUI levelYazisi;

    public GameObject gameOverPaneli;
    public GameObject winPaneli;

    public AudioClip kazanmaSesi;

    // -------------------- OYUN VERÝLERÝ --------------------
    [Header("Oyun Verileri")]
    public int toplamPuan = 0;   // Oyuncunun toplam skoru
    public int kalanCan = 3;     // Oyuncunun kalan caný
    public int suankiLevel = 1;  // Þu an hangi leveldeyiz

    // -------------------- SES & DURUM --------------------
    public AudioClip oyunBittiSesi;
    private bool oyunBittiMi = false; // GameOver bir kere çalýþsýn diye

    // -------------------- SPAWNER REFERANSI --------------------
    // Zorluðu ayarlamak için Spawner'a eriþiyoruz
    private Spawner spawner;

    void Start()
    {
        // Sahnedeki Spawner'ý bulup referansýný al
        spawner = FindObjectOfType<Spawner>();

        // Oyun baþlarken zaman normal akmalý
        Time.timeScale = 1f;

        // Can yazýsýný ilk baþta güncelle
        GuncelleCanYazisi();

        // GameOver paneli baþta kapalý olsun
        if (gameOverPaneli != null)
            gameOverPaneli.SetActive(false);

        // Oyun baþý zorluk ayarlarý (Level 1)
        if (spawner != null)
        {
            // Saniye: 2.5 | Hýz: 0.8 | Düþman: Yok | Ýksir: Yok
            spawner.ZorlukGuncelle(2.5f, 0.8f, false, false);
        }

        // Level yazýsýný ekranda göster
        LevelYazisiniGuncelle();
    }

    // -------------------- PUAN KAZANMA --------------------
    public void PuanKazan(int gelenPuan)
    {
        // Skoru artýr
        toplamPuan += gelenPuan;

        // Skor yazýsýný güncelle
        if (skorYazisi != null)
            skorYazisi.text = "SCORE: " + toplamPuan.ToString();

        // Puan deðiþtiyse level kontrolü yap
        LevelKontrol();
    }

    // -------------------- LEVEL KONTROL SÝSTEMÝ --------------------
    void LevelKontrol()
    {
        if (spawner == null) return;

        // -------- KAZANMA KONTROLÜ --------
        // Oyuncu 1000 puana ulaþýrsa oyunu kazanýr
        if (toplamPuan >= 1000)
        {
            OyunKazanildi();
            return; // Level sistemine girmesin
        }

        // -------- LEVEL 1 (0 - 100) --------
        if (toplamPuan < 100)
        {
            if (suankiLevel != 1)
            {
                suankiLevel = 1;
                spawner.ZorlukGuncelle(3.0f, 2.0f, false, false);
                LevelYazisiniGuncelle();
            }
        }
        // -------- LEVEL 2 (100 - 180) --------
        else if (toplamPuan >= 100 && toplamPuan < 180)
        {
            if (suankiLevel != 2)
            {
                suankiLevel = 2;
                Debug.Log("Level 2 Baþladý");

                // Daha hýzlý spawn + iksir aktif
                spawner.ZorlukGuncelle(2.0f, 3.0f, false, true);
                LevelYazisiniGuncelle();
            }
        }
        // -------- LEVEL 3 (180 - 350) --------
        else if (toplamPuan >= 180 && toplamPuan < 350)
        {
            if (suankiLevel != 3)
            {
                suankiLevel = 3;
                Debug.Log("Level 3 Baþladý");

                // Düþman gemileri oyuna giriyor
                spawner.ZorlukGuncelle(1.5f, 1f, true, true);
                LevelYazisiniGuncelle();
            }
        }
        // -------- LEVEL 4 (350 - 600) --------
        else if (toplamPuan >= 350 && toplamPuan < 600)
        {
            if (suankiLevel != 4)
            {
                suankiLevel = 4;
                Debug.Log("Level 4 Baþladý");

                spawner.ZorlukGuncelle(1.2f, 1.5f, true, true);
                LevelYazisiniGuncelle();
            }
        }
        // -------- LEVEL 5 (600+) --------
        else if (toplamPuan >= 600)
        {
            if (suankiLevel != 5)
            {
                suankiLevel = 5;
                Debug.Log("Level 5 Baþladý - SON SEVÝYE");

                spawner.ZorlukGuncelle(0.8f, 2f, true, true);
                LevelYazisiniGuncelle();
            }
        }
    }

    // -------------------- LEVEL YAZISI --------------------
    void LevelYazisiniGuncelle()
    {
        if (levelYazisi != null)
        {
            levelYazisi.text = "LEVEL " + suankiLevel;

            // Eðer kapalýysa aç
            if (!levelYazisi.gameObject.activeInHierarchy)
                levelYazisi.gameObject.SetActive(true);

            // Yazýyý 2 saniye gösterip gizlemek için coroutine
            StartCoroutine(YaziEfekti());
        }
    }

    IEnumerator YaziEfekti()
    {
        levelYazisi.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f); // TimeScale'den etkilenmez
        levelYazisi.gameObject.SetActive(false);
    }

    // -------------------- CAN SÝSTEMÝ --------------------
    public void CanAzalt()
    {
        kalanCan--;
        GuncelleCanYazisi();

        // Can bittiyse oyun biter
        if (kalanCan <= 0)
            OyunBitti();
    }

    public void CanKazan()
    {
        // Can için üst sýnýr koyduk
        if (kalanCan < 100)
        {
            kalanCan++;
            GuncelleCanYazisi();
        }
    }

    void GuncelleCanYazisi()
    {
        if (canYazisi != null)
            canYazisi.text = "LIVES: " + kalanCan.ToString();
    }

    // -------------------- GAME OVER --------------------
    void OyunBitti()
    {
        // Bir kere çalýþsýn diye
        if (oyunBittiMi) return;
        oyunBittiMi = true;

        // Game Over sesi
        if (oyunBittiSesi != null)
            AudioSource.PlayClipAtPoint(oyunBittiSesi, Camera.main.transform.position);

        // Paneli aç
        if (gameOverPaneli != null)
            gameOverPaneli.SetActive(true);

        // Arayüzü gizle
        if (skorYazisi != null) skorYazisi.gameObject.SetActive(false);
        if (levelYazisi != null) levelYazisi.gameObject.SetActive(false);
        if (canYazisi != null) canYazisi.gameObject.SetActive(false);

        // Oyunu tamamen durdur
        Time.timeScale = 0f;
    }

    // -------------------- TEKRAR DENE --------------------
    public void TekrarDene()
    {
        // Ayný sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // -------------------- ANA MENÜ --------------------
    public void AnaMenuyeDon()
    {
        // Menüye giderken zamaný aç
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // -------------------- KAZANMA --------------------
    void OyunKazanildi()
    {
        Debug.Log("Oyun Kazanýldý!");

        if (winPaneli != null)
            winPaneli.SetActive(true);

        // Arka UI'larý kapat
        if (skorYazisi != null) skorYazisi.gameObject.SetActive(false);
        if (levelYazisi != null) levelYazisi.gameObject.SetActive(false);
        if (canYazisi != null) canYazisi.gameObject.SetActive(false);

        // Kazanma sesi
        if (kazanmaSesi != null)
            AudioSource.PlayClipAtPoint(kazanmaSesi, Camera.main.transform.position);

        // Oyunu durdur
        Time.timeScale = 0f;
    }
}
