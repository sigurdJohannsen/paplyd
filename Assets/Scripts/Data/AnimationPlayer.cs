using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class AnimationPlayer : MonoBehaviour {

    public string MundFigure = "";
    public string OjneFigure = "";
    public string KropFigure = "";

    private SoundAnimation currentSoundAnimation;
    private float timeWaited;
    private AudioSource audioSource;
    private List<AudioClip> audioClips;


    [Header("SpriteRenderers of the prefab")]
    public SpriteRenderer Mund;
    public SpriteRenderer Ojne;
    public SpriteRenderer Krop;

    Animator animator;

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
        
        timeWaited = 0.0f;
        Sprite tempMund = null, tempOjne = null, tempKrop = null;
        for (int i = 0; i < currentSoundAnimation.TimeStepList.Count; i++)
        {
            yield return new WaitForSecondsRealtime(currentSoundAnimation.TimeStepList[i].time - timeWaited);
            timeWaited = currentSoundAnimation.TimeStepList[i].time;
            if (AnimationDatabase.Instance.KropList.TryGetValue(KropFigure + currentSoundAnimation.TimeStepList[i].Krop, out tempKrop))
            {
               
                Krop.sprite = tempKrop;
            }
            else
            {
                Debug.LogError(KropFigure + currentSoundAnimation.TimeStepList[i].Krop + "could not be found");
            }
            if (AnimationDatabase.Instance.MundList.TryGetValue(MundFigure + currentSoundAnimation.TimeStepList[i].Mund, out tempMund))
            {
                Mund.sprite = tempMund;
            }
            else
            {
                Debug.LogError(MundFigure + currentSoundAnimation.TimeStepList[i].Mund + "could not be found");
            }
            if (AnimationDatabase.Instance.OjneList.TryGetValue(OjneFigure + currentSoundAnimation.TimeStepList[i].Ojne, out tempOjne))
            {
                Ojne.sprite = tempOjne;
            }
            else
            {
                Debug.LogError(OjneFigure + currentSoundAnimation.TimeStepList[i].Ojne + "could not be found");
            }

            AnimatorAssist(currentSoundAnimation.TimeStepList[i].Animation);

        }
        yield return true;
    }
}
