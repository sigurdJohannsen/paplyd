using UnityEngine;

public class AnimationTester : MonoBehaviour {

    AnimationPlayer ap;
    bool playing = false;
    private void Start()
    {
        ap = GetComponent<AnimationPlayer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && !playing)
        {
            playing = true;
            StartCoroutine(ap.PlayAnimation("Monkey", (bool b) => AnimationCallback(b)));
        }
        if (Input.GetKeyDown(KeyCode.R) && !playing)
        {
            playing = true;
            StartCoroutine(ap.PlayAnimation("Rooster", (bool b) => AnimationCallback(b)));
        }
        if (Input.GetKeyDown(KeyCode.E) && !playing)
        {
            playing = true;
            StartCoroutine(ap.PlayAnimation("Elephant", (bool b) => AnimationCallback(b)));
        }
    }

    private void AnimationCallback(bool b) { playing = false; }
}
