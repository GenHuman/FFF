using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    FoodManager foodManager;
    int type = 0;
    float energyAmount = 100f;
    float energyIncease = 5f;

    private void Start()
    {
        foodManager = FoodManager.Instance;
    }

    void Update()
    {
        transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Time.deltaTime * 50, transform.rotation.eulerAngles.z);

        energyAmount += energyIncease * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Agent"))
        {
            Naviagtor n = other.GetComponent<Naviagtor>();
            n.digest(energyAmount,type);
            foodManager.foodEaten(gameObject);
        }
    }
}
