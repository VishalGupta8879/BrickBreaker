using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Global Variable
    public Rigidbody2D ball;
    public Transform leftLimit, rightLimit, upperLimit, bottomLimit;

    private Vector2 startPos;

    private float mouseXPos;
    private float ballPos;

    private float zDistance, leftCorner, rightCorner;    

    private Camera cam;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        zDistance = transform.position.z - cam.transform.position.z;
        startPos = transform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;

        //Limiting the movement of Paddle
        leftCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).x + sprite.bounds.size.x / 2;        
        rightCorner = cam.ViewportToWorldPoint(new Vector3(1, 0, zDistance)).x - sprite.bounds.size.x / 2;
        
        
        PositionBorder();
    }

    #region Placing borders on edge of screen
    //Placing the borders on the edge of the screen
    void PositionBorder()
    {
        float OrthoWidth = cam.orthographicSize * cam.aspect;
        leftLimit.position = new Vector3(transform.localPosition.x - OrthoWidth, 0.0F, 0);
        rightLimit.transform.position = new Vector3(transform.localPosition.x + OrthoWidth - rightLimit.GetComponent<SpriteRenderer>().bounds.size.x / 6, 0.0F, 0);

        upperLimit.transform.position = (cam.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)));

        upperLimit.transform.position = new Vector3(upperLimit.transform.position.x, upperLimit.transform.localPosition.y - upperLimit.GetComponent<SpriteRenderer>().bounds.size.x / 6, 0);

        bottomLimit.transform.position = new Vector3(0, transform.localPosition.y - OrthoWidth, 0);
    }
    #endregion

    #region Paddle Movement with mouse position
    // Move paddle with mouse
    private void MoveWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDistance;
        mouseXPos = cam.ScreenToWorldPoint(mousePos).x;
        Vector3 paddlePos = gameObject.transform.position;
        paddlePos.x = Mathf.Clamp(mouseXPos, leftCorner, rightCorner);
        gameObject.transform.position = paddlePos;
    }

    private void OnMouseDrag()
    {
        if (GameManager.instance.IsBallServed() && GameManager.instance.gameState == GameState.Playing)
            MoveWithMouse();
    }
    #endregion

}
