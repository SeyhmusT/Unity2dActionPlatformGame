using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private string ID;
    [SerializeField] private int ammo;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shooting shooting))
        {
            shooting.AddStorageAmmo(ID, ammo);
            source.PlayOneShot(clip);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
