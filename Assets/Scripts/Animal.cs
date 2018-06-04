using UnityEngine;

/*
 * This script tells the Sound script if a sound is currently hovering over an animal to drop a sound on.
 * The script must be placed on all animals.
 * */
public class Animal : MonoBehaviour {

    public static bool hoveringOverValidObject;

    public Sound soundAttached;
    public bool occupied,
                swappingTakingPlace;

    private EventManager eventManager;
    private AnimationPlayer animationPlayer;

    private void Start() {
        animationPlayer = GetComponent<AnimationPlayer>();
        if (soundAttached != null) {
            soundAttached.SetAnimal(this);
            occupied = true;
        }
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnAnimalWasClicked += new OnAnimalWasClickedEventHandler(OnAnimalWasClicked);
    }

    private void Disable() {
        eventManager.OnAnimalWasClicked -= new OnAnimalWasClickedEventHandler(OnAnimalWasClicked);
    }

    private void OnAnimalWasClicked(Animal animal) {
        if (animal == this) {
            StartCoroutine(animationPlayer.PlayAnimation(soundAttached.animationName));
        }
    }

    private void OnMouseOver() {
        hoveringOverValidObject = true;
    }

    private void OnMouseExit() {
        hoveringOverValidObject = false;
    }
}
