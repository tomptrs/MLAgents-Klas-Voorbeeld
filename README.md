## Inleiding tot de usecase

Een agent wordt getraind om eerst een blokje te vinden en vervolgens zich naar de groene zone te begeven.

## Observaties, acties en beloning

In het geval van onze agent zijn de observaties diegene die hij maakt door "rond" te kijken. Dit verwezenlijken we door onze agent rayperception sensor 3D componenenten te geven. Verder geven we ook een boolean "hasTouchedEnemy" mee zodat tensorflow weet dat deze observatie belangrijk zal zijn om tot een policy te komen om zich naar de groene zone te begeven.

## Acties

Op de ene of andere manier moet het algoritme leren om de agent a.h.w. te besturen. Met andere woorden, het algoritme moet een actie voorstellen. Wij, de ontwikkelaar mappen de acties naar beweging. In het geval van de agent zijn er 5 mogelijke acties.
  
  - 3 rotaties acties : links en rechts roteren en niet roteren
  - 2 voorwaartse beweging acties: voorwaarts bewegen en niet bewegen

Rotatie is belangrijk omdat onze agent moet kunnen rondkijken waar het blokje zich bevindt en waar de groene zone ergens is.

## Beloning

Het beloningsmechanisme (eng: rewards, incentives) vertelt het leeralgoritme of de voorgestelde actie de agent dichter bij het einddoel van de leeroefening brengt of niet.

We belonen onze agent met +0.5 waarde als hij heb blokje kan vinden en aanraken, en vervolgens krijgt de agent een beloning van +1 als hij na het raken van het blokje zich naar de groene zone begeeft. Hier stopt dan ook de episode.

Verder krijgt onze agent een afstraffing van -1 als hij van het platform valt. In deze situatie stopt de episode ook onmiddellijk.

# Unity omgeving aanmaken

We maken een empty object dat we de naam environment geven. Daaronder maken we onze speelobjecten aan, namelijk onze Plane, Player (of agent), Enemy (of blokje), en nieuw plane dat als groene zone fungeert.

### Environment

Aan het parent object Environment wijzen we een script toe (theEnvironment.cs) 

´´´
public class TheEnvironment : MonoBehaviour
{

    
    public GameObject EnemyPrefab;
    private GameObject Enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Enemies = transform.Find("Enemies").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearEnvironment()
    {
        foreach (Transform enemy in Enemies.transform)
        {
            GameObject.Destroy(enemy.gameObject);
        }

    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(EnemyPrefab.gameObject);

        newEnemy.transform.SetParent(Enemies.transform);
        float rx = Random.Range(-4f, 4);
        float rz = Random.Range(-4f, 2);
        newEnemy.transform.localPosition = new Vector3(rx, 0.5f, rz);
    }
}
´´´

We hebben van onze enemy een prefab gemaakt, en via het "TheEnvironment" script gaan we elke episode de enemy dynamisch in onze omgeving plaatsen. Op die manier kunnen we heel makkelijk uitbreiden naar verschillende vijanden. Ook is er een CleanEnvironment methode aangemaakt die bij het aanroepen van EndEpisode() de vijand zal verwijderen. Dit is nodig in bijvoorbeeld het geval dat onze agent de vijand nog niet gevonden heeft en van het vlak valt. Dan wordt er opnieuw "SpawnEnemy()" opgeroepen en zullen er 2 vijanden op het speelbord verschijnen, wat in onze use case niet de bedoeling is.

Verder is het script eyeAgentSimpelExtended aan onze agent toegewezen. In de Start() methode refereren we naar TheEnvironment door volgend commando: 
environment = GetComponentInParent<TheEnvironment>();

Op die manier kunnen we bij OnEpisodeBegin() steeds ClearEnvironment() en SpawnEnemy() oproepen. 

´´´
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

´´´
