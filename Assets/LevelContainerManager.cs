using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;
using System;
/// <summary>
/// This manager needs to be set in the scene, and setup accordingly for the level to function right
/// </summary>
public class LevelContainerManager : BaseSingleton<LevelContainerManager>{

    public List<SoundContainer> Animals;

    public int TotalAnimals;
    public int TotalCorrectAnimals;
    private void OnValidate()
    {
        if (Animals.Count != 0)
            return;
        Animals.AddRange(FindObjectsOfType<SoundContainer>());
        TotalAnimals = Animals.Count;
        CheckWin();
    }

    public void Start()
    {
        OnValidate();
    }
    public void CheckWin()
    {
        TotalCorrectAnimals = 0;
        foreach (var animal in Animals)
        {
            if (animal.Animal == animal.Sound.Sound)
                TotalCorrectAnimals++;
        }
        if (TotalAnimals == TotalCorrectAnimals)
            Win();
    }

    private void Win()
    {
     
    }
}
