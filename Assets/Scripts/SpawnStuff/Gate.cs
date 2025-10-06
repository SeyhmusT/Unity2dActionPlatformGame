using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    public SpawnData spawnDataForOtherLevel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Yeni bölüm için spawn point'i kaydet (genellikle başlangıç noktası)
            SaveLoadManager.instance.Save(spawnDataForOtherLevel, SaveLoadManager.instance.folderName,SaveLoadManager.instance.fileName);
            
            // Checkpoint dosyasını sil (yeni bölümde checkpoint yok)
            SaveLoadManager.instance.DeleteSaveFile(SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            
            Player player = collision.GetComponent<Player>();
            player.gatherInput.DisablePlayerMap();
            player.physicsControl.ResetVelocity();
            LevelManager.instance.LoadLevelString(levelToLoad);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
