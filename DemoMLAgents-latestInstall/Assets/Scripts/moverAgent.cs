using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class moverAgent : Agent
{
    public TextMeshPro scoreBoard;
    public GameObject largeObject;
    public GameObject smallObject;
    public Renderer background;
    public Material defaultMaterial;
    public Material badMaterial;
    public Material goodMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard.text = GetCumulativeReward().ToString("f4");

    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        float r = Random.Range(0f, 10.0f);
        if(r > 5) {
            largeObject.transform.localPosition = new Vector3(-4, 0.5f, 0);
            smallObject.transform.localPosition = new Vector3(4, 0.25f, 0);
        }
        else
        {
            largeObject.transform.localPosition = new Vector3(4, 0.5f, 0);
            smallObject.transform.localPosition = new Vector3(-4, 0.25f, 0);
        }

        //base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
       // sensor.AddObservation(transform.position.x);//my x position
        sensor.AddObservation(largeObject.transform.localPosition.x); 
        sensor.AddObservation(smallObject.transform.localPosition.x);        
        sensor.AddObservation(largeObject.transform.localScale.y);
        sensor.AddObservation(smallObject.transform.localScale.y);

        //        base.CollectObservations(sensor);
    }

    /*  public override void OnActionReceived(float[] vectorAction)
      {
          Debug.LogWarning(vectorAction[0]);
          if (vectorAction[0] == 0)
          {
              AddReward(-0.001f); //voor niets te doen aftrek van de reword
              return;
          }
          if(vectorAction[0]!=0)
          {
             // AddReward(0.001f);      //bij bewegen krijg je een positieve award om beweging te stimuleren
              if(vectorAction[0] == 1)
                  transform.localPosition += new Vector3(1 * Time.deltaTime, 0, 0);
              if (vectorAction[0] == 2)
                  transform.localPosition += new Vector3(-1 * Time.deltaTime, 0, 0);
          }
          //  base.OnActionReceived(vectorAction);
      }

      public override void Heuristic(float[] actionsOut)
      {
          actionsOut[0] = 0f;
          if (Input.GetKey(KeyCode.LeftArrow))
              actionsOut[0] = 2;
          if (Input.GetKey(KeyCode.RightArrow))
              actionsOut[0] = 1;
          // base.Heuristic(actionsOut);
      }*/

    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;
        Debug.LogWarning(vectorAction[0]);
        if (vectorAction[0] == 0)
        {
            AddReward(-0.001f); //voor niets te doen aftrek van de reword
            return;
        }
        if (vectorAction[0] != 0)
        {
            // AddReward(0.001f);      //bij bewegen krijg je een positieve award om beweging te stimuleren
            if (vectorAction[0] == 1)
                transform.localPosition += new Vector3(1 * Time.deltaTime, 0, 0);
            if (vectorAction[0] == 2)
                transform.localPosition += new Vector3(-1 * Time.deltaTime, 0, 0);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int movement = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement = 2;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement = 1;
        }
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = movement;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("large"))
        {
            AddReward(1);
            EndEpisode();
            StartCoroutine(SwapGroundMaterial(goodMaterial, 0.5f));
            Debug.LogWarning("large");
            // scoreBoard.text = "large";
        }

        if (collision.transform.CompareTag("small"))
        {
            Debug.LogWarning("small");            
            AddReward(-0.5f);
 //           EndEpisode();
            StartCoroutine(SwapGroundMaterial(badMaterial, 0.5f));
            // scoreBoard.text = "small";
        }
    }

    private IEnumerator SwapGroundMaterial(Material material, float time)
    {
        background.material = material;
        yield return new WaitForSeconds(time);
        background.material = defaultMaterial;
    }
}
