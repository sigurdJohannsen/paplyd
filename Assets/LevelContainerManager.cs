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

    public GameObject WinScreen;
    public GameObject VidereObj;
    public int TotalAnimals;
    public int TotalCorrectAnimals;

    private void OnValidate()
    {
        Setup();
    }
    public void Setup()
    {
        if (Animals.Count != 0)
            return;
        Animals.AddRange(FindObjectsOfType<SoundContainer>());
        TotalAnimals = Animals.Count;
        CheckWin();
    }
    public void Start()
    {
        Setup();
    }
    public void CheckWin()
    {
        TotalCorrectAnimals = 0;
        foreach (var animal in Animals)
        {
            animal.GetComponent<Animator>().SetBool("Correct", animal.Animal == animal.Sound.Sound);
            if (animal.Animal == animal.Sound.Sound)
            {
                TotalCorrectAnimals++;
            }
        }
        if (TotalAnimals == TotalCorrectAnimals)
            StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(2.0f);
        WinScreen.SetActive(true);
        while(WinScreen.GetComponent<CanvasGroup>().alpha < 1.0f)
        {
            WinScreen.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
            yield return null;
        }
        VidereObj.SetActive(true);
    }
    public void CloseWinScreen()
    {
        WinScreen.SetActive(false);
    }
    public void Videre()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
