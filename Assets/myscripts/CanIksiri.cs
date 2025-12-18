using UnityEngine;

public class CanIksiri : MonoBehaviour
{
    public float hiz = 3f;

    // --- SES DEÐÝÞKENÝ ---
    public AudioClip toplamaSesi;
    // ---------------------

    void Update()
    {
        transform.Translate(Vector2.down * hiz * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

   
}