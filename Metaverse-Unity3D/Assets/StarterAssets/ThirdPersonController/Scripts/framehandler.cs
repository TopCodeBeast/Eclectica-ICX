using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class framehandler : MonoBehaviour
{
    int i = 0;
    public nft[] nfto;
    // Start is called before the first frame update
    void Start()
    {
        
     

    }

    public void initiate(nft[] nftt)
    {
        Debug.Log("i am nft called");
        this.nfto = nftt;
        InvokeRepeating("updateFrames", 2.0f, 600.3f);
    }

    void updateFrames()
    {
       
        foreach (Transform child in transform)
        {
            Debug.Log(child.GetChild(1).gameObject.GetComponent<ImageLoader>());
            child.GetChild(1).gameObject.GetComponent<ImageLoader>().updateImage(nfto[i]);

            i = (i+1)%nfto.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
