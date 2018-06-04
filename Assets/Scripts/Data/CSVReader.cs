using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader
{

    private SoundAnimation tempSoundAnimation;
    private SoundAnimation.TimeStep tempTimeStep;

    public void LoadData()
    {
        TextAsset itemData = Resources.Load<TextAsset>("CSVSounds");
        string[] data = itemData.text.Split(new char[] { '\n' });
        //Debug.Log(data.Length); // note there's a line too much at the end.

        for (int i = 1; i < data.Length; i++)
        {
            //  Skip the first line, since it's the labels: SoundTag, TimeStep, Mund, Ojne, Krop
            string[] row = data[i].Split(new char[] { ',' });
            if (row[0] != "")
            {
                tempSoundAnimation = new SoundAnimation();
                AnimationDatabase.Instance.SoundAnimationDictionary.Add(row[0], tempSoundAnimation);
                tempSoundAnimation.Tag = row[0];
            }
            tempTimeStep = new SoundAnimation.TimeStep();
            tempSoundAnimation.TimeStepList.Add(tempTimeStep);
            float.TryParse(row[1], out tempTimeStep.time);
            tempTimeStep.Mund = row[2];
            tempTimeStep.Ojne = row[3];
            tempTimeStep.Krop = row[4]; 
            tempTimeStep.Animation = row[5].Trim(); //Added trim to remove newline
        }
    }
}

