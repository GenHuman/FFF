using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    public float visionAngle = 30;
    public float visionRadius = 3;


    public GameObject parent;
    public Naviagtor navigator;

    public bool debug = false;

    public float distanceToClosest = 100f;


    #region scan
    public bool scanning = false;
    public int scanType = 0;
    public float scanSpeed = 5f;
    private float sprinklerOriginalAngle = 30f;
    private float sprinklerAngle = 30f;
    private float randomScanTimer = 0;
    private int randomCounter = 0;
    private bool scanningLeft = false;
    public bool scanEnded = false;
    float startingAngle = -200f;
    #endregion

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionRadius), Color.blue, Time.deltaTime);
            Debug.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionRadius), Color.blue, Time.deltaTime);
        }
        if (scanning)
        {
            scan();
        }

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            //Debug.Log("FoodDetected on enter");
            Vector3 v = new Vector3(other.transform.position.x, 0, other.transform.position.z);

            if (debug)
            {

                //Debug.Log(Vector3.Angle(transform.forward, v));
                Debug.DrawLine(transform.position, other.transform.position, Color.red, Time.deltaTime);

            }

            if (Vector3.Angle(transform.forward, v) < visionAngle / 2f)
            {
                //Debug.Log(Vector3.Angle(transform.forward, v)+ " < " + visionAngle / 2f);
                if (navigator.objective != null)
                {
                    /*if (!navigator.AlmostEqual(navigator.objective.transform.position, other.transform.position, 0.1f))
                    {
                        navigator.NewObjective(other.gameObject);
                    }
                }
                else
                {
                   // Debug.Log("FoodDetected on enter in angle");
                    navigator.NewObjective(other.gameObject);
                }
            }
        }
    }*/

    public bool toggleScaning()
    {
        scanning = !scanning;
        if (scanning) startingAngle = transform.rotation.y;


        return scanning;
    }
    public bool toggleScaning(bool x)
    {
        scanning = x;
        if (scanning) startingAngle = transform.rotation.y;


        return scanning;
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            // Debug.Log("FoodDetected on stay");
            Vector3 v = new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z);
            Vector3 forw = Vector3.forward + transform.position;
            if (debug)
            {


                //Debug.Log(Vector3.Angle(transform.forward, v));
                Debug.DrawLine(Vector3.zero, transform.forward, Color.red, Time.deltaTime);
                Debug.DrawLine(Vector3.zero, v, Color.red, Time.deltaTime);
                Debug.DrawLine(transform.position, other.transform.position, Color.red, Time.deltaTime);

            }

            if (Vector3.Angle(transform.forward, v) < visionAngle / 2f)
            {
                //Debug.Log(Vector3.Angle(transform.forward, v)+ " < " + visionAngle / 2f);
                if (navigator.objective != null)
                {
                    /*if (!navigator.AlmostEqual(navigator.objective.transform.position, other.transform.position, 0.1f))
                    {
                        navigator.NewObjective(other.gameObject);
                    }*/
                }
                else
                {
                    // Debug.Log("FoodDetected on enter in angle");
                    scanning = false;
                    randomScanTimer = 0;
                    sprinklerAngle = 30;

                    if (other.transform.position.y > transform.position.y)
                    {
                        navigator.NewObjective(other.gameObject,true);

                        //navigator.needToJump();
                    }
                    else
                    {
                        navigator.NewObjective(other.gameObject, false);
                    }
                   
                    
                    Debug.Log("objc y: "+other.transform.position.y+ "this y: " + transform.position.y);

                    resetRotation();
                }
            }
        }
    }

    private void scan()
    {
        if (!scanEnded)
        {
            switch (scanType)
            {
                case -1:
                    Debug.Log("AHHH");
                    randomScanTimer += Time.deltaTime;
                    if (randomScanTimer >= 2f)
                    {
                        scanEnded = true;
                    }

                    break;
                case Constants.SCAN_LEFT:
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - Time.deltaTime * scanSpeed, transform.rotation.eulerAngles.z);
                    if (startingAngle - 0.1 < transform.rotation.y && transform.rotation.y < startingAngle + 0.1)
                    {
                        scanEnded = true;
                        startingAngle = -200;
                    }
                    break;
                case Constants.SCAN_RIGHT:
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Time.deltaTime * scanSpeed, transform.rotation.eulerAngles.z);
                    if (startingAngle - 0.1 < transform.rotation.y && transform.rotation.y < startingAngle + 0.1)
                    {
                        scanEnded = true;
                        startingAngle = -200;
                    }
                    break;
                case Constants.SCAN_SPRINKLER:
                    float rotation = transform.rotation.eulerAngles.y;
                    if (transform.rotation.eulerAngles.y > 180)
                    {
                        rotation -= 360;
                    }
                    if (scanningLeft)
                    {

                        if (rotation > -sprinklerAngle)
                        {
                            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - Time.deltaTime * scanSpeed, transform.rotation.eulerAngles.z);
                        }
                        else
                        {
                            scanningLeft = false;
                            if (sprinklerAngle < 180)
                            {
                                sprinklerAngle += 15;
                            }
                            else
                            {
                                scanEnded = true;
                            }


                        }
                    }
                    else
                    {
                        if (rotation < sprinklerAngle)
                        {
                            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Time.deltaTime * scanSpeed, transform.rotation.eulerAngles.z);
                        }
                        else
                        {

                            scanningLeft = true;
                        }
                    }
                    break;

                case Constants.SCAN_RANDOM:
                    randomScanTimer += Time.deltaTime;
                    if (randomScanTimer >= 2 / scanSpeed)
                    {
                        if (randomCounter < (int)(360f / visionAngle))
                        {
                            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Random.Range(-180, 180), transform.rotation.eulerAngles.z);
                            randomScanTimer = 0;
                            randomCounter++;
                        }
                        else
                        {
                            randomScanTimer = 0;
                            randomCounter = 0;
                            resetRotation();
                            scanEnded = true;
                        }
                    }
                    break;
            }
        }
        else
        {
            scanning = false;
        }
    }


    public void resetRotation()
    {
        transform.rotation = parent.transform.rotation;
    }

    public void resetScanner()
    {
        scanning = false;
        sprinklerOriginalAngle = 30f;
        sprinklerAngle = 30f;
        randomScanTimer = 0;
        randomCounter = 0;
        scanningLeft = false;
        scanEnded = false;
        startingAngle = -200f;
    }



}
