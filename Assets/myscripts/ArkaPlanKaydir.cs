using UnityEngine;

public class ArkaPlanKaydir : MonoBehaviour
{
    public float hiz = 0.5f; // Kayma hýzý
    public float bitisY = -10f; // Resim nerede bitiyor?
    public float baslangicY = 10f; // Nereye ýþýnlanacak?

    void Update()
    {
        // Aþaðý doðru kaydýr
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        // Eðer çok aþaðý indiyse, tekrar yukarý ýþýnla (Döngü)
        if (transform.position.y <= bitisY)
        {
            transform.position = new Vector2(transform.position.x, baslangicY);
        }
    }
}