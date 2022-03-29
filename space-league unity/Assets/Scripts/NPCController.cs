using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Transform shootingPoint;
    [SerializeField] Transform raycastPoint;

    [SerializeField] Transform virtualBody;
    [SerializeField] Transform vbRaycastPoint;

    [SerializeField] Renderer shieldSprite;
    [SerializeField] Renderer atackSprite;

    [SerializeField] Renderer holdBombSprite;

    [SerializeField] GameObject bomb;

    // Do przeniesienia do GameController
    // Dostêpne wartoœci: "BluePlayer vs RedNPC", "RedPlayer vs BlueNPC", "BluePlayer vs RedPlayer"
    // Na czas testów domyœlna wartoœæ "BluePlayer vs NPC"
    string gameMode = "BluePlayer vs NPC";

    [SerializeField] GameObject teamBlueNPCTop;
    [SerializeField] GameObject teamBlueNPCMiddle;
    [SerializeField] GameObject teamBluePlayer;
    [SerializeField] GameObject teamBlueNPCBottom;

    [SerializeField] GameObject teamRedNPCTop;
    [SerializeField] GameObject teamRedNPCMiddle;
    [SerializeField] GameObject teamRedPlayer;
    [SerializeField] GameObject teamRedNPCBottom;

    // Blue Team = false, Red Team = true
    bool team;

    GameObject enemyNPCTop;
    GameObject enemyNPCMiddle;
    GameObject enemyPlayer;
    GameObject enemyNPCBottom;

    GameObject friendNPCTop;
    GameObject friendNPCMiddle;
    GameObject friendPlayer;
    GameObject friendNPCBottom;

    [SerializeField] Transform fieldPointRightGoal;
    [SerializeField] Transform fieldPointLeftGoal;

    Transform fieldPointGoal;

    string goalName;

    [SerializeField] GameObject attackAnimation;

    [SerializeField] float moveSpeed = 7f;

    [SerializeField] float shootForce = 2.5f;

    bool ifEnemyNPCNearbyForDefence = false;
    bool ifEnemyNPCTopNearbyForDefence = false;
    bool ifEnemyNPCMiddleNearbyForDefence = false;
    bool ifEnemyPlayerNearbyForDefence = false;
    bool ifEnemyNPCBottomNearbyForDefence = false;

    bool ifEnemyNPCNearbyForAttack = false;
    bool ifEnemyNPCTopNearbyForAttack = false;
    bool ifEnemyNPCMiddleNearbyForAttack = false;
    bool ifEnemyPlayerNearbyForAttack = false;
    bool ifEnemyNPCBottomNearbyForAttack = false;

    bool ifEnemyNPCNearbyForPassingRandom = false;
    bool ifEnemyNPCTopNearbyForPassingRandom = false;
    bool ifEnemyNPCMiddleNearbyForPassingRandom = false;
    bool ifEnemyPlayerNearbyForPassingRandom = false;
    bool ifEnemyNPCBottomNearbyForPassingRandom = false;

    bool ifEnemyNPCNearbyForMovementBlocking = false;
    bool ifEnemyNPCTopNearbyForMovementBlocking = false;
    bool ifEnemyNPCMiddleNearbyForMovementBlocking = false;
    bool ifEnemyPlayerNearbyForMovementBlocking = false;
    bool ifEnemyNPCBottomNearbyForMovementBlocking = false;

    bool enemyNPCHoldBomb = false;
    bool enemyNPCTopHoldBomb = false;
    bool enemyNPCMiddleHoldBomb = false;
    bool enemyNPCBottomHoldBomb = false;

    float enemyNPCTopDistance;
    float enemyNPCMiddleDistance;
    float enemyPlayerDistance;
    float enemyNPCBottomDistance;

    float acceptableInaccuracyOfNpcPosition = 0.15f;

    float minimumDistanceForShieldActivate = 2.0f;
    float minimumDistanceForAttackActivate = 2.0f;
    float minimumDistanceForPassingRandom = 3.0f;
    float minimumDistanceForMovementBlocking = 1.7f;

    bool shieldSpriteEnabled;
    public bool ShieldSpriteEnabled
    {
        get { return shieldSpriteEnabled; }
        set { shieldSpriteEnabled = value; }
    }

    float shieldDuration = 1.5f;

    float shieldDurationRestart = 1.5f;

    float waitForShield = 0f;

    float waitForShieldRestart = 1.8f;

    bool duringAttack = false;

    float attackDuration = 1.15f;

    float attackDurationRestart = 0.15f;

    float waitForAttack = 0f;

    float waitForAttackRestart = 1.5f;

    float shortPauseBeforeAttack = 0.06f;

    float shortPauseBeforeAttackRestart = 0.06f;

    Vector2 lookDir;
    Vector3 vbLookDir;

    bool holdBomb = false;

    public bool HoldBomb
    {
        get { return holdBomb; }
        set { holdBomb = value; }
    }

    float angle;
    float angleDifference = 12;

    float vbAngle;

    float waitTimeBeforeShoot = 0.11f;
    const float waitTimeBeforeShootRestart = 0.11f;

    public float holdBombTimer = 21.5f;

    public float HoldBombTimer
    {
        get { return holdBombTimer; }
        set { holdBombTimer = value; }
    }

    public const float restartholdBombTimer = 21.5f;

    float movementBlockingDuration = 0;
    float rbPositionXCopy;
    float rbPositionYCopy;

    string raycastColliderName = "none";
    string raycastColliderName2 = "none";

    bool isPassing;
    bool isPassingRandom;
    bool isShooting = false;
    bool objectToPassSelected = false;

    bool ifDrawFieldPoint = true;

    float randomX;
    float randomY;
    float randomXmin;
    float randomXmax;
    float randomYmin;
    float randomYmax;

    float diffX;
    float diffY;

    bool state;
    float timeBetweenStateChanging = 0;

    Vector2 distanceToBomb;
    float distanceToBombX;
    float distanceToBombY;

    bool ifEnemy1;
    bool ifEnemy2;
    bool ifEnemy3;

    bool followTheBombSwitcher = true;
    bool followTheBomb = true;
    float ifClosserToBombTimer = 0;
    float ifFurtherToBombTimer = 0;

    void Awake()
    {
        //Application.targetFrameRate = 0;
    }

    void Start()
    {
        // Finding which team the NPC belongs to
        if (gameObject.name == "TeamBlueNPCTop" || gameObject.name == "TeamBlueNPCMiddle" || gameObject.name == "TeamBlueNPCBottom")
        {
            team = false; // Blue Team = false

            fieldPointGoal = fieldPointRightGoal;

            goalName = "GoalRight";

            if (teamRedNPCTop)
            {
                enemyNPCTop = teamRedNPCTop;
            }

            if (teamRedNPCMiddle)
            {
                enemyNPCMiddle = teamRedNPCMiddle;
            }

            if (teamRedPlayer)
            {
                enemyPlayer = teamRedPlayer;
            }

            if (teamRedNPCBottom)
            {
                enemyNPCBottom = teamRedNPCBottom;
            }


            if (teamBlueNPCTop)
            {
                friendNPCTop = teamBlueNPCTop;
            }

            if (teamBlueNPCMiddle)
            {
                friendNPCMiddle = teamBlueNPCMiddle;
            }

            if (teamBluePlayer)
            {
                friendPlayer = teamBluePlayer;
            }

            if (teamBlueNPCBottom)
            {
                friendNPCBottom = teamBlueNPCBottom;
            }

        }
        else
        {
            team = true; // Red Team = true

            fieldPointGoal = fieldPointLeftGoal;

            goalName = "GoalLeft";

            if (teamBlueNPCTop)
            {
                enemyNPCTop = teamBlueNPCTop;
            }

            if (teamBlueNPCMiddle)
            {
                enemyNPCMiddle = teamBlueNPCMiddle;
            }

            if (teamBluePlayer)
            {
                enemyPlayer = teamBluePlayer;
            }

            if (teamBlueNPCBottom)
            {
                enemyNPCBottom = teamBlueNPCBottom;
            }


            if (teamRedNPCTop)
            {
                friendNPCTop = teamRedNPCTop;
            }

            if (teamRedNPCMiddle)
            {
                friendNPCMiddle = teamRedNPCMiddle;
            }

            if (teamRedPlayer)
            {
                friendPlayer = teamRedPlayer;
            }

            if (teamRedNPCBottom)
            {
                friendNPCBottom = teamRedNPCBottom;
            }
        }

        //transform.position = new Vector3(-5.0f, 5.0f, 0f);
        transform.position = StartingPosition(gameObject);

        waitForShield = 0;

        waitForAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (holdBomb)
        {
            holdBombTimer -= Time.deltaTime;
            holdBombSprite.enabled = true;
        }
        else
        {
            // Spróbowaæ dodaæ poni¿sze zmienne do instrukcji warunkowej, po udanym ataku przeciwnika
            holdBombTimer = restartholdBombTimer;
            holdBombSprite.enabled = false;
            isPassingRandom = false;
            isPassing = false;
            isShooting = false;
            objectToPassSelected = false;
            waitTimeBeforeShoot = waitTimeBeforeShootRestart;
        }

        if (enemyNPCTop)
        {
            if (enemyNPCTop.GetComponent<NPCController>().HoldBomb)
            {
                enemyNPCTopHoldBomb = true;
            }
            else
            {
                enemyNPCTopHoldBomb = false;
            }
        }

        if (enemyNPCMiddle)
        {
            if (enemyNPCMiddle.GetComponent<NPCController>().HoldBomb)
            {
                enemyNPCMiddleHoldBomb = true;
            }
            else
            {
                enemyNPCMiddleHoldBomb = false;
            }
        }

        if (enemyNPCBottom)
        {
            if (enemyNPCBottom.GetComponent<NPCController>().HoldBomb)
            {
                enemyNPCBottomHoldBomb = true;
            }
            else
            {
                enemyNPCBottomHoldBomb = false;
            }
        }

        if (enemyNPCTopHoldBomb || enemyNPCMiddleHoldBomb || enemyNPCBottomHoldBomb)
        {
            enemyNPCHoldBomb = true;
        }
        else
        {
            enemyNPCHoldBomb = false;
        }

        if (ifDrawFieldPoint)
        {
            DrawFieldPoint(out randomX, out randomY, out ifDrawFieldPoint);
        }

        //Bomb controll when this NPC hold the bomb
        // If the NPC didn't reached a random point, disable bomb rendering and enable bomb sprite imitation inside spaceship
        if (holdBomb && (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            bomb.GetComponent<Renderer>().enabled = false;
            holdBombSprite.enabled = true;
            bomb.transform.position = shootingPoint.position;
            bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
        }

        // If the distance between the NPC and the random point is between 0 and acceptableInaccuracyOfNpcPosition, set the bomb to position shootingPoint.position.
        // In other words, if the NPC reached a random point, complete the task from the condition below
        if (holdBomb && ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            bomb.transform.position = shootingPoint.position;
            bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
        }

        // DEFENCE ----------------------------------------
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
        else if (holdBomb && waitForShield <= 0 && ifEnemyNPCNearbyForDefence)
        {
            shieldSprite.enabled = true;
            shieldSpriteEnabled = true;
            waitForShield = waitForShieldRestart;
        }

        if (waitForShield > 0)
        {
            if (!shieldSprite.enabled)
            {
                waitForShield -= Time.deltaTime;
            }
        }

        Attack();

        // NPC passes bomb after button pressing
        if (Input.GetButtonDown("Fire2") && holdBomb && !isShooting && !isPassingRandom)
        {
            isPassing = true;
        }

        // NPC passes bomb if holdBombTimer < 0
        if ((holdBombTimer < 0) && holdBomb && !isShooting && !isPassing)
        {
            isPassingRandom = true;
        }

        // NPC passes bomb if enemy nearby and waitForShield > 0
        if (ifEnemyNPCNearbyForPassingRandom && !isShooting && !isPassing && (waitForShield > 0) && !shieldSprite.enabled)
        {
            isPassingRandom = true;
        }

        if (ifEnemyNPCNearbyForPassingRandom && !isShooting && !isPassing && shieldSpriteEnabled)
        {
            isPassingRandom = true;
        }
    }

    void FixedUpdate()
    {

        //Vector3 playerPosition = friendPlayer.GetComponent<Transform>().position;
        //Vector3 playerDistance = transform.position - playerPosition;

        RaycastHit2D hit = Physics2D.Raycast(raycastPoint.position, transform.TransformDirection(Vector2.right), 50f);

        if (hit)
        {
            raycastColliderName = hit.collider.name;
        }

        Debug.DrawRay(raycastPoint.position, transform.TransformDirection(Vector2.right) * 50f, Color.red);

        Debug.DrawRay(vbRaycastPoint.position, vbRaycastPoint.TransformDirection(Vector2.right) * 50f, Color.green);

        // Behavior when an enemy player is nearby.
        if (enemyNPCTop)
        {
            enemyNPCTopDistance = Vector2.Distance(rb.position, enemyNPCTop.GetComponent<Rigidbody2D>().position);

            if (holdBomb && enemyNPCTopDistance < minimumDistanceForShieldActivate)
            {
                ifEnemyNPCTopNearbyForDefence = true;
            }
            else
            {
                ifEnemyNPCTopNearbyForDefence = false;
            }

            if (holdBomb && enemyNPCTopDistance < minimumDistanceForPassingRandom)
            {
                ifEnemyNPCTopNearbyForPassingRandom = true;
            }
            else
            {
                ifEnemyNPCTopNearbyForPassingRandom = false;
            }
            
            if (enemyNPCTopDistance < minimumDistanceForMovementBlocking)
            {
                ifEnemyNPCTopNearbyForMovementBlocking = true;
            }
            else
            {
                ifEnemyNPCTopNearbyForMovementBlocking = false;
            }

            if (enemyNPCTopDistance < minimumDistanceForAttackActivate)
            {
                ifEnemyNPCTopNearbyForAttack = true;
            }
            else
            {
                ifEnemyNPCTopNearbyForAttack = false;
            }

        }

        if (enemyNPCMiddle)
        {

            enemyNPCMiddleDistance = Vector2.Distance(rb.position, enemyNPCMiddle.GetComponent<Rigidbody2D>().position);

            if (holdBomb && enemyNPCMiddleDistance < minimumDistanceForShieldActivate)
            {
                ifEnemyNPCMiddleNearbyForDefence = true;
            }
            else
            {
                ifEnemyNPCMiddleNearbyForDefence = false;
            }

            if (holdBomb && enemyNPCMiddleDistance < minimumDistanceForPassingRandom)
            {
                ifEnemyNPCMiddleNearbyForPassingRandom = true;
            }
            else
            {
                ifEnemyNPCMiddleNearbyForPassingRandom = false;
            }
            
            if (enemyNPCMiddleDistance < minimumDistanceForMovementBlocking)
            {
                ifEnemyNPCMiddleNearbyForMovementBlocking = true;
            }
            else
            {
                ifEnemyNPCMiddleNearbyForMovementBlocking = false;
            }

            if (enemyNPCMiddleDistance < minimumDistanceForAttackActivate)
            {
                ifEnemyNPCMiddleNearbyForAttack = true;
            }
            else
            {
                ifEnemyNPCMiddleNearbyForAttack = false;
            }
        }

        if (enemyPlayer)
        {

            enemyPlayerDistance = Vector2.Distance(rb.position, enemyPlayer.GetComponent<Rigidbody2D>().position);

            if (holdBomb && enemyPlayerDistance < minimumDistanceForShieldActivate)
            {
                ifEnemyPlayerNearbyForDefence = true;
            }
            else
            {
                ifEnemyPlayerNearbyForDefence = false;
            }

            if (holdBomb && enemyPlayerDistance < minimumDistanceForPassingRandom)
            {
                ifEnemyPlayerNearbyForPassingRandom = true;
            }
            else
            {
                ifEnemyPlayerNearbyForPassingRandom = false;
            }
            
            if (enemyPlayerDistance < minimumDistanceForMovementBlocking)
            {
                ifEnemyPlayerNearbyForMovementBlocking = true;
            }
            else
            {
                ifEnemyPlayerNearbyForMovementBlocking = false;
            }

            if (enemyPlayerDistance < minimumDistanceForAttackActivate)
            {
                ifEnemyPlayerNearbyForAttack = true;
            }
            else
            {
                ifEnemyPlayerNearbyForAttack = false;
            }
        }

        if (enemyNPCBottom)
        {
            enemyNPCBottomDistance = Vector2.Distance(rb.position, enemyNPCBottom.GetComponent<Rigidbody2D>().position);

            if (holdBomb && enemyNPCBottomDistance < minimumDistanceForShieldActivate)
            {
                ifEnemyNPCBottomNearbyForDefence = true;
            }
            else
            {
                ifEnemyNPCBottomNearbyForDefence = false;
            }

            if (holdBomb && enemyNPCBottomDistance < minimumDistanceForPassingRandom)
            {
                ifEnemyNPCBottomNearbyForPassingRandom = true;
            }
            else
            {
                ifEnemyNPCBottomNearbyForPassingRandom = false;
            }
            
            if (enemyNPCBottomDistance < minimumDistanceForMovementBlocking)
            {
                ifEnemyNPCBottomNearbyForMovementBlocking = true;
            }
            else
            {
                ifEnemyNPCBottomNearbyForMovementBlocking = false;
            }

            if (enemyNPCBottomDistance < minimumDistanceForAttackActivate)
            {
                ifEnemyNPCBottomNearbyForAttack = true;
            }
            else
            {
                ifEnemyNPCBottomNearbyForAttack = false;
            }
        }

        if (ifEnemyNPCTopNearbyForDefence || ifEnemyNPCMiddleNearbyForDefence || ifEnemyPlayerNearbyForDefence || ifEnemyNPCBottomNearbyForDefence)
        {
            ifEnemyNPCNearbyForDefence = true;
        }
        else
        {
            ifEnemyNPCNearbyForDefence = false;
        }

        if (ifEnemyNPCTopNearbyForPassingRandom || ifEnemyNPCMiddleNearbyForPassingRandom || ifEnemyPlayerNearbyForPassingRandom || ifEnemyNPCBottomNearbyForPassingRandom)
        {
            ifEnemyNPCNearbyForPassingRandom = true;
        }
        else
        {
            ifEnemyNPCNearbyForPassingRandom = false;
        }

        if (ifEnemyNPCTopNearbyForMovementBlocking || ifEnemyNPCMiddleNearbyForMovementBlocking || ifEnemyPlayerNearbyForMovementBlocking || ifEnemyNPCBottomNearbyForMovementBlocking)
        {
            ifEnemyNPCNearbyForMovementBlocking = true;
        }
        else
        {
            ifEnemyNPCNearbyForMovementBlocking = false;
        }

        if (ifEnemyNPCTopNearbyForAttack || ifEnemyNPCMiddleNearbyForAttack || ifEnemyPlayerNearbyForAttack || ifEnemyNPCBottomNearbyForAttack)
        {
            ifEnemyNPCNearbyForAttack = true;
        }
        else
        {
            ifEnemyNPCNearbyForAttack = false;
        }

        diffX = Mathf.Abs(rb.position.x - randomX);

        diffY = Mathf.Abs(rb.position.y - randomY);

        //Movement when NPC hold the bomb
        if (holdBomb && !isPassing && !isPassingRandom && (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))
        {
            state = true;

            lookDir = new Vector2(randomX, randomY) - rb.position;

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
            bool ifFriendPlayerOrNPCHoldBomb = false;

            if (friendPlayer)
            {
                if (friendPlayer.GetComponent<PlayerController>().HoldBomb)
                {
                    ifFriendPlayerOrNPCHoldBomb = true;
                }
            }

            if (friendNPCTop)
            {
                if (friendNPCTop.GetComponent<NPCController>().HoldBomb)
                {
                    ifFriendPlayerOrNPCHoldBomb = true;
                }
            }

            if (friendNPCMiddle)
            {
                if (friendNPCMiddle.GetComponent<NPCController>().HoldBomb)
                {
                    ifFriendPlayerOrNPCHoldBomb = true;
                }
            }

            if (friendNPCBottom)
            {
                if (friendNPCBottom.GetComponent<NPCController>().HoldBomb)
                {
                    ifFriendPlayerOrNPCHoldBomb = true;
                }
            }

            // When teammates hold the bomb
            //if (Player1Controller.holdBomb || NPCBottomController12.holdBomb)
            //PRZYGOTOWANY if (friendPlayer.GetComponent<PlayerController>().HoldBomb || friendNPCTop.GetComponent<NPCController>().HoldBomb || friendNPCMiddle.GetComponent<NPCController>().HoldBomb || friendNPCBottom.GetComponent<NPCController>().HoldBomb)
            //TYLKO NPC BOTTOM if (friendPlayer.GetComponent<PlayerController>().HoldBomb || friendNPCBottom.GetComponent<NPCController>().HoldBomb)
            if (ifFriendPlayerOrNPCHoldBomb)
            {
                if (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))
                {
                    state = true;
                    // Preventing NPCs from getting stuck to each other when moving in opposite directions. The determinant is the time (movementBlockingDuration) the NPCs stay close to each other.
                    if (movementBlockingDuration > 0.08f)
                    {
                        lookDir = new Vector2(randomX, randomY) - rb.position;

                        rbPositionXCopy = rb.position.x;
                        rbPositionYCopy = rb.position.y;

                        //Debug.Log("NPC Top Mathf.Abs(rb.rotation) = " + Mathf.Abs(rb.rotation));
                        if (Mathf.Abs(rb.rotation) <= 90)
                        {
                            rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x - 1.2f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y + 1.5f) * moveSpeed * Time.deltaTime)));
                        }
                        else
                        {
                            rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x + 1.6f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y - 1.3f) * moveSpeed * Time.deltaTime)));
                        }

                        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                        Aiming(angle);

                        //if ((enemyDistanceX >= 1.5f) || (enemyDistanceY >= 1.5f))
                        //if ((enemyNPCTopDistanceX >= 1.5f) || (enemyNPCTopDistanceY >= 1.5f))
                        //if (enemyNPCTopDistance >= 1.5f)
                        if (!ifEnemyNPCNearbyForMovementBlocking)
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
                    // Start timing the time when the enemy is nearby
                    //if (((enemyDistanceX < 1.7f) && (enemyDistanceY < 1.7f)) || ((Mathf.Abs(playerDistance.x) < 1.7f) && (Mathf.Abs(playerDistance.y) < 1.7f)))
                    //if (((enemyNPCTopDistanceX < 1.7f) && (enemyNPCTopDistanceY < 1.7f)) || ((Mathf.Abs(playerDistance.x) < 1.7f) && (Mathf.Abs(playerDistance.y) < 1.7f)))
                    //if (enemyNPCTopDistance < 1.7f || ((Mathf.Abs(playerDistance.x) < 1.7f) && (Mathf.Abs(playerDistance.y) < 1.7f)))
                    //if (ifEnemyNPCNearbyForMovementBlocking || ((Mathf.Abs(playerDistance.x) < 1.7f) && (Mathf.Abs(playerDistance.y) < 1.7f)))
                    if (ifEnemyNPCNearbyForMovementBlocking)
                    {
                        movementBlockingDuration += Time.deltaTime;
                    }
                }
                else
                {
                    state = false;

                    lookDir = (Vector2)fieldPointGoal.position - rb.position;

                    angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                    Aiming(angle);
                }
            }
            else
            {
                // When bomb is on the groung or enemy player hold the bomb

                state = true;

                lookDir = (Vector2)bomb.transform.position - rb.position;

                //Debug.Log("NPCTop:  lookDir.x = " + lookDir.x + "  lookDir.y = " + lookDir.y);

                // Nale¿y rozbudowaæ o odleg³oœci miêdzy wszystkimi poszczególnymi NPC
                //float differenceInDistanceToTheBombBetweenNPCPlayers = Mathf.Abs((Mathf.Pow(lookDir.x, 2) + Mathf.Pow(lookDir.y, 2)) - (Mathf.Pow(NPCBottomController.lookDir.x, 2) + Mathf.Pow(NPCBottomController.lookDir.y, 2)));
                // Lekko poprawione dzia³anie, odwo³anie z wykorzystaniem friendNPCBottom.GetComponent<NPCController>()
                //float differenceInDistanceToTheBombBetweenNPCPlayers = Mathf.Abs((Mathf.Pow(lookDir.x, 2) + Mathf.Pow(lookDir.y, 2)) - (Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.y, 2)));

                // Check which NPC is closer to the bomb. followTheBombSwitcher is used to prevent NPCs glitching.
                //if ( ((Mathf.Pow(lookDir.x, 2) + Mathf.Pow(lookDir.y, 2)) <= (Mathf.Pow(NPCBottomController12.lookDir.x, 2) + Mathf.Pow(NPCBottomController12.lookDir.y, 2))) || (differenceInDistanceToTheBombBetweenNPCPlayers < 10))

                //// Chwilowo wy³¹czone porównywanie, który NPC jest najbli¿ej bomby. Wszystkie NPC pod¹¿aj¹ za bomb¹.
                //if (followTheBombSwitcher)
                //{
                //    followTheBombSwitcher = false;

                //    float[] distancesToBomb = new float[3];

                //    float pow2DistanceToBombOfThisNPC = Mathf.Pow(lookDir.x, 2) + Mathf.Pow(lookDir.y, 2);

                //    float pow2DistanceToBombOfNPCTop;
                //    float pow2DistanceToBombOfPlayer;
                //    float pow2DistanceToBombOfNPCMiddle;
                //    float pow2DistanceToBombOfNPCBottom;

                //    //float pow2DistanceToBombOfNPCBottom = Mathf.Pow(NPCBottomController.lookDir.x, 2) + Mathf.Pow(NPCBottomController.lookDir.y, 2);

                //    // Checking distance to bomb of others teammates
                //    if (team)
                //    {
                //        if (gameObject.name != "TeamRedNPCTop")
                //        {
                //            pow2DistanceToBombOfNPCTop = Mathf.Pow(friendNPCTop.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCTop.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[0] = pow2DistanceToBombOfNPCTop;
                //        }
                //    }
                //    else
                //    {
                //        if (gameObject.name != "TeamBlueNPCTop")
                //        {
                //            pow2DistanceToBombOfNPCTop = Mathf.Pow(friendNPCTop.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCTop.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[0] = pow2DistanceToBombOfNPCTop;
                //        }
                //    }

                //    if (team)
                //    {
                //        if (gameObject.name != "TeamRedNPCMiddle")
                //        {
                //            pow2DistanceToBombOfNPCMiddle = Mathf.Pow(friendNPCMiddle.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCMiddle.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[1] = pow2DistanceToBombOfNPCMiddle;
                //        }
                //    }
                //    else
                //    {
                //        if (gameMode == "BluePlayer vs NPC")
                //        {
                //            pow2DistanceToBombOfPlayer = Mathf.Pow(friendPlayer.GetComponent<PlayerController>().lookDir.x, 2) + Mathf.Pow(friendPlayer.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[1] = pow2DistanceToBombOfPlayer;
                //        }
                //        // Nale¿y uzupe³niæ inne tryby gry
                //        else
                //        {
                //            if (gameObject.name != "TeamBlueNPCMiddle")
                //            {
                //                pow2DistanceToBombOfNPCMiddle = Mathf.Pow(friendNPCMiddle.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCMiddle.GetComponent<NPCController>().lookDir.y, 2);
                //                distancesToBomb[1] = pow2DistanceToBombOfNPCMiddle;
                //            }
                //        }
                //    }

                //    if (team)
                //    {
                //        if (gameObject.name != "TeamRedNPCBottom")
                //        {
                //            pow2DistanceToBombOfNPCBottom = Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[2] = pow2DistanceToBombOfNPCBottom;
                //        }
                //    }
                //    else
                //    {
                //        if (gameObject.name != "TeamBlueNPCBottom")
                //        {
                //            pow2DistanceToBombOfNPCBottom = Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.x, 2) + Mathf.Pow(friendNPCBottom.GetComponent<NPCController>().lookDir.y, 2);
                //            distancesToBomb[2] = pow2DistanceToBombOfNPCBottom;
                //        }
                //    }

                //    // Designating teammate closest to the bomb
                //    float minDistanceOfOtherTeamMates = distancesToBomb.Min();

                //    if ((pow2DistanceToBombOfThisNPC <= minDistanceOfOtherTeamMates) || (differenceInDistanceToTheBombBetweenNPCPlayers < 2))
                //    {
                //        followTheBomb = true;
                //    }
                //    else
                //    {
                //        followTheBomb = false;
                //    }
                //}

                // If this NPC is closer to the bomb, go for bomb
                if (followTheBomb)
                {
                    ifClosserToBombTimer += Time.deltaTime;

                    if (ifClosserToBombTimer > 0.2f)
                    {
                        followTheBombSwitcher = true;
                        ifClosserToBombTimer = 0;
                    }

                    //Debug.Log("NPC Top is closer");

                    //rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
                    rb.MovePosition(new Vector2((rb.position.x + lookDir.normalized.x * moveSpeed * Time.deltaTime), (rb.position.y + lookDir.normalized.y * moveSpeed * Time.deltaTime)));

                    angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                    Aiming(angle);

                }
                // If NPC Bottom is closer to the bomb, go to random field point
                else
                {
                    ifFurtherToBombTimer += Time.deltaTime;

                    if (ifFurtherToBombTimer > 0.2f)
                    {
                        followTheBombSwitcher = true;
                        ifFurtherToBombTimer = 0;
                    }

                    //Debug.Log("NPC Top is NOT closer");
                    if (!(diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) || !(diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))
                    {
                        state = true;

                        if (movementBlockingDuration > 0.08f)
                        {
                            lookDir = new Vector2(randomX, randomY) - rb.position;

                            rbPositionXCopy = rb.position.x;
                            rbPositionYCopy = rb.position.y;

                            if (Mathf.Abs(transform.rotation.z) <= 90)
                            {
                                rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x - 1.0f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y + 1.2f) * moveSpeed * Time.deltaTime)));
                            }
                            else
                            {
                                rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x + 1.2f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y - 1.0f) * moveSpeed * Time.deltaTime)));
                            }

                            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                            Aiming(angle);

                            //if ((enemyDistanceX >= 1.5f) || (enemyDistanceY >= 1.5f))
                            //if ((enemyNPCTopDistanceX >= 1.5f) || (enemyNPCTopDistanceY >= 1.5f))
                            if (!ifEnemyNPCNearbyForMovementBlocking)
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

                        //if (((enemyDistanceX < 1.5f) && (enemyDistanceY < 1.5f)) || ((Mathf.Abs(playerDistance.x) < 1.5f) && (Mathf.Abs(playerDistance.y) < 1.5f)))
                        //if (((enemyNPCTopDistanceX < 1.5f) && (enemyNPCTopDistanceY < 1.5f)) || ((Mathf.Abs(playerDistance.x) < 1.5f) && (Mathf.Abs(playerDistance.y) < 1.5f)))
                        //if (enemyNPCTopDistance < 1.5f || ((Mathf.Abs(playerDistance.x) < 1.5f) && (Mathf.Abs(playerDistance.y) < 1.5f)))
                        //if (ifEnemyNPCNearbyForMovementBlocking || ((Mathf.Abs(playerDistance.x) < 1.5f) && (Mathf.Abs(playerDistance.y) < 1.5f)))
                        if (ifEnemyNPCNearbyForMovementBlocking)
                        {
                            movementBlockingDuration += Time.deltaTime;
                        }
                    }
                }
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
        //if ((holdBomb && !isPassing && ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))) || isShooting || (holdBomb && !isPassing && hit.collider.name == "GoalRight"))
        if (isShooting || (holdBomb && !isPassing && !isPassingRandom && (raycastColliderName == goalName || ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition)))))
        {
            isShooting = true;
            lookDir = (Vector2)fieldPointGoal.position - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);

            // Shooting after a certain time
            if (rb.rotation == angle || holdBombTimer < -0.5f)
            {
                if (waitTimeBeforeShoot < 0)
                {
                    //NPCController.Shoot(bomb, shootForce);
                    Shooting.Shoot(bomb);
                    //bomb.layer = 0;
                    holdBomb = false;
                    isShooting = false;
                    ifDrawFieldPoint = true;
                    raycastColliderName = "none";
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

        // Passing to friendly Player
        if (holdBomb && isPassing && friendPlayer)
        {
            lookDir = (Vector2)friendPlayer.GetComponent<Transform>().position - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);

            // Passing bomb after a certain time
            if (rb.rotation == angle || holdBombTimer < -0.5f)
            {
                if (waitTimeBeforeShoot < 0)
                {
                    //NPCController.Shoot(bomb, shootForce);
                    Shooting.Shoot(bomb);
                    //bomb.layer = 0;
                    holdBomb = false;
                    isPassing = false;
                    ifDrawFieldPoint = true;
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
        // Shooting for goal or passing to random player or
        else if (holdBomb && isPassingRandom)
        {
            List<Vector3> objectsForPassing = new List<Vector3>();

            objectsForPassing.Add(fieldPointGoal.position);

            if (friendPlayer)
            {
                objectsForPassing.Add(friendPlayer.GetComponent<Transform>().position);
            }

            if (friendNPCTop)
            {
                objectsForPassing.Add(friendNPCTop.GetComponent<Transform>().position);
            }

            if (friendNPCMiddle)
            {
                objectsForPassing.Add(friendNPCMiddle.GetComponent<Transform>().position);
            }

            if (friendNPCBottom)
            {
                objectsForPassing.Add(friendNPCBottom.GetComponent<Transform>().position);
            }

            int counter = 0;

            do
            {
                vbLookDir = objectsForPassing[counter] - virtualBody.position;
                vbAngle = Mathf.Atan2(vbLookDir.y, vbLookDir.x) * Mathf.Rad2Deg;

                virtualBody.rotation = Quaternion.Euler(0, 0, vbAngle);

                RaycastHit2D hit2 = Physics2D.Raycast(vbRaycastPoint.position, vbRaycastPoint.TransformDirection(Vector2.right), 50f);

                if (hit2)
                {
                    raycastColliderName2 = hit2.collider.name;

                    if (raycastColliderName2 == goalName)
                    {
                        lookDir = (Vector2)fieldPointGoal.position - rb.position;
                        objectToPassSelected = true;
                    }

                    if (friendPlayer && !objectToPassSelected)
                    {
                        if (raycastColliderName2 == friendPlayer.name)
                        {
                            lookDir = (Vector2)friendPlayer.GetComponent<Transform>().position - rb.position;
                            objectToPassSelected = true;
                        }
                    }

                    if (friendNPCTop && !objectToPassSelected)
                    {
                        if (raycastColliderName2 == friendNPCTop.name)
                        {
                            lookDir = (Vector2)friendNPCTop.GetComponent<Transform>().position - rb.position;
                            objectToPassSelected = true;
                        }
                    }

                    if (friendNPCMiddle && !objectToPassSelected)
                    {
                        if (raycastColliderName2 == friendNPCMiddle.name)
                        {
                            lookDir = (Vector2)friendNPCMiddle.GetComponent<Transform>().position - rb.position;
                            objectToPassSelected = true;
                        }
                    }

                    if (friendNPCBottom && !objectToPassSelected)
                    {
                        if (raycastColliderName2 == friendNPCBottom.name)
                        {
                            lookDir = (Vector2)friendNPCBottom.GetComponent<Transform>().position - rb.position;
                            objectToPassSelected = true;
                        }
                    }
                }

                counter++;

            } while (counter != objectsForPassing.Count && !objectToPassSelected);

            if (!objectToPassSelected)
            {
                lookDir = new Vector2(-rb.position.x, -rb.position.y);
                objectToPassSelected = true;
            }

            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Aiming(angle);

            // Shooting bomb fot goal or passing bomb to teammate player or teammate NPC after a certain time (waitTimeBeforeShoot)
            // wait Time Before Shoot it is there to improve the precision of the shot
            if (rb.rotation == angle || holdBombTimer < -0.5f)
            {
                if (waitTimeBeforeShoot < 0)
                {
                    //NPCController.Shoot(bomb, shootForce);
                    Shooting.Shoot(bomb);
                    //bomb.layer = 0;
                    holdBomb = false;
                    isPassingRandom = false;
                    ifDrawFieldPoint = true;
                    raycastColliderName2 = "none";
                    objectToPassSelected = false;
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
        //Debug.Log("holdBomb: " + holdBomb + "  isPassingRandom: " + isPassingRandom + "  objectToPassSelected: " + objectToPassSelected);
        //Debug.Log("vbRotationZ = " + vbRotationZ + "  vbAngle = " + vbAngle);
    }

    public Vector3 StartingPosition(GameObject gameObject)
    {
        float x;
        float y;

        switch (gameObject.name)
        {
            case "TeamBlueNPCTop":
                x = -5f;
                y = 5f;
                break;
            case "TeamBlueNPCMiddle":
                x = -3f;
                y = 0f;
                break;
            case "TeamBlueNPCBottom":
                x = -5f;
                y = -5f;
                break;
            case "TeamRedNPCTop":
                x = 5f;
                y = 5f;
                break;
            case "TeamRedNPCMiddle":
                x = 3f;
                y = 0f;
                break;
            case "TeamRedNPCBottom":
                x = 5f;
                y = -5f;
                break;
            default:
                x = 0f;
                y = 5f;
                break;
        }
        return new Vector3(x, y, 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionObjectName = collision.gameObject.name;

        // If NPC rich the fields point, reduce the velocity made by collision with other player to prevent unwanted NPC vibrations
        if (!state)
        {
            if (friendPlayer)
            {
                if (collisionObjectName == friendPlayer.name)
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }

            if (enemyPlayer)
            {
                if (collisionObjectName == enemyPlayer.name)
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
        }
    }

    // Draw the point on the field from which the NPC will release the next shoot or will be waiting for the pass.
    public void DrawFieldPoint(out float randomX, out float randomY, out bool ifDrawFieldPoint)
    {
        switch (gameObject.name)
        {
            case "TeamBlueNPCTop":
                randomXmin = 7;
                randomXmax = 11;
                randomYmin = 1;
                randomYmax = 6;
                break;
            case "TeamBlueNPCMiddle":
                randomXmin = 4;
                randomXmax = 6;
                randomYmin = -3;
                randomYmax = 3;
                break;
            case "TeamBlueNPCBottom":
                randomXmin = 8;
                randomXmax = 12;
                randomYmin = -1;
                randomYmax = -6;
                break;
            case "TeamRedNPCTop":
                randomXmin = -8;
                randomXmax = -12;
                randomYmin = 1;
                randomYmax = 6;
                break;
            case "TeamRedNPCMiddle":
                randomXmin = -4;
                randomXmax = -6;
                randomYmin = -3;
                randomYmax = 3;
                break;
            case "TeamRedNPCBottom":
                randomXmin = -7;
                randomXmax = -11;
                randomYmin = -1;
                randomYmax = -6;
                break;
            default:
                randomXmin = -7;
                randomXmax = 7;
                randomYmin = -4;
                randomYmax = 4;
                break;
        }

        randomX = Random.Range(randomXmin, randomXmin);
        randomY = Random.Range(randomYmin, randomYmax);
        ifDrawFieldPoint = false;
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

    void Attack()
    {
        if (duringAttack)
        {
            attackDuration -= Time.deltaTime;

            if (attackDuration <= 0)
            {
                //atackSprite.enabled = false;
                duringAttack = false;
                attackDuration = attackDurationRestart;
            }
        }
        //else if (EnNPCTopController.holdBomb && (waitForAttack <= 0) && (enemyDistanceX < 2.0f) && (enemyDistanceY < 2.0f))
        //else if (enemyNPCTop.GetComponent<EnNPCTopController>().HoldBomb && (waitForAttack <= 0) && (enemyNPCTopDistanceX < 2.0f) && (enemyNPCTopDistanceY < 2.0f))
        //else if (enemyNPCTop.GetComponent<EnNPCTopController>().HoldBomb && (waitForAttack <= 0) && enemyNPCTopDistance < 2.0f)
        else if (ifEnemyNPCNearbyForAttack && waitForAttack <= 0)
        {
            shortPauseBeforeAttack -= Time.deltaTime;
            //Debug.Log("shortPauseBeforeAttack " + shortPauseBeforeAttack);

            if (shortPauseBeforeAttack <= 0)
            {
                //atackSprite.enabled = true;
                duringAttack = true;
                waitForAttack = waitForAttackRestart;
                shortPauseBeforeAttack = shortPauseBeforeAttackRestart;

                GameObject attackAnimationInst = Instantiate(attackAnimation, transform.position, transform.rotation);

                attackAnimationInst.transform.parent = gameObject.transform;

                SpriteRenderer attackAnimationSprite = attackAnimationInst.GetComponent<SpriteRenderer>();

                if (attackAnimationSprite)
                {
                    attackAnimationSprite.sortingOrder = 2;
                }

                Destroy(attackAnimationInst, .4f);

                // Testowe przejêcie - faktyczne przejêcie tylko wtedy gdy Raycast trafi przeciwnika

                if (ifEnemyNPCTopNearbyForAttack)
                {
                    if (enemyNPCTop.GetComponent<NPCController>().HoldBomb)
                    {
                        if (!enemyNPCTop.GetComponent<NPCController>().ShieldSpriteEnabled)
                        {
                            enemyNPCTop.GetComponent<NPCController>().HoldBomb = false;
                            enemyNPCTop.GetComponent<NPCController>().HoldBombTimer = restartholdBombTimer;
                            HoldBomb = true;
                        }
                    }
                }

                if (ifEnemyNPCMiddleNearbyForAttack)
                {
                    if (enemyNPCMiddle.GetComponent<NPCController>().HoldBomb)
                    {
                        if (!enemyNPCMiddle.GetComponent<NPCController>().ShieldSpriteEnabled)
                        {
                            enemyNPCMiddle.GetComponent<NPCController>().HoldBomb = false;
                            enemyNPCMiddle.GetComponent<NPCController>().HoldBombTimer = restartholdBombTimer;
                            HoldBomb = true;
                        }
                    }
                }

                if (ifEnemyPlayerNearbyForAttack)
                {
                    if (enemyPlayer.GetComponent<PlayerController>().HoldBomb)
                    {
                        if (!enemyPlayer.GetComponent<PlayerController>().ShieldSpriteEnabled)
                        {
                            enemyPlayer.GetComponent<PlayerController>().HoldBomb = false;
                            enemyPlayer.GetComponent<PlayerController>().HoldBombTimer = restartholdBombTimer;
                            HoldBomb = true;
                        }
                    }
                }

                if (ifEnemyNPCBottomNearbyForAttack)
                {
                    if (enemyNPCBottom.GetComponent<NPCController>().HoldBomb)
                    {
                        if (!enemyNPCBottom.GetComponent<NPCController>().ShieldSpriteEnabled)
                        {
                            enemyNPCBottom.GetComponent<NPCController>().HoldBomb = false;
                            enemyNPCBottom.GetComponent<NPCController>().HoldBombTimer = restartholdBombTimer;
                            HoldBomb = true;
                        }
                    }
                }
            }
        }
        else
        {
            shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
        }

        if (waitForAttack > 0)
        {
            if (!duringAttack)
            {
                waitForAttack -= Time.deltaTime;
            }
        }
    }

}
