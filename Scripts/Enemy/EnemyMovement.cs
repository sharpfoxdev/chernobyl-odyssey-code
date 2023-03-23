using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing set of methods, that are used in the children classes, 
/// that needs some helper methods for moving around (like EnemyPatrol or EnemySearch)
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// Checks if we reached target - the x coordinate of target equals our x coordinate. This solves issue, 
    /// when the target is floating high in the air  and we cannot move towards it. Originally there was check, 
    /// if the target is witin our collider, which worked as long as the target was witin the colliders height. 
    /// </summary>
    /// <param name="target">Target to reach</param>
    /// <returns>True if we reached target</returns>
    protected bool ReachedTarget(Vector2 target) {
        Collider2D enemyCollider = gameObject.GetComponent<Collider2D>();
        //this check is like this in a case the target is in the air, so we dont end up jumping weirdly in one spot
        //so we are checking, if we are within a small radius on of the target on the x axis
        if (Mathf.Abs(target.x - transform.position.x) < 0.5) {
            return true;
        }
        return false;
        //enemyCollider.OverlapPoint(target) || 
        //(target.x-1) < transform.position.x && transform.position.x < (target.x+1)
    }
    /// <summary>
    /// Instead of using y axis of the target (which could be flying somewhere where we cant reach, 
    /// we use y axis point of self to move towards target only in left right direction
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    protected Vector3 MoveTowardsOnXAxis(Vector2 target, float speed) {

        return Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
    }
    /// <summary>
    /// If appripriate, changes the direction the enemy is facing based on towards
    /// which target he is moving. 
    /// </summary>
    /// <param name="movingTowards">Target towards which the enemy is moving</param>
    protected void FaceCorrectDirection(Vector3 movingTowards) {

        bool facingRight;
        if(transform.eulerAngles.y == 0) {
            facingRight = false;
        }
        else{
            facingRight = true;
        }
        if (transform.position.x < movingTowards.x && !facingRight) {
            //we are to the left of target and want to move right
            Flip();
        }
        if (transform.position.x > movingTowards.x && facingRight) {
            //we are to the right of target and want to move left
            Flip();
        }
    }
    /// <summary>
    /// Makes enemy face the other way around, including
    /// </summary>
    protected void Flip() {
        // THIS MAKES FLOATING POINT IMPRECISIONS! DO NOT COMPARE ROTATION
        //VALUE AFTER USING Rotate() FUNCTION ON EQUALITY!
        //transform.Rotate(0f, 180f, 0f);

        //this fixes the impresition problem by creating a new value each time
        //instead of editing an old one
        if (transform.eulerAngles.y == 0) {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
