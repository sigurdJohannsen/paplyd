using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour {

    public int Level;

    public void Clicked()
    {
        LevelManager.Instance.LoadScene(Level);
    }
}
