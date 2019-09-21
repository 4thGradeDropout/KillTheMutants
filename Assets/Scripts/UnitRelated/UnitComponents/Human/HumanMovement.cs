using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class HumanMovement : CharacterMovement
{
	// Update is called once per frame
    protected override void FixedUpdate()
    {
        if (attack.AnimationInProgress)
            return;

        Vector2 currentPos = rigidBody.position;
        float horizontalInput = Input.GetAxis("Horizontal");

        int sas;
        string shit;
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
}
