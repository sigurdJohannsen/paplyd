using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public Animal animalClicked;

    private EventManager eventManager;

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (Physics2D.Raycast(mousePos2D, Vector2.zero)) {
                if (hit.collider != null && hit.collider.GetComponent<Animal>() != null && !hit.collider.GetComponent<Animal>().soundSwapInProgress) {
                    animalClicked = hit.collider.GetComponent<Animal>();
                    StartCoroutine(WaitForClick());
                }
            }            
        }
    }
    
    //  Distinguish a click from a drag initiation.
    IEnumerator WaitForClick() {
        yield return new WaitForSeconds(0.3f);
        if (!animalClicked.dragStarted) {
            eventManager.InvokeAnimalWasClicked(animalClicked);
        }
    }
}
