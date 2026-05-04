using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 1;

    void Start() {
        Destroy(gameObject,lifetime);
        
    }

}
