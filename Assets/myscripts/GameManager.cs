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
        if (skorYazisi != null) skorYazisi.text = "SKOR: " + toplamPuan.ToString();

        // --- DÜZELTME 3: Fonksiyonun doðru adýný yazýyoruz ---
        LevelKontrol();
    }

    void LevelKontrol()
    {
        // Spawner yoksa hata vermesin diye güvenlik önlemi
        if (spawner == null) return;

        // --- DÜZELTME 2: 'skor' yerine 'toplamPuan' yazýyoruz ---
        if (toplamPuan < 100) // --- LEVEL 1 ---
        {
            if (suankiLevel != 1)
            {
                suankiLevel = 1;
                Debug.Log("Level 1 Baþladý");
                spawner.ZorlukGuncelle(3.0f, 2.0f, false, false);
                LevelYazisiniGuncelle(); // Level yazýsý çýksýn
            }
        }
        else if (toplamPuan >= 100 && toplamPuan < 250) // --- LEVEL 2 ---
        {
            if (suankiLevel != 2)
            {
                suankiLevel = 2;
                Debug.Log("Level 2 Baþladý");
                spawner.ZorlukGuncelle(2.0f, 5.0f, false, true);
                LevelYazisiniGuncelle();
            }
        }
        else if (toplamPuan >= 250) // --- LEVEL 3 ---
        {
            if (suankiLevel != 3)
            {
                suankiLevel = 3;
                Debug.Log("Level 3 Baþladý");
                spawner.ZorlukGuncelle(1.5f, 5.0f, true, true);
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
        if (canYazisi != null) canYazisi.text = "CAN: " + kalanCan.ToString();
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
}