using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sript fo medkit, that is pickable. Can be picked by enemy and player. 
/// </summary>
public class Medkit : MonoBehaviour {
    [SerializeField]
    private float HealPoints = 50;

    /// <summary>
    /// Heals objects that enters its collider if it has Health component attached
    /// </summary>
    /// <param name="hitInfo">Object that entered the collider of medkit</param>
    void OnTriggerEnter2D(Collider2D hitInfo) {
        Health healthSystem = hitInfo.GetComponent<Health>(); //try to get Health component on entered object
        if (healthSystem != null) //we found component Health on hit object
        {
            healthSystem.Heal(HealPoints);
        }
        Destroy(gameObject);
    }
}