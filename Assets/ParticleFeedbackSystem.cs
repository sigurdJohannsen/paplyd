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
    AudioSource audio;

    private void PlayParticle(ParticleSystem ps)
    {
        ps.Play();
        if (ps.Equals(Start))
        {
            Audio.clip = AudioManager.Instance.GetAudio(Sounds.GrabSound);
            Audio.loop = false;
        }
        else if (ps.Equals(Follow))
        {
            Audio.clip = AudioManager.Instance.GetAudio(Sounds.HoverSound);
            Audio.loop = true;
        }
        else if (ps.Equals(End))
        {
            Audio.clip = AudioManager.Instance.GetAudio(Sounds.DropSound);
            Audio.loop = false;
        }
        Audio.Play();
    }

    public AudioSource Audio
    {
        get
        {
            if (!audio)
                audio = GetComponent<AudioSource>();
            return audio;
        }
    }

    public void SwapParticleFeedback(GameObject from, GameObject to)
    {
        StartCoroutine(SwapParticleCoroutine(to, from));
    }
    private IEnumerator SwapParticleCoroutine(GameObject from, GameObject to)
    {
        _state = ParticleState.None;
        startObj = from;
        SetParticleColor(from);
        var col = from.GetComponent<PolygonCollider2D>();
        Vector3 size = col.bounds.extents * from.transform.localScale.x;
        size *= 0.25f;
        transform.localScale = size;
        startObjPos = col.bounds.center;
        transform.position = startObjPos;
        col = to.GetComponent<PolygonCollider2D>();
        Vector3 toPos = col.bounds.center;
        PlayParticle(Start);
        
        while (Start.isPlaying) yield return null;
        PlayParticle(Follow);
        float t = 0;
        while (Vector3.Distance(transform.position, toPos) > 0.5f)
        {
            t += 2.0f * Time.deltaTime;
            transform.position = Vector3.Lerp(startObjPos, toPos, t);
            yield return null;
        }
        Follow.Stop();
        transform.position = toPos;
        PlayParticle(End);
    }
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
                    PlayParticle(Follow);
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

                StartCoroutine(FlyBack(startObjPos));
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
    IEnumerator FlyBack(Vector3 backPos)
    {
        while (Start.isPlaying) yield return null;
        if(Follow.isStopped)
        {
            PlayParticle(Follow);
        }
        else
        {
            AudioManager.Instance.PlayAudio(Sounds.ReturnSound);
        }
        Vector3 startPos = transform.position;
        float t = 0;
        while (Vector3.Distance(transform.position, backPos) > 0.5f)
        {
            t += 2.0f * Time.deltaTime ;
            transform.position = Vector3.Lerp(startPos, backPos, t);
            yield return null;
        }
        Follow.Stop();
        transform.position = backPos;
        PlayParticle(End);

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
        PlayParticle(Start);
        _state = ParticleState.Start;
    }
    public void EndDrag(GameObject go)
    {
        var col = go.GetComponent<PolygonCollider2D>();
        Vector3 destination = col.bounds.center;
        _state = ParticleState.FlyBack;
        StartCoroutine(FlyBack(destination));

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