using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        player.DeactivateCurrentWeapon();
        SpawnMode.spawnFromCheckPoint = true;
    
        player.gatherInput?.DisablePlayerMap();
        linkedPhysics.ResetVelocity();
        
        // Oyuncu collider'larını devre dışı bırak ki düşmanlar artık vuramaya devam etmesin
        player.playerStats.DisableStatsCollider();
        
        if (linkedPhysics.grounded)
            linkedAnimator.SetBool("Death", true);
        else
        {
            // air death animation needed
            linkedAnimator.SetBool("Death", true);
        }
    }
    

    public void ResetGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
        if (File.Exists(loadPath))
        {
            CheckpointData checkData = new CheckpointData();
            SaveLoadManager.instance.Load(checkData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckPoint);
            LevelManager.instance.LoadLevelString(checkData.sceneToLoad);

        }
        else
        {
            LevelManager.instance.RestartLevel();
 
        }
    }
}
