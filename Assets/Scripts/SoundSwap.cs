using UnityEngine;

public class SoundSwap : MonoBehaviour {

    private EventManager eventManager;
    private new SpriteRenderer renderer;
    private Vector3 startPosition,
                    targetPosition;
    private bool swappingTakingPlace;
    public float Timer = 0,
                 MoveSpeed = 1;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnSoundsSwapped += new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnDisable() {
        eventManager.OnSoundsSwapped -= new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnSoundsSwapped(Transform origin, Transform target) {
        renderer.sprite = target.GetComponent<Animal>().soundAttached.GetComponent<SpriteRenderer>().sprite;
        renderer.enabled = true;
        transform.localPosition = origin.localPosition;
        startPosition = origin.localPosition;
        targetPosition = target.localPosition;
        swappingTakingPlace = true;
    }

    private void Update() {
        if (swappingTakingPlace) {
            Timer += Time.deltaTime * MoveSpeed;
            if (transform.position != targetPosition) {
                transform.position = Vector3.Lerp(startPosition, targetPosition, Timer);
            }
            else {
                swappingTakingPlace = false;
                renderer.enabled = false;
                eventManager.InvokeSwappedSoundReachedDestination();
                Timer = 0;
            }
        }
    }
}
