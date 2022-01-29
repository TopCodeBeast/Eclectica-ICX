using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour

{
    public GameObject hel;
    public bool isoccupy;

    void start()
    {
        isoccupy = false;

    }
   
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("elfsdjf");   
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            
        }
    }

    public void onPresent()
    {
        var loadedObject = Resources.Load("Presenter");
        hel = (GameObject)Instantiate(loadedObject, new Vector3(9.72f, 2.25f, -4.24f), Quaternion.identity);

    }
    public void onStopPresent()
    {
        Destroy(hel);
    }
}
