using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing FSM state of enemy AI, when the enemy follows player around the map. 
/// This state gets entered, when the player enters vision cone of the player and stays witin it. 
/// </summary>
public class EnemyFollowPlayer : EnemyMovement, EnemyState
{
    private Coroutine followPlayer;
    [SerializeField]
    private float followSpeed = 9f;
    private Transform player; 
    [SerializeField]
    private Enemy enemyContext;
    [SerializeField]
    private Animator animator;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    /// <summary>
    /// Called from Enemy context, starts following coroutine. 
    /// </summary>
    public void Act() {
        followPlayer = StartCoroutine(FollowPlayer());
    }
    /// <summary>
    /// Called from context. Stops follow coroutine, then changes state of FSM accoding to the PlayerVisibility
    /// </summary>
    /// <param name="newPlayerVisibility"></param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility) {
        StopCoroutine(followPlayer);
        switch(newPlayerVisibility){
            case PlayerVisibility.OutOfReach:
                enemyContext.ChangeState(gameObject.GetComponent<EnemySearchPlayer>());
                break;
            case PlayerVisibility.CloseProximity:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyCombat>());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Every frame we get the player position and we move towards it. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPlayer() {
        animator.CrossFadeInFixedTime("Aim Rifle", 0.2f, 1);
        animator.CrossFadeInFixedTime("Walk Holding Rifle", 0.1f, 0);
        while (enemyContext.playerVisibility == PlayerVisibility.Visible) {
            FaceCorrectDirection(player.position);
            transform.position = MoveTowardsOnXAxis(player.position, followSpeed); 
            yield return null;
        }

    }
}
