using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing FSM state of enemy AI, when the enemy searches player around the map. 
/// This state gets entered, when the player enters vision cone of the player and stays witin it. 
/// </summary>
public class EnemySearchPlayer : EnemyMovement, EnemyState 
{
    private Coroutine searchPlayer;
    private Transform player; 
    [SerializeField]
    private Enemy enemyContext;
    [SerializeField]
    private float followSpeed = 9f;
    [SerializeField]
    private Animator animator;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    /// <summary>
    /// Called from Enemy context, starts search coroutine. 
    /// </summary>
    public void Act() {
        searchPlayer = StartCoroutine(SearchPlayer());
    }
    /// <summary>
    /// Called from context. Stops search coroutine, then changes state of FSM accoding to the PlayerVisibility
    /// </summary>
    /// <param name="newPlayerVisibility"></param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility) {
        StopCoroutine(searchPlayer);
        switch (newPlayerVisibility) {
            case PlayerVisibility.Visible:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyFollowPlayer>());
                break;
            case PlayerVisibility.CloseProximity:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyCombat>());
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Enemy checks the last position, where he saw player, moves there and looks around. If the player
    /// doesn't re-enter vision cone, enemy goes back to patrolling. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchPlayer() {
        animator.CrossFadeInFixedTime("Aim Rifle", 0.2f, 1);
        animator.CrossFadeInFixedTime("Walk Holding Rifle", 0.2f, 0);
        Vector3 playerLastSeen = player.position;
        while (enemyContext.playerVisibility == PlayerVisibility.OutOfReach && !ReachedTarget(playerLastSeen)) {
            FaceCorrectDirection(playerLastSeen);
            transform.position = MoveTowardsOnXAxis(playerLastSeen, followSpeed);
            yield return null;
        }
        for(int i = 0; i < 2; i++) {
            animator.CrossFadeInFixedTime("Idle Holding Rifle", 0.1f, 0);
            yield return new WaitForSeconds(1f);
            Flip();
        }
        yield return new WaitForSeconds(1f);
        enemyContext.ChangeState(gameObject.GetComponent<EnemyPatrolPath>());
    }
}
