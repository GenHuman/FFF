using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    public int agentAmount = 5;
    public GameObject agentPrefab;
    public GameObject eggPrefab;
    public Vector3 startingPosition = new Vector3(2, 3.5f, 2);

    public Transform activeAgentParent;
    public Transform inactiveAgentParent;
    public Queue<GameObject> inactiveAgentList = new Queue<GameObject>();
    public List<GameObject> activeAgentList = new List<GameObject>();

    public Transform activeEggParent;
    public Transform inactiveEggParent;
    public Queue<GameObject> inactiveEggList = new Queue<GameObject>();
    public List<GameObject> activeEggList = new List<GameObject>();

    public Transform cam;


    #region characteristics


    #region movement
        public float speed = 10f;
        public float movingEnergyConsumption = 2f;
        public int actionAfterFeast = 1;
        #endregion

        #region foodManagement
        public float energy = 100f;
        public float[] eatingEfficiency = { 1f, 0f, 0f };
        public float idleEnergyConsumption = 0.5f;
        #endregion


        #region scouting
        public int scoutingType = Constants.SCOUT_RANDOM;
        public float scoutingSpeed = 7.5f;
        public float scoutingConsumption = 1f;
        #endregion

        #region scan
        public int scanType = Constants.SCAN_RANDOM;
        public float scanSpeed = 5f;
        public float visionAngle = 30;
        public float visionRadius = 3;
        #endregion


        #region jump
        public float jumpEnergyConsumption = 40f;
        public float forwardJumpForce = 5f;
        public float upJumpForce = 4f;
        public bool backJump = false;
        public float backJumpForce = 2f;
        public float upBackJumpForce = 2f;
        #endregion


        #region Focus

        public float maxFocus = 10;

        #endregion

        #region Heir
        
        public float heirEnergy = 0.5f;
        public float hatchtime = 10;
        public int energyForEgg = 300;

        #endregion


    #endregion


    void Start()
    {
        for(int i = 0; i<agentAmount; i++)
        {
            GameObject agente = Instantiate(agentPrefab, inactiveAgentParent);
            inactiveAgentList.Enqueue(agente);
            agente.SetActive(false);

            GameObject egg = Instantiate(eggPrefab, inactiveEggParent);
            inactiveEggList.Enqueue(egg);
            egg.SetActive(false);
        }
        GameObject agent = inactiveAgentList.Dequeue();
        agent.transform.parent = activeAgentParent;
        agent.transform.position = startingPosition;
        agent.GetComponent<Naviagtor>().setVariables(this, 100);

        agent.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int createEgg(Transform parentTransform, int startingEnergy)
    {
        if (inactiveEggList.Count > 0 && activeAgentList.Count + activeEggList.Count < 5)
        {
            float energyForHeir = 0;
            if (heirEnergy < 1)
            {
                energyForHeir = heirEnergy * (float)startingEnergy;
            }
            else
            {
                energyForHeir = heirEnergy;
            }
            int energy2 = startingEnergy - (int)energyForHeir;


            GameObject egg = inactiveEggList.Dequeue();
            activeEggList.Add(egg);

            egg.transform.parent = activeEggParent;
            egg.SetActive(true);

            egg.transform.position = parentTransform.position;
            egg.transform.rotation = parentTransform.rotation;

            egg.GetComponent<EggScript>().setVariables((int)energyForHeir, hatchtime, this);


            return energy2;
        }
        else
        {
            return startingEnergy;
        }
    }

    public GameObject hatchEgg(GameObject egg, int startingEnergy)
    {
        
        activeEggList.Remove(egg);
        
        inactiveEggList.Enqueue(egg);
        GameObject newAgent = inactiveAgentList.Dequeue();
        activeAgentList.Add(newAgent);
        newAgent.transform.parent = activeAgentParent;
        egg.transform.parent = inactiveEggParent;
        egg.SetActive(false);
        newAgent.transform.position = egg.transform.position;
        newAgent.transform.rotation = egg.transform.rotation;
        newAgent.GetComponent<Naviagtor>().setVariables(this, startingEnergy);
        newAgent.SetActive(true);
        return newAgent;
    }
}
