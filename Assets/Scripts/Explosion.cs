﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }
}
