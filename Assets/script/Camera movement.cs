using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramovement : MonoBehaviour
{
    // private float speed = 5; 
    private GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("HeroKnight");
    }

    // Update is called once per frame
    void Update()
    {
        float newX = player.transform.position.x;


        transform.position = new Vector3(newX, 0f, -10f);
    }
}
