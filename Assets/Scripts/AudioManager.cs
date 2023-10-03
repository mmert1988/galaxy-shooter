using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _explosionEffectSource;
    private AudioSource _powerUpEffectSource;

    private void Start()
    {
        _explosionEffectSource = gameObject.transform.Find("ExplosionEffect").GetComponent<AudioSource>();
        _powerUpEffectSource = gameObject.transform.Find("PowerUpEffect").GetComponent<AudioSource>();
    }

    public void PlayExplosionEffect()
    {
        _explosionEffectSource.Play();
    }

    public void PlayPowerUpEffect()
    {
        _powerUpEffectSource.Play();
    }
}
