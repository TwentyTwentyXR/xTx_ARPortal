using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button prevSpawnButton, nextSpawnButton, clearSpawnButton;

    public TextMeshProUGUI currSpawn, currWorld, planeState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        currSpawn.text = WorldManager.s_instance.arController.GetCurrentObjectName();
        currWorld.text = WorldManager.s_instance.GetCurrentWorldName();

        if(WorldManager.s_instance.arController.spawnableObjects.Count == 0)
        {
            prevSpawnButton.interactable = false;
            nextSpawnButton.interactable = false;
            clearSpawnButton.interactable = false;
        }
        else
        {
            clearSpawnButton.interactable = true;
            prevSpawnButton.interactable = true;
            nextSpawnButton.interactable = true;
        }
    }

    public void PrevObject()
    {
        WorldManager.s_instance.arController.ChangeToPrevObject();
    }

    public void NextObject()
    {
        WorldManager.s_instance.arController.ChangeToNextObject();
    }

    public void ClearObjects()
    {
        WorldManager.s_instance.arController.ClearARObjects();
    }

    public void SpawnPlaneSwitcher()
    {
        WorldManager.s_instance.PlaneGeneratorSwitch();
        if(WorldManager.s_instance.planeGenerator.activeSelf)
        {
            planeState.text = "ON";
        }
        else
        {
            planeState.text = "OFF";
        }
    }
}
