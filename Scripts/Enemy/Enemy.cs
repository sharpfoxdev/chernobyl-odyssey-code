using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This enum is used by Enemy and it's states to determine, whether the Enemy can see the Player. This then
/// affects the way Enemy reacts to the Player. 
/// </summary>
public enum PlayerVisibility {
    OutOfReach,
    Visible,
    CloseProximity
}

/// <summary>
/// Interface for the Finite State Machine states of Enemy AI. 
/// </summary>
public interface EnemyState {
    /// <summary>
    /// Starts performing the job which the given state is intended for. 
    /// </summary>
    public void Act();

    /// <summary>
    /// Get's called, when player moves in/out of vision cone, which then in most cases
    /// triggers the change of state of FSM. The change gets handled by the state itself. 
    /// </summary>
    /// <param name="newPlayerVisibility">New visibility of the player. </param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility);

}

/// <summary>
/// Context class for the Enemy AI, which is implemented as finite state machine. It holds reference
/// to the current state of the machine and player visibility. It delegates work to the current state
/// and also notifies it, if the player visibility has changed. This often changes current state, so
/// this class also provides method changing current state, that gets called from within the states
/// when applicable. States are aware of each other. 
/// </summary>
public class Enemy : MonoBehaviour
{
    private EnemyState enemyState;
    public PlayerVisibility playerVisibility;

    /// <summary>
    /// Sets up playerVisibility as well as initial state of FSM
    /// </summary>
    private void Start()
    {
        enemyState = gameObject.GetComponent<EnemyPatrolPath>();
        //if the player is actually in different state, it doesn't matter
        //as in the next frame it will be fixed to the correct state
        playerVisibility = PlayerVisibility.OutOfReach; 
        Act();
    }
    /// <summary>
    /// Delegates work to the current state of FSM
    /// </summary>
    public void Act() {
        enemyState.Act();
    }
    /// <summary>
    /// Informs current state of FSM, that the playerVisibility has changed. State then takes
    /// necessary steps to handle the change. 
    /// </summary>
    /// <param name="newPlayerVisibility">New player visibility</param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility) {
        playerVisibility = newPlayerVisibility;
        enemyState.PlayerVisibilityChanged(newPlayerVisibility);
    }
    /// <summary>
    /// Gets called from within the state, when the state of state machine changes to different state. 
    /// </summary>
    /// <param name="newState">Changes current enemyState and delegates work to it. </param>
    public void ChangeState(EnemyState newState) {
        enemyState = newState;
        Act(); 
    }
    
}
