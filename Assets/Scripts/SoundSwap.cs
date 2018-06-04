using UnityEngine;

/*
 * This script is used to control the object that fakes the swapped sound's movement from one animal to another.
 * The script also resets swappingTakingPlace for animals involved in the swap.
 * */
public class SoundSwap : MonoBehaviour {

    private EventManager eventManager;
    private new SpriteRenderer renderer;
    private Transform soundSwapPoolTransform;
    private Vector3 startPosition,
                    targetPosition;
    public bool swappingTakingPlace;
    public float Timer = 0,
                 MoveSpeed = 1;

    private Animal swapAnimalA,
                   swapAnimalB;

    private void Awake() {
        soundSwapPoolTransform = FindObjectOfType<SoundSwapPool>().transform;
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.OnSoundsSwapped += new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnDisable() {
        eventManager.OnSoundsSwapped -= new OnSoundsSwappedEventHandler(OnSoundsSwapped);
    }

    private void OnSoundsSwapped(Transform origin, Transform target, SoundSwap soundSwapper) {
        if (soundSwapper == this) {
            //Debug.Log("sound swap invoked");
            transform.SetParent(null);
            renderer.sprite = target.GetComponent<Animal>().soundAttached.GetComponent<SpriteRenderer>().sprite;
            renderer.enabled = true;
            transform.localPosition = origin.localPosition;
            startPosition = origin.localPosition;
            targetPosition = target.localPosition;
            swappingTakingPlace = true;

            swapAnimalA = origin.GetComponent<Animal>();
            swapAnimalB = target.GetComponent<Animal>();
        }
    }

    private void Update() {
        if (swappingTakingPlace) {
            Timer += Time.deltaTime * MoveSpeed;
            if (transform.position != targetPosition) {
                transform.position = Vector3.Lerp(startPosition, targetPosition, Timer);
            }
            else {
                //Debug.Log("sound swap reached endpoint");
                swappingTakingPlace = false;
                renderer.enabled = false;
                eventManager.InvokeSwappedSoundReachedDestination();
                Timer = 0;
                transform.SetParent(soundSwapPoolTransform);
                swapAnimalA.busy = false;
                swapAnimalB.busy = false;
            }
        }
    }
}
