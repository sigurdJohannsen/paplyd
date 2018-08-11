using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BaseClasses;
using UnityEngine.UI;

namespace Level
{

    public delegate Event OnInputDragBegin();
    public delegate Event OnInputDragUpdate();
    public delegate Event OnInputDragEnd();

    public class DragManager : BaseSingleton<DragManager>
    {
        public bool Locked = false;

        GameObject _fromObject;
        GameObject _toObject;

        [SerializeField] GameObject _swapGraphic;
        public void Start()
        {
            _swapGraphic.SetActive(false);
        }

        public void DragBegin(GameObject go)
        {
            if (Locked) return;
            else Locked = true;
            _fromObject = go;
            _swapGraphic.SetActive(true);
        }
        public void DragUpdate()
        {
            _swapGraphic.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition); //;    
        }
        public void DragEnd()
        {
            Locked = false;
            _swapGraphic.SetActive(false);
        }
    }
}
