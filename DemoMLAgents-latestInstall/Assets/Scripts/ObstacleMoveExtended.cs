using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ObstacleMoveExtended : MonoBehaviour
{

    [SerializeField]
    private float speed = 4f;

    private JumperAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GameObject.Find("Agent").GetComponent<JumperAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = Random.Range(3, 6);
        transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {      
            if (collision.transform.CompareTag("Wall"))
            {
               // Debug.Log("raak");
                agent.AddReward(1f);
                Destroy(this.gameObject);
               // agent.EndEpisode();
        }
            
       
    }
}
