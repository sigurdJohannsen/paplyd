using System.Collections.Generic;
using UnityEngine;

/*
 * This script allow the player to drag a sound from an animal, and place the sound on a different animal.
 * The Animal script tells this script, when a sound is hovering over an animal.
 * The script must be placed on all sounds.
 * */
public class Sound : MonoBehaviour {

    public string animationName;
    public Animal myAnimal;
    private Vector3 myPosition;
    private PolygonCollider2D myPolygonCollider;
    private new Renderer renderer;
    private SoundSwap soundSwap;
    private EventManager eventManager;
    private bool dragStarted,
                 iAmBeingDragged;
    private Transform soundSwapPoolTransform;
    private List<PolygonCollider2D> myPolygonColliderList = new List<PolygonCollider2D>();



    public void SetAnimal(Animal animal) {
        myAnimal = animal;
        transform.localPosition = myAnimal.transform.localPosition;
        myPosition = transform.localPosition;
        //  Copy the scale of the animal, to properly scale the polygoncollider for the sound.
        transform.localScale = myAnimal.transform.localScale;

    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnDraggingStarted += new OnDraggingStartedEventHandler(OnDraggingStarted);
        eventManager.OnDraggingEnded += new OnDraggingEndedEventHandler(OnDraggingEnded);
    }

    private void OnDisable() {
        eventManager.OnDraggingStarted -= new OnDraggingStartedEventHandler(OnDraggingStarted);
        eventManager.OnDraggingEnded -= new OnDraggingEndedEventHandler(OnDraggingEnded);
    }

    private void Awake() {
        myPolygonCollider = GetComponent<PolygonCollider2D>();
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }

    private void Start() {
        soundSwap = FindObjectOfType<SoundSwap>();
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
    }

    private void OnDraggingStarted() {
        transform.GetComponent<PolygonCollider2D>().enabled = false;
        if (iAmBeingDragged) {
            renderer.enabled = true;
        }
    }

    private void OnDraggingEnded() {
        dragStarted = false;
        transform.GetComponent<PolygonCollider2D>().enabled = true;
        if (iAmBeingDragged) {
            renderer.enabled = false;
            iAmBeingDragged = false;
        }
    }
}
