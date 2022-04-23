using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombExplode;

    [SerializeField] GameObject teamBluePlayer;
    [SerializeField] GameObject teamBlueNPCTop;
    [SerializeField] GameObject teamBlueNPCMiddle;
    [SerializeField] GameObject teamBlueNPCBottom;

    [SerializeField] GameObject teamRedPlayer;
    [SerializeField] GameObject teamRedNPCTop;
    [SerializeField] GameObject teamRedNPCMiddle;
    [SerializeField] GameObject teamRedNPCBottom;

    //[SerializeField] GameObject pauseMenu;
    //GameMenus gameMenusScript;

    [SerializeField] GameObject countingTimeAfterGoal;



    public static int teamBlueScore = 0;
    public static int teamRedScore = 0;

    public static bool onGround = true;

    public static bool afterGoal = false;

    float timeAfterGoal = 0.1f;
    float timeAfterGoalRestart = 0.1f;

    bool ifAnyPlayerOrNPCHoldBomb = false;

    bool teamBluePlayerHoldBomb = false;
    bool teamBlueNPCTopHoldBomb = false;
    bool teamBlueNPCMiddleHoldBomb = false;
    bool teamBlueNPCBottomHoldBomb = false;
    bool teamRedPlayerHoldBomb = false;
    bool teamRedNPCTopHoldBomb = false;
    bool teamRedNPCMiddleHoldBomb = false;
    bool teamRedNPCBottomHoldBomb = false;

    private void Start()
    {
        //gameMenusScript = pauseMenu.GetComponent<GameMenus>();
    }

    private void Update()
    {
        GameObject[] NPCandPlayers = { teamBluePlayer, teamBlueNPCTop, teamBlueNPCMiddle, teamBlueNPCBottom, teamRedPlayer, teamRedNPCTop, teamRedNPCMiddle, teamRedNPCBottom };
        string[] NPCandPlayersNames = { "TeamBluePlayer", "TeamBlueNPCTop", "TeamBlueNPCMiddle", "TeamBlueNPCBottom", "TeamRedNPCPlayer", "TeamRedNPCTop", "TeamRedNPCMiddle", "TeamRedNPCBottom" };

        bool ifAnyPlayerOrNPCHoldBombChecker = false;

        int i = 0;
        // if Any Player Or NPC Hold Bomb
        foreach (string item in NPCandPlayersNames)
        {
            if (GameObject.Find(item) != null)
            {
                if (NPCandPlayers[i].GetComponent<PlayerController>())
                {
                    if (NPCandPlayers[i].GetComponent<PlayerController>().HoldBomb)
                    {
                        ifAnyPlayerOrNPCHoldBomb = true;
                        ifAnyPlayerOrNPCHoldBombChecker = true;
                        gameObject.layer = 6;
                    }
                }
                else
                {
                    if (NPCandPlayers[i].GetComponent<NPCController>().HoldBomb)
                    {
                        ifAnyPlayerOrNPCHoldBomb = true;
                        ifAnyPlayerOrNPCHoldBombChecker = true;
                        gameObject.layer = 6;
                    }
                }
            }
            i++;
        }

        if (!ifAnyPlayerOrNPCHoldBombChecker)
        {
            ifAnyPlayerOrNPCHoldBomb = false;
            gameObject.layer = 0;
        }

        if (afterGoal)
        {
            timeAfterGoal -= Time.deltaTime;
        }

        if (timeAfterGoal <= 0)
        {
            countingTimeAfterGoal.SetActive(true);
            CountingTimeAfterGoal.ifPauseAfterGoal = true;
            afterGoal = false;
            timeAfterGoal = timeAfterGoalRestart;
        }

        if (GameMenus.ifStart)
        {
            countingTimeAfterGoal.SetActive(false);
            CountingTimeAfterGoal.ifPauseAfterGoal = false;
            afterGoal = false;
            timeAfterGoal = timeAfterGoalRestart;
        }

        if (GameController.ifGameOver)
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<Renderer>().enabled = true;
            onGround = true;
            gameObject.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        string collisionObjectTag = collision.gameObject.tag;
        string collisionObjectName = collision.gameObject.name;
        //Debug.Log(collisionObjectName);

        if (collisionObjectTag == "GoalRight" && !GameController.ifGameOver)
        {
            afterGoal = true;

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            teamBlueScore += 1;
            FindObjectOfType<AudioManager>().Play("Goal");

            if (teamRedPlayer)
            {
                teamRedPlayer.GetComponent<PlayerController>().HoldBomb = true;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.transform.position = teamRedPlayer.transform.position;
            }
            else
            {
                teamRedNPCMiddle.GetComponent<NPCController>().HoldBomb = true;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.transform.position = teamRedNPCMiddle.transform.position;
            }   
        }
        else if (collisionObjectTag == "GoalLeft" && !GameController.ifGameOver)
        {
            afterGoal = true;

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            teamRedScore += 1;
            FindObjectOfType<AudioManager>().Play("Goal");

            if (teamBluePlayer)
            {
                teamBluePlayer.GetComponent<PlayerController>().HoldBomb = true;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.transform.position = teamBluePlayer.transform.position;
            }
            else
            {
                teamBlueNPCMiddle.GetComponent<NPCController>().HoldBomb = true;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.transform.position = teamBlueNPCMiddle.transform.position;
            }
        }
        else if (collisionObjectTag == "PlayerOrNPC" && !GameController.ifGameOver)
        {
            onGround = false;

            if ((collisionObjectName == "TeamBluePlayer") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //Player1Controller.holdBomb = true;
                teamBluePlayer.GetComponent<PlayerController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamBlueNPCTop") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //NPCTopController.holdBomb = true;
                teamBlueNPCTop.GetComponent<NPCController>().HoldBomb = true;
            }            
            else if ((collisionObjectName == "TeamBlueNPCMiddle") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //NPCBottomController12.holdBomb = true;
                teamBlueNPCMiddle.GetComponent<NPCController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamBlueNPCBottom") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //NPCBottomController12.holdBomb = true;
                teamBlueNPCBottom.GetComponent<NPCController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamRedNPCPlayer") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //EnNPCTopController.holdBomb = true;
                teamRedPlayer.GetComponent<PlayerController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamRedNPCTop") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //EnNPCTopController.holdBomb = true;
                teamRedNPCTop.GetComponent<NPCController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamRedNPCMiddle") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //EnNPCTopController.holdBomb = true;
                teamRedNPCMiddle.GetComponent<NPCController>().HoldBomb = true;
            }
            else if ((collisionObjectName == "TeamRedNPCBottom") && (gameObject.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic))
            {
                //EnNPCTopController.holdBomb = true;
                teamRedNPCBottom.GetComponent<NPCController>().HoldBomb = true;
            }
        }
    }
}
