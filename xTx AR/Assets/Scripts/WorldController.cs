using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public Transform worldObjects;

    public PortalController portalController;
    
    public void SetWorldObjects(bool state)
    {
        foreach(Transform child in worldObjects)
        {
            child.gameObject.SetActive(state);
        }

        portalController.transform.gameObject.SetActive(state);
    }
}
