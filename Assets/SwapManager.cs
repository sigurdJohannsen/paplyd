using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;
public class SwapManager : BaseSingleton<SwapManager>{

    public ParticleFeedbackSystem particleSwapFeedback;
    public void SwapSounds(GameObject from, GameObject to)
    {
        particleSwapFeedback.SwapParticleFeedback(from, to);
        SoundProperty temp = from.GetComponent<SoundContainer>().Sound;
        from.GetComponent<SoundContainer>().Sound = to.GetComponent<SoundContainer>().Sound;
        to.GetComponent<SoundContainer>().Sound = temp;
        print($"swapping sounds from {from.name} to {to.name}");
        LevelContainerManager.Instance.CheckWin();
    }
    
}
