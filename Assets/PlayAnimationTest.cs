using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationTest : MonoBehaviour {

    AnimationPlayer AP;
	// Use this for initialization
	void Start () {
        AP = GetComponent<AnimationPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		

        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(AP.PlayAnimation("Rooster"));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(AP.PlayAnimation("Elephant"));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(AP.PlayAnimation("Monkey"));
        }

    }
}
