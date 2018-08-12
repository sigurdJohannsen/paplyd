using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Level;
using SpriteGlow;
using UnityEngine.Events;

public class SoundContainer : MonoBehaviour {

    public string Animal;

    [SerializeField] public SoundProperty Sound;


    public float ScaleIncreasePercentage = 20.0f;
    private bool locked;
    private bool hover;
    private Vector3 scale;
   
    AnimationPlayer animationPlayer;

    public void Start()
    {
        scale = transform.localScale;
    }
    public void OnClick()
    {
        if (!animationPlayer)
            animationPlayer = GetComponent<AnimationPlayer>();
        if (!locked)
        {
            StartCoroutine(animationPlayer.PlayAnimation(Sound.Sound, () => AnimationCallback()));
            locked = true;
        }
    }
    private void OnFocusExit()
    {
        hover = false;
    }
    private void OnFocusEnter()
    {
        hover = true;
    }
    private void AnimationCallback()
    {
        locked = false;
    }
    public void Hover()
    {
        hover = true;
    }
    public void OutOfFocus()
    {
        hover = false;
    }
    public void OnClickDragEnd()
    {
        hover = false;
    }
    public void Hovering(bool on)
    {
        if (locked) return;
        //Scaling
        if (on && transform.localScale.x < scale.x * (1 + (ScaleIncreasePercentage / 100)))
        {
            transform.localScale += Vector3.one * Time.deltaTime;
            return;
        }
        if (!on && transform.localScale.x > scale.x)
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
            return;
        }
        //coloring
       // if (on == hover)
        //    return;
     
    }

    public void Update()
    {
        Hovering(hover);
    }
  
}
