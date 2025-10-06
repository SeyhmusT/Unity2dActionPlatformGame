using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnControl : MonoBehaviour
{
    private Transform player;
    [SerializeField] private SpawnIdentifier[] spawnPoints;
    [SerializeField] private SpawnIdentifier[] spawnCheckPoints;
    private SpawnData spawnData = new SpawnData();
    private CheckpointData checkData = new CheckpointData();
    private bool canLoadFromCheckpoint = false;
    void Start()
    {
        //SaveLoadManager.instance.DeleteFolder(SaveLoadManager.instance.folderName);
        player = FindAnyObjectByType<Player>().transform;
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if (File.Exists(loadPath))
        {
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            if (checkData.sceneToLoad == SceneManager.GetActiveScene().name)
            {
                canLoadFromCheckpoint = true;
            }
        }
        if (SpawnMode.spawnFromCheckPoint == true && canLoadFromCheckpoint == true)
            {
                foreach (SpawnIdentifier spawnID in spawnCheckPoints)
                {
                    if (spawnID.spawnKey == checkData.checkPointKey)
                    {
                        player.transform.position = spawnID.transform.position;
                        break;
                    }
                }
                if (checkData.facingRight == false)
                {
                    player.GetComponent<Player>().ForceFlip();
                }
                SpawnMode.spawnFromCheckPoint = false;

            }
            else
            {
                SaveLoadManager.instance.Load(spawnData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
                foreach (SpawnIdentifier spawnID in spawnPoints)
                {
                    if (spawnID.spawnKey == spawnData.spawnPointKey)
                    {
                        player.transform.position = spawnID.transform.position;
                        break;
                    }
                }
                if (spawnData.facingRight == false)
                {
                    player.GetComponent<Player>().ForceFlip();
                }
                SpawnMode.spawnFromCheckPoint = false;
            }
       
    }

    
}
