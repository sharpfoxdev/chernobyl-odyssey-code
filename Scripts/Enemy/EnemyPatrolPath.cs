using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class representing FSM state of enemy AI, when the enemy follows patrol path. 
/// This state gets entered, when player dies, or from EnemySearchPlayer state, when enemy cannot
/// find player. 
/// </summary>
public class EnemyPatrolPath : EnemyMovement, EnemyState 
{
    [SerializeField]
    private Transform patrolPath;
    [SerializeField]
    private float speed = 7f;
    [SerializeField]
    private float waitTime = 0.5f;
    [SerializeField]
    private Enemy enemyContext;
    private Vector3[] waypoints;
    private Coroutine followingPath;
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// In the beginning gets all the waypoints and starts following them
    /// </summary>
    private void Awake() {
        waypoints = new Vector3[patrolPath.childCount];
        for(int i = 0; i< patrolPath.childCount; i++) {
            waypoints[i] = patrolPath.GetChild(i).position;
        }    
    }
    /// <summary>
    /// Called from Enemy context, starts patrol coroutine. 
    /// </summary>
    public void Act() {
        followingPath = StartCoroutine(FollowPathCoroutine());
    }
    /// <summary>
    /// Called from context. Stops patroling coroutine, then changes state of FSM accoding to the PlayerVisibility
    /// </summary>
    /// <param name="newPlayerVisibility"></param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility) {
        StopCoroutine(followingPath);
        switch (newPlayerVisibility) {
            case PlayerVisibility.Visible:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyFollowPlayer>());
                break;
            case PlayerVisibility.CloseProximity:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyCombat>());
                break;
            case PlayerVisibility.OutOfReach:
                //this gets called when player dies and close combat changes the state to Patroling, 
                //but because the state changed from closeProximity to OutOfReach, PlayerVisibilityChanged()
                //is called on this object
                enemyContext.ChangeState(gameObject.GetComponent<EnemyPatrolPath>());
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Coroutine for following patrol path. We start at the first waypoint
    /// and then follow the path. After reaching next waypoint, we wait for a bit and
    /// then move to the following one. 
    /// </summary>
    /// <returns></returns>
    IEnumerator FollowPathCoroutine() {
        animator.CrossFadeInFixedTime("None", 0.2f, 1);
        animator.CrossFadeInFixedTime("Walk Holding Rifle", 0.2f, 0);
        int targetWaypointIndex = 0;

        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        FaceCorrectDirection(targetWaypoint);

        while (true) {
            // BE CAREFUL NOT TO CROSSFADE WITH ITSELF!
            //animator.Play("Walk Holding Rifle", 0); 
            if (ReachedTarget(targetWaypoint)){
                animator.CrossFadeInFixedTime("Idle Holding Rifle", 0.3f, 0, float.NegativeInfinity);
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                //we wait for a bit before moving again
                yield return new WaitForSeconds(waitTime);
                FaceCorrectDirection(targetWaypoint);
                animator.CrossFadeInFixedTime("Walk Holding Rifle", 0.2f, 0, float.NegativeInfinity);
            }
            transform.position = MoveTowardsOnXAxis(targetWaypoint, speed);
            yield return null;
        }
    }


    /// <summary>
    /// Draws spheres and lines along the patrol path and its waypoints and edges
    /// </summary>
    private void OnDrawGizmos() {
        //gets first waypoint
        Vector3 startWaypointPosition = patrolPath.GetChild(0).position;
        Vector3 previousWaypointPosition = startWaypointPosition;
        foreach(Transform waypoint in patrolPath) {
            Gizmos.DrawSphere(waypoint.position, 0.5f);
            Gizmos.DrawLine(previousWaypointPosition, waypoint.position);
            previousWaypointPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousWaypointPosition, startWaypointPosition);
    }

}
