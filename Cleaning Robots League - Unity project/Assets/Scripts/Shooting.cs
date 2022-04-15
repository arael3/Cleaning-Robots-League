using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    static float shootForce = 2.3f;

    // Update is called once per frame
    void Update()
    {
       
    }

    public static void Shoot(GameObject bombRef)
    {
        bombRef.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        bombRef.GetComponent<Renderer>().enabled = true;

        bombRef.GetComponent<Rigidbody2D>().AddForce(bombRef.transform.right * shootForce, ForceMode2D.Impulse);

        Bomb.onGround = true;
    }
}
