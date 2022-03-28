using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTopMovement1 : MonoBehaviour
{
    public float moveSpeed = 7f;

    public Rigidbody2D rb;
    
    public Camera cam;

    public Rigidbody2D boundWith;

    public GameObject bomb;

    public Renderer holdBombSprite;

    public Transform fieldPointRightRow5Column9;

    public Transform fieldPointRightGoal;

    public Transform nPCTopShootingPoint;

    Vector2 lookDir;


    void Start()
    {
        transform.position = new Vector3(-4.0f, 5.0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //Movement when NPC hold the bomb
        if (NPCTopShooting.holdBomb && ((Mathf.Abs(rb.position.x - fieldPointRightRow5Column9.position.x) > 1f) || (Mathf.Abs(rb.position.y - fieldPointRightRow5Column9.position.y) > 1f)))
        {
            lookDir = (Vector2)fieldPointRightRow5Column9.position - rb.position;
            rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            bomb.GetComponent<Renderer>().enabled = false;
            holdBombSprite.enabled = true;
            bomb.transform.position = nPCTopShootingPoint.position;
        }

        //Movement when the bomb is on the ground
        if (!NPCTopShooting.holdBomb)
        { 
            lookDir = (Vector2)bomb.transform.position - rb.position;
            rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
        }
    }
}
