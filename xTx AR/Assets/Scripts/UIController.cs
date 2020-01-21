using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button prevSpawnButton, nextSpawnButton;

    public TextMeshProUGUI currSpawn, currWorld;

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
}
