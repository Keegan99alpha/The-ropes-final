using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int count;


    void Start()
    {
        count = 0;
    }

    public void addItem()
    {
        count++;
    }

    public void useItem()
    {
        count--;
    }

    void Update()
    {
        
    }
}
