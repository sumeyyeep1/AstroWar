using System.Collections;
using System.Collections.Generic; // Listeler için gerekli olabilir
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public TextMeshProUGUI skorYazisi;
    public TextMeshProUGUI canYazisi;
    public TextMeshProUGUI levelYazisi;
    public GameObject gameOverPaneli;
    public GameObject winPaneli;
    public AudioClip kazanmaSesi;
    [Header("Oyun Verileri")]
    public int toplamPuan = 0; // Senin puan deðiþkenin bu
    public int kalanCan = 3;
    public int suankiLevel = 1;

    // Ses
    public AudioClip oyunBittiSesi;
    private bool oyunBittiMi = false;

    // --- DÜZELTME 1: Spawner'ý burada tanýmlýyoruz ---
    private Spawner spawner;

    void Start()
    {
        // --- DÜZELTME 1 DEVAMI: Spawner'ý bulup hafýzaya alýyoruz ---
        spawner = FindObjectOfType<Spawner>();

        Time.timeScale = 1f;
        GuncelleCanYazisi();

        if (gameOverPaneli != null) gameOverPaneli.SetActive(false);

        // Baþlangýç ayarýný yapýyoruz (Artýk 'spawner' deðiþkenini kullanýyoruz)
        if (spawner != null)
        {
            spawner.ZorlukGuncelle(2.5f, 0.8f, false, false);
        }

        LevelYazisiniGuncelle();
    }

    public void PuanKazan(int gelenPuan)
    {
        toplamPuan += gelenPuan;
        if (skorYazisi != null) skorYazisi.text = "SCORE: " + toplamPuan.ToString();

        // --- DÜZELTME 3: Fonksiyonun doðru adýný yazýyoruz ---
        LevelKontrol();
    }

    void LevelKontrol()
    {
        if (spawner == null) return;

        // --- BU KISMI EKLE (Zafer Kontrolü) ---
        if (toplamPuan >= 1000) // 1000 puana gelince kazan
        {
            OyunKazanildi();
            return; // Aþaðýdaki kodlarý çalýþtýrma
        }
        // --- LEVEL 1 (0 - 100 Puan) ---
        if (toplamPuan < 100)
        {
            if (suankiLevel != 1)
            {
                suankiLevel = 1;
                // Saniye: 3.0, Hýz: 2.0, Gemi: Yok, Ýksir: Yok
                spawner.ZorlukGuncelle(3.0f, 2.0f, false, false);
                LevelYazisiniGuncelle();
            }
        }
        // --- LEVEL 2 (100 - 180 Puan) ---
        else if (toplamPuan >= 100 && toplamPuan < 180)
        {
            if (suankiLevel != 2)
            {
                suankiLevel = 2;
                Debug.Log("Level 2 Baþladý");
                // Saniye: 2.0 (Hýzlandý), Hýz: 3.0, Gemi: Yok, Ýksir: Var
                spawner.ZorlukGuncelle(2.0f, 3.0f, false, true);
                LevelYazisiniGuncelle();
            }
        }
        // --- LEVEL 3 (180 - 350 Puan) ---
        else if (toplamPuan >= 180 && toplamPuan < 350)
        {
            if (suankiLevel != 3)
            {
                suankiLevel = 3;
                Debug.Log("Level 3 Baþladý");
                // Saniye: 1.5, Hýz: 1.5 (Düzelttiðimiz ayar), Gemi: VAR, Ýksir: Var
                spawner.ZorlukGuncelle(1.5f, 1f, true, true);
                LevelYazisiniGuncelle();
            }
        }
        // --- LEVEL 4 (350 - 600 Puan) ---
        else if (toplamPuan >= 350 && toplamPuan < 600)
        {
            if (suankiLevel != 4)
            {
                suankiLevel = 4;
                Debug.Log("Level 4 Baþladý");
                // Saniye: 1.2 (Daha sýk), Hýz: 1.8 (Biraz daha hýzlý)
                spawner.ZorlukGuncelle(1.2f, 1.5f, true, true);
                LevelYazisiniGuncelle();
            }
        }
        // --- LEVEL 5 (600+ Puan) ---
        else if (toplamPuan >= 600)
        {
            if (suankiLevel != 5)
            {
                suankiLevel = 5;
                Debug.Log("Level 5 Baþladý - SON SEVÝYE");
                // Saniye: 0.8 (Çok sýk), Hýz: 2.2 (Çok hýzlý)
                spawner.ZorlukGuncelle(0.8f, 2f, true, true);
                LevelYazisiniGuncelle();
            }
        }
    }

    void LevelYazisiniGuncelle()
    {
        if (levelYazisi != null)
        {
            levelYazisi.text = "LEVEL " + suankiLevel;
            // Coroutine baþlatmak için gameObject'in aktif olmasý lazým
            if (levelYazisi.gameObject.activeInHierarchy == false)
                levelYazisi.gameObject.SetActive(true);

            StartCoroutine(YaziEfekti());
        }
    }

    IEnumerator YaziEfekti()
    {
        // Yazýyý göster
        levelYazisi.gameObject.SetActive(true);
        // 2 saniye bekle
        yield return new WaitForSecondsRealtime(2f);
        // Yazýyý gizle
        levelYazisi.gameObject.SetActive(false);
    }

    public void CanAzalt()
    {
        kalanCan--;
        GuncelleCanYazisi();
        if (kalanCan <= 0) OyunBitti();
    }

    public void CanKazan()
    {
        if (kalanCan < 100) // Sýnýr
        {
            kalanCan++;
            GuncelleCanYazisi();
        }
    }

    void GuncelleCanYazisi()
    {
        if (canYazisi != null) canYazisi.text = "LIVES: " + kalanCan.ToString();
    }

    void OyunBitti()
    {
        if (oyunBittiMi) return;
        oyunBittiMi = true;

        if (oyunBittiSesi != null) AudioSource.PlayClipAtPoint(oyunBittiSesi, Camera.main.transform.position);

        if (gameOverPaneli != null) gameOverPaneli.SetActive(true);

        if (skorYazisi != null) skorYazisi.gameObject.SetActive(false);
        if (levelYazisi != null) levelYazisi.gameObject.SetActive(false);
        if (canYazisi != null) canYazisi.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void TekrarDene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // --- BU KISMI EN ALTA YAPIÞTIR ---

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f; // Oyunu durdurduðumuz için zamaný tekrar açmalýyýz, yoksa menü donuk baþlar
        SceneManager.LoadScene("MainMenu"); // DÝKKAT: Senin menü sahnenin adý "MainMenu" olmalý
    }

    // Kazanma ekranýný açan yardýmcý fonksiyon
    void OyunKazanildi()
    {
        Debug.Log("Oyun Kazanýldý!");

        if (winPaneli != null) winPaneli.SetActive(true); // Paneli aç

        // Arkadaki yazýlarý gizle ki temiz görünsün
        if (skorYazisi != null) skorYazisi.gameObject.SetActive(false);
        if (levelYazisi != null) levelYazisi.gameObject.SetActive(false);
        if (canYazisi != null) canYazisi.gameObject.SetActive(false);

        // Zafer sesi çal
        if (kazanmaSesi != null) AudioSource.PlayClipAtPoint(kazanmaSesi, Camera.main.transform.position);

        Time.timeScale = 0f; // Oyunu dondur
    }
}