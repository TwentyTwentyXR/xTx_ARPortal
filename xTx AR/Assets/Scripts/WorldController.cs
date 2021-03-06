﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public string worldName;
    public int worldValue = 1;
    public Material skyBoxMaterial;

    public Transform worldObjects;

    public PortalController portalController;

    public List<ARObject> portalSpawnableObjects;
    public int portalSpawnableIndex = 0;

    public GameObject tempSkybox;
    
    public void Awake()
    {
        SetWorldObjects(true);
    }

    public void SetWorldObjects(bool state)
    {
        Debug.Log(state);
        foreach(Transform child in worldObjects)
        {
            child.gameObject.SetActive(state);
        }

        portalController.transform.gameObject.SetActive(state);

    }

    public void Transition(bool inOtherWorld)
    {
        
        for (int i = 0; i < portalSpawnableObjects.Count; i++)
        {
            for (int ii = 0; ii < portalSpawnableObjects[i].spawnedObjects.Count; ii++)
            {
                portalSpawnableObjects[i].spawnedObjects[ii].SetActive(inOtherWorld);
            }
        }

        if (tempSkybox) tempSkybox.SetActive(!inOtherWorld);

        if (inOtherWorld)
        {
            RenderSettings.skybox = skyBoxMaterial;
        }
        else
        {
            RenderSettings.skybox = WorldManager.s_instance.generalSkybox;
        }
    }
}
