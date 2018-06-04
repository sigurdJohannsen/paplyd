using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseClasses;

public class AnimationDatabase : BaseSingleton<AnimationDatabase> {

    public Dictionary<string,SoundAnimation> SoundAnimationDictionary;

    private string mundPath = "MundSprites/";
    private string ojnePath = "OjneSprites/";
    private string kropPath = "KropSprites/";
    public Dictionary<string,Sprite> MundList;
    public Dictionary<string,Sprite> OjneList;
    public Dictionary<string,Sprite> KropList;

    public SoundAnimation IdleSoundAnimation;

    public void Start()
    {
        IdleSoundAnimationGenerator();
        SoundAnimationDictionary = new Dictionary<string, SoundAnimation>();
        MundList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(mundPath))
        {
            MundList.Add(sprite.name, sprite);
        }
        OjneList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(ojnePath))
        {
            OjneList.Add(sprite.name, sprite);
        }
        KropList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(kropPath))
        {
            KropList.Add(sprite.name, sprite);
        }
        CSVReader csv = new CSVReader();
        csv.LoadData();
        Debug.Log("Data has been loaded");
    }
    private void IdleSoundAnimationGenerator()
    {
        IdleSoundAnimation = new SoundAnimation();
        SoundAnimation.TimeStep timeStep = new SoundAnimation.TimeStep();
        timeStep.Animation = "0";
        timeStep.Krop = "1";
        timeStep.Mund = "1";
        timeStep.Ojne = "1";
        timeStep.time = 0.0f;
        IdleSoundAnimation.TimeStepList.Add(timeStep);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var item in SoundAnimationDictionary)
            {
                Debug.Log(item.Key + " = " + item.Value.Tag);
                foreach (var i in item.Value.TimeStepList)
                {
                    Debug.Log(i.time + " , " + i.Mund + " , " + i.Ojne + " , " + i.Krop);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var s in KropList)
            {
                Debug.Log(s.Key + " , " + s.Value);
            }
        }
    }
}
