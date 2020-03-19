using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Naviagtor : MonoBehaviour
{
    Transform camer;

    public TeamScript teamscript;
    #region movement
    public float speed = 10f;
    public float movingEnergyConsumption = 2f;
    bool moving = false;
    Vector3 moveDir = Vector3.forward;
    public GameObject objective = null;
    public Vision vision;

    public int actionAfterFeast = -1;

    #endregion

    #region foodManagement
    public float energy = 100f;
    public float[] eatingEfficiency = { 1f, 0f ,0f};
    public float idleEnergyConsumption = 0.5f;
    public int energyForEgg = 300;
    #endregion


    #region scouting
    public int scoutingType = 0;
    public bool scouting = false;
    public float scoutingSpeed = 5f;
    public float scoutingConsumption = 1f;
    float scoutingTimer = 0f;
    float scoutingFocus = 2f;
    bool collided = false;

    #endregion

    #region scan
    public int scanType = 0;
    public bool scanning = false;
    public float scanSpeed = 5f;



    Rigidbody rb;

    #endregion


    #region jump
    public bool needToJumpV = false;
    public bool jumping = false;
    public float jumpEnergyConsumption = 40f;
    public float forwardJumpForce = 5f;
    public float upJumpForce = 4f;
    bool backJump = false;
    public float backJumpForce = 2f;
    public float upBackJumpForce = 2f;
    float distToGround;
    #endregion


    #region Focus
    public float focusTimer = 10;
    public float maxFocus = 10;
    public bool focusedOnObjective = false;

    #endregion

    public TextMeshPro text;
   

    public bool debug = false;

    // Start is called before the first frame update
    void Start()
    {
        //moveDir =  transform.forward;
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        text.text = energy.ToString();
        //text.transform.LookAt(camer);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(transform.position, transform.position + transform.forward , Color.red, Time.deltaTime);
        //rb.AddRelativeForce(moveDir * speed * Time.deltaTime, ForceMode.VelocityChange);



        if (focusedOnObjective)
        {

            scouting = false;
            if (objective == null || !objective.activeSelf)
            {
                focusedOnObjective = false;
                moving = false;
                scouting = true;
                
            }
            else
            {
                GoToObjective();
                focusTimer -= Time.deltaTime;
                if (focusTimer <= 0)
                {
                    objective = null;
                    focusTimer = maxFocus;
                }
            }

        }
        else
        {
            moving = false;
            scouting = true;
        }
        /*else
        {
            if (scoutScaning)
            {
                moving = false;
            }
        }*/

        
        //transform.position += moveDir * speed * Time.deltaTime;
        scanning = vision.scanning;
        if (moving)
        {
            rb.AddRelativeForce(moveDir * speed * Time.deltaTime, ForceMode.VelocityChange);
            consumeEnergy(movingEnergyConsumption);
        }
        else if(scouting)
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), transform.forward, Color.white, Time.deltaTime);
            int layerMask = 1 << 8;
            if (!collided)
            {
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), transform.forward, 1f, layerMask))
                {
                    collided = true;
                }
            }
            scout();
        }else if (scanning)
        {

        }
        rb.maxAngularVelocity = 0.0001f;
    }

    public Vector3 NewDirection()
    {
        float x = UnityEngine.Random.Range(-1, 1);
        float z = UnityEngine.Random.Range(-1, 1);
        Vector3 r = new Vector3(x, 0, z);
        return r.normalized;
    }
    public Quaternion NewRotation()
    {
        bool nocollision;
        float y = UnityEngine.Random.Range(-180, 180);
        Quaternion r = Quaternion.Euler(0, y, 0);
        while (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), r * transform.forward,2f, 1 << 8))
        {
            y = UnityEngine.Random.Range(0, 359);
            r = Quaternion.Euler(0, y, 0);
            
        }
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), r * transform.forward, Color.green, 5000);
        return r;
    }

    public bool GoToObjective()
    {


        /*if (debug)
        {
            Debug.Log("RUN");
            Debug.DrawLine(transform.position, moveDir, Color.red, Time.deltaTime);
        }*/
        //moveDir = new Vector3 (objective.transform.position.x, transform.position.y, objective.transform.position.z);
        needToJumpV = objective.transform.position.y > transform.position.y;

        transform.LookAt(new Vector3(objective.transform.position.x, transform.position.y, objective.transform.position.z));
        if (needToJumpV && IsGrounded())
        {
            needToJump();
        }
        moving = true;
        //moveDir = transform.forward;
        

        return AlmostEqual(transform.position, objective.transform.position, 0.05f);
    }


    public bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

    public void NewObjective(GameObject go)
    {
        objective = go;
        focusedOnObjective = true;
        focusTimer = maxFocus;
    }

    public void NewObjective(GameObject go, bool needjump)
    {
        objective = go;
        focusedOnObjective = true;
        needToJumpV = needjump;
    }


    public float digest(float energyAmount, int type)
    {

        energy += eatingEfficiency[type] * energyAmount;
        objective = null;
        focusedOnObjective = false;
        switch (actionAfterFeast) {

            case Constants.ACTION_SCAN:
                scan();
                break;
            case Constants.ACTION_SCOUT:
                scout();
                break;
            default:
                break;
        }
        if (energy > energyForEgg)
        {
            Debug.Log("OldEnergy "+energy);
            energy = teamscript.createEgg(transform, (int)energy);
            Debug.Log("Spawn EGG");
            Debug.Log("NewEnergy "+energy);

        }
        return energy;
    }

    public float consumeEnergy(float energyAmount)
    {

        energy -= (energyAmount * Time.deltaTime);
        text.text = ((int)energy).ToString();

        return energy;

    }


    public void scout()
    {
        if (!jumping)
        {
            switch (scoutingType)
            {
                case Constants.SCOUT_STILL:
                    if (!vision.scanning)
                    {

                        vision.resetScanner();
                        vision.toggleScaning(true);
                    }
                    break;
                case Constants.SCOUT_RANDOM:
                    if (vision.scanEnded)
                    {
                        transform.rotation = transform.rotation * NewRotation();
                        vision.resetScanner();
                    }
                    else if (!vision.scanning && scoutingTimer < scoutingFocus)
                    {


                        rb.AddRelativeForce(moveDir * scoutingSpeed * Time.deltaTime, ForceMode.VelocityChange);
                        scoutingTimer += Time.deltaTime;
                    }
                    else if (!vision.scanning && scoutingTimer >= scoutingFocus)
                    {
                        vision.toggleScaning(true);
                        scoutingTimer = 0;
                    }
                    break;
                case Constants.SCOUT_COLLISION:
                    if (!vision.scanning)
                    {
                        if (collided)
                        {
                            transform.rotation = transform.rotation * NewRotation();
                            vision.resetScanner();
                            collided = false;
                            vision.toggleScaning(true);
                        }
                        else
                        {
                            rb.AddRelativeForce(moveDir * scoutingSpeed * Time.deltaTime, ForceMode.VelocityChange);
                        }
                    }

                    break;

                    /* case Constants.SCOUT_COLLISION_PARALLEL:
                         randomScanTimer += Time.deltaTime;
                         if (randomScanTimer >= 2 / scanSpeed)
                         {
                             transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(-180, 180), transform.rotation.eulerAngles.z);
                             randomScanTimer = 0;
                         }
                         break;
                 case Constants.SCOUT_RANDOM_PARALLEL:
                     randomScanTimer += Time.deltaTime;
                     if (randomScanTimer >= 2 / scanSpeed)
                     {
                         transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(-180, 180), transform.rotation.eulerAngles.z);
                         randomScanTimer = 0;
                     }
                     break;*/
            }
            consumeEnergy(scoutingConsumption);
        }
    }

    public void scan()
    {
        vision.resetScanner();
        vision.toggleScaning(true);
        scout();
    }

    public void needToJump() {

        jumping = true;
        if (backJump)
        {
            rb.AddRelativeForce((transform.forward * -backJumpForce) + (transform.up * upBackJumpForce), ForceMode.Impulse);
            consumeEnergy(jumpEnergyConsumption*2);
        }
        else
        {
            rb.AddRelativeForce((transform.up * upJumpForce), ForceMode.Impulse);
            consumeEnergy(jumpEnergyConsumption);
        }
        jumping = false;
    }


    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }


    public void setVariables(TeamScript ts,int heirEnergy)
    {
        teamscript = ts;
        #region movement
          speed = ts.speed;
          movingEnergyConsumption = ts.movingEnergyConsumption;
          actionAfterFeast = ts.actionAfterFeast;
        #endregion

        #region foodManagement

          energy = heirEnergy;
          Array.Copy(ts.eatingEfficiency, eatingEfficiency,ts.eatingEfficiency.Length);
          idleEnergyConsumption = ts.idleEnergyConsumption;
          energyForEgg = ts.energyForEgg;
        #endregion


        #region scouting
        scoutingType = ts.scoutingType;
          scoutingSpeed = ts.scoutingSpeed;
          scoutingConsumption = ts.scoutingConsumption;
        #endregion

        #region scan
        scanType = ts.scanType;
        scanSpeed = ts.scanSpeed;
        vision.scanType = scanType;
        vision.scanSpeed = scanSpeed;
        vision.visionAngle = ts.visionAngle;
        vision.visionRadius = ts.visionRadius;
        #endregion


        #region jump
        jumpEnergyConsumption = ts.jumpEnergyConsumption;
        forwardJumpForce = ts.forwardJumpForce;
        upJumpForce = ts.upJumpForce;
        backJump = ts.backJump;
        backJumpForce = ts.backJumpForce;
        upBackJumpForce = ts.upBackJumpForce;
        #endregion


        #region Focus

        maxFocus = ts.maxFocus;

        #endregion

}

