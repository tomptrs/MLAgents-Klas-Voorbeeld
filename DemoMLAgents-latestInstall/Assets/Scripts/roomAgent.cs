using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class roomAgent : Agent
{
    Rigidbody mRigidBody;
    public float Speed = 5f;
    public List<GameObject> enemies;
    private int Aantal = 0;
   // public GameObject mDoor;
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody>();
        enemies = new List<GameObject>();

        
    }

    public override void OnEpisodeBegin()
    {
        Aantal = 0;
        foreach (GameObject enemy in enemies)
        {
            Debug.Log("On episode begin");
            float rX = Random.Range(-7, +7);
            float rZ = Random.Range(-6, +6);
            Debug.Log(rX + "," + rZ);
            enemy.transform.localPosition = new Vector3(rX, 0.5f, rZ);
            enemy.SetActive(true);
           
        }
        float rXp = Random.Range(-7, +7);
        float rZp = Random.Range(-6, +6);
        transform.localPosition = new Vector3(rXp, 0.5f, rZp);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
        sensor.AddObservation(Aantal);
       
    }

    public float rotationSpeed = 100f;
 /*   public override void OnActionReceived(float[] vectorAction)
    {

        if (transform.localPosition.y < -1)
        {
            AddReward(-1f);
            EndEpisode();
        }

        if (vectorAction[0] == 0)
        {
            //AddReward(-0.001f);
            //AddReward(-0.001f); //verplicht agent om te roteren
        }
        else
        {
            if (vectorAction[0] == 1)//left rotation
            {
                rotationSpeed = 5f;
            }
            if (vectorAction[0] == 2)//right rotation
            {
                rotationSpeed = -5f;
            }
            //transform.Rotate(new Vector3(0, rotation, 0), Space.Self);
            float rotation = rotationSpeed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
            
            transform.Rotate(0, rotation, 0);
        }

        if (vectorAction[1] == 0)
        {
            AddReward(-0.001f); //verplicht agent om forward te bewegen

        }
        else
        {            
            mRigidBody.velocity = transform.forward * Speed;
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
           
            actionsOut[1] = 1;
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            // Destroy(collision.gameObject);
            //collision.gameObject.transform.localPosition = new Vector3(100, 100, 100);
            collision.gameObject.SetActive(false);
           
            Aantal++;
            AddReward(0.3f*Aantal);

            if (Aantal == 3)
                EndEpisode();

            //EndEpisode();
        }

      /*  if (collision.transform.CompareTag("Door"))
        {
            Debug.Log("Collide with Door");
            AddReward(0.5f);
          //  mDoor.SetActive(false);
        }
      */
        if (collision.transform.CompareTag("Wall"))
        {
           
            AddReward(-0.1f);
        }
        if (collision.transform.CompareTag("Exit"))
        {  
            AddReward(-1f);
            EndEpisode();
        }
    }
}
