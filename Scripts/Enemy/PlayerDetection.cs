using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

/// <summary>
/// Script, that constantly checks, whether enemy can see player. This scripts then calls Enemy script (context
/// of FSM) which then sends the info further into current state. 
/// </summary>
public class PlayerDetection : MonoBehaviour
{
    [SerializeField]
    private Light2D spotlightVision; //spotlight, we use it to get info about view angle
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private float closeDistance;
    [SerializeField]
    private Transform eyesTransform; //point, from which enemy looks
    [SerializeField]
    private LayerMask obstacleMask; //layer mask for obstacles through which enemy cannot see player
    [SerializeField]
    private Enemy enemyObject; //used for informing Enemy, that the visibility of player changed
    private PlayerVisibility playerVisibility; //current player visibility from the point of enemy
    private float viewAngle;
    private Transform player; //player, that we detect
    private Color originalLightColor; //used when changing color of the light
    

    /// <summary>
    /// We set up needed variables in the Start method
    /// </summary>
    private void Start() {
        viewAngle = spotlightVision.pointLightOuterAngle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalLightColor = spotlightVision.color;
        playerVisibility = PlayerVisibility.OutOfReach;
    }

    /// <summary>
    /// Every update we check whether the player is in the vision cone and if yes, whether
    /// he is also in close proximity. Based on that we change vision cone color and inform Enemy object, 
    /// that the visibility of the player changed. Enemy then handles that information further
    /// </summary>
    private void Update() {
        if (CanSeePlayer()) {
            spotlightVision.color = Color.yellow;
            playerVisibility = PlayerVisibility.Visible;
            if (PlayerInCloseDistance()) {
                spotlightVision.color = Color.red;
                playerVisibility = PlayerVisibility.CloseProximity;
            }
        }
        else {
            playerVisibility = PlayerVisibility.OutOfReach;
            spotlightVision.color = originalLightColor;
        }
        if (enemyObject.playerVisibility != playerVisibility) {
            enemyObject.PlayerVisibilityChanged(playerVisibility);
        }

    }

    /// <summary>
    /// Checks, whether conditions for view distance, view angle and no obstacle hold, if yes, we can see
    /// the player and the method returns true
    /// </summary>
    /// <returns>Returns true, if the player is in the vision cone of the enemy. </returns>
    private bool CanSeePlayer() {
        if(PlayerInViewDistance() && PlayerInViewAngle() && NoObstacleBetweenEnemyAndPlayer()) {
            return true;
        }
        return false;
    }
    

    /// <summary>
    /// Checks, if player is in view distance to the enemy and returns true, if yes. 
    /// </summary>
    /// <returns>True, if player is in close distance from the enemy. </returns>
    private bool PlayerInViewDistance() {
        if(Vector3.Distance(player.position, eyesTransform.position) < viewDistance) {
            Debug.Log("in view distance");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks, whether the player is in the view angle of the enemy. Enemy is looking into some direction, 
    /// from that direction to both sides is the viewAngle / 2 (in total the full viewAngle) and we check, whether
    /// the angle from the direction enemy is facing to the player is smaller (within) than the viewAngle / 2 (one 
    /// side of the viewAngle). 
    ///                       
    ///                                         . view angle one side
    ///                                       . 
    ///                                     .
    ///                                   .
    ///                           enemy o--------------- direction enemy is facing
    ///                                   .
    ///  player outside viewAngle o         .       o player in between direction enemy is facing and viewAngle / 2
    ///                                       .
    ///                                         . view angle other side
    /// </summary>
    /// <returns>Returns true, if the player is in the view angle of enemy. </returns>
    private bool PlayerInViewAngle() {
        Vector3 directionToPlayer = (player.position - eyesTransform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(eyesTransform.up, directionToPlayer);
        if (angleBetweenEnemyAndPlayer < viewAngle / 2f) {
            Debug.Log("in view angle");
            return true;
        }
        return false;
    }
    /// <summary>
    /// Checks, whether there is an obstacle between player and the enemy using raycasts
    /// </summary>
    /// <returns>Returns true, if there is no obstacle between player and enemy. </returns>
    private bool NoObstacleBetweenEnemyAndPlayer() {
        // makes raycast from eyes to player, if the cast hits obstacle, it returns true
        if (!Physics.Linecast(eyesTransform.position, player.position, obstacleMask)) {
            // we didnt hit obstacle with the raycast
            Debug.Log("No obstacle");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks, if player is in close distance to the enemy and returns true, if yes. 
    /// This method doesn't take in account, if the player is hiding behind the obstacle or
    /// whether he is in view angle, this has to be checked with other methods. 
    /// </summary>
    /// <returns>True, if player is in close distance from the enemy. </returns>
    private bool PlayerInCloseDistance() {
        if (Vector3.Distance(player.position, eyesTransform.position) < closeDistance) {
            Debug.Log("in view distance");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Draws lines for zones of visibility and close proximity of player from the enemy
    /// </summary>
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyesTransform.position, eyesTransform.up * viewDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(eyesTransform.position, eyesTransform.up * closeDistance);
    }
}
