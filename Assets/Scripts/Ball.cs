using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D ballRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody2D>();
    }

    #region Get Ball Position

    //Get the ball current position
    internal Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
    #endregion

    #region Ball movement

    //Add force to the ball for movement
    internal void AddForce(Vector2 force)
    {
        if (GameManager.instance.gameState == GameState.Playing)
            ballRigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    //Check Velocity of ball is static or not
    internal void CheckVelocity()
    {
        // Prevent ball from rolling in the same directon forever
        if (ballRigidBody.velocity.x == 0)
        {
            ballRigidBody.velocity = new Vector2(Random.Range(1, speed), ballRigidBody.velocity.y);
        }
        else if (ballRigidBody.velocity.y == 0)
        {
            ballRigidBody.velocity = new Vector2(ballRigidBody.velocity.x, Random.Range(1, speed));
        }
    }
    #endregion

    #region Collision with the Ball

    //Detect collision with the ball
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "brick")
        {
            col.gameObject.GetComponent<BrickManager>().SetCount();

            GameManager.instance.currentScore++;
            GameManager.instance.SetScore();
        }

        if (col.gameObject.tag == "Finish")
        {
            GameManager.instance.SetGameOver();
        }
    }
    #endregion
}
