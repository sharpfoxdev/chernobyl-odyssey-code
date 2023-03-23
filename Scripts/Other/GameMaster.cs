using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class, that persists across different scenes and remembers the last checkpoint, that the player
/// crossed, so it can respawn the player there. There should be just one instance of GameMaster in
/// the game. 
/// </summary>
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance  {get; private set; }
    [SerializeField]
    public Vector2 lastCheckpointPosition;
    [SerializeField]
    public bool playerRespawned = false;

    [SerializeField]
    private Transform InitialCheckpoint;
    /// <summary>
    /// Sets up GameMaster object, so there is just one. 
    /// </summary>
    private void Awake() {
        if (instance == null) {
            instance = this;
            //we don't want to destroy game master when changing/loading scenes
            //as it has to remember the last checkpoint the player went through
            DontDestroyOnLoad(instance);
        }
        else {
            //in a case there is already a gameMaster, we destroy the game objdect
            //so we don't end up with multiple game masters in the same scene
            Destroy(gameObject);
        }
        lastCheckpointPosition = InitialCheckpoint.position;
    }
}
