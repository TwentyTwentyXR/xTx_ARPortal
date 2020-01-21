using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalController : MonoBehaviour
{
    protected Transform device;

    public WorldController worldController;

    protected bool wasInFront;

    public bool inOtherWorld;

    protected bool hasCollided;

    public string tagUI;

    void Start()
    {
        device = WorldManager.s_instance.player;
        SetMaterials(false);
    }

    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
        var portalStencil = !fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;

        Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
        Shader.SetGlobalInt("_PortalStencil", (int)portalStencil);
        //foreach (var mat in materials)
        //{
        //   mat.SetInt("_StencilTest", (int)stencilTest);
        //}
    }

    void Update()
    {
        WhileCameraColliding();
    }

    bool GetIsInFront()
    {
        Vector3 worldPos = device.position + device.forward * Camera.main.nearClipPlane; 
        Vector3 pos = transform.InverseTransformPoint(worldPos);

        return pos.z >= 0 ? true: false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != device)
        {
            return;
        }

        wasInFront = GetIsInFront();
        hasCollided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != device)
        {
            return;
        }

        hasCollided = false;
    }

    private void WhileCameraColliding()
    {
        if (!hasCollided)
        {
            return;
        }
            

        bool isInFront = GetIsInFront();

        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);

            WorldManager.s_instance.PortalTriggered(inOtherWorld);

            if (inOtherWorld)
            {
                OnEnterWorld();
            }
            else
            {
                OnExitWorld();
            }
            
        }

        wasInFront = isInFront;
    }

    private void OnDestroy()
    {
        SetMaterials(false);
    }

    protected virtual void OnEnterWorld() 
    {
        Debug.Log("Enter");
        WorldManager.s_instance.currPortal = worldController;
        WorldManager.s_instance.arController.SetUpARController(worldController.portalSpawnableObjects, 
                                                               worldController.portalSpawnableIndex);
    }
    protected virtual void OnExitWorld() 
    {
        Debug.Log("Exit");
        WorldManager.s_instance.currPortal = null;
        WorldManager.s_instance.OnReturnToEarth();
    }

}
