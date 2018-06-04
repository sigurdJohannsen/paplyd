﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    public string MundFigure = "";
    public string OjneFigure = "";
    public string KropFigure = "";

    private SoundAnimation currentSoundAnimation;
    private float timeWaited;
    private AudioSource audioSource;
    private List<AudioClip> audioClips;
    private float audioTime;
    Sprite tempMund = null, tempOjne = null, tempKrop = null;
    [Header("SpriteRenderers of the prefab")]
    public SpriteRenderer Mund;
    public SpriteRenderer Ojne;
    public SpriteRenderer Krop;

    Animator animator;
    bool lort = false;


    private void AnimatorAssist(string value)
    {
        int i;
        int.TryParse(value,out i);
        if (!animator)
            animator = GetComponent<Animator>();
        animator.SetInteger("CSV", i);
    }

    public IEnumerator PlayAnimation(string tag)
    {
        AnimationDatabase.Instance.SoundAnimationDictionary.TryGetValue(tag,out currentSoundAnimation);

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load("Sounds/"+tag) as AudioClip;
        audioSource.Play();
        audioTime = audioSource.clip.length;
        timeWaited = 0.0f;
       
        for (int i = 0; i < currentSoundAnimation.TimeStepList.Count + 1; i++)
        {
            if(i == currentSoundAnimation.TimeStepList.Count)
            {
                lort = true;
                currentSoundAnimation = AnimationDatabase.Instance.IdleSoundAnimation;
                yield return new WaitForSecondsRealtime(audioTime - timeWaited);
                SetSprites(0);
                AnimatorAssist("0"); // idle
                StopAllCoroutines();
            }
            yield return new WaitForSecondsRealtime(currentSoundAnimation.TimeStepList[i].time - timeWaited);
            timeWaited = currentSoundAnimation.TimeStepList[i].time;
            SetSprites(i);
            AnimatorAssist(currentSoundAnimation.TimeStepList[i].Animation);
            if (lort)
                Debug.Log("Jeg er blevet løjet for");

        }
        

        yield return true;
    }

    public void SetSprites(int csv)
    {
        if (AnimationDatabase.Instance.KropList.TryGetValue(KropFigure + currentSoundAnimation.TimeStepList[csv].Krop, out tempKrop))
        {
            Krop.sprite = tempKrop;
        }
        else
        {
            Debug.LogError(KropFigure + currentSoundAnimation.TimeStepList[csv].Krop + "could not be found");
        }
        if (AnimationDatabase.Instance.MundList.TryGetValue(MundFigure + currentSoundAnimation.TimeStepList[csv].Mund, out tempMund))
        {
            Mund.sprite = tempMund;
        }
        else
        {
            Debug.LogError(MundFigure + currentSoundAnimation.TimeStepList[csv].Mund + "could not be found");
        }
        if (AnimationDatabase.Instance.OjneList.TryGetValue(OjneFigure + currentSoundAnimation.TimeStepList[csv].Ojne, out tempOjne))
        {
            Ojne.sprite = tempOjne;
        }
        else
        {
            Debug.LogError(OjneFigure + currentSoundAnimation.TimeStepList[csv].Ojne + "could not be found");
        }

    }
}
