using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for handling player death and respawning. 
/// </summary>
public class Player : MonoBehaviour
{
    private GameMaster gameMaster;
    private Health health;
    private void Start()
    {
        gameMaster = GameMaster.instance;
        health = gameObject.GetComponent<Health>();
        if (GameMaster.instance.playerRespawned == true) {
            //we died and then respawned
            health.SetHealth(60f);

        }
        transform.position = gameMaster.lastCheckpointPosition;

    }

    /// <summary>
    /// We check each frame, whether we died, if yes, we respawn player. We only change players position,
    /// as reloading the scene breaks music player. Might be reworked in the future. UPDATE - this got reworked
    /// and now we reload the scene. It improved the gameplay. 
    /// </summary>
    private void Update()
    {
        if (health.HealthIsZero()) {
            //health.SetHealth(60f);
            //transform.position = gameMaster.lastCheckpointPosition;
            //maybe not reload scene
            GameMaster.instance.playerRespawned = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
