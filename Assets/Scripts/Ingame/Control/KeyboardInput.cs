using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Control
{
    public class KeyboardInput : MonoBehaviour
    {
        private PlayerInput playerInput;
        private void Awake () {
            playerInput = gameObject.GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                throw new System.Exception("Cannot find PlayerInput of : " + gameObject.name);
            }
        }
        private void Update () {
            HandleInput();
        }
        private void HandleInput () {
            Vector2 moveDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                //moveDirection += new Vector2(0, 1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += new Vector2(-1, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                //moveDirection += new Vector2(0, -1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += new Vector2(1, 0);
            }
            playerInput.Move(moveDirection.normalized);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerInput.Jump();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                playerInput.JumpPressing();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                playerInput.JumpUp();
            }
        }
    }
}
