using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodManager : MonoBehaviour
{
    #region Singleton
    private static FoodManager _instance;
    public static FoodManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject prefab;
    public Transform activeFoodParent;
    public Transform inactiveFoodParent;

    public int maxFood = 100;

    public float foodRate = 1f;
    public float foodTimer = 0;

    public float XLowerLimit = 2f;
    public float YLowerLimit = 2f;
    public float XUpperLimit = 34f;
    public float YUpperLimit = 34f;

    /*public float XLowerLimit = 11f;
    public float YLowerLimit = 11f;
    public float XUpperLimit = 27f;
    public float YUpperLimit = 27f;*/

    public bool foodTimerActive = true;


    public int activeFood = 0;

    public Queue<GameObject> inactiveFoodList = new Queue<GameObject>();
    public List<GameObject> activeFoodList = new List<GameObject>();

    void Start()
    {
        //spawnFeast(0, 20);
        for(int i = 0; i<maxFood; i++)
        {
            GameObject go = Instantiate(prefab, new Vector3(0, 100, 0), prefab.transform.rotation, inactiveFoodParent.transform);
            go.SetActive(false);
            go.transform.parent = inactiveFoodParent;
            inactiveFoodList.Enqueue(go);
        }
    }


    void Update()
    {
        if (foodTimerActive)
        {
            foodTimer += Time.deltaTime;
        }
        if(foodTimer>= foodRate)
        {
            spawnFood(0);
            foodTimer = 0;
            if(activeFood>= maxFood)
            {
                foodTimerActive = false;
                foodTimer = 0;
            }
        }
    }




    public void foodEaten(GameObject food)
    {
        activeFood--;
        activeFoodList.Remove(food);
        inactiveFoodList.Enqueue(food);
        food.transform.parent = inactiveFoodParent;
        foodTimerActive = true;
        food.SetActive(false);
    }

    public bool spawnFood(int seed)
    {
        //System.Random rand = new System.Random(seed);
        float x = 2;
        //float y = 5;
        float z = 2;
        
        int layerMask = 1 << 8;
        x = UnityEngine.Random.Range(XLowerLimit, XUpperLimit);
        z = UnityEngine.Random.Range(YLowerLimit, YUpperLimit);
        RaycastHit rh = new RaycastHit();

        if (Physics.Raycast(new Vector3(x, 10, z), Vector3.down, out rh, 20f, layerMask))
        {
            GameObject food = inactiveFoodList.Dequeue();
            food.transform.position = new Vector3(x, rh.point.y + 1, z);
            food.transform.parent = activeFoodParent;
            food.SetActive(true);
            activeFoodList.Add(food);
            activeFood++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void spawnFeast(int seed, int spawnAmount)
    {
        System.Random rand = new System.Random(seed);
        float x = 2;
        //float y = 5;
        float z = 2;
       
        int layerMask = 1 << 8;

        RaycastHit rh = new RaycastHit();
        for (int i = 0; i < spawnAmount; i++)
        {
           
            x = (float)(rand.NextDouble() * XUpperLimit + XLowerLimit);
            z = (float)(rand.NextDouble() * YUpperLimit + YLowerLimit);



            if (Physics.Raycast(new Vector3(x, 10, z), Vector3.down, out rh, 20f, layerMask))
            {
                GameObject food = inactiveFoodList.Dequeue();
            food.transform.position = new Vector3(x, rh.point.y + 1, z);
            food.transform.parent = activeFoodParent;
            food.SetActive(true);
            activeFoodList.Add(food);
            activeFood++;
                activeFood++;
            }
            else
            {
                i--;
            }
        }
    }

}
