using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationAnimation : MonoBehaviour {


    [Range (0,1)]
    public float VibrationVolume;

    private int animationID;
    private float animationVibrationLevel;
    public List<Transform> ListOfVibrationElements = new List<Transform>();
    private List<Vector3> originalPositions = new List<Vector3>();
    public void ResetPositions()
    {
        for (int i = 0; i < ListOfVibrationElements.Count; i++)
        {
            ListOfVibrationElements[i].localPosition = originalPositions[i];
        }
    }
    public void Awake()
    {
        originalPositions = new List<Vector3>();
        for (int i = 0; i < ListOfVibrationElements.Count; i++)
        {
            originalPositions.Add(ListOfVibrationElements[i].localPosition);
        }
    }
    public void Vibrate(int animation)
    {
        animationID = animation;
        ResetPositions();
        switch (animation)
        {
            case 0:
                animationVibrationLevel = 0f;
                break;
            case 1:
                animationVibrationLevel = 1.5f;
                break;
            case 2:
                animationVibrationLevel = 1.0f;
                break;
            case 3:
                animationVibrationLevel = 0.5f;
                break;
            default:
                break;
        }
    }
    private void LateUpdate()
    {
        if(animationID != 0)
        {
            VibrateAnimation(animationVibrationLevel);
        }

    }
    private void VibrateAnimation(float VibrateAmount)
    {
        
        for (int i = 0; i < ListOfVibrationElements.Count; i++)
        {
            ListOfVibrationElements[i].localPosition +=  Random.insideUnitSphere * VibrateAmount * VibrationVolume;
        }
    }
}
