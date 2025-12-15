using UnityEngine;
using UnityEngine.SceneManagement; // Sahne deðiþimi için þart

public class MainMenu : MonoBehaviour
{
    public void OyunaBasla()
    {
        // "SampleScene" senin oyununun olduðu sahnenin adýdýr. 
        // Eðer adýný deðiþtirdiysen buraya o adý yazmalýsýn.
        SceneManager.LoadScene("SampleScene");
    }

    public void OyundanCik()
    {
        Debug.Log("Oyundan Çýkýldý!");
        Application.Quit();
    }
}