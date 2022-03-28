using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess;
using UnityEngine.SceneManagement;

namespace Chess
{

    public class CameraSizeHandler : MonoBehaviour
    {

        //Set the Camera orthographic size according to Screen Resolution
        void Start()
        {
            SpriteRenderer containerSpriteRenderer = GameObject.Find("GameContainer").GetComponent<SpriteRenderer>();

            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = containerSpriteRenderer.bounds.size.x / containerSpriteRenderer.bounds.size.y;

            if (screenRatio >= targetRatio)
            {
                Camera.main.orthographicSize = containerSpriteRenderer.bounds.size.y / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                Camera.main.orthographicSize = containerSpriteRenderer.bounds.size.y / 2 * differenceInSize;
            }
        }
    }
}