using UnityEngine;

public class AnimationTester : MonoBehaviour {

    AnimationPlayer ap;
    private void Start()
    {
        ap = GetComponent<AnimationPlayer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)|| Input.touchCount > 1)
        {
            StartCoroutine(ap.PlayAnimation("Monkey", (bool b) => AnimationCallback(b)));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ap.PlayAnimation("Rooster", (bool b) => AnimationCallback(b)));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ap.PlayAnimation("Elephant", (bool b) => AnimationCallback(b)));
        }
    }

    private void AnimationCallback(bool b) { }
}
