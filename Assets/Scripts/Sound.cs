using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script allow the player to drag a sound from an animal, and place the sound on a different animal.
 * The Animal script tells this script, when a sound is hovering over an animal.
 * The script must be placed on all sounds.
 * */
public class Sound : MonoBehaviour, IDragHandler, IEndDragHandler {

    public Animal myAnimal;
    private Vector3 myPosition;
    private PolygonCollider2D myPolygonCollider;
    private new Renderer renderer;
    private SoundSwap soundSwap;
    private EventManager eventManager;
    private bool dragStarted,
                 iAmBeingDragged;
    private Transform soundSwapPoolTransform;

    public void SetAnimal(Animal animal) {
        myAnimal = animal;
        transform.localPosition = myAnimal.transform.localPosition;
        myPosition = transform.localPosition;
        //  Copy the scale of the animal, to properly scale the polygoncollider for the sound.
        transform.localScale = myAnimal.GetComponent<Transform>().localScale;
        myPolygonCollider.SetPath(0, myAnimal.GetComponent<PolygonCollider2D>().GetPath(0));
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
        //  Copy the scale of the animal, to properly scale the polygoncollider for the sound.
        if (myAnimal != null) {
            transform.localScale = myAnimal.GetComponent<Transform>().localScale;
        }
        soundSwap = FindObjectOfType<SoundSwap>();
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
    }

    private void OnMouseDown() {
        StartCoroutine(WaitForClick());
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
        if (!dragStarted && !myAnimal.swappingTakingPlace) {
            dragStarted = true;
            iAmBeingDragged = true;
            eventManager.InvokeDraggingStarted();
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null) {
            if (Animal.hoveringOverValidObject && !hit.transform.GetComponent<Animal>().swappingTakingPlace) {
                //  Snap sound to the new animal.
                transform.localPosition = hit.transform.localPosition;
                myPosition = transform.localPosition;

                if (hit.transform.GetComponent<Animal>().occupied) {
                    Sound otherAttachedSound = hit.transform.GetComponent<Animal>().soundAttached;
                    Animal thisAnimal = myAnimal;
                    //  Swap sounds between animals.
                    hit.transform.GetComponent<Animal>().soundAttached = this;
                    SetAnimal(hit.transform.GetComponent<Animal>());
                    thisAnimal.soundAttached = otherAttachedSound;
                    otherAttachedSound.SetAnimal(thisAnimal);
                    //AkSoundEngine.PostEvent("Monkey", gameObject);      //play the sound of a sound being placed on an animal(?)
                    if (soundSwapPoolTransform.GetChild(0) != null) {
                        thisAnimal.swappingTakingPlace = true;
                        hit.transform.GetComponent<Animal>().swappingTakingPlace = true;
                        eventManager.InvokeSoundsSwapped(hit.transform.GetComponent<Animal>().transform, thisAnimal.transform, soundSwapPoolTransform.GetChild(0).GetComponent<SoundSwap>());       //add soundSwap object from soundSwapPool.
                    }
                }   
                else {
                    //  Move sound to empty animal.
                    if (myAnimal != null) {
                        myAnimal.occupied = false;
                        myAnimal.soundAttached = null;
                    }
                    hit.transform.GetComponent<Animal>().occupied = true;
                    hit.transform.GetComponent<Animal>().soundAttached = this;
                    SetAnimal(hit.transform.GetComponent<Animal>());
                }
            }
            else {
                //  If sound is dropped inside of designated area, but animal is busy, return sound to origin.
                transform.localPosition = myPosition;
            }
        }
        else {
            //  If sound is dropped outside of designated area, return sound to origin.
            transform.localPosition = myPosition;
        }
        eventManager.InvokeDraggingEnded();
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

    //  Distinguish a click from a drag initiation.
    IEnumerator WaitForClick() {
        yield return new WaitForSeconds(0.3f);
        if (!dragStarted) {
            eventManager.InvokeAnimalWasClicked();
        }
    }
}
