using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    [Header("Spawn")]
    public string folderName = "SaveFiles";
    public string fileName = "SpawnPoint.json";

    [Header("Checkpoint")]
    public string fileCheckPoint = "CheckPoint.json";

    [Header("Minimap")]
    public string minimapFileName = "Minimap.json";
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Save<T>(T dataToSave, string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }
    public void Load<T>(T dataToLoadInto, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(loadPath))
        {
            string result = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(result, dataToLoadInto);
        }
    }

    public void DeleteSaveFile(string folderName, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
    }
    public void DeleteFolder(string folderName)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

    public void SaveExample(ExampleData dataToSave, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }

    public void LoadExample(ExampleData dataToLoadInto, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, dataToLoadInto);
        }
    }

    public void DeleteExample(string fileNmae)
    {
        string dataPath = Path.Combine(Application.persistentDataPath, fileNmae);
        File.Delete(dataPath);
    }
}
