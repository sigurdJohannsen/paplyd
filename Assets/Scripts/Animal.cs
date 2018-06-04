using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script must be placed on all animals.
 * */
public class Animal : MonoBehaviour, IDragHandler, IEndDragHandler {
    
    public Sound soundAttached;
    public static bool hoveringOverValidObject;
    public bool hasSound,
                busy;

    private AnimationPlayer animationPlayer;
    public bool dragStarted,
                iAmBeingDragged;

    private EventManager eventManager;
    private InputManager inputManager;
    private Transform soundSwapPoolTransform;
    private Transform currentDingling;
    private Vector3 startPosition,
                    targetPosition;

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
        currentDingling = FindObjectOfType<Dingling>().transform;
        if (soundAttached != null) {
            hasSound = true;
        }
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        animationPlayer = GetComponent<AnimationPlayer>();
        eventManager.OnAnimalWasClicked += new OnAnimalWasClickedEventHandler(OnAnimalWasClicked);
        eventManager.OnDraggingEnded += new OnDraggingEndedEventHandler(OnDraggingEnded);
    }

    private void OnDisable() {
        eventManager.OnAnimalWasClicked -= new OnAnimalWasClickedEventHandler(OnAnimalWasClicked);
        eventManager.OnDraggingEnded -= new OnDraggingEndedEventHandler(OnDraggingEnded);
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Drag event!");
        //update dingling position
        currentDingling.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        if (!dragStarted) {
            currentDingling.GetComponent<SpriteRenderer>().enabled = true;
            Debug.Log("Drag started!");
            dragStarted = true;
            busy = true;
            eventManager.InvokeDraggingStarted();
        }
    }
    
    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("Drag ended!");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null) {
            if (hoveringOverValidObject && !hit.transform.GetComponent<Animal>().busy) {
                //  Snap sound to the new animal.
                targetPosition = hit.transform.localPosition;
                startPosition = transform.localPosition;

                Debug.Log("valid target");
                if (hit.transform.GetComponent<Animal>().hasSound) {
                    Debug.Log("has sound");
                    Sound otherAttachedSound = hit.transform.GetComponent<Animal>().soundAttached;
                    Animal thisAnimal = inputManager.animalClicked;
                    //  Swap sounds between animals.
                    hit.transform.GetComponent<Animal>().soundAttached = inputManager.animalClicked.soundAttached;
                    SetAnimal(hit.transform.GetComponent<Animal>());
                    thisAnimal.soundAttached = otherAttachedSound;
                    otherAttachedSound.SetAnimal(thisAnimal);

                    if (soundSwapPoolTransform.GetChild(0) != null) {
                        Debug.Log("got child 0");
                        hit.transform.GetComponent<Animal>().busy = true;
                        eventManager.InvokeSoundsSwapped(hit.transform.GetComponent<Animal>().transform, thisAnimal.transform, soundSwapPoolTransform.GetChild(0).GetComponent<SoundSwap>());        //add soundSwap object from soundSwapPool.
                    }
                    Debug.Log("sounds swapped");
                }
                else {
                    //  Move sound to empty animal.
                    hit.transform.GetComponent<Animal>().hasSound = true;
                    hit.transform.GetComponent<Animal>().soundAttached = inputManager.animalClicked.soundAttached;

                    SetAnimal(hit.transform.GetComponent<Animal>());
                    if (inputManager.animalClicked != null) {
                        inputManager.animalClicked.hasSound = false;
                        inputManager.animalClicked.soundAttached = null;
                    }
                }
            }
            else {
                //  If sound is dropped inside of designated area, but animal is busy, return sound to origin.
                currentDingling.transform.transform.localPosition = startPosition;
            }
        }
        else {
            //  If sound is dropped outside of designated area, return sound to origin.
            currentDingling.transform.transform.localPosition = startPosition;
        }
        eventManager.InvokeDraggingEnded();
    }

    public void SetAnimal(Animal animal) {
        inputManager.animalClicked = animal;
        currentDingling.transform.localPosition = inputManager.animalClicked.transform.localPosition;
        startPosition = currentDingling.transform.localPosition;
        //currentDingling.transform.localScale = inputManager.animalClicked.transform.localScale;
    }

    private void OnMouseOver() {
        hoveringOverValidObject = true;
    }

    private void OnMouseExit() {
        hoveringOverValidObject = false;
    }

    private void OnAnimalWasClicked(Animal animal) {
        if (this == animal) {
            StartCoroutine(animationPlayer.PlayAnimation(soundAttached.animationName));
        }
    }

    private void OnDraggingEnded() {
        dragStarted = false;
        currentDingling.GetComponent<SpriteRenderer>().enabled = false;
    }
}
