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

    //[SerializeField] GameObject pauseMenu;
    //GameMenus gameMenusScript;

    // Do przeniesienia do GameController
    // Dost?pne warto?ci: "BluePlayer vs RedNPC", "RedPlayer vs BlueNPC", "BluePlayer vs RedPlayer"
    // Na czas test?w domy?lna warto?? "BluePlayer vs NPC"
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

    float moveSpeed = 7f;

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

    int acceptableDifferenceInDistanceToBomb = 1;

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

    // Time for HoldBombTimer is shorter for NPC than for Player, because NPC have additional time when aiming, for increase a precise of shots.
    float holdBombTimer = 1.5f;

    public float HoldBombTimer
    {
        get { return holdBombTimer; }
        set { holdBombTimer = value; }
    }

    const float restartholdBombTimer = 1.5f;

    float movementBlockingDuration = 0;
    float rbPositionXCopy;
    float rbPositionYCopy;

    string raycastColliderName = "none";
    string raycastColliderName2 = "none";

    bool isPassing;
    bool isPassingRandom;
    bool isShooting = false;
    bool objectToPassSelected = false;

    bool ifDrawFieldPointForAttack = true;

    bool ifDrawFieldPointForDefence = true;

    float randomXForAttack;
    float randomYForAttack;
    float randomXminForAttack;
    float randomXmaxForAttack;
    float randomYminForAttack;
    float randomYmaxForAttack;

    float randomXForDefence;
    float randomYForDefence;
    float randomXminForDefence;
    float randomXmaxForDefence;
    float randomYminForDefence;
    float randomYmaxForDefence;

    

    float diffForAttack;
    float diffForDefence;

    //float diffXForAttack;
    //float diffYForAttack;
    //float diffXForDefence;
    //float diffYForDefence;

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
        //gameMenusScript = pauseMenu.GetComponent<GameMenus>();

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
        if (GameController.ifGameOver || GameMenus.ifStart)
        {
            holdBomb = false;
            holdBombSprite.enabled = false;

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            holdBombTimer = restartholdBombTimer;
            shieldDuration = shieldDurationRestart;
            shieldSprite.enabled = false;
            shieldSpriteEnabled = false;
            attackDuration = 0f;
            duringAttack = false;
            waitForShield = 0f;
            waitForAttack = 0f;
            transform.position = StartingPosition(gameObject);
        }

        if (Bomb.afterGoal)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            holdBombTimer = restartholdBombTimer;
            shieldDuration = shieldDurationRestart;
            shieldSprite.enabled = false;
            shieldSpriteEnabled = false;
            attackDuration = 0f;
            duringAttack = false;
            waitForShield = 0f;
            waitForAttack = 0f;
            transform.position = StartingPosition(gameObject);
        }

        if (!GameMenus.ifPause && !CountingTimeAfterGoal.ifPauseAfterGoal && !GameController.ifGameOver)
        {
            if (holdBomb)
            {
                holdBombTimer -= Time.deltaTime;
                holdBombSprite.enabled = true;
            }
            else
            {
                // Spr?bowa? doda? poni?sze zmienne do instrukcji warunkowej, po udanym ataku przeciwnika
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

            if (ifDrawFieldPointForAttack)
            {
                DrawFieldPointForAttack(out randomXForAttack, out randomYForAttack, out ifDrawFieldPointForAttack);
            }

            if (ifDrawFieldPointForDefence)
            {
                DrawFieldPointForDefence(out randomXForDefence, out randomYForDefence, out ifDrawFieldPointForDefence);
            }

            //Bomb controll when this NPC hold the bomb
            // If the NPC didn't reached a random point, disable bomb rendering and enable bomb sprite imitation inside spaceship
            if (holdBomb && !(diffForAttack >= 0 && diffForAttack <= acceptableInaccuracyOfNpcPosition))
            {
                bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                bomb.GetComponent<Renderer>().enabled = false;
                holdBombSprite.enabled = true;
                bomb.transform.position = shootingPoint.position;
                bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
            }

            // If the distance between the NPC and the random point is between 0 and acceptableInaccuracyOfNpcPosition, set the bomb to position shootingPoint.position.
            // In other words, if the NPC reached a random point, complete the task from the condition below
            if (holdBomb && (diffForAttack >= 0 && diffForAttack <= acceptableInaccuracyOfNpcPosition))
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
                if (ifEnemyPlayerNearbyForDefence)
                {
                    int randomRangeToReduceEfficiencyOfShieldActivation = Random.Range(1, 101);
                    if (randomRangeToReduceEfficiencyOfShieldActivation == 1)
                    {
                        shieldSprite.enabled = true;
                        shieldSpriteEnabled = true;
                        waitForShield = waitForShieldRestart;
                        FindObjectOfType<AudioManager>().Play("Shield");
                    }
                }
                else
                {
                    shieldSprite.enabled = true;
                    shieldSpriteEnabled = true;
                    waitForShield = waitForShieldRestart;
                    FindObjectOfType<AudioManager>().Play("Shield");
                }
                
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
            if (Input.GetKeyDown(KeyCode.E) && holdBomb && !isShooting && !isPassingRandom)
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
    }

    void FixedUpdate()
    {
        if (!GameMenus.ifPause && !CountingTimeAfterGoal.ifPauseAfterGoal && !GameController.ifGameOver)
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

            diffForAttack = Vector2.Distance(rb.position, new Vector2(randomXForAttack, randomYForAttack));

            diffForDefence = Vector2.Distance(rb.position, new Vector2(randomXForDefence, randomYForDefence));

            //diffXForAttack = Mathf.Abs(rb.position.x - randomXForAttack);

            //diffYForAttack = Mathf.Abs(rb.position.y - randomYForAttack);

            //diffXForDefence = Mathf.Abs(rb.position.x - randomXForDefence);

            //diffYForDefence = Mathf.Abs(rb.position.y - randomYForDefence);

            //Movement when this NPC hold the bomb
            if (holdBomb && !isPassing && !isPassingRandom && !(diffForAttack >= 0 && diffForAttack <= acceptableInaccuracyOfNpcPosition))
            {
                state = true;

                lookDir = new Vector2(randomXForAttack, randomYForAttack) - rb.position;

                Movement(lookDir);

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
                    if (!(diffForAttack >= 0 && diffForAttack <= acceptableInaccuracyOfNpcPosition))
                    {
                        state = true;

                        lookDir = new Vector2(randomXForAttack, randomYForAttack) - rb.position;

                        rbPositionXCopy = rb.position.x;
                        rbPositionYCopy = rb.position.y;

                        Movement(lookDir);

                        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                        Aiming(angle);

                        // Start timing the time when the enemy is nearby
                        if (ifEnemyNPCNearbyForMovementBlocking)
                        {
                            movementBlockingDuration += Time.deltaTime;
                        }
                        else
                        {
                            movementBlockingDuration = 0;
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

                    if (followTheBombSwitcher)
                    {
                        followTheBombSwitcher = false;

                        int friendPlayerDistanceToBomb = 9999;
                        int friendNPCTopDistanceToBomb = 9999;
                        int friendNPCMiddleDistanceToBomb = 9999;
                        int friendNPCBottomDistanceToBomb = 9999;

                        int thisNPCDistanceToBomb = (int)Vector3.Distance(gameObject.transform.position, bomb.transform.position);

                        if (friendPlayer)
                        {
                            friendPlayerDistanceToBomb = (int)Vector3.Distance(friendPlayer.GetComponent<Transform>().position, bomb.transform.position);
                        }

                        if (friendNPCTop)
                        {
                            if (friendNPCTop.name != gameObject.name)
                            {
                                friendNPCTopDistanceToBomb = (int)Vector3.Distance(friendNPCTop.GetComponent<Transform>().position, bomb.transform.position);
                            }
                        }

                        if (friendNPCMiddle)
                        {
                            if (friendNPCMiddle.name != gameObject.name)
                            {
                                friendNPCMiddleDistanceToBomb = (int)Vector3.Distance(friendNPCMiddle.GetComponent<Transform>().position, bomb.transform.position);
                            }
                        }

                        if (friendNPCBottom)
                        {
                            if (friendNPCBottom.name != gameObject.name)
                            {
                                friendNPCBottomDistanceToBomb = (int)Vector3.Distance(friendNPCBottom.GetComponent<Transform>().position, bomb.transform.position);
                            }
                        }

                        int[] teammatesDistancesToBomb = { friendPlayerDistanceToBomb, friendNPCTopDistanceToBomb, friendNPCMiddleDistanceToBomb, friendNPCBottomDistanceToBomb };

                        // The NPC closest to the bomb will follow the bomb. 
                        // If the difference in distance to the bomb between two NPCs is less than the acceptableDifferenceInDistanceToBomb, the NPC also follows the bomb.
                        if ((thisNPCDistanceToBomb - acceptableDifferenceInDistanceToBomb) < teammatesDistancesToBomb.Min())
                        {
                            followTheBomb = true;
                            //Debug.Log(gameObject.name + ": followTheBomb: " + followTheBomb);
                        }
                        else
                        {
                            followTheBomb = false;
                            //Debug.Log(gameObject.name + ": followTheBomb: " + followTheBomb);
                        }
                    }

                    // If this NPC is closer to the bomb, go for bomb
                    if (followTheBomb)
                    {
                        if (gameObject.name == "TeamRedNPCBottom")
                        {
                            //Debug.Log(gameObject.name + ": go to bomb");
                        }

                        ifClosserToBombTimer += Time.deltaTime;

                        // ifClosserToBombTimer is used to prevent NPCs glitching. Thus, the above checking which NPC is closer will be executed when the ifClosserToBombTimer exceeds the set time.
                        if (ifClosserToBombTimer > 0.6f)
                        {
                            followTheBombSwitcher = true;
                            ifClosserToBombTimer = 0;
                        }

                        lookDir = (Vector2)bomb.transform.position - rb.position;

                        //Debug.Log("NPC Top is closer");

                        //rb.MovePosition(rb.position + lookDir.normalized * moveSpeed * Time.deltaTime);
                        Movement(lookDir);

                        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                        Aiming(angle);

                    }
                    // If other NPC is closer to the bomb, go to random field point
                    else
                    {
                        if (gameObject.name == "TeamRedNPCBottom")
                        {
                            //Debug.Log(gameObject.name + ": go to point");
                        }

                        ifFurtherToBombTimer += Time.deltaTime;

                        if (ifFurtherToBombTimer > 0.6f)
                        {
                            followTheBombSwitcher = true;
                            ifFurtherToBombTimer = 0;
                        }

                        // If the distance to the point is greater than the set value, the NPC moves to the point.
                        // When the distance to a point is less than the set value (when the NPC reaches the point), its velocity is set to zero to prevent glitching.

                        // The glitch is that when the NPC reaches the point, it has some velocity which causes the NPC to move for a while longer,
                        // which causes the NPC to move away from the point, which again requires a return to the point and add a little bit of velocity.
                        // This situation loops, causing unwanted NPC vibrations. That's why, in else statement below___ rb.velocity = new Vector2(0, 0); ___was used.
                        if (diffForDefence > 0.1f)
                        {
                            state = true;

                            lookDir = new Vector2(randomXForDefence, randomYForDefence) - rb.position;

                            rbPositionXCopy = rb.position.x;
                            rbPositionYCopy = rb.position.y;

                            Movement(lookDir);

                            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                            Aiming(angle);

                            // Start timing the time when the enemy is nearby
                            if (ifEnemyNPCNearbyForMovementBlocking)
                            {
                                movementBlockingDuration += Time.deltaTime;
                            }
                            else
                            {
                                movementBlockingDuration = 0;
                            }
                        }
                        else
                        {
                            rb.velocity = new Vector2(0, 0);
                        }
                    }
                }
            }

            // To raczej nie dzia?a :(
            // If state if changing to often (unwanted NPC vibrations) reduce the velocity of NPC to prevent unwanted NPC vibrations
            if (state)
            {
                timeBetweenStateChanging += Time.deltaTime;
            }
            else
            {
                if ((0 < timeBetweenStateChanging) && (timeBetweenStateChanging < 0.1f))
                {
                    rb.velocity = new Vector2(0, 0);
                    //Debug.Log(gameObject.name + ": state changing to often");
                }
                timeBetweenStateChanging = 0;
            }

            // Aiming for the goal and shooting
            //if ((holdBomb && !isPassing && ((diffX >= 0 && diffX <= acceptableInaccuracyOfNpcPosition) && (diffY >= 0 && diffY <= acceptableInaccuracyOfNpcPosition))) || isShooting || (holdBomb && !isPassing && hit.collider.name == "GoalRight"))
            if (isShooting || (holdBomb && !isPassing && !isPassingRandom && (raycastColliderName == goalName || (diffForAttack >= 0 && diffForAttack <= acceptableInaccuracyOfNpcPosition))))
            {
                isShooting = true;
                lookDir = (Vector2)fieldPointGoal.position - rb.position;
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

                Aiming(angle);

                // Shooting after a certain time. NPC need some time for a precise shots.
                if (rb.rotation == angle || holdBombTimer < -0.5f)
                {
                    if (waitTimeBeforeShoot < 0)
                    {
                        //NPCController.Shoot(bomb, shootForce);
                        Shooting.Shoot(bomb);
                        //bomb.layer = 0;
                        holdBomb = false;
                        isShooting = false;
                        ifDrawFieldPointForAttack = true;
                        ifDrawFieldPointForDefence = true;
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

                // Passing bomb after a certain time. NPC need some time for a precise shots.
                if (rb.rotation == angle || holdBombTimer < -0.5f)
                {
                    if (waitTimeBeforeShoot < 0)
                    {
                        //NPCController.Shoot(bomb, shootForce);
                        Shooting.Shoot(bomb);
                        //bomb.layer = 0;
                        holdBomb = false;
                        isPassing = false;
                        ifDrawFieldPointForAttack = true;
                        ifDrawFieldPointForDefence = true;
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

                // Shooting bomb for goal or passing bomb to teammate player or teammate NPC after a certain time (waitTimeBeforeShoot)
                // wait Time Before Shoot it is there to improve the precision of the NPC shot
                if (rb.rotation == angle || holdBombTimer < -0.5f)
                {
                    if (waitTimeBeforeShoot < 0)
                    {
                        //NPCController.Shoot(bomb, shootForce);
                        Shooting.Shoot(bomb);
                        //bomb.layer = 0;
                        holdBomb = false;
                        isPassingRandom = false;
                        ifDrawFieldPointForAttack = true;
                        ifDrawFieldPointForDefence = true;
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

            // This condition is triggered in an emergency situation where the NPC does not shoot the bomb even though the time of holdBombTimer has expired.
            if (holdBomb && holdBombTimer < -0.6f)
            {
                Shooting.Shoot(bomb);
                holdBomb = false;
                isPassingRandom = false;
                ifDrawFieldPointForAttack = true;
                ifDrawFieldPointForDefence = true;
                raycastColliderName2 = "none";
                objectToPassSelected = false;
                waitTimeBeforeShoot = waitTimeBeforeShootRestart;
                holdBombTimer = restartholdBombTimer;
                holdBombSprite.enabled = false;
            }
        }
    }

    void Movement(Vector2 lookDir)
    {
        if (movementBlockingDuration < 0.08f)
        {
            rb.MovePosition(new Vector2((rb.position.x + lookDir.normalized.x * moveSpeed * Time.deltaTime), (rb.position.y + lookDir.normalized.y * moveSpeed * Time.deltaTime)));
        }
        // Preventing NPCs from getting stuck to each other when moving in opposite directions. The determinant is the time (movementBlockingDuration) the NPCs stay close to each other.
        else
        {
            if (Mathf.Abs(rb.rotation) <= 90)
            {
                rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x - 1.2f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y + 1.5f) * moveSpeed * Time.deltaTime)));
            }
            else
            {
                rb.MovePosition(new Vector2((rb.position.x + (lookDir.normalized.x + 1.6f) * moveSpeed * Time.deltaTime), (rb.position.y + (lookDir.normalized.y - 1.3f) * moveSpeed * Time.deltaTime)));
            }
        }
        
    }

    public Vector3 StartingPosition(GameObject gameObject)
    {
        float x;
        float y;

        switch (gameObject.name)
        {
            case "TeamBlueNPCTop":
                x = -6f;
                y = 5f;
                break;
            case "TeamBlueNPCMiddle":
                x = -4f;
                y = 0f;
                break;
            case "TeamBlueNPCBottom":
                x = -6f;
                y = -5f;
                break;
            case "TeamRedNPCTop":
                x = 6f;
                y = 5f;
                break;
            case "TeamRedNPCMiddle":
                x = 4f;
                y = 0f;
                break;
            case "TeamRedNPCBottom":
                x = 6f;
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
    public void DrawFieldPointForAttack(out float randomXForAttack, out float randomYForAttack, out bool ifDrawFieldPointForAttack)
    {
        switch (gameObject.name)
        {
            case "TeamBlueNPCTop":
                randomXminForAttack = 8;
                randomXmaxForAttack = 12;
                randomYminForAttack = 1;
                randomYmaxForAttack = 7;
                break;
            case "TeamBlueNPCMiddle":
                randomXminForAttack = 4;
                randomXmaxForAttack = 7;
                randomYminForAttack = -4;
                randomYmaxForAttack = 4;
                break;
            case "TeamBlueNPCBottom":
                randomXminForAttack = 9;
                randomXmaxForAttack = 13;
                randomYminForAttack = -1;
                randomYmaxForAttack = -7;
                break;
            case "TeamRedNPCTop":
                randomXminForAttack = -9;
                randomXmaxForAttack = -13;
                randomYminForAttack = 1;
                randomYmaxForAttack = 7;
                break;
            case "TeamRedNPCMiddle":
                randomXminForAttack = -4;
                randomXmaxForAttack = -7;
                randomYminForAttack = -4;
                randomYmaxForAttack = 4;
                break;
            case "TeamRedNPCBottom":
                randomXminForAttack = -8;
                randomXmaxForAttack = -12;
                randomYminForAttack = -1;
                randomYmaxForAttack = -7;
                break;
            default:
                randomXminForAttack = -8;
                randomXmaxForAttack = 8;
                randomYminForAttack = -5;
                randomYmaxForAttack = 5;
                break;
        }

        randomXForAttack = Random.Range(randomXminForAttack, randomXminForAttack);
        randomYForAttack = Random.Range(randomYminForAttack, randomYmaxForAttack);
        ifDrawFieldPointForAttack = false;
    }

    public void DrawFieldPointForDefence(out float randomXForDefence, out float randomYForDefence, out bool ifDrawFieldPointForDefence)
    {
        switch (gameObject.name)
        {
            case "TeamBlueNPCTop":
                randomXminForDefence = -8;
                randomXmaxForDefence = -12;
                randomYminForDefence = 1;
                randomYmaxForDefence = 7;
                break;
            case "TeamBlueNPCMiddle":
                randomXminForDefence = -4;
                randomXmaxForDefence = -7;
                randomYminForDefence = -4;
                randomYmaxForDefence = 4;
                break;
            case "TeamBlueNPCBottom":
                randomXminForDefence = -9;
                randomXmaxForDefence = -13;
                randomYminForDefence = -1;
                randomYmaxForDefence = -7;
                break;
            case "TeamRedNPCTop":
                randomXminForDefence = 9;
                randomXmaxForDefence = 13;
                randomYminForDefence = 1;
                randomYmaxForDefence = 7;
                break;
            case "TeamRedNPCMiddle":
                randomXminForDefence = 4;
                randomXmaxForDefence = 7;
                randomYminForDefence = -4;
                randomYmaxForDefence = 4;
                break;
            case "TeamRedNPCBottom":
                randomXminForDefence = 8;
                randomXmaxForDefence = 12;
                randomYminForDefence = -1;
                randomYmaxForDefence = -7;
                break;
            default:
                randomXminForDefence = -8;
                randomXmaxForDefence = 8;
                randomYminForDefence = -5;
                randomYmaxForDefence = 5;
                break;
        }

        randomXForDefence = Random.Range(randomXminForDefence, randomXminForDefence);
        randomYForDefence = Random.Range(randomYminForDefence, randomYmaxForDefence);
        ifDrawFieldPointForDefence = false;
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
        else if (ifEnemyNPCNearbyForAttack && waitForAttack <= 0)
        {
            // Proste przej?cie - spr?bowa? ustawi? przej?cie gdy Raycast trafi przeciwnika

            if (ifEnemyNPCTopNearbyForAttack)
            {
                AttackFinalization(enemyNPCTop);
            }

            if (ifEnemyNPCMiddleNearbyForAttack)
            {
                AttackFinalization(enemyNPCMiddle);
            }

            if (ifEnemyPlayerNearbyForAttack)
            {
                AttackFinalization(enemyPlayer);
            }

            if (ifEnemyNPCBottomNearbyForAttack)
            {
                AttackFinalization(enemyNPCBottom);
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

    void AttackFinalization(GameObject enemy)
    {
        // Checking if the enemy is NPC or player. If NPC use GetComponent<NPCController>(), else player use GetComponent<PlayerController>()
        if (enemy.GetComponent<NPCController>())
        {
            if (enemy.GetComponent<NPCController>().HoldBomb)
            {
                if (!enemy.GetComponent<NPCController>().ShieldSpriteEnabled)
                {
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

                        int randomAttackSoundNumber = Random.Range(1, 3);

                        // Play random attack sound when attack
                        if (randomAttackSoundNumber == 1)
                            FindObjectOfType<AudioManager>().Play("Attack1");
                        else
                            FindObjectOfType<AudioManager>().Play("Attack2");

                        enemy.GetComponent<NPCController>().HoldBomb = false;
                        enemy.GetComponent<NPCController>().HoldBombTimer = restartholdBombTimer;
                        enemy.GetComponent<NPCController>().isShooting = false;
                        enemy.GetComponent<NPCController>().ifDrawFieldPointForAttack = true;
                        enemy.GetComponent<NPCController>().ifDrawFieldPointForDefence = true;
                        HoldBomb = true;
                    }
                    else
                    {
                        shortPauseBeforeAttack -= Time.deltaTime;
                    }
                }
                else
                {
                    shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
                }
            }
            else
            {
                shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
            }
        }
        else
        {
            if (enemy.GetComponent<PlayerController>().HoldBomb)
            {
                if (!enemy.GetComponent<PlayerController>().ShieldSpriteEnabled)
                {
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

                        int randomAttackSoundNumber = Random.Range(1, 3);

                        // Play random attack sound when attack
                        if (randomAttackSoundNumber == 1)
                            FindObjectOfType<AudioManager>().Play("Attack1");
                        else
                            FindObjectOfType<AudioManager>().Play("Attack2");

                        enemy.GetComponent<PlayerController>().HoldBomb = false;
                        enemy.GetComponent<PlayerController>().HoldBombTimer = restartholdBombTimer;
                        HoldBomb = true;
                    }
                    else
                    {
                        shortPauseBeforeAttack -= Time.deltaTime;
                    }
                }
                else
                {
                    shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
                }
            }
            else
            {
                shortPauseBeforeAttack = shortPauseBeforeAttackRestart;
            }
        }
    }

}
