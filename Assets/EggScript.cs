using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    public int heirEnergy;
    public float hatchTime;
    public float timer = 0;
    bool timerRunning = true;
    TeamScript ts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRunning)
        timer += Time.deltaTime;
        if (timer >= hatchTime )
        {
            ts.hatchEgg(gameObject, heirEnergy);
            timerRunning = false;
            timer = 0;
        }
    }

    public void setVariables(int energy, float hatchtime,TeamScript teams)
    {
        heirEnergy = energy;
        hatchTime = hatchtime;
        timerRunning = true;
        ts = teams;
    }
}
