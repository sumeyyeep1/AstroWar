using UnityEngine;

public class CanIksiri : MonoBehaviour
{
    private float hiz = 1.2f; // Baþlangýçta çok yavaþ (Normali 3 idi)
    public AudioClip toplamaSesi;
    void Start()
    {
        // GameManager'ý bulup levele göre hýzý çok az arttýralým
        GameManager yonetici = FindObjectOfType<GameManager>();

        if (yonetici != null)
        {
            // Level baþýna sadece 0.2 birim hýz ekle (Çok yavaþ artýþ)
            // Örnek: Level 1=1.7f, Level 5=2.5f (Hala yavaþ)
            hiz += (yonetici.suankiLevel * 0.2f);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }
}