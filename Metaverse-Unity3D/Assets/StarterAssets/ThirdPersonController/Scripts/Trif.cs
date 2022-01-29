using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using StarterAssets;

public class Trif : MonoBehaviour
{
    public bool[] sitt;
    [DllImport("__Internal")]
    private static extern void createChannel(string obj_name, string channel_name);
    [DllImport("__Internal")]
    private static extern void leaveChannel(string obj_name);

    string address;
    string channel_name;
    void start()
    {
        sitt[0] = false;
        sitt[1] = false;
        sitt[2] = false;
        sitt[3] = false;
        sitt[4] = false;
        
       

    }
    private void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "Player")
        {

            address = collider.gameObject.GetComponent<CreateConnection>().contract;
            collider.gameObject.GetComponent<ThirdPersonController>().tablee = this.name; 
            channel_name = address + this.name;
            Debug.Log("hetansh: IN: " + channel_name);
            createChannel(channel_name, channel_name);
        }    
        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
           
            channel_name = address + this.name;
            Debug.Log("hetansh: OUT: " + channel_name);
            leaveChannel(channel_name);
        }
    }
}
