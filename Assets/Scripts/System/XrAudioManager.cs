using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrAudioManager : MonoBehaviour
{
    [SerializeField] TheWall wall;
    [SerializeField] AudioSource wallSource;
    [SerializeField] AudioClip destroyWallClip;
    [SerializeField] private AudioClip fallbackClip;
    private const string FallBackClip_Name = "fallbackClip";

    void OnEnable()
    {
        if (fallbackClip == null)
        {
            fallbackClip = AudioClip.Create(FallBackClip_Name, 1, 1, 1000, true);
        }

        if (wall != null)
        {
            destroyWallClip = wall.getDestroyClip;
            if (destroyWallClip == null)
            {
                destroyWallClip = fallbackClip;
            }
            wall.OnDestroy.AddListener(OnDestroyWall);
        }
    }

    private void OnDestroyWall()
    {
        if(wallSource != null)
        {
            wallSource.Play();
        }
    }

    void OnDisable()
    {
         if (wall != null)
        {
            wall.OnDestroy.RemoveListener(OnDestroyWall);
        }
    }
}
