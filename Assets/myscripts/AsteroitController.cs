using UnityEngine;

public class AsteroitController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // SENARYO 1: Eðer çarpan þey MERMÝ ise (Puaný burada veriyoruz)
        if (other.gameObject.CompareTag("mermi"))
        {
            // --- DÜZELTME BURADA ---
            // Sadece mermi vurunca puan kazanýlýr
            GameManager yonetici = FindObjectOfType<GameManager>();
            if (yonetici != null) yonetici.PuanKazan(10);
            // -----------------------

            Destroy(other.gameObject); // Mermiyi yok et
            Destroy(gameObject);       // Taþý yok et
        }

        // SENARYO 2: Eðer çarpan þey PLAYER (GEMÝ) ise (Puan yok, sadece hasar)
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);       // Taþý yok et
            Debug.Log("Gemi hasar aldý!"); // (Buraya ilerde CanAzalt eklenecek)
        }
    }
}