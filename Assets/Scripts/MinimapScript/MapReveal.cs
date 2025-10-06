using UnityEngine;

public class MapReveal : MonoBehaviour
{
    [SerializeField] string mapKey;
    private MinimapDisplayControl mapDisplayControl;
    void Start()
    {
        mapDisplayControl = FindAnyObjectByType<MinimapDisplayControl>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mapDisplayControl.LoadMinimapData();
            mapDisplayControl.minimapData.AddToListWitchCheck(mapKey);
            mapDisplayControl.DisplayUnlockedMinimaps();
        }
    }
}
