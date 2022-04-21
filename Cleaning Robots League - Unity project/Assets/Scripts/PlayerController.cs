using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 7f;

    public Rigidbody2D rb;
    public Renderer holdBombSprite;
    public Transform shootingPoint;
    [SerializeField] Renderer shieldSprite;
    public Camera cam;

    [SerializeField] GameObject teamBlueNPCTop;
    [SerializeField] GameObject teamBlueNPCMiddle;
    [SerializeField] GameObject teamBluePlayer;
    [SerializeField] GameObject teamBlueNPCBottom;

    [SerializeField] GameObject teamRedNPCTop;
    [SerializeField] GameObject teamRedNPCMiddle;
    [SerializeField] GameObject teamRedPlayer;
    [SerializeField] GameObject teamRedNPCBottom;

    GameObject enemyNPCTop;
    GameObject enemyNPCMiddle;
    GameObject enemyPlayer;
    GameObject enemyNPCBottom;

    GameObject friendNPCTop;
    GameObject friendNPCMiddle;
    GameObject friendNPCBottom;

    bool ifEnemyNPCNearbyForAttack = false;
    bool ifEnemyNPCTopNearbyForAttack = false;
    bool ifEnemyNPCMiddleNearbyForAttack = false;
    bool ifEnemyPlayerNearbyForAttack = false;
    bool ifEnemyNPCBottomNearbyForAttack = false;

    float enemyNPCTopDistance;
    float enemyNPCMiddleDistance;
    float enemyPlayerDistance;
    float enemyNPCBottomDistance;

    float minimumDistanceForAttackActivate = 2.0f;

    Vector2 movement;
    Vector2 mousePos;

    public GameObject bomb;

    public GameObject shootAnimation;

    [SerializeField] Rigidbody2D enemy;

    [SerializeField] GameObject attackAnimation;

    Vector2 lookDir;

    float shieldDuration = 1.5f;

    float shieldDurationRestart = 1.5f;

    public float waitForShield = 0f;

    public const float waitForShieldRestart = 1.8f;

    bool shieldSpriteEnabled;
    public bool ShieldSpriteEnabled
    {
        get { return shieldSpriteEnabled; }
        set { shieldSpriteEnabled = value; }
    }

    bool beforeAttack = false;

    bool duringAttack = false;

    float attackDuration = 1.15f;

    float attackDurationRestart = 1.15f;

    public float waitForAttack = 0f;

    public const float waitForAttackRestart = 1.5f;

    float shortPauseBeforeAttack = 0.06f;

    float shortPauseBeforeAttackRestart = 0.06f;

    float enemyDistanceX;
    float enemyDistanceY;

    float timeForShipAnimation = 0;

    bool waitForShoot = false;

    static bool holdBomb = false;

    public bool HoldBomb
    {
        get { return holdBomb; }
        set { holdBomb = value; }
    }

    float holdBombTimer = 1.5f;

    public float HoldBombTimer
    {
        get { return holdBombTimer; }
        set { holdBombTimer = value; }
    }

    public const float restartholdBombTimer = 1.5f;

    private void Start()
    {
        
        //transform.position = new Vector3(-3.0f, 0f, 0f);
        transform.position = StartingPosition(gameObject);

        // Finding which team the NPC belongs to
        if (gameObject.name == "TeamBluePlayer")
        {
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

            if (teamBlueNPCBottom)
            {
                friendNPCBottom = teamBlueNPCBottom;
            }

        }
        else
        {
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

            if (teamRedNPCBottom)
            {
                friendNPCBottom = teamRedNPCBottom;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("GameMenus.ifPause " + GameMenus.ifPause);
        Debug.Log("CountingTimeAfterGoal.ifPauseAfterGoal " + CountingTimeAfterGoal.ifPauseAfterGoal);
        if (Bomb.afterGoal)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            holdBombTimer = restartholdBombTimer;
            shieldDuration = 0f;
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
            movement.x = Input.GetAxisRaw("Horizontal");

            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x != 0 || movement.y != 0)
            {
                if (!FindObjectOfType<AudioManager>().isPlaying("P1Moving"))
                {
                    FindObjectOfType<AudioManager>().Play("P1Moving");
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().Stop("P1Moving");
            }

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (holdBomb)
            {
                holdBombTimer -= Time.deltaTime;
                bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                bomb.GetComponent<Renderer>().enabled = false;
                holdBombSprite.enabled = true;
                bomb.transform.position = shootingPoint.position;
                bomb.transform.rotation = Quaternion.Euler(0f, 0f, gameObject.transform.eulerAngles.z);
            }
            else
            {
                // Spróbowaæ dodaæ poni¿sze zmienne do instrukcji warunkowej, po udanym ataku przeciwnika
                holdBombTimer = restartholdBombTimer;
                holdBombSprite.enabled = false;
            }

            if (Input.GetButtonDown("Fire1") && !GameMenus.ifPause)
            {
                if (holdBomb)
                {
                    waitForShoot = true;

                    GameObject shootAnimInst = Instantiate(shootAnimation, transform.position, transform.rotation);
                    shootAnimInst.transform.parent = gameObject.transform;
                    SpriteRenderer shootAnimInstSprite = shootAnimInst.GetComponent<SpriteRenderer>();
                    if (shootAnimInstSprite)
                    {
                        shootAnimInstSprite.sortingOrder = 2;
                    }
                    Destroy(shootAnimInst, .4f);
                }
            }

            if (waitForShoot)
            {
                timeForShipAnimation += Time.deltaTime;

                if (timeForShipAnimation >= .05f)
                {
                    Shooting.Shoot(bomb);
                    //bomb.layer = 0;
                    holdBomb = false;
                    holdBombTimer = restartholdBombTimer;
                    holdBombSprite.enabled = false;

                    waitForShoot = false;
                    timeForShipAnimation = 0;
                }

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
            else if (Input.GetButtonDown("Fire2") && waitForShield <= 0)
            {
                shieldSprite.enabled = true;
                shieldSpriteEnabled = true;
                FindObjectOfType<AudioManager>().Play("Shield");
                waitForShield = waitForShieldRestart;
            }

            if (waitForShield > 0)
            {
                if (!shieldSprite.enabled)
                {
                    waitForShield -= Time.deltaTime;
                }
            }

            // Attack
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
            //else if (TeamRedNPCTop.GetComponent<EnNPCTopController>().HoldBomb && (waitForAttack <= 0) && (enemyDistanceX < 2) && (enemyDistanceY < 2))
            else if ((Input.GetKeyDown(KeyCode.Space) && waitForAttack <= 0) || beforeAttack)
            {
                beforeAttack = true;
                shortPauseBeforeAttack -= Time.deltaTime;
                //Debug.Log("shortPauseBeforeAttack " + shortPauseBeforeAttack);

                if (shortPauseBeforeAttack <= 0)
                {
                    //atackSprite.enabled = true;
                    duringAttack = true;
                    waitForAttack = waitForAttackRestart;
                    beforeAttack = false;

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

                    // Testowe przejêcie - faktyczne przejêcie tylko wtedy gdy Raycast trafi przeciwnika
                    //if (!TeamRedNPCTop.GetComponent<EnNPCTopController>().ShieldSpriteEnabled)
                    //{
                    //    TeamRedNPCTop.GetComponent<EnNPCTopController>().HoldBomb = false;
                    //    holdBomb = true;
                    //}


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

    void FixedUpdate()
    {
        if (!GameMenus.ifPause && !CountingTimeAfterGoal.ifPauseAfterGoal && !GameController.ifGameOver)
        {
            // Behavior when an enemy player is nearby.
            //enemyDistanceX = Mathf.Abs(rb.position.x - enemy.position.x);
            //enemyDistanceY = Mathf.Abs(rb.position.y - enemy.position.y);

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            //cam.transform.position = rb.position;

            lookDir = mousePos - rb.position;

            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            rb.rotation = angle;


            // Behavior when an enemy player is nearby.
            if (enemyNPCTop)
            {
                enemyNPCTopDistance = Vector2.Distance(rb.position, enemyNPCTop.GetComponent<Rigidbody2D>().position);

                if (enemyNPCTop.GetComponent<NPCController>().HoldBomb && enemyNPCTopDistance < minimumDistanceForAttackActivate)
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

                if (enemyNPCMiddle.GetComponent<NPCController>().HoldBomb && enemyNPCMiddleDistance < minimumDistanceForAttackActivate)
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

                if (enemyPlayer.GetComponent<PlayerController>().HoldBomb && enemyPlayerDistance < minimumDistanceForAttackActivate)
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

                if (enemyNPCBottom.GetComponent<NPCController>().HoldBomb && enemyNPCBottomDistance < minimumDistanceForAttackActivate)
                {
                    ifEnemyNPCBottomNearbyForAttack = true;
                }
                else
                {
                    ifEnemyNPCBottomNearbyForAttack = false;
                }
            }

            if (ifEnemyNPCTopNearbyForAttack || ifEnemyNPCMiddleNearbyForAttack || ifEnemyPlayerNearbyForAttack || ifEnemyNPCBottomNearbyForAttack)
            {
                ifEnemyNPCNearbyForAttack = true;
            }
            else
            {
                ifEnemyNPCNearbyForAttack = false;
            }
        }
    }

    public Vector3 StartingPosition(GameObject gameObject)
    {
        float x;
        float y;

        switch (gameObject.name)
        {
            case "TeamBluePlayer":
                x = -4f;
                y = 0f;
                break;
            case "TeamRedPlayer":
                x = 4f;
                y = 0f;
                break;  
            default:
                x = 0f;
                y = -5f;
                break;
        }
        return new Vector3(x, y, 0f);
    }
}
