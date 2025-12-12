using UnityEngine;

public class DusmanMermisi : MonoBehaviour
{
    public float hiz = 10f;

    void Start()
    {
        // 3 saniye sonra sahneden silinsin (Performans için)
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // DÝKKAT: Resim zaten aþaðý baktýðý ve biz onu döndürmediðimiz için
        // ona direkt "Aþaðý (Down) Git" diyoruz.
        transform.Translate(Vector2.down * hiz * Time.deltaTime);
    }
}