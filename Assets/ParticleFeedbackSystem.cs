using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFeedbackSystem : MonoBehaviour {

    ParticleState _state;

    GameObject startObj;
    Vector3 startObjPos;
    [SerializeField]
    ParticleSystem Start,
    Follow,
    End;

    private void SetParticleColor(GameObject go)
    {
        Color color = go.GetComponent<SoundContainer>().Sound.Color;
        ParticleSystem.MainModule mainStart = Start.main;
        ParticleSystem.MainModule mainFollow = Follow.main;
        ParticleSystem.MainModule mainEnd = End.main;
        mainStart.startColor = color;
        mainFollow.startColor = color;
        mainEnd.startColor = color;
    }

    public void Update()
    {
        switch (_state)
        {
            case ParticleState.None:
                return;
            case ParticleState.Start:
                if (!Start.isPlaying)
                {
                    _state = ParticleState.Follow;
                    Follow.Play();
                    return;
                }
                break;
            case ParticleState.Follow:
                transform.position = Vector3.Lerp(transform.position, TouchManager.Instance.ray.origin, Time.deltaTime * 8.0f);
                break;
            case ParticleState.End:
                if (!End.isPlaying)
                {
                    _state = ParticleState.None;
                    return;
                }
                break;
            case ParticleState.FlyBack:
                if(Follow.isPlaying || End.isPlaying)
                {
                    
                }
                else
                {
                    _state = ParticleState.None;
                }
               
                break;
        }
    }
    public void StopAll()
    {
        switch (_state)
        {
            case ParticleState.Start:
                Start.Stop();
                break;
            case ParticleState.Follow:
                StartCoroutine(FlyBack());
                _state = ParticleState.FlyBack;
                break;
            case ParticleState.End:

                break;
            case ParticleState.None:
                break;
            case ParticleState.FlyBack:
                Follow.Stop();
                End.Stop();
                break;
            default:
                break;
        }
        _state = ParticleState.None;
    }
    IEnumerator FlyBack()
    {
        Vector3 startPos = transform.position;
        float t = 0;
        while (Vector3.Distance(transform.position, startObjPos) > 0.5f)
        {
            t += 2.0f * Time.deltaTime ;
            transform.position = Vector3.Lerp(startPos, startObjPos,t);
            yield return null;
        }
        Follow.Stop();
        transform.position = startObjPos;
        End.Play();
    }
    public void StartDrag(GameObject go)
    {
        startObj = go;
        SetParticleColor(go);
        var col = go.GetComponent<PolygonCollider2D>();
        Vector3 size = col.bounds.extents * go.transform.localScale.x;
        startObjPos = col.bounds.center;
        transform.position = startObjPos;
        size *= 0.25f;
        transform.localScale = size;
        Start.Play();
        _state = ParticleState.Start;
    }
    public void EndDrag()
    {
        Follow.Stop();
        
        End.Play();
        _state = ParticleState.End;
    }

    public enum ParticleState
{
    None,
    Start,
    Follow,
    End,
    FlyBack,
}
}