public void upateVariables(TeamScript ts)
    {
        teamscript = ts;
        #region movement
        speed = ts.speed;
        movingEnergyConsumption = ts.movingEnergyConsumption;
        actionAfterFeast = ts.actionAfterFeast;
        #endregion

        #region foodManagement
        Array.Copy(ts.eatingEfficiency, eatingEfficiency, ts.eatingEfficiency.Length);
        idleEnergyConsumption = ts.idleEnergyConsumption;
        energyForEgg = ts.energyForEgg;
        #endregion


        #region scouting
        scoutingType = ts.scoutingType;
        scoutingSpeed = ts.scoutingSpeed;
        scoutingConsumption = ts.scoutingConsumption;
        #endregion

        #region scan
        scanType = ts.scanType;
        scanSpeed = ts.scanSpeed;
        vision.scanType = scanType;
        vision.scanSpeed = scanSpeed;
        vision.visionAngle = ts.visionAngle;
        vision.visionRadius = ts.visionRadius;
        #endregion


        #region jump
        jumpEnergyConsumption = ts.jumpEnergyConsumption;
        forwardJumpForce = ts.forwardJumpForce;
        upJumpForce = ts.upJumpForce;
        backJump = ts.backJump;
        backJumpForce = ts.backJumpForce;
        upBackJumpForce = ts.upBackJumpForce;
        #endregion


        #region Focus

        maxFocus = ts.maxFocus;

        #endregion

    }
}
