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
        private SpriteRenderer spriteRenderer;

        private CharacterState prevState;
        private void Awake () {
            character = gameObject.GetComponent<PlayerCharacter>();
            if (character == null) {
                throw new System.Exception("Cannot find PlayerCharacter of : " + gameObject.name);
            }
            animator = transform.Find("sprite")?.gameObject.GetComponent<Animator>();
            if (animator == null) {
                throw new System.Exception("Cannot find Animator of : " + gameObject.name);
            }

            spriteRenderer = transform.Find("sprite")?.gameObject.GetComponent<SpriteRenderer>();
            if (animator == null) {
                throw new System.Exception("Cannot find SpriteRenderer of : " + gameObject.name);
            }

            prevState = CharacterState.DEFAULT;
        }

        private void Update () {
            if (character.direction == CharacterDirection.LEFT)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            if (prevState != character.state)
            {
                switch (character.state)
                {
                    case CharacterState.DEFAULT:
                        animator.Play("default");
                        break;
                    case CharacterState.RUN:
                        animator.Play("run");
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
