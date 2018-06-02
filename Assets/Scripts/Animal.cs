using UnityEngine;
using UnityEngine.EventSystems;

/*
 * This script tells the DragHandler if a sound is currently hovering over an animal to drop a sound on.
 * The script must be placed on all animals.
 * */
public class Animal : MonoBehaviour, IDropHandler {

    public static bool hoveringOverValidObject;

    public Sound soundAttached;
    public bool occupied;

    private void Awake() {
        if (soundAttached != null) {
            soundAttached.SetAnimal(this);
            occupied = true;
        }
    }
    
    public void OnDrop(PointerEventData eventData) {
        /*if (hoveringOverValidObject) {
            occupied = true;
            Debug.Log("sound dropped here!");
        }*/
        //PolygonCollider2D clickableObject = transform.>
        //this method might not be needed, as we already have this information in DragHandler. Call an event instead to handle this.
    }

    void OnMouseOver() {
        hoveringOverValidObject = true;
    }

    void OnMouseExit() {
        hoveringOverValidObject = false;
    }
}
