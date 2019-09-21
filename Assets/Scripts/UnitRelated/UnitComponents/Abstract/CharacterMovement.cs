using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public abstract class CharacterMovement : MonoBehaviour
    {
        public float movementSpeed = 1f;
        public CharacterSoundsPlayer SoundPlayer;

        protected CharacterRenderer renderer;
        protected Rigidbody2D rigidBody;
        protected Attack attack;

        protected void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            renderer = GetComponentInChildren<CharacterRenderer>();
            attack = GetComponent<Attack>();
        }

        protected abstract void FixedUpdate();

        protected bool MovingNow(Vector2 direction)
        {
            return direction.magnitude > movementThreshold;
        }

        /// <summary>
        /// Character movement starts only if magnitude of controls'
        /// direction vector is greater than this value
        /// </summary>
        protected float movementThreshold = .01f;
    }
}
