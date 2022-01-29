using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Aibot : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void aibot_enable();

    [DllImport("__Internal")]
    private static extern void aibot_disable();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hey, I am bot!");
            aibot_enable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            aibot_disable();
        }
    }
}
