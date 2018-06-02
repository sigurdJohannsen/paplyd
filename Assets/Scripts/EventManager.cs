using UnityEngine;

public delegate void OnDraggingEventHandler();
public delegate void OnDraggingEndedEventHandler();

public class EventManager : MonoBehaviour {
    public static EventManager instance = null;

    public event OnDraggingEventHandler OnDragging;
    public event OnDraggingEndedEventHandler OnDraggingEnded;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void InvokeDragging() {
        if (OnDragging != null) {
            //Debug.Log("EVENT MANAGER: InvokeDragging");
            OnDragging();
        }
    }

    public void InvokeDraggingEnded() {
        if (OnDraggingEnded != null) {
            //Debug.Log("EVENT MANAGER: InvokeDraggingEnded");
            OnDraggingEnded();
        }
    }
}
