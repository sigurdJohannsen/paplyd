using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;
public class SwapManager : BaseSingleton<SwapManager>{

    public void SwapSounds(GameObject from, GameObject to)
    {
        SoundProperty temp = from.GetComponent<SoundContainer>().Sound;
        from.GetComponent<SoundContainer>().Sound = to.GetComponent<SoundContainer>().Sound;
        to.GetComponent<SoundContainer>().Sound = temp;

        print($"swapping sounds from {from.name} to {to.name}");
    }
    
}
