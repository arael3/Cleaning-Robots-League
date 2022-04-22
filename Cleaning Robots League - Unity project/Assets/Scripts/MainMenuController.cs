using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    //[SerializeField] GameObject mainMenu;
    //GameMenus gameMenusScript;
    // Start is called before the first frame update
    void Start()
    {
        //gameMenusScript = mainMenu.GetComponent<GameMenus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMenus.ifStart)
        {
            GameMenus.CountingDownAndStartMatch();
        }
    }
}
