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
        //Wwise
        //PostEvent.Play(currentSoundAnimation.Tag);
        timeWaited = 0.0f;
        Sprite tempMund = null, tempOjne = null, tempKrop = null;
        for (int i = 0; i < currentSoundAnimation.TimeStepList.Count; i++)
        {
            yield return new WaitForSeconds(currentSoundAnimation.TimeStepList[i].time - timeWaited);
            timeWaited += currentSoundAnimation.TimeStepList[i].time;
            AnimationDatabase.Instance.MundList.TryGetValue(currentSoundAnimation.TimeStepList[i].Mund, out tempMund);
            AnimationDatabase.Instance.MundList.TryGetValue(currentSoundAnimation.TimeStepList[i].Ojne, out tempOjne);
            AnimationDatabase.Instance.MundList.TryGetValue(currentSoundAnimation.TimeStepList[i].Krop, out tempKrop);
            Mund.sprite = tempMund;
            Ojne.sprite = tempOjne;
            Krop.sprite = tempKrop;
        }
        yield return true;
    }
}
