using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;

public class UtilitiesManager : BaseSingleton<UtilitiesManager>{

    private Camera mainCamera;

    public Camera MainCamera
    {
        get
        {
            if (!mainCamera)
                mainCamera = Camera.main;
            return mainCamera;
        }
    }
}
