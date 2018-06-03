using UnityEngine;

/*
 * This script tells the Sound script if a sound is currently hovering over an animal to drop a sound on.
 * The script must be placed on all animals.
 * */
public class Animal : MonoBehaviour {

    public static bool hoveringOverValidObject;

    public Sound soundAttached;
    public bool occupied;

    private EventManager eventManager;

    private void Start() {
        if (soundAttached != null) {
            soundAttached.SetAnimal(this);
            occupied = true;
        }
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
    }

    /*public void OnDrop(PointerEventData eventData) {
        if (hoveringOverValidObject) {
            occupied = true;
            Debug.Log("sound dropped here!");
        }
        //PolygonCollider2D clickableObject = transform.>
        //this method might not be needed, as we already have this information in DragHandler. Call an event instead to handle this.
    }*/

    private void OnMouseDown() {
        eventManager.InvokeAnimalWasClicked();
    }

    private void OnMouseOver() {
        hoveringOverValidObject = true;
    }

    private void OnMouseExit() {
        hoveringOverValidObject = false;
    }
}
