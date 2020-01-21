using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public string worldName;

    public Transform worldObjects;

    public PortalController portalController;

    public List<ARObject> portalSpawnableObjects;
    public int portalSpawnableIndex = 0;

    public void Awake()
    {
        SetWorldObjects(true);
    }

    public void SetWorldObjects(bool state)
    {
        foreach(Transform child in worldObjects)
        {
            child.gameObject.SetActive(state);
        }

        portalController.transform.gameObject.SetActive(state);

    }
}
