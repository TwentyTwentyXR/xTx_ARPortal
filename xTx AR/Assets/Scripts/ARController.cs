using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore;
using TMPro;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    public List<ARObject> spawnableObjects;
    public int spawnableIndex = 0;

    private GameObject lastSpawnedObject;

    private const float k_PrefabRotation = 180.0f;

    private bool m_IsQuitting = false;

    public void Awake()
    {
        Application.targetFrameRate = 60;

    }

    public void Update()
    {
        _UpdateApplicationLifecycle();

        if (spawnableObjects.Count == 0 || !spawnableObjects[spawnableIndex] || !WorldManager.s_instance.planeGenerator.activeSelf)
        {
            return;
        }
      

        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Should not handle input if the player is pointing on UI.
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(WorldManager.s_instance.player.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                // Choose the prefab based on the Trackable that got hit.
                GameObject prefab;
                if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                    {
                        //prefab = GameObjectVerticalPlanePrefab;
                        return;
                    }
                    else
                    {
                        prefab = spawnableObjects[spawnableIndex].arGameObjectPrefab;
                    }
                }
                else
                {
                    prefab = spawnableObjects[spawnableIndex].arGameObjectPrefab;
                }

                // Instantiate prefab at the hit pose.

                //Debug.Log(spawnableObjects[spawnableIndex].transform.childCount);

                if (spawnableObjects[spawnableIndex].spawnedObjects.Count < 1 || spawnableObjects[spawnableIndex].bCanSpawnMultiple)
                {
                    lastSpawnedObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

                    spawnableObjects[spawnableIndex].spawnedObjects.Add(lastSpawnedObject);

                    if (lastSpawnedObject.GetComponent<WorldController>())
                    {
                        WorldManager.s_instance.activePortals.Add(lastSpawnedObject.GetComponent<WorldController>());
                    }
                }
                else
                {
                    lastSpawnedObject = spawnableObjects[spawnableIndex].spawnedObjects[0];

                    var oldAnchor = lastSpawnedObject.transform.parent;

                    lastSpawnedObject.transform.parent = null;

                    Destroy(oldAnchor.gameObject);

                    lastSpawnedObject.transform.position = hit.Pose.position;

                    lastSpawnedObject.transform.rotation = hit.Pose.rotation;
                }


                
                // Compensate for the hitPose rotation facing away from the raycast (i.e.
                // camera).
                lastSpawnedObject.transform.Rotate(0, k_PrefabRotation, 0, Space.Self);

                // Create an anchor to allow ARCore to track the hitpoint as understanding of
                // the physical world evolves.
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make game object a child of the anchor.
                lastSpawnedObject.transform.parent = anchor.transform;
            }
        }
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    public void SetGameObject(ARObject newObject)
    {

    }

    public void SetUpARController(List<ARObject> list, int index)
    {
        lastSpawnedObject = null;

        spawnableObjects = list;

        spawnableIndex = index;

        WorldManager.s_instance.uiController.UpdateUI();
    }

    public string GetCurrentObjectName()
    {
        if(spawnableObjects.Count > 0 && spawnableObjects[spawnableIndex])
        {
            return spawnableObjects[spawnableIndex].name;
        }
        else
        {
            return "N/A";
        }
    }

    public void ChangeToPrevObject()
    {
        if(spawnableIndex < 1)
        {
            spawnableIndex = spawnableObjects.Count - 1;
        }
        else
        {
            spawnableIndex -= 1;
        }

        WorldManager.s_instance.uiController.UpdateUI();
    }

    public void ChangeToNextObject()
    {
        spawnableIndex = (spawnableIndex + 1) % spawnableObjects.Count;

        WorldManager.s_instance.uiController.UpdateUI();
    }

    public void ClearARObjects()
    {
        if (spawnableObjects == null || spawnableObjects.Count == 0)
        {
            return;
        }

        if (spawnableObjects[0].bIsPortal)
        {
            WorldManager.s_instance.activePortals.Clear();
        }

        foreach (ARObject obj in spawnableObjects)
        {
            if(obj.GetComponent<WorldController>())
            {

            }

            for(int i = 0; i < obj.spawnedObjects.Count; i++)
            {
                var oldAnchor = obj.spawnedObjects[i].transform.parent;

                obj.spawnedObjects[i].transform.parent = null;

                Destroy(oldAnchor.gameObject);

                Destroy(obj.spawnedObjects[i]);
            }
            obj.spawnedObjects.Clear();
        }

    }

}
