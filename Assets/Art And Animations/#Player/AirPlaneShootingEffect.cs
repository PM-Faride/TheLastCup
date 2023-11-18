using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AirPlaneShootingEffect : MonoBehaviour
{
    private SkeletonAnimation vfx;
    [SpineAnimation] public string vfxAnime;
    void Start()
    {
        vfx = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        vfx.AnimationName = vfxAnime;
    }
}
