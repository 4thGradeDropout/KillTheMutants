using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 1f;

    public Vector2 CurrentDirection { get; set; }

    protected CharacterRenderer renderer;
    protected Rigidbody2D rigidBody;
    protected Attack attack;
    protected Dying dying;
    protected CharacterSoundsPlayer soundPlayer;

    public void ReceiveDirection(Vector2 newDir)
    {
        CurrentDirection = newDir;
    }

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<CharacterRenderer>();
        attack = GetComponent<Attack>();
        dying = GetComponent<Dying>();
        soundPlayer = GetComponentInChildren<CharacterSoundsPlayer>();
    }

    public virtual void FixedUpdate()
    {
        if (attack.AnimationInProgress)
            return;

        if (MovingNow())
            Move();
        else
            StandStill();
    }

    protected void Move()
    {
        if (dying.Dead)
            return;

        Vector2 currentPos = rigidBody.position;
        Vector2 inputVector = Vector2.ClampMagnitude(CurrentDirection, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);
        rigidBody.MovePosition(newPos);
    }

    protected void StandStill()
    {
        renderer.SetDirection(new Vector2(0,0));
    }

    public bool MovingNow()
    {
        return CurrentDirection.magnitude > movementThreshold;
    }

    /// <summary>
    /// Character movement starts only if magnitude of controls'
    /// direction vector is greater than this value
    /// </summary>
    public static float movementThreshold = .1f;
}