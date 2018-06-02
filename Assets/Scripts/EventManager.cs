﻿using UnityEngine;

public delegate void OnDraggingStartedEventHandler();
public delegate void OnDraggingEndedEventHandler();
public delegate void OnSoundsSwappedEventHandler();
public delegate void OnCorrectSoundEventHandler();
public delegate void OnWrongSoundEventHandler();
public delegate void OnAnimalWasClickedEventHandler();

public class EventManager : MonoBehaviour {
    public static EventManager instance = null;

    public event OnDraggingStartedEventHandler OnDraggingStarted;
    public event OnDraggingEndedEventHandler OnDraggingEnded;
    public event OnSoundsSwappedEventHandler OnSoundsSwapped;
    public event OnCorrectSoundEventHandler OnCorrectSound;
    public event OnWrongSoundEventHandler OnWrongSound;
    public event OnAnimalWasClickedEventHandler OnAnimalWasClicked;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void InvokeDraggingStarted() {
        if (OnDraggingStarted != null) {
            //Debug.Log("EVENT MANAGER: InvokeDraggingStarted");
            OnDraggingStarted();
        }
    }

    public void InvokeDraggingEnded() {
        if (OnDraggingEnded != null) {
            //Debug.Log("EVENT MANAGER: InvokeDraggingEnded");
            OnDraggingEnded();
        }
    }

    public void InvokeSoundsSwapped() {
        if (OnSoundsSwapped != null) {
            //Debug.Log("EVENT MANAGER: InvokeSoundsSwapped");
            OnSoundsSwapped();
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

    public void InvokeAnimalWasClicked() {
        if (OnAnimalWasClicked != null) {
            Debug.Log("EVENT MANAGER: InvokeAnimalWasClicked");
            OnAnimalWasClicked();
        }
    }
}
