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

    public void ReceiveDirection(Vector2 newDir)
    {
        CurrentDirection = newDir;
    }

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<CharacterRenderer>();
        attack = GetComponent<Attack>();
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
        Vector2 currentPos = rigidBody.position;
        Vector2 inputVector = Vector2.ClampMagnitude(CurrentDirection, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);
        soundPlayer.TurnFootstepsOn();
        rigidBody.MovePosition(newPos);
    }

    protected void StandStill()
    {
        renderer.SetDirection(new Vector2(0,0));
        soundPlayer.TurnFootstepsOff();
    }

    protected bool MovingNow()
    {
        return CurrentDirection.magnitude > movementThreshold;
    }

    /// <summary>
    /// Character movement starts only if magnitude of controls'
    /// direction vector is greater than this value
    /// </summary>
    public static float movementThreshold = .1f;

    protected CharacterRenderer renderer;
    protected Rigidbody2D rigidBody;
    protected Attack attack;
    protected CharacterSoundsPlayer soundPlayer;
}