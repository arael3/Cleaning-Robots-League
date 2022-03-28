using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTopCharacterMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float movementForce = 0.0001f;

    public Rigidbody2D rb;
    public Camera cam;

    public Rigidbody2D boundWith;

    public Transform filedPointRW5C9;
    Vector2 V2filedPointRW5C9;

    Vector2 lookDir;

    Vector2 player1Movement;

    Vector2 movement;

    Vector2 npcNewPosition;

    private CharacterController characterController;

    void Start()
    {
        V2filedPointRW5C9 = filedPointRW5C9.position;

        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        player1Movement = boundWith.position;
        //Debug.Log("player1Movement: " + player1Movement);
        npcNewPosition.x = player1Movement.x - 4f;
        npcNewPosition.y = player1Movement.y + 5f;
        //gameObject.transform.position = npcNewPosition;

        //Debug.Log("V2filedPointRW5C9 = " + V2filedPointRW5C9);
    }

    void FixedUpdate()
    {
        //Movement when NPC hold the bomb
        int positionX = (int)transform.position.x;
        int positionY = (int)transform.position.y;
        if ((Mathf.Abs(positionX - V2filedPointRW5C9.x) > 1f) || (Mathf.Abs(positionY - V2filedPointRW5C9.y) > 1f))
        {
            lookDir = V2filedPointRW5C9 - rb.position;
            //rb.MovePosition((Vector2)transform.position + lookDir.normalized * moveSpeed * Time.deltaTime);
            characterController.Move(transform.position + (Vector3)lookDir.normalized * moveSpeed * Time.deltaTime);
        }
        float x = Mathf.Abs(rb.position.x - V2filedPointRW5C9.x);
        float y = Mathf.Abs(rb.position.y - V2filedPointRW5C9.y);
        Debug.Log("rb.position.x - V2filedPointRW5C9.x = " + x + "  rb.position.y - V2filedPointRW5C9.y = " + y);
        
        //rb.AddForce(transform.right * movementForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

        //rb.MovePosition(npcNewPosition * Time.fixedDeltaTime);

        //cam.transform.position = rb.position;


        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        rb.rotation = angle;

    }
}
