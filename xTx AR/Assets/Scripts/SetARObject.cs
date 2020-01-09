using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetARObject : MonoBehaviour
{
    public ARController controller;
    public GameObject arObject;

    public void ChangeCurrentARObject()
    {
        controller.SetGameObject(arObject);
    }
}
