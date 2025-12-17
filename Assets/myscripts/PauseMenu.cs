using UnityEngine;
using UnityEngine.SceneManagement; // Menüye dönmek istersen gerekli

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Oyunun durup durmadýðýný kontrol eden deðiþken
    public GameObject pauseMenuUI; // Inspector'dan atayacaðýmýz Panel

    void Update()
    {
        // "ESC" tuþuna basýnca da çalýþmasý için (Opsiyonel)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Oyunu Devam Ettir
    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Paneli gizle
        Time.timeScale = 1f; // Zamaný normal hýzýna getir
        GameIsPaused = false;
    }

    // Oyunu Durdur
    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Paneli göster
        Time.timeScale = 0f; // Zamaný dondur (Fizik ve hareket durur)
        GameIsPaused = true;
    }

    // Ana Menüye Dön (Opsiyonel)
    public void LoadMenu()
    {
        Time.timeScale = 1f; // Menüye dönerken zamaný tekrar açmayý unutma!
        SceneManager.LoadScene("MainMenu"); // "Menu" senin sahne adýn olmalý
    }

    // Oyundan Çýk
    public void QuitGame()
    {
        Debug.Log("Oyundan çýkýlýyor...");
        Application.Quit();
    }
}