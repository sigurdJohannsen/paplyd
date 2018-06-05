using BaseClasses;
using UnityEngine;

public delegate void OnDraggingStartedEventHandler(Animal animal);
public delegate void OnDraggingEndedEventHandler();
public delegate void OnSoundsSwappedEventHandler(Transform origin, Transform target, SoundSwap soundSwapper);
public delegate void OnSwappedSoundReachedDestinationEventHandler();
public delegate void OnCorrectSoundEventHandler();
public delegate void OnWrongSoundEventHandler();
public delegate void OnAnimalWasClickedEventHandler(Animal animal);
public delegate void OnGameLevelLoadedEventHandler();

public class EventManager : BaseSingleton<EventManager> {

    public event OnDraggingStartedEventHandler OnDraggingStarted;
    public event OnDraggingEndedEventHandler OnDraggingEnded;
    public event OnSoundsSwappedEventHandler OnSoundsSwapped;
    public event OnSwappedSoundReachedDestinationEventHandler OnSwappedSoundReachedDestination;
    public event OnCorrectSoundEventHandler OnCorrectSound;
    public event OnWrongSoundEventHandler OnWrongSound;
    public event OnAnimalWasClickedEventHandler OnAnimalWasClicked;
    public event OnGameLevelLoadedEventHandler OnGameLevelLoaded;

    public void InvokeDraggingStarted(Animal animal) {
        if (OnDraggingStarted != null) {
            //Debug.Log("EVENT MANAGER: InvokeDraggingStarted");
            OnDraggingStarted(animal);
        }
    }

    public void InvokeDraggingEnded() {
        if (OnDraggingEnded != null) {
            //Debug.Log("EVENT MANAGER: InvokeDraggingEnded");
            OnDraggingEnded();
        }
    }

    public void InvokeSoundsSwapped(Transform origin, Transform target, SoundSwap soundSwapper) {
        if (OnSoundsSwapped != null) {
            //Debug.Log("EVENT MANAGER: InvokeSoundsSwapped");
            OnSoundsSwapped(origin, target, soundSwapper);
        }
    }
    
    public void InvokeSwappedSoundReachedDestination() {
        if (OnSwappedSoundReachedDestination != null) {
            //Debug.Log("EVENT MANAGER: InvokeSwappedSoundReachedDestination");
            OnSwappedSoundReachedDestination();
        }
    }

    public void InvokeCorrectSound() {
        if (OnCorrectSound != null) {
            //Debug.Log("EVENT MANAGER: InvokeCorrectSound");
            OnCorrectSound();
        }
    }

    public void InvokeWrongSound() {
        if (OnWrongSound != null) {
            //Debug.Log("EVENT MANAGER: InvokeWrongSound");
            OnWrongSound();
        }
    }

    public void InvokeAnimalWasClicked(Animal animal) {
        if (OnAnimalWasClicked != null) {
            //Debug.Log("EVENT MANAGER: InvokeAnimalWasClicked");
            OnAnimalWasClicked(animal);
        }
    }

    public void InvokeGameLevelLoaded() {
        if (OnGameLevelLoaded != null) {
            //Debug.Log("EVENT MANAGER: InvokeGameLevelLoaded");
            OnGameLevelLoaded();
        }
    }
}
