using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Billboard script that makes canvas with Health bar always to face main camera. 
/// Otherwise the health bar gets flipped when enemy moves back and forwards changing
/// directions. 
/// </summary>
public class Billboard : MonoBehaviour
{
    void LateUpdate() {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
