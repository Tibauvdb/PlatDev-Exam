using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoYieldBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        //IEnumerator<int> powerReeks = PowerReeks(2);
        //while(powerReeks.MoveNext() )
        //{
        //    //i++;
        //    Debug.Log("Next" + powerReeks.Current);
        //}
        StartCoroutine(PowerReeks());

    }

    IEnumerator PowerReeks(/*int num*/)
    {
        /*  for (int i = 0; i < num; i++)
          {
              Debug.Log(i);
              yield return (i * num);
          }*/
        int i = 0;
        while (true)
        {
            /* yield return (i * i);
             i++;*/

            Debug.Log("Coroutine: " + (i * i));
            yield return new WaitForSeconds(2);
            i++;
        }
    }

    IEnumerator WaitForSeconds()
    {
        float t = Time.realtimeSinceStartup;

        while(t+2 > Time.realtimeSinceStartup)
        {
            yield return null;

        }

    }
}
