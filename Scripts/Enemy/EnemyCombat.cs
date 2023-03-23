using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing combat state of enemy AI in finite state machine. This state gets entered,
/// when the player is in the vision cone of enemy and is very close to him. 
/// </summary>
public class EnemyCombat : MonoBehaviour, EnemyState
{
    [SerializeField]
    private Enemy enemyContext;
    private Health playerHealth;
    private Coroutine combat;
    private AudioSource shootSound;
    [SerializeField]
    private SpriteRenderer muzleFlash;
    [SerializeField]
    private Animator animator;

    void Start() {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        shootSound = gameObject.GetComponent<AudioSource>();
    }
    /// <summary>
    /// Called from Enemy context, starts combat coroutine. 
    /// </summary>
    public void Act() {
        combat = StartCoroutine(CombatCoroutine());
    }
    /// <summary>
    /// Called from context. Stops combat coroutine (but lets playing the sound and animation one, so 
    /// it doesn't cut it off abruptly. Then changes state of FSM accoding to the PlayerVisibility
    /// </summary>
    /// <param name="newPlayerVisibility"></param>
    public void PlayerVisibilityChanged(PlayerVisibility newPlayerVisibility) {
        StopCoroutine(combat);
        switch (newPlayerVisibility) {
            case PlayerVisibility.Visible:
                enemyContext.ChangeState(gameObject.GetComponent<EnemyFollowPlayer>());
                break;
            case PlayerVisibility.OutOfReach:
                enemyContext.ChangeState(gameObject.GetComponent<EnemySearchPlayer>());
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Performs combat coroutine. Damages player and then waits before repeating. If
    /// the player dies, Enemy goes to patrolling. Be careful that the next frame PlayerDetection
    /// finds out, that the player is now out of reach and calls ChangeState() on now changed 
    /// EnemyPatrolPath to change to itself (EnemyPatrolPath). The change to the patrol 
    /// path has to be here, otherwise when the PlayerDetection would find out, that
    /// the player is outside of the vision cone after dying, it would call ChangeState() on 
    /// this script, which by default changes state to EnemySearchPlayer, not EnemyPatrolPath, 
    /// when the player leaves close proximity. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CombatCoroutine() {
        animator.CrossFadeInFixedTime("Idle Holding Rifle", 0.1f, 0);

        while (true) {
            shootSound.Play();
            animator.Play("Shot Rifle", 1);
            playerHealth.TakeDamage(30);
            if (playerHealth.HealthIsZero()) {
                break;
            }
            yield return new WaitForSeconds(1f);
            //animator.Play("None", 1);
        }
        enemyContext.ChangeState(gameObject.GetComponent<EnemyPatrolPath>());
        yield return null;
    }
    /// <summary>
    /// Plays sound and placeholder animation of shooting, which is for now just
    /// enabling and disabling of muzzle flash SpriteRender. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySoundAndAnimation() {
        shootSound.Play();
        //muzleFlash.enabled = true;
        animator.PlayInFixedTime("Shot Rifle", 1, 0.1f);
        //animator.Play("Shot Rifle", 1, 0f);
        //yield return new WaitForSeconds(0.05f);
        //muzleFlash.enabled = false;
        yield return null;
    }
    
}
