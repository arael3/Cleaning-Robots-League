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

    public static bool onGround = true;

    bool ifAnyPlayerOrNPCHoldBomb = false;

    bool teamBluePlayerHoldBomb = false;
    bool teamBlueNPCTopHoldBomb = false;
    bool teamBlueNPCMiddleHoldBomb = false;
    bool teamBlueNPCBottomHoldBomb = false;
    bool teamRedPlayerHoldBomb = false;
    bool teamRedNPCTopHoldBomb = false;
    bool teamRedNPCMiddleHoldBomb = false;
    bool teamRedNPCBottomHoldBomb = false;

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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        string collisionObjectTag = collision.gameObject.tag;
        string collisionObjectName = collision.gameObject.name;
        //Debug.Log(collisionObjectName);

        if (collisionObjectTag == "GoalRight")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            Score.teamBlueScore += 1;
            gameObject.transform.position = new Vector3(0f, 0f, 0f);

        }
        else if (collisionObjectTag == "GoalLeft")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Score.teamRedScore += 1;
            gameObject.transform.position = new Vector3(0f, 0f, 0f);

        }
        else if (collisionObjectTag == "PlayerOrNPC")
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
