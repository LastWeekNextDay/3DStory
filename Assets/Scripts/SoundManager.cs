using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    
    public void PlaySound(string audioName, Vector3? pos = null)
    {
        var foundAudioClip = audioClips.Find(audioClip => audioClip.name == audioName);
        if (foundAudioClip == null) return;
        pos ??= transform.position;
        AudioSource.PlayClipAtPoint(foundAudioClip, (Vector3)pos);
    }

    public void PlaySound(string audioName, AudioSource source)
    {
        var foundAudioClip = audioClips.Find(audioClip => audioClip.name == audioName);
        if (foundAudioClip == null) return;
        source.PlayOneShot(foundAudioClip);
    }
}
