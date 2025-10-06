using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MinimapDisplayControl : MonoBehaviour
{
    [SerializeField] private string firstMinimapToReveal;
    [SerializeField] private List<MinimapID> minimapIDs = new List<MinimapID>();
    public MinimapData minimapData = new MinimapData();
    private string loadPath;
    void Start()
    {
        LoadMinimapData();
        DisplayUnlockedMinimaps();
    }

    public void LoadMinimapData()
    {
        loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.minimapFileName);
        if (File.Exists(loadPath))
        {
            SaveLoadManager.instance.Load(minimapData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.minimapFileName);
        }
    }

    public void DisplayUnlockedMinimaps()
    {
        if (minimapData.mapKeys.Count == 0)
        {
            // decide what to do
            foreach (MinimapID mapID in minimapIDs)
            {
                if (mapID.mapKey == firstMinimapToReveal)
                {
                    mapID.gameObject.SetActive(true);
                    minimapData.AddToListWitchCheck(firstMinimapToReveal);
                    break;
                }
            }
        }
        else
        {
            foreach (string key in minimapData.mapKeys)
            {
                foreach (MinimapID mapID in minimapIDs)
                {
                    if (key == mapID.mapKey)
                    {
                        mapID.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        SaveLoadManager.instance.Save(minimapData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.minimapFileName);

    }


}
