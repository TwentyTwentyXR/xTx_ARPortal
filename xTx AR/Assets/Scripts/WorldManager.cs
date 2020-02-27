using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldManager : MonoBehaviour
{
    public static WorldManager s_instance;

    public Transform player;
    public ARController arController;
    public GameObject planeGenerator;
    public UIController uiController;
    
    public List<WorldController> activePortals;
    public WorldController currPortal;


    public List<ARObject> portalPrefabs;
    public int portalIndex = 0;

    public bool bInRealWorld = true;

    public Material generalSkybox;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(s_instance.gameObject);
            s_instance = this;
        }

        if(!player)
            player = Camera.main.transform;

        var stencilTest = false ? CompareFunction.NotEqual : CompareFunction.Equal;
        var portalStencil = !false ? CompareFunction.NotEqual : CompareFunction.Equal;

        Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
        Shader.SetGlobalInt("_PortalStencil", (int)portalStencil);

        OnReturnToEarth();

        /*
        foreach (WorldController wc in worlds)
        {
            wc.worldObjects.gameObject.SetActive(true);
        }
        */
    }

    public void Start()
    {
        //ReactivateWorlds();
    }

    public void PortalTriggered(bool inOtherWorld)
    {
        bInRealWorld = !inOtherWorld; 
        if(inOtherWorld)
        {
            DeactiveAllOtherWorlds();
        }
        else
        {
            ReactivateWorlds();
        }
    }

    public void ReactivateWorlds()
    {
        foreach(WorldController wc in activePortals)
        {
            wc.SetWorldObjects(true);
        }
    }

    public void DeactiveAllOtherWorlds()
    {
        foreach (WorldController wc in activePortals)
        {
            if(!wc.portalController.inOtherWorld) 
                wc.SetWorldObjects(false);
        }
    }

    public string GetCurrentWorldName()
    {
        if(currPortal)
        {
            return currPortal.worldName;
        }
        else
        {
            return "Earth";
        }
    }

    public void OnReturnToEarth()
    {
        arController.SetUpARController(portalPrefabs, portalIndex);
    }

    public void PlaneGeneratorSwitch()
    {
        planeGenerator.SetActive(!planeGenerator.activeSelf);
    }
}
