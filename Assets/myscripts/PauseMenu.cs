using UnityEngine;
using UnityEngine.SceneManagement; // Sahne deðiþtirmek (menüye dönmek) için gerekli

// Bu script oyunu durdurma (pause), devam ettirme (resume)
// menüye dönme ve oyundan çýkma iþlemlerini kontrol eder
public class PauseMenu : MonoBehaviour
{
    // Oyun þu anda duraklatýldý mý?
    // static olduðu için her yerden eriþilebilir
    public static bool GameIsPaused = false;

    // Pause menü paneli (Canvas içindeki panel)
    // Inspector'dan atanýr
    public GameObject pauseMenuUI;

    void Update()
    {
        // ESC tuþuna basýldýðýnda pause aç/kapa yapýlýr
        // (Buton dýþýnda klavye desteði olsun diye)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Eðer oyun zaten duruyorsa devam ettir
            if (GameIsPaused)
            {
                Resume();
            }
            // Eðer oyun çalýþýyorsa durdur
            else
            {
                Pause();
            }
        }
    }

    // -------------------- OYUNA DEVAM --------------------
    public void Resume()
    {
        // Pause menü panelini gizle
        pauseMenuUI.SetActive(false);

        // Zamaný normale döndür
        // (hareket, fizik, Update tekrar çalýþýr)
        Time.timeScale = 1f;

        // Oyun artýk durmuyor
        GameIsPaused = false;
    }

    // -------------------- OYUNU DURDUR --------------------
    public void Pause()
    {
        // Pause menü panelini göster
        pauseMenuUI.SetActive(true);

        // Zamaný tamamen durdur
        // (Time.deltaTime = 0 olur)
        Time.timeScale = 0f;

        // Oyun duraklatýldý
        GameIsPaused = true;
    }

    // -------------------- ANA MENÜYE DÖN --------------------
    public void LoadMenu()
    {
        // Menüye geçerken zamaný mutlaka aç
        // Yoksa menü de donmuþ olur
        Time.timeScale = 1f;

        // Ana menü sahnesini yükle
        // Sahne adý Build Settings ile birebir ayný olmalý
        SceneManager.LoadScene("MainMenu");
    }

    // -------------------- OYUNDAN ÇIKIÞ --------------------
    public void QuitGame()
    {
        // Editörde test ederken görmek için
        Debug.Log("Oyundan çýkýlýyor...");

        // Build alýnmýþ oyunda uygulamayý kapatýr
        Application.Quit();
    }
}
