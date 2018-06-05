using UnityEngine;
using System.Collections.Generic;

/*
 * This script must be placed on the dingling.
 * The script updates the visibility of the dragged dingling.
 * The color of the dingling is set according to the current sound.
 * */
public class Dingling : MonoBehaviour {
    
    private EventManager eventManager;
    private Transform dingling;
    private Color color;
    private List<SpriteRenderer> spriteRendererList = new List<SpriteRenderer>();

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnDraggingStarted += new OnDraggingStartedEventHandler(OnDraggingStarted);
        eventManager.OnDraggingEnded += new OnDraggingEndedEventHandler(OnDraggingEnded);
        dingling = transform.GetChild(0);
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            spriteRendererList.Add(spriteRenderer);
        }
        dingling.gameObject.SetActive(false);
    }

    private void OnDisable() {
        eventManager.OnDraggingStarted -= new OnDraggingStartedEventHandler(OnDraggingStarted);
        eventManager.OnDraggingEnded -= new OnDraggingEndedEventHandler(OnDraggingEnded);
    }

    private void OnDraggingStarted(Animal animal) {
        foreach (SpriteRenderer spriteRenderer in spriteRendererList) {
            spriteRenderer.color = animal.soundAttached.dinglingColor;
        }
        dingling.gameObject.SetActive(true);
    }

    private void OnDraggingEnded() {
        dingling.gameObject.SetActive(false);
    }
}
