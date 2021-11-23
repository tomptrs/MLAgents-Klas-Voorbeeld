using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class eyeAgent : Agent
{
    public TextMeshPro scoreBoard;
    Rigidbody mRigidBody;
    public float Speed = 1f;
    public GameObject mEnemy;
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard.text = GetCumulativeReward().ToString("f4");
    }

    public override void OnEpisodeBegin()
    {
        float r = Random.Range(0f, 10.0f);
        if (r > 5)
        {
            mEnemy.transform.localPosition = new Vector3(-16, 0.5f, -10);
           
        }
        else
        {
            mEnemy.transform.localPosition = new Vector3(-16, 0.5f, 10);
            
        }

        transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

   /* public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }*/

    float rotation = 0f;
    public float rotationSpeed = 100f;
    /*public override void OnActionReceived(float[] vectorAction)
    {
        // Penalty given each step to encourage agent to finish task quickly.
        AddReward(-0.001f);
       
        if (transform.localPosition.y < -0.5f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        if (vectorAction[0] == 0)
        {           
           
        }
        else{

            float rotation = rotationSpeed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
            transform.Rotate(0, rotation, 0);

        }

        if(vectorAction[1] == 0)
        {
                     
        }

        else
        {            
            Vector3 translation = transform.forward * Speed  * Time.deltaTime;
            transform.Translate(translation, Space.World);
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        Debug.Log(actionsOut[1]);
        actionsOut[0] = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))    //left turn
            actionsOut[0] = 1; 
        if (Input.GetKey(KeyCode.RightArrow))   //right turn
            actionsOut[0] = 2;

        actionsOut[1] = 0f;
        if (Input.GetKey(KeyCode.UpArrow))    //Move forward
        {
            Debug.Log("forward movement");
            actionsOut[1] = 1;
        }
    }*/

    private void LeftTurn()
    {

    }
    private void RightTurn()
    {

    }

    private void Move()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Debug.Log("Collide with Enemy");
            AddReward(1);
            EndEpisode();
        }

    }
}
