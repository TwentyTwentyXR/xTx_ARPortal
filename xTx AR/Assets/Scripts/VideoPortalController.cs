﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPortalController : PortalController
{
    public VideoPlayer video;

    protected override void OnEnterWorld()
    {
        base.OnEnterWorld();
        video.Play();
    }

    protected override void OnExitWorld()
    {
        base.OnExitWorld();
        video.Stop();
    }
}
