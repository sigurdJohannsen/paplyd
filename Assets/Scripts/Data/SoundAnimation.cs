using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimation {

    public string Tag;
    public List<TimeStep> TimeStepList = new List<TimeStep>();

    public class TimeStep
    {
        public float time;
        public string Mund;
        public string Ojne;
        public string Krop;
        public string Animation;
    }

}
