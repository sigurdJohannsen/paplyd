using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script must be placed on all animals.
 * The scripts allows the player to drag from one animal to another.
 * The script updates the position of the dragged dingling.
 * */
public class Animal : MonoBehaviour, IDragHandler, IEndDragHandler {
    
    public string animalName;
    public Sound soundAttached;
    public static bool hoveringAboveValidObject;
    public bool hasSound,
                soundSwapInProgress,
                dragStarted;
    private AnimationPlayer animationPlayer;
    private EventManager eventManager;
    private InputManager inputManager;
    private Transform soundSwapPoolTransform;
    private Dingling currentDingling;
    private Vector3 startPosition,
                    targetPosition;
    private bool animalIsReadyToAnimate = true;

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
        if (soundAttached != null) {
            hasSound = true;
        }
        soundAttached.SetCurrentAnimal(this);   //  Used for debugging current position of sounds, in a visual way.
        currentDingling = FindObjectOfType<Dingling>();
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
        //update dingling position
        currentDingling.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        if (!dragStarted && !soundSwapInProgress) {
            dragStarted = true;
            eventManager.InvokeDraggingStarted(this);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData) {
        if (dragStarted) {
            //Debug.Log("Drag ended!");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null) {
                if (hoveringAboveValidObject && !hit.transform.GetComponent<Animal>().soundSwapInProgress && hit.transform != transform) {
                    Animal secondAnimal = hit.transform.GetComponent<Animal>();
                    //Debug.Log("is soundSwapInProgress= " + hit.transform.GetComponent<Animal>().soundSwapInProgress);
                    //  Snap sound to the new animal.
                    targetPosition = hit.transform.localPosition;
                    startPosition = transform.localPosition;

                    if (secondAnimal.hasSound) {
                        Sound otherAttachedSound = secondAnimal.soundAttached;
                        Animal firstAnimal = inputManager.animalClicked;
                        //  Swap sounds between animals.
                        secondAnimal.soundAttached = inputManager.animalClicked.soundAttached;
                        SetClickedAnimal(secondAnimal);
                        firstAnimal.soundAttached = otherAttachedSound;
                        firstAnimal.soundAttached.SetCurrentAnimal(secondAnimal);
                        otherAttachedSound.SetCurrentAnimal(firstAnimal);

                        if (soundSwapPoolTransform.GetChild(0) != null) {
                            firstAnimal.soundSwapInProgress = true;
                            secondAnimal.soundSwapInProgress = true;
                            eventManager.InvokeSoundsSwapped(secondAnimal.transform, firstAnimal.transform, soundSwapPoolTransform.GetChild(0).GetComponent<SoundSwap>());        //add soundSwap object from soundSwapPool.
                        }

                        //  One of more correct animal was placed!
                        if ((firstAnimal.animalName == firstAnimal.soundAttached.name) || (secondAnimal.animalName == secondAnimal.soundAttached.name)) {
                            eventManager.InvokeCorrectSound();
                        }
                    }
                    else {
                        //  Move sound to empty animal.
                        secondAnimal.hasSound = true;
                        secondAnimal.soundAttached = inputManager.animalClicked.soundAttached;

                        SetClickedAnimal(secondAnimal);
                        if (inputManager.animalClicked != null) {
                            inputManager.animalClicked.hasSound = false;
                            inputManager.animalClicked.soundAttached = null;
                        }
                    }
                }
                else {
                    //  If sound is dropped inside of designated area, but animal has soundSwapInProgress, return sound to origin.
                    currentDingling.transform.transform.localPosition = startPosition;
                    soundSwapInProgress = false;
                }
            }
            else {
                //  If sound is dropped outside of designated area, return sound to origin.
                currentDingling.transform.transform.localPosition = startPosition;
                soundSwapInProgress = false;
            }
            eventManager.InvokeDraggingEnded();
        }
    }

    public void SetClickedAnimal(Animal animal) {
        inputManager.animalClicked = animal;
        currentDingling.transform.localPosition = inputManager.animalClicked.transform.localPosition;
        startPosition = currentDingling.transform.localPosition;
    }

    private void OnMouseOver() {
        hoveringAboveValidObject = true;
    }

    private void OnMouseExit() {
        hoveringAboveValidObject = false;
    }

    private void OnAnimalWasClicked(Animal animal) {
        if (this == animal && animal.animalIsReadyToAnimate && !animal.soundSwapInProgress) {
            animalIsReadyToAnimate = false;
            //only allow this if not already engaged in animation.
            StartCoroutine(animationPlayer.PlayAnimation(soundAttached.soundName, (bool b) => AnimationCallback(b)));
        }
    }

    private void AnimationCallback(bool b) {
        animalIsReadyToAnimate = b;
    }

    private void OnDraggingEnded() {
        dragStarted = false;
    }
}
