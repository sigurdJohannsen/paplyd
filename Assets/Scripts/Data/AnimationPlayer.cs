using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    private SoundAnimation currentSoundAnimation;
    private float timeWaited;


    [Header("SpriteRenderers of the prefab")]
    public SpriteRenderer Mund;
    public SpriteRenderer Ojne;
    public SpriteRenderer Krop;


    public IEnumerator PlayAnimation(string tag)
    {
        AnimationDatabase.Instance.SoundAnimationDictionary.TryGetValue(tag,out currentSoundAnimation);
        Debug.Log(tag);
        if(GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().clip = Resources.Load("Sounds/" + tag) as AudioClip;
            GetComponent<AudioSource>().Play();
        }
        timeWaited = 0.0f;
        Sprite tempMund = null, tempOjne = null, tempKrop = null;
        for (int i = 0; i < currentSoundAnimation.TimeStepList.Count; i++)
        {
            yield return new WaitForSecondsRealtime(currentSoundAnimation.TimeStepList[i].time - timeWaited);
            timeWaited = currentSoundAnimation.TimeStepList[i].time;
            AnimationDatabase.Instance.MundList.TryGetValue(currentSoundAnimation.TimeStepList[i].Mund, out tempMund);
            AnimationDatabase.Instance.OjneList.TryGetValue(currentSoundAnimation.TimeStepList[i].Ojne, out tempOjne);
            if (AnimationDatabase.Instance.KropList.TryGetValue(currentSoundAnimation.TimeStepList[i].Krop, out tempKrop))
                Krop.sprite = tempKrop;
            else
                Debug.LogError(currentSoundAnimation.TimeStepList[i].Krop + " not found");
            Mund.sprite = tempMund;
            Ojne.sprite = tempOjne;
        }
        yield return true;
    }
}
