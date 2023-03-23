using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script used by player to deal damage to enemies. 
/// </summary>
public class MeleeAttack : MonoBehaviour
{
    [SerializeField]
    private Transform attackZonePosition;
    [SerializeField]
    private float attackZoneRange;
    [SerializeField]
    private float damage = 20;
    [SerializeField]
    private LayerMask enemyMask;
    private AudioSource slashSound;
    [SerializeField]
    private SpriteRenderer knife;
    [SerializeField]
    private Animator animator;

    private void Start() {
        slashSound = gameObject.GetComponent<AudioSource>();
    }
    /// <summary>
    /// Callback function called by input system when key bound to attack is pressed
    /// </summary>
    /// <param name="context">Information from the input system</param>
    public void OnMeleeAttack(InputAction.CallbackContext context) {
        if (context.action.triggered) {
            Attack();
        }
    }
    /// <summary>
    /// Attacks object in the attack zone. If we approach enemy unseen, we damage full blow, but if the enemy can
    /// see us, we damage the enemy only a little bit. This is to prevent player just constantly clicking attack. 
    /// </summary>
    private void Attack() {
        slashSound.Play();
        animator.Play("Attack Main Hand 3", 0);
        //StartCoroutine(AttackSoundAndVisuals());
        //gets list of all enemies, that are in attack zone
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackZonePosition.position, attackZoneRange, enemyMask);
        //deals damage to all of them
        foreach(Collider2D enemy in enemiesToDamage) {
            if(enemy.GetComponent<Enemy>().playerVisibility == PlayerVisibility.OutOfReach) {
                //we approached enemy unseen
                enemy.GetComponent<Health>().TakeDamage(damage);
            }
            else {
                //enemy can see us, so we deal only a little damage
                enemy.GetComponent<Health>().TakeDamage(damage/10);
            }
        }
    }
    /// <summary>
    /// Creates pseudo animation of attack by enabling knife sprite. Also plays audio of swoosh. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackSoundAndVisuals() {
        slashSound.Play();
        knife.enabled = true;
        yield return new WaitForSeconds(0.2f);
        knife.enabled = false;
    }
    /// <summary>
    /// Draws gizmos of the attack zone
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackZonePosition.position, attackZoneRange);
    }
}
