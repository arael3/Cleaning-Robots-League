using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTopShooting : MonoBehaviour
{
    public Rigidbody2D rb;

    public Rigidbody2D playerOrNPC;

    public Transform shootingPoint;

    public Transform fieldPointRightRow5Column9;
    public Transform fieldPointRightGoal;

    float timer = 0.2f;
    float restartTimer = 0.2f;
    float angle;
    float angleDifference = 10;

    float i = -179;
    float j = 0;

    public GameObject bombPrefab;

    public Rigidbody2D bomb;

    public Renderer holdBombSprite;

    Vector2 lookDir;

    public float shootForce = 0.003f;

    public static bool holdBomb = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (holdBomb)
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (holdBomb && (Mathf.Abs(rb.position.x - fieldPointRightRow5Column9.position.x) < 1.0f) && (Mathf.Abs(rb.position.y - fieldPointRightRow5Column9.position.y) < 1.0f))
        {
            lookDir = (Vector2)fieldPointRightGoal.position - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(rb.rotation - angle) <= angleDifference)
            {
                rb.rotation = angle;
            }
            else if (((rb.rotation >= 0) && (rb.rotation < angle) || (rb.rotation < 0) && (rb.rotation < angle)) && rb.rotation != angle)
            {
                rb.rotation += angleDifference;
            }
            else if (((rb.rotation >= 0) && (rb.rotation > angle) || (rb.rotation < 0) && (rb.rotation > angle)) && rb.rotation != angle)
            {
                rb.rotation -= angleDifference;
            }

            if ((timer < 0) && (rb.rotation == angle))
            {
                
                Shoot();
                timer = restartTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }

        }

        //if (j == 0)
        //{
        //    if (i < 180)
        //    {
        //        i++;
        //        rb.rotation = i;
        //    }
        //    else
        //    {
        //        j = 1;
        //    }
        //}
        //else
        //{
        //    if (i > -180)
        //    {
        //        i--;
        //        rb.rotation = i;
        //    }
        //    else
        //    {
        //        j = 0;
        //    }
        //}

    }

    void Shoot()
    {
        //GameObject bombToShoot = Instantiate(bombPrefab, shootingPoint.position, rb.transform.rotation);
        //Rigidbody2D rigidbodyOfInstantiateBomb = bombToShoot.GetComponent<Rigidbody2D>();
        //rigidbodyOfInstantiateBomb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        bomb.bodyType = RigidbodyType2D.Dynamic;
        bomb.GetComponent<Renderer>().enabled = true;
        holdBombSprite.enabled = false;
        bomb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        holdBomb = false;
        Bomb.onGround = true;
    }

}
