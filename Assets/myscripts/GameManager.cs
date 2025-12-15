using UnityEngine;
using TMPro; // TextMeshPro kullandýðýn için bu kütüphane þart
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public TextMeshProUGUI skorYazisi;
    public TextMeshProUGUI canYazisi;
    public TextMeshProUGUI levelYazisi;
    public GameObject gameOverPaneli;

    [Header("Oyun Ayarlarý")]
    public int toplamPuan = 0;
    int kalanCan = 3;
    int suankiLevel = 1;

    [Header("Ses Efektleri")]
    public AudioClip oyunBittiSesi; // --- YENÝ: Oyun Bitme Sesi ---
    private bool oyunBittiMi = false; // Sesin bir kere çalmasý için kontrol

    void Start()
    {
        Time.timeScale = 1f;
        GuncelleCanYazisi();

        if (gameOverPaneli != null) gameOverPaneli.SetActive(false);

        // OYUN BAÞLAR BAÞLAMAZ LEVEL YAZISINI GÖSTER VE GÝZLE
        LevelYazisiniGuncelle();
    }

    public void PuanKazan(int gelenPuan)
    {
        toplamPuan += gelenPuan;
        if (skorYazisi != null) skorYazisi.text = "SKOR: " + toplamPuan.ToString();

        // --- LEVEL 2 KONTROLÜ ---
        if (toplamPuan >= 300 && suankiLevel == 1)
        {
            suankiLevel = 2;
            LevelYazisiniGuncelle();

            Spawner fabrika = FindObjectOfType<Spawner>();
            // Fabrika hýzlandýrma kodlarýný buraya ekleyebilirsin
        }

        // --- LEVEL 3 KONTROLÜ ---
        if (toplamPuan >= 700 && suankiLevel == 2)
        {
            suankiLevel = 3;
            LevelYazisiniGuncelle();

            Spawner fabrika = FindObjectOfType<Spawner>();
            if (fabrika != null) fabrika.UretimiHizlandir();
        }
    }

    // --- YAZIYI GÖSTERÝP GÝZLEYEN FONKSÝYON ---
    void LevelYazisiniGuncelle()
    {
        if (levelYazisi != null)
        {
            levelYazisi.text = "LEVEL " + suankiLevel;
            StartCoroutine(YaziEfekti());
        }
    }

    IEnumerator YaziEfekti()
    {
        levelYazisi.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        levelYazisi.gameObject.SetActive(false);
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

    public void CanKazan()
    {
        if (kalanCan < 3)
        {
            kalanCan++;
            GuncelleCanYazisi();
        }
    }

    void GuncelleCanYazisi()
    {
        if (canYazisi != null) canYazisi.text = "CAN: " + kalanCan.ToString();
    }

    void OyunBitti()
    {
        // --- YENÝ: KONTROL VE SES ---
        if (oyunBittiMi == true) return; // Zaten bittiyse tekrar çalýþma
        oyunBittiMi = true; // Bitttiðini iþaretle

        // Oyun Bitme Sesini Çal (Kamera pozisyonunda)
        if (oyunBittiSesi != null)
        {
            AudioSource.PlayClipAtPoint(oyunBittiSesi, Camera.main.transform.position);
        }
        // -----------------------------

        // --- REKOR KAYDETME ---
        int eskiRekor = PlayerPrefs.GetInt("EnYuksekSkor", 0);
        if (toplamPuan > eskiRekor)
        {
            PlayerPrefs.SetInt("EnYuksekSkor", toplamPuan);
            PlayerPrefs.Save();
        }

        if (gameOverPaneli != null) gameOverPaneli.SetActive(true);

        // Diðer yazýlarý gizle
        if (skorYazisi != null) skorYazisi.gameObject.SetActive(false);
        if (levelYazisi != null) levelYazisi.gameObject.SetActive(false);
        if (canYazisi != null) canYazisi.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void TekrarDene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}