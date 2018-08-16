using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
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
    public List<SpriteRenderer> Krop;
    WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();
    Animator animator;

    private string[] alphabeticInterger= {"","b","c","d" };

    private VibrationAnimation vibrationAnimation;

    public void Awake()
    {
        
        if (!vibrationAnimation && GetComponent<VibrationAnimation>())
        {
            vibrationAnimation = GetComponent<VibrationAnimation>();
        }
        if (!animator)
            animator = GetComponent<Animator>();
    }

    private void AnimatorAssist(string value)
    {
        int i;
        int.TryParse(value,out i);
        
        animator.SetInteger("CSV", i);
        if (vibrationAnimation)
        {
            vibrationAnimation.Vibrate(i);
        }
    }

    public IEnumerator PlayAnimation(string tag, UnityAction animationCallback)
    {
        AnimationDatabase.Instance.SoundAnimationDictionary.TryGetValue(tag, out currentSoundAnimation);
     
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
        audioSource.clip = Resources.Load("Sounds/"+tag) as AudioClip;
        audioSource.Play();
        audioTime = audioSource.clip.length;
        timeWaited = 0.0f;
       
        for (int i = 0; i < currentSoundAnimation.TimeStepList.Count + 1; i++)
        {
           
            if (i == currentSoundAnimation.TimeStepList.Count)
            {
                yield return new WaitForSecondsRealtime(audioTime - timeWaited);
                currentSoundAnimation = AnimationDatabase.Instance.IdleSoundAnimation;
                SetSprites(0);
                AnimatorAssist("0"); // idle
                animationCallback();
            }
            else
            {
                AnimatorAssist(currentSoundAnimation.TimeStepList[i].Animation);
                
                while (currentSoundAnimation.TimeStepList[i].time > timeWaited)
                {
                    yield return waitForEnd;
                    timeWaited += Time.deltaTime;
                    SetSprites(i);
                }
                
                /*
                yield return new WaitForSecondsRealtime(currentSoundAnimation.TimeStepList[i].time - timeWaited);
                yield return waitForEnd;
                timeWaited = currentSoundAnimation.TimeStepList[i].time;
                SetSprites(i);
                AnimatorAssist(currentSoundAnimation.TimeStepList[i].Animation);
                */
            }
        }
        
        yield return true;
    }

    public void SetSprites(int csv)
    {
        for (int i = 0; i < Krop.Count; i++)
        {
            if (AnimationDatabase.Instance.KropList.TryGetValue(KropFigure + currentSoundAnimation.TimeStepList[csv].Krop + alphabeticInterger[i], out tempKrop))
            {
                
                Krop[i].sprite = tempKrop;
            }
            else
            {
                Debug.LogError(KropFigure + currentSoundAnimation.TimeStepList[csv].Krop + "could not be found");
            }
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
