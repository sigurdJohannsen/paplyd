using BaseClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseSingleton<GameManager> {

    public List<Animal> animalList = new List<Animal>();
    public List<Sound> soundList = new List<Sound>();
    public List<Color> colorList = new List<Color>();

    private EventManager eventManager;
    
    void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        eventManager.OnCorrectSound += new OnCorrectSoundEventHandler(OnCorrectSound);
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        animalList.Clear();
        soundList.Clear();

        foreach (Animal animal in FindObjectsOfType<Animal>()) {
            animalList.Add(animal);
        }
        foreach (Sound sound in FindObjectsOfType<Sound>()) {
            soundList.Add(sound);
            sound.dinglingColor = colorList[soundList.Count - 1];
        }

        if (animalList.Count != 0) {
            eventManager.InvokeGameLevelLoaded();
        }
    }

    private void OnCorrectSound() {
        //  Check if all sounds were correctly placed on all animals!
        int correctAnimals = 0;

        foreach (Animal animal in FindObjectsOfType<Animal>()) {
            if ((animal.animalName == animal.soundAttached.name)) {
                correctAnimals++;
            }
        }

        if (correctAnimals == animalList.Count) {
            eventManager.InvokeGameLevelCompleted();
        }
    }
}
