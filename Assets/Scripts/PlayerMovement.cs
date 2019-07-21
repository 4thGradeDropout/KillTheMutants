using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed = 1f;

    public CharacterSoundsPlayer SoundPlayer;

	CharacterRenderer renderer;
	Rigidbody2D rigidBody;
    Attack attack;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<CharacterRenderer>();
        attack = GetComponent<Attack>();
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (attack.AnimationInProgress)
            return;

        Vector2 currentPos = rigidBody.position;
        float horizontalInput = -Input.GetAxis("Horizontal");

        int sas;
        if (horizontalInput > 0.1f)
            sas = 0;

        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        renderer.SetDirection(movement);

        if (MovingNow(movement))
            SoundPlayer.TurnFootstepsOn();
        else
            SoundPlayer.TurnFootstepsOff();

        rigidBody.MovePosition(newPos);
    }

    bool MovingNow(Vector2 direction)
    {
        return direction.magnitude > .01f;
    }
}
