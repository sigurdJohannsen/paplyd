using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BaseClasses;
using Level;

public class TouchManager : BaseSingleton<TouchManager>
{

    RaycastHit _raycastHit;
    RaycastHit2D _raycastHit2D;
    Collider2D _colliderEnter;
    public Ray ray;
    public float SecondsToDrag = 0.000f;
    [SerializeField]
    private float _secondsOfDrag = 0.0f;

    public ParticleFeedbackSystem particleFeedbackSystem;
    public ParticleSystem FollowGraphic;
    public ParticleSystem GrabGraphic;
    public ParticleSystem DropGraphic;
    private Color soundColor;
    bool _mouseDown;
    bool _dragging;
    private GameObject _firstClickObject;
    private new void Awake()
    {
       
        Input.simulateMouseWithTouches = true;
        base.Awake();
    }
    public void GrabEffect()
    {
        particleFeedbackSystem.StartDrag(_firstClickObject);
    }
    public void StopEffect()
    {
        particleFeedbackSystem.StopAll();

    }
    public void DropEffect()
    {
        particleFeedbackSystem.EndDrag();
    }
    private void SetParticleColor(ParticleSystem ps)
    {
        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.startColor = soundColor;
    }
    IEnumerator GrapEffectCoroutine()
    {
        GrabGraphic.gameObject.SetActive(true);
        SetParticleColor(GrabGraphic);
        GrabGraphic.Play();
        Vector3 size = Vector3.one;
        if (_firstClickObject.GetComponent<PolygonCollider2D>())
        {
            var col = _firstClickObject.GetComponent<PolygonCollider2D>();
            size = col.bounds.extents * _firstClickObject.transform.localScale.x;
            GrabGraphic.transform.position = col.bounds.center;
        }
        
        size *= 0.25f;
        GrabGraphic.transform.localScale = size;
        for (float i = 0; i < 1.0f; i += Time.deltaTime * 6.0f)
        {
            //GrabGraphic.transform.localScale = size * ( 1.0f - (i * 0.8f));
            yield return null;
        }
        GrabGraphic.Stop();
        GrabGraphic.gameObject.SetActive(false);
        GrabGraphic.transform.localScale = Vector3.one;
        FollowGraphic.gameObject.SetActive(true);
        SetParticleColor(FollowGraphic);
        FollowGraphic.Play();
        FollowGraphic.transform.position = GrabGraphic.transform.position;
        while(_mouseDown)
        {
            FollowGraphic.transform.position = Vector3.Lerp(FollowGraphic.transform.position,ray.origin,.15f);
            yield return null;
        }
        FollowGraphic.Stop();
        FollowGraphic.gameObject.SetActive(false);
        DropGraphic.gameObject.SetActive(true);
        DropGraphic.transform.position = ray.origin;
        SetParticleColor(DropGraphic);
        DropGraphic.Play();
        yield return new WaitForSeconds(0.5f);
        DropGraphic.Stop();
        DropGraphic.gameObject.SetActive(false);
    }

 
    
    private void FocusBehaviour(Collider2D hitCol)
    {
        if(hitCol == null)
        {
            if (_colliderEnter != null)
            {
                _colliderEnter.gameObject.SendMessage("OnFocusExit");
                _colliderEnter = null;
            }
            return;
        }
        if (_raycastHit2D.collider == _colliderEnter)
        {
            return;
        }
        _colliderEnter = _raycastHit2D.collider;
        _colliderEnter.SendMessage("OnFocusEnter");
    }

    private void Update()
    {
        if (!EventManager.InteractionAvailable) return;
        ray = UtilitiesManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        _raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, 10.0f);

        FocusBehaviour(_raycastHit2D.collider);
        
        if (!_mouseDown)
        {
            if (!Input.GetMouseButtonDown(0)) return;
            OnMouseButtonDown();
            return;
        }
        if (!Input.GetMouseButton(0))
        {
            OnMouseButtonUp();
            _mouseDown = false;
            _secondsOfDrag = 0.0f;
            return;
        }

        _secondsOfDrag += Time.deltaTime;
        if (_secondsOfDrag > SecondsToDrag && !_dragging)
        {
            _dragging = true;
            GrabEffect();
            //_firstClickObject.SendMessage("DragBegin");
        }
    }
  
    private void OnMouseButtonUp()
    {
        if (!_raycastHit2D.collider)
        {
            StopEffect();
            _firstClickObject = null;
            _dragging = false;
            return;
        }
        if (!_dragging)
        {
            //A click event has happened, and not a drag event
            _firstClickObject.SendMessage("OnClick");
            _firstClickObject = null;
            return;
        }
        if (_raycastHit2D.collider && _firstClickObject)
        {
            _firstClickObject.SendMessage("OnClickDragEnd");
            DropEffect();
            SwapManager.Instance.SwapSounds(_firstClickObject, _raycastHit2D.collider.gameObject);
            _firstClickObject = null;
            _dragging = false;
        }
    }

    private void OnMouseButtonDown()
    {
        if (_raycastHit2D.collider)
        {
            _firstClickObject = _raycastHit2D.collider.gameObject;
            soundColor = _firstClickObject.GetComponent<SoundContainer>().Sound.Color;
            _mouseDown = true;
        }
    }
}