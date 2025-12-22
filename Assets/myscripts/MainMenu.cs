using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçiþi yapmak için gerekli

// Bu script ana menüdeki butonlarý kontrol eder
// (Oyuna baþla – Oyundan çýk)
public class MainMenu : MonoBehaviour
{
    // -------------------- OYUNA BAÞLA --------------------
    public void OyunaBasla()
    {
        // Oyun sahnesini yükler
        // "SampleScene" Build Settings'teki sahne adýyla birebir ayný olmalý
        // Eðer sahne adýný deðiþtirirsek burada da deðiþtirmemiz gerekir
        SceneManager.LoadScene("SampleScene");
    }

    // -------------------- OYUNDAN ÇIK --------------------
    public void OyundanCik()
    {
        // Editör modundayken konsolda görmek için
        Debug.Log("Oyundan Çýkýldý!");

        // Build alýnmýþ oyunda uygulamayý tamamen kapatýr
        // Unity Editor'de çalýþmaz, normaldir
        Application.Quit();
    }
}
