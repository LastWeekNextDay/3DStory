using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSounds", menuName = "ScriptableObjects/CharacterSounds", order = 1)]
public class CharacterSounds : ScriptableObject
{
    public AudioClip IdleSound;
    public AudioClip HurtSound;
    public AudioClip DeathSound;
    public AudioClip DashSound;
}
