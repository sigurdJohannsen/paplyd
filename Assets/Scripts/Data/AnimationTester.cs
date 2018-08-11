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
            StartCoroutine(ap.PlayAnimation("Monkey", () => AnimationCallback()));
        }
        if (Input.GetKeyDown(KeyCode.R) && !playing)
        {
            playing = true;
            StartCoroutine(ap.PlayAnimation("Rooster", () => AnimationCallback()));
        }
        if (Input.GetKeyDown(KeyCode.E) && !playing)
        {
            playing = true;
            StartCoroutine(ap.PlayAnimation("Elephant", () => AnimationCallback()));
        }
    }

    private void AnimationCallback() { playing = false; }
}
