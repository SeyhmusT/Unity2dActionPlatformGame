using System.Collections.Generic;

[System.Serializable]
public class ExampleData
{
    public int exampleInt;
    public string exampleString;
}
[System.Serializable]
public class SpawnData
{
    public string spawnPointKey;
    public bool facingRight;
    public SpawnData()
    {
        spawnPointKey = "Start";
        facingRight = true;
    }
}
[System.Serializable]
public class CheckpointData
{
    public string sceneToLoad;
    public string checkPointKey;
    public bool facingRight;
    public CheckpointData()
    {
        sceneToLoad = "Level 1";
        checkPointKey = "Check1";
        facingRight = true;

    }
}
[System.Serializable]
public class MinimapData
{
    public List<string> mapKeys = new List<string>();
    public void AddToListWitchCheck(string keyToAdd)
    {
        if (mapKeys.Contains(keyToAdd))
            return;

        mapKeys.Add(keyToAdd);
    }

}

[System.Serializable]

public class WeaponData
{
    public string ID;
    public int currentAmmo;
    public int storageAmmo;
}


