using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnNPCTopController : MonoBehaviour
{
    public float moveSpeed = 7f;

    public Rigidbody2D rb;

    public Camera cam;

    public Rigidbody2D boundWith;

    public GameObject bomb;

    public Renderer holdBombSprite;

    [SerializeField] Renderer shieldSprite;

    [SerializeField] GameObject TeamBluePlayer;
    [SerializeField] GameObject TeamBlueNPCTop;
    [SerializeField] GameObject TeamBlueNPCBottom;

    [SerializeField] GameObject TeamRedPlayer;
    [SerializeField] GameObject TeamRedNPCTop;
    [SerializeField] GameObject TeamRedNPCBottom;

    bool shieldSpriteEnabled;
    public bool ShieldSpriteEnabled
    {
        get { return shieldSpriteEnabled; }
        set { shieldSpriteEnabled = value; }
    }

    float shieldDuration = 1.5f;

    float shieldDurationRestart = 1.5f;

    float waitForShield = 1.8f;

    float waitForShieldRestart = 1.8f;

    [SerializeField] Renderer atackSprite;

    float attackDuration = 0.15f;

    float attackDurationRestart = 0.15f;

    float waitForAttack = 1.5f;

    float waitForAttackRestart = 1.5f;

    float shortPauseBeforeAttack = 0.06f;

    float shortPauseBeforeAttackRestart = 0.06f;

    [SerializeField] Rigidbody2D enemy;


    public Transform fieldPointLeftGoal;

    public Transform shootingPoint;

    Vector2 lookDir;

    public float shootForce = 0.003f;

    bool holdBomb = false;

    public bool HoldBomb
    {
        get { return holdBomb; }
        set { holdBomb = value; }
    }

    float angle;
    float angleDifference = 12;

    float x;
    float y;

    float waitTimeBeforeShoot = 0.11f;
    const float waitTimeBeforeShootRestart = 0.11f;

    public float holdBombTimer = 1.5f;

    public float HoldBombTimer
    {
        get { return holdBombTimer; }
        set { holdBombTimer = value; }
    }

    public const float restartholdBombTimer = 1.5f;

    float movementBlockingDuration = 0;
    float rbPositionXCopy;
    float rbPositionYCopy;

    bool isPassing;
    bool isShooting = false;

    float enemyDistanceX;
    float enemyDistanceY;

    float acceptableInaccuracyOfNpcPosition = 0.25f;

    bool drawFieldPoint = true;

    float randomX;
    float randomY;

    float diffX;
    float diffY;

    bool state;
    float timeBetweenStateChanging = 0;

    void Start()
    {
        transform.position = new Vector3(5.0f, 5.0f, 0f);

        waitForShield = 0;

        waitForAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("En NPC Top holdBomb: " + holdBomb);

        if (drawFieldPoint)
        {
            randomX = RandomX();
            randomY = RandomY();
            drawFieldPoint = false;
        }

        //Bomb controll when NPC hold the bomb
        if (holdBomb && (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            bomb.GetComponent<Renderer>().enabled = false;
            holdBombSprite.enabled = true;
            bomb.transform.position = shootingPoint.position;
            bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
        }

        if (holdBomb && ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            bomb.transform.position = shootingPoint.position;
            bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
        }

        // Defence
        if (shieldSprite.enabled)
        {
            shieldDuration -= Time.deltaTime;

            if (shieldDuration <= 0)
            {
                shieldSprite.enabled = false;
                shieldSpriteEnabled = false;
                shieldDuration = shieldDurationRestart;
            }
        }
        else if (holdBomb && (waitForShield <= 0) && (enemyDistanceX < 2.0f) && (enemyDistanceY < 2.0f))
        {
            shieldSprite.enabled = true;
            shieldSpriteEnabled = true;
            waitForShield = waitForShieldRestart;
            //Debug.Log("Enemy nearby!");
        }

        if (waitForShield > 0)
        {
            if (!shieldSprite.enabled)
            {
                waitForShield -= Time.deltaTime;
            }
        }

        //NPCControllerTest.AttackOld(ref atackSprite, ref attackDuration, ref waitForAttack, ref shortPauseBeforeAttack, ref holdBomb, TeamBlueNPCTop, enemyDistanceX, enemyDistanceY);

        // Attack
        if (atackSprite.enabled)
        {
            attackDuration -= Time.deltaTime;

            if (attackDuration <= 0)
            {
                atackSprite.enabled = false;
                attackDuration = attackDurationRestart;
            }
        }
        else if (TeamBlueNPCTop.GetComponent<NPCController>().HoldBomb && (waitForAttack <= 0) && (enemyDistanceX < 2) && (enemyDistanceY < 2))
        {
            shortPauseBeforeAttack -= Time.deltaTime;
            //Debug.Log("shortPauseBeforeAttack " + shortPauseBeforeAttack);

            if (shortPauseBeforeAttack <= 0)
            {
                atackSprite.enabled = true;
                waitForAttack = waitForAttackRestart;
                shortPauseBeforeAttack = shortPauseBeforeAttackRestart;

                // Testowe przejêcie - faktyczne przejêcie tylko wtedy gdy Raycast trafi przeciwnika
                if (!TeamBlueNPCTop.GetComponent<NPCController>().ShieldSpriteEnabled)
                {
                    //NPCTopController.holdBomb = false;
                    TeamBlueNPCTop.GetComponent<NPCController>().HoldBomb = false;
                    TeamBlueNPCTop.GetComponent<NPCController>().HoldBombTimer = NPCController.restartholdBombTimer;
                    holdBomb = true;
                }
            }
        }
        else
        {
            shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
        }

        if (waitForAttack > 0)
        {
            if (!atackSprite.enabled)
            {
                waitForAttack -= Time.deltaTime;
            }
        }

        // NPC passes bomb after button pressing
        if ((Input.GetButtonDown("Fire2") || holdBombTimer < 0) && holdBomb && !isShooting)
        {
            isPassing = true;
        }

        // NPC passes bomb if enemy nearby and waitForShield > 0
        if (holdBomb && !isShooting && (waitForShield > 0) && !shieldSprite.enabled && (enemyDistanceX < 3.0f) && (enemyDistanceY < 3.0f))
        {
            isPassing = true;
        }

        if (holdBomb && !isShooting && shieldSpriteEnabled && (enemyDistanceX < 3.0f) && (enemyDistanceY < 3.0f))
        {
            isPassing = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 p1Position = GameObject.Find("TeamBluePlayer").transform.position;
        Vector3 p1Distance = transform.position - p1Position;

        // Behavior when an enemy player is nearby.
        enemyDistanceX = Mathf.Abs(rb.position.x - enemy.position.x);
        enemyDistanceY = Mathf.Abs(rb.position.y - enemy.position.y);

        //diffX = Mathf.Abs(rb.position.x - fieldPointRightRow5Column9.position.x);
        diffX = Mathf.Abs(rb.position.x - randomX);
        //diffY = Mathf.Abs(rb.position.y - fieldPointRightRow5Column9.position.y);
        diffY = Mathf.Abs(rb.position.y - randomY);

        if (holdBomb)
        {
            holdBombTimer -= Time.deltaTime;
            holdBombSprite.enabled = true;
            bomb.layer = 6;
        }
        else
        {
            holdBombSprite.enabled = false;
        }

        //Movement when NPC hold the bomb
        //if (holdBomb && !isPassing && !fieldPointRightRow5Column9Collider2D.bounds.Contains(transform.position))
        if (holdBomb && !isPassing && (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            state = true;

            holdBombTimer -= Time.deltaTime;
            //lookDir = (Vector2)fieldPointRightRow5Column9.position - rb.position;
            lookDir = new Vector2(randomX, randomY) - rb.position;

            //rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
            rb.MovePosition(new Vector2((rb.position.x + lookDir.normalized.x * moveSpeed * Time.deltaTime), (rb.position.y + lookDir.normalized.y * moveSpeed * Time.deltaTime)));

            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);
        }
        else 
        {
            state = false;
        }

        // Movement and aiming when the bomb is on the ground or other Player hold the bomb
        if (!holdBomb)
        {
            //if (Player1Controller.holdBomb)
            if (TeamBluePlayer.GetComponent<PlayerController>().HoldBomb)
            {
                if (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))
                {
                    state = true;

                    if (movementBlockingDuration > 0.08f)
                    {
                        lookDir = new Vector2(randomX, randomY) - rb.position;

                        rbPositionXCopy = rb.position.x;
                        rbPositionYCopy = rb.position.y;

                        //Debug.Log("En NPC Top Mathf.Abs(rb.rotation) = " + Mathf.Abs(rb.rotation));
                        if (Mathf.Abs(rb.rotation) <= 90)
                        {
                            rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x - 1.0f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y + 1.2f) * moveSpeed * Time.deltaTime)));
                        }
                        else
                        {
                            rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x + 1.2f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y - 1.0f) * moveSpeed * Time.deltaTime)));
                        }

                        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                        Aiming(angle);

                        if ((enemyDistanceX >= 1.5f) || (enemyDistanceY >= 1.5f))
                        {
                            movementBlockingDuration = 0;
                        }
                    }
                    else
                    {
                        lookDir = new Vector2(randomX, randomY) - rb.position;

                        rbPositionXCopy = rb.position.x;
                        rbPositionYCopy = rb.position.y;

                        rb.MovePosition(new Vector2((rb.position.x + lookDir.normalized.x * moveSpeed * Time.deltaTime), (rb.position.y + lookDir.normalized.y * moveSpeed * Time.deltaTime)));

                        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                        Aiming(angle);
                    }

                    if (((enemyDistanceX < 1.5f) && (enemyDistanceY < 1.5f)) || ((Mathf.Abs(p1Distance.x) < 1.5f) && (Mathf.Abs(p1Distance.y) < 1.5f)))
                    {
                        movementBlockingDuration += Time.deltaTime;
                    }
                }
                else
                {
                    state = false;

                    lookDir = (Vector2)fieldPointLeftGoal.position - rb.position;

                    angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                    Aiming(angle);
                }

                //Debug.Log("shakingCounter = " + shakingCounter);
                //Debug.Log("x = " + x + " | y = " + y + " | Big contain " + fieldPointRightRow5Column9Collider2DBig.bounds.Contains(transform.position) + " Smal contain " + fieldPointRightRow5Column9Collider2D.bounds.Contains(transform.position));
                //Debug.Log("Mathf.Approximately(rb.position.x, 9): " + Mathf.Approximately(rb.position.x, 9) + " | Mathf.Approximately(rb.position.y, 5): " + Mathf.Approximately(rb.position.y, 5));

            }
            else
            {
                state = true;

                lookDir = (Vector2)bomb.transform.position - rb.position;

                //rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
                rb.MovePosition(new Vector2((rb.position.x + lookDir.normalized.x * moveSpeed * Time.deltaTime), (rb.position.y + lookDir.normalized.y * moveSpeed * Time.deltaTime)));

                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                Aiming(angle);
            }

        }

        // If state if changing to often (unwanted NPC vibrations) reduce the velocity of NPC to prevent unwanted NPC vibrations
        if (state)
        {
            timeBetweenStateChanging += Time.deltaTime;
        }
        else
        {
            if ((timeBetweenStateChanging > 0) && (timeBetweenStateChanging < 0.1f))
            {
                rb.velocity = new Vector2(0, 0);
            }
            timeBetweenStateChanging = 0;
        }

        // Aiming for the goal and shooting
        //if (holdBomb && !isPassing && fieldPointRightRow5Column9Collider2D.bounds.Contains(transform.position))
        if ((holdBomb && !isPassing && ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))) || isShooting)
        {
            holdBombTimer -= Time.deltaTime;
            isShooting = true;
            lookDir = (Vector2)fieldPointLeftGoal.position - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);

            // Shooting after a certain time
            if (rb.rotation == angle || holdBombTimer < -1f)
            {
                if (waitTimeBeforeShoot < 0)
                {
                    Shooting.Shoot(bomb);
                    bomb.layer = 0;
                    holdBomb = false;
                    isShooting = false;
                    drawFieldPoint = true;
                    waitTimeBeforeShoot = waitTimeBeforeShootRestart;
                    holdBombTimer = restartholdBombTimer;
                    holdBombSprite.enabled = false;
                }
                else
                {
                    waitTimeBeforeShoot -= Time.deltaTime;
                }
            }

        }

        // Passing to Player1
        if (holdBomb && isPassing && 0>1)
        {
            holdBombTimer -= Time.deltaTime;
            lookDir = (Vector2)p1Position - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);

            // Passing bomb after a certain time
            if (rb.rotation == angle || holdBombTimer < -1f)
            {
                if (waitTimeBeforeShoot < 0)
                {
                    Shooting.Shoot(bomb);
                    bomb.layer = 0;
                    holdBomb = false;
                    isPassing = false;
                    drawFieldPoint = true;
                    waitTimeBeforeShoot = waitTimeBeforeShootRestart;
                    holdBombTimer = restartholdBombTimer;
                    holdBombSprite.enabled = false;
                }
                else
                {
                    waitTimeBeforeShoot -= Time.deltaTime;
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionObjectName = collision.gameObject.name;

        // If NPC rich the fields point, reduce the velocity made by collision with other player to prevent unwanted NPC vibrations
        if (!state)
        {
            if (collisionObjectName == "TeamBluePlayer")
            {
                rb.velocity = new Vector2(0, 0);
                //Debug.Log(" collisionObjectName: " + collisionObjectName);
            }
        }
    }

    private float RandomX()
    {
        float randomX = Random.Range(-7, -11);

        return randomX;
    }

    private float RandomY()
    {
        float randomY = Random.Range(1, 6);
        //float randomY = Random.Range(5, 5);

        return randomY;
    }

    void Aiming(float angle)
    {
        float rbRotationCopy = rb.rotation;

        if (Mathf.Abs(rb.rotation - angle) <= angleDifference)
        {
            rb.rotation = angle;
        }
        else if (angle >= 0 && rb.rotation >= 0)
        {
            if (angle > rb.rotation)
            {
                rb.rotation += angleDifference;
            }

            else if (angle < rb.rotation)
            {
                rb.rotation -= angleDifference;
            }
        }

        else if (angle >= 0 && rb.rotation <= 0)
        {
            if ((angle - rb.rotation) > 180)
            {
                rbRotationCopy -= angleDifference;
                if (rbRotationCopy <= -180)
                {
                    rb.rotation = rb.rotation - angleDifference + 360;
                }
                else
                {
                    rb.rotation -= angleDifference;
                }
            }

            else
            {
                rb.rotation += angleDifference;
            }
        }

        else if (angle <= 0 && rb.rotation <= 0)
        {
            if (angle < rb.rotation)
            {
                rb.rotation -= angleDifference;
            }

            else if (angle > rb.rotation)
            {
                rb.rotation += angleDifference;
            }
        }

        else if (angle <= 0 && rb.rotation >= 0)
        {
            if ((rb.rotation - angle) > 180)
            {
                rbRotationCopy += angleDifference;
                if (rbRotationCopy >= 180)
                {
                    rb.rotation = rb.rotation + angleDifference - 360;
                }
                else
                {
                    rb.rotation += angleDifference;
                }
            }

            else
            {
                rb.rotation -= angleDifference;
            }
        }
    }

}
