﻿using UnityEngine;

/*
 * This script is used to control the object that fakes the swapped sound's movement from one animal to another.
 * The script also resets soundSwapInProgress for animals involved in the swap.
 * */
public class SoundSwap : MonoBehaviour {

    private EventManager eventManager;
    private new SpriteRenderer spriteRenderer;
    private Transform soundSwapPoolTransform;
    private Vector3 startPosition,
                    targetPosition;
    public bool swappingTakingPlace;
    public float Timer = 0,
                 MoveSpeed = 1;

    private Animal swapAnimalA,
                   swapAnimalB;

    private void Awake() {
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnSoundsSwapped += new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnDisable() {
        eventManager.OnSoundsSwapped -= new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnSoundsSwapped(Transform origin, Transform target, SoundSwap soundSwapper) {
        if (soundSwapper == this) {
            StartSwapProcess(origin, target);
        }
    }

    private void Update() {
        if (swappingTakingPlace) {
            Timer += Time.deltaTime * MoveSpeed;
            if (transform.position != targetPosition) {
                transform.position = Vector3.Lerp(startPosition, targetPosition, Timer);
            }
            else {
                eventManager.InvokeSwappedSoundReachedDestination();
                EndSwapProcess();
            }
        }
    }

    private void StartSwapProcess(Transform origin, Transform target) {
        swappingTakingPlace = true;

        transform.SetParent(null);
        spriteRenderer.color = target.GetComponent<Animal>().soundAttached.Color;
        spriteRenderer.enabled = true;
        transform.localPosition = origin.localPosition;
        startPosition = origin.localPosition;
        targetPosition = target.localPosition;

        swapAnimalA = origin.GetComponent<Animal>();
        swapAnimalB = target.GetComponent<Animal>();
    }

    private void EndSwapProcess() {
        swappingTakingPlace = false;

        transform.SetParent(soundSwapPoolTransform);
        spriteRenderer.enabled = false;
        Timer = 0;

        swapAnimalA.soundSwapInProgress = false;
        swapAnimalB.soundSwapInProgress = false;
    }
}
