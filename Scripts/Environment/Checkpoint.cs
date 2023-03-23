using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for checkpoint logic. 
/// </summary>
public class Checkpoint : MonoBehaviour
{
    private GameMaster gameMaster;
    private void Start() {
        gameMaster = GameMaster.instance;
    }
    /// <summary>
    /// If player eneted the collider of checkpoint, we remember this checkpoint as
    /// a last visited checkpoint in GameMaster. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other) {
        //if the thing that entered the collider is player
        if (other.CompareTag("Player")) {
            //we save this checkpoint as a last checkpoint player went through
            gameMaster.lastCheckpointPosition = transform.position;
        }
    }
}
