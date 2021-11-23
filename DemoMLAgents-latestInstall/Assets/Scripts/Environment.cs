using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject Player;
    private roomAgent rAgent;
   
    public TextMeshPro txtScoreBoard;


    // Start is called before the first frame update
    void Start()
    {
        //scoreBoard = transform.GetComponentInChildren<TextMeshPro>();
       
        rAgent = Player.GetComponent<roomAgent>();
       
       
       }

    // Update is called once per frame
   

    private void FixedUpdate()
    {
        txtScoreBoard.text = rAgent.GetCumulativeReward().ToString("f2");
    }
}
