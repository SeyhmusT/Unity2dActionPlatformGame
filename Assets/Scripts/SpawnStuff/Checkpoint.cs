using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriter;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private CheckpointData checkPointData;
    private void Start()
{
    string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
    if (File.Exists(loadPath))
    {
        CheckpointData helpCheck = new CheckpointData();
        SaveLoadManager.instance.Load(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if (helpCheck.checkPointKey == checkPointData.checkPointKey && 
            helpCheck.sceneToLoad == SceneManager.GetActiveScene().name)
        {
            spriter.sprite = spriteEnabled;
        }
    }
}

public void Activate()
{
    spriter.sprite = spriteEnabled;
    checkPointData.sceneToLoad = SceneManager.GetActiveScene().name;  // Sahne bilgisini güncelle
    SaveLoadManager.instance.Save(checkPointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
}

    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = this;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckpoint>().checkPoint = null;

        }
    }

   
}
