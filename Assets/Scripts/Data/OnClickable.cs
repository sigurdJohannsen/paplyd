using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// tried to implement something automatic, but made a simple interface OnClickHandler, which didnt work so we are running this again.
/// 
/// </summary>
public class OnClickable : MonoBehaviour
{
    /*
        public virtual void OnValidate()
        {
            if (!GetComponent<Transform>()) return;
            if (GetComponent<Collider>() || GetComponent<Collider2D>() || GetComponent<BoxCollider>())  return;

            if (GetComponent<MeshFilter>())
            {
               gameObject.AddComponent<MeshCollider>();
                return;
            }
            if(GetComponent<SpriteRenderer>())
            {
                gameObject.AddComponent<PolygonCollider2D>();
                return;
            }
            Debug.LogError($"{gameObject.name} has no mesh or sprite, trying to find a mesh in the children");
        }
        // this method is called from TouchManager
    
    */
    public virtual void OnClick()
    {
        Debug.LogError("OnClick was called, but no overridden method was called");
    }

}
