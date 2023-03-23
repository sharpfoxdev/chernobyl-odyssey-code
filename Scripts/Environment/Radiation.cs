using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for dealing damage to the player and enemies, when they enter radiation zone. 
/// </summary>
public class Radiation : MonoBehaviour
{

    /// <summary>
    /// When objects with Health component enters radiation collider, it starts
    /// taking damage over time coroutine
    /// </summary>
    /// <param name="hitInfo"></param>
    void OnTriggerEnter2D(Collider2D hitInfo) {
        Health healthSystem = hitInfo.GetComponent<Health>(); //try to get Health component on entered object
        if (healthSystem != null) //we found component Health on hit object
        {
            healthSystem.StartDamageOverTime();
        }
    }
    /// <summary>
    /// When object with Health component leaves radiation collider, we
    /// stop the coroutine of taking damage over time
    /// </summary>
    /// <param name="hitInfo"></param>
    void OnTriggerExit2D(Collider2D hitInfo) {
        Health healthSystem = hitInfo.GetComponent<Health>(); //try to get Health component on entered object
        if (healthSystem != null) //we found component Health on hit object
        {
            healthSystem.StopDamageOverTime();
        }
    }
}
