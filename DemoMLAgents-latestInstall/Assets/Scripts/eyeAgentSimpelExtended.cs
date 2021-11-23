using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class eyeAgentSimpelExtended : Agent
{
    //public var
    public TextMeshPro scoreBoard;
    public float rotationSpeed = 100f;
    public float Speed = 1f;
    public GameObject mEnemy;


    //private var
    private Rigidbody mRigidBody;
    private bool hasTouchedEnemy = false;
    private TheEnvironment environment;
    private float rotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        environment = GetComponentInParent<TheEnvironment>();
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

    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;
        //AddReward(-0.001f);

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

        if (vectorAction[1] != 0)        
        {
            Vector3 translation = transform.forward * Speed * Time.deltaTime;
            transform.Translate(translation, Space.World);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int movementRotation = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementRotation = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movementRotation = 2;
        }
        int movementForward = 0;
        if (Input.GetKey(KeyCode.UpArrow))    //Move forward
        {

            movementForward = 1;
        }
       
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = movementRotation;
        discreteActionsOut[1] = movementForward;
    }   


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
