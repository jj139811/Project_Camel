using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ingame.Character;

namespace Ingame.Animation
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        private PlayerCharacter character;
        private Animator animator;
        private GameObject spriteObject;

        private CharacterState prevState;

        public bool flipRun = false;
        private void Awake () {
            character = gameObject.GetComponent<PlayerCharacter>();
            if (character == null)
            {
                throw new System.Exception("Cannot find PlayerCharacter of : " + gameObject.name);
            }
            spriteObject = transform.Find("sprite")?.gameObject;
            if (spriteObject == null)
            {
                throw new System.Exception("Cannot find sprite of : " + gameObject.name);
            }
            animator = spriteObject.GetComponent<Animator>();
            if (animator == null)
            {
                throw new System.Exception("Cannot find Animator of : " + spriteObject.name);
            }

            prevState = CharacterState.DEFAULT;
        }

        private void FixedUpdate () {
            if (character.direction == CharacterDirection.LEFT)
            {
                spriteObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                spriteObject.transform.localScale = new Vector3(1, 1, 1);
            }
            if (prevState != character.state)
            {
                Debug.Log("state changed from: " + prevState.ToString() + " to: " + character.state.ToString());
                switch (character.state)
                {
                    case CharacterState.DEFAULT:
                        animator.Play("default");
                        break;
                    case CharacterState.RUN:
                        if (flipRun)
                        {
                            animator.Play("run_flip");
                        }
                        else
                        {
                            animator.Play("run");
                        }
                        break;
                    default:
                        animator.Play("default");
                        break;
                }
                prevState = character.state;
            }
        }
    }
}
