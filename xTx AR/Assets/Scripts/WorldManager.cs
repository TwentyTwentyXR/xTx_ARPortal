using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager s_instance;
    public Transform player;
    public List<WorldController> worlds;

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

        foreach (WorldController wc in worlds)
        {
            wc.worldObjects.gameObject.SetActive(true);
        }
    }

    public void Start()
    {
        ReactivateWorlds();
    }

    public void PortalTriggered(bool inOtherWorld)
    {
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
        foreach(WorldController wc in worlds)
        {
            wc.SetWorldObjects(true);
        }
    }

    public void DeactiveAllOtherWorlds()
    {
        foreach (WorldController wc in worlds)
        {
            if(!wc.portalController.inOtherWorld) 
                wc.SetWorldObjects(false);
        }
    }
}
