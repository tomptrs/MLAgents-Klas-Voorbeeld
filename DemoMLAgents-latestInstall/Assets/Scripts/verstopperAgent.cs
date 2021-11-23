using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class verstopperAgent : Agent
{

    Rigidbody m_AgentRb;

    // Start is called before the first frame update
    void Start()
    {
        // Cache the agent rigidbody
        m_AgentRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAgent(float[] act)
    {
        if(transform.position.y < 0)
        {
            AddReward(-1f);
            EndEpisode();
        }

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 1f, ForceMode.VelocityChange);
    }

    public override void OnEpisodeBegin()
    {   
        m_AgentRb.velocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = GetRandomSpawnPos();
    }

    public Vector3 GetRandomSpawnPos()
    {
        bool foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;

        while (!foundNewSpawnLocation)
        {
            var randomPosX = Random.Range(-8, 8);
            var randomPosZ = Random.Range(-8, 8);

            randomSpawnPos = new Vector3(randomPosX, 1, randomPosZ);

            if (Physics.CheckBox(randomSpawnPos, new Vector3(0f, 2.5f, 0f)) == false &&
                Physics.CheckBox(randomSpawnPos, new Vector3(-2.3f, 2.5f, 4.5f))
                )
            {
                foundNewSpawnLocation = true;
            }
        }

        return randomSpawnPos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
  
    }

   /* public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log(MaxStep);
        AddReward(-0.0001f);
        MoveAgent(vectorAction);
    }

    public override void Heuristic(float[] actionsOut)
    {
      
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }
    
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            AddReward(1f);
            EndEpisode();
        }
    }


}
