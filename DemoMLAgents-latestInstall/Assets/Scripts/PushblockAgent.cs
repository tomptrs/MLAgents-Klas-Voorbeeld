using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PushblockAgent : Agent
{
    public TextMeshPro scoreBoard;
    Rigidbody mRigidBody;
    public float Speed = 1f;
    public GameObject mEnemy;
    bool hasTouchedEnemy = false;
    private EnvironmentPushblock environment;
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        environment = GetComponentInParent<EnvironmentPushblock>();
        Debug.Log(environment);
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard.text = GetCumulativeReward().ToString("f4");
    }

    public override void OnEpisodeBegin()
    {
        //clear environment first
        environment.ClearEnvironment();

        float rx = Random.Range(-4f, 4);
        float rz = Random.Range(-4f, 4);

        environment.SpawnEnemy();
       
        transform.localPosition = new Vector3(0, 0.5f, 0);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        hasTouchedEnemy = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(hasTouchedEnemy);
    }

    float rotation = 0f;
    public float rotationSpeed = 100f;
 /*   public override void OnActionReceived(float[] vectorAction)
    {
        // Penalty given each step to encourage agent to finish task quickly.
        AddReward(-0.001f);

        if (transform.localPosition.y < -0.5f)
        {
            AddReward(-1f);
            EndEpisode();
        }

        if (vectorAction[0] != 0)
        {

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
        
        actionsOut[0] = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))    //left turn
            actionsOut[0] = 1; 
        if (Input.GetKey(KeyCode.RightArrow))   //right turn
            actionsOut[0] = 2;

        actionsOut[1] = 0f;
        if (Input.GetKey(KeyCode.UpArrow))    //Move forward
        {
          
            actionsOut[1] = 1;
        }
    }
 */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (!hasTouchedEnemy)
            {
                Debug.Log("Collide with Enemy");
                hasTouchedEnemy = true;
                Destroy(collision.gameObject);
                AddReward(0.5f);
            }
            else
            {
                AddReward(-0.1f);
            }
        }
        if (collision.transform.CompareTag("goal"))
        {
            if (hasTouchedEnemy)
            {
                AddReward(1f);
                EndEpisode();
                Debug.Log("Collide with goal");
            }
           
        }

    }
}
