using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFeedbackSystem : MonoBehaviour {

    ParticleState _state;

    [SerializeField]
    ParticleSystem Start,
    Follow,
    End;
    public void Update()
    {
        switch (_state)
        {
            case ParticleState.None:
                return;
            case ParticleState.Start:
                if(!Start.isPlaying)
                {
                    _state = ParticleState.Follow;
                    return;
                }
                break;
            case ParticleState.Follow:
                transform.position = TouchManager.Instance.ray.origin;
                break;
            case ParticleState.End:
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
                Follow.Stop();
                break;
            case ParticleState.End:
                break;
            default: 
                break;
        }
        _state = ParticleState.None;
    }

    public void StartDrag()
    {
        Start.Play();
        _state = ParticleState.Start;
    }

    public enum ParticleState
{
    None,
    Start,
    Follow,
    End,
}
}