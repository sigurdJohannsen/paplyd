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


    public void Start()
    {
        SoundAnimationDictionary = new Dictionary<string, SoundAnimation>();
        MundList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(mundPath))
        {
            MundList.Add(sprite.name, sprite);
            Debug.Log(sprite + " added to mund");
        }
        OjneList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(ojnePath))
        {
            OjneList.Add(sprite.name, sprite);
            Debug.Log(sprite + " added to øjn");
        }
        KropList = new Dictionary<string, Sprite>();
        foreach (var sprite in Resources.LoadAll<Sprite>(kropPath))
        {
            KropList.Add(sprite.name, sprite);
            Debug.Log(sprite + " added to krop");
        }
        CSVReader csv = new CSVReader();
        csv.LoadData();
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var item in SoundAnimationDictionary)
            {
                Debug.Log(item.Key+" = "+ item.Value.Tag);
                foreach (var i in item.Value.TimeStepList)
                {
                    Debug.Log("tid: "+i.time+" , Mund: "+i.Mund+" ,Øjne: "+i.Ojne+" ,Krop: "+i.Krop);
                }
            }
        }
        
    }
}
