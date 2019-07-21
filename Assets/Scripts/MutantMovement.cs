using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MutantMovement : MonoBehaviour
{
    public float movementSpeed = 1f;

    public CharacterSoundsPlayer SoundPlayer;

    CharacterRenderer renderer;
    Rigidbody2D rigidBody;
    MutantAttack attack;

    GameObject CurrentTarget
    {
        get
        {
            return attack.CurrentTarget;
        }
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<CharacterRenderer>();
        attack = GetComponent<MutantAttack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attack.AnimationInProgress)
            return;

        Vector2 currentPos = rigidBody.position;

        Vector2 direction = GetDesiredDirection();
        direction = Vector2.ClampMagnitude(direction, 1);
        Vector2 movement = direction * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);

        if (MovingNow(movement))
            SoundPlayer.TurnFootstepsOn();
        else
            SoundPlayer.TurnFootstepsOff();

        rigidBody.MovePosition(newPos);
    }

    Vector2 GetDesiredDirection()
    {
        Vector2 enemyPos = CurrentTarget.transform.position;

        if (enemyPos.x == float.NaN || enemyPos.y == float.NaN)
            return new Vector2(float.NaN, float.NaN);

        Vector2 myPos = rigidBody.position;
        return enemyPos - myPos;
    }

    bool MovingNow(Vector2 direction)
    {
        return direction.magnitude > .01f;
    }
}
