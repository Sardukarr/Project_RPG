﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FallowCamera : MonoBehaviour
    {
        [SerializeField] Transform target=null;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }

}