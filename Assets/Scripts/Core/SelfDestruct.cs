using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class SelfDestruct : MonoBehaviour
    {
        [SerializeField] float time=5f;
        void Start()
        {
            Destroy(gameObject, time);
        }
    }
}