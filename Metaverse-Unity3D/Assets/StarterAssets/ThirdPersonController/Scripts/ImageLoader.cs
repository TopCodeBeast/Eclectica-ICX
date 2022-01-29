    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    public string url = "https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg";
    public Renderer thisRenderer;
    public int price;
    public string id;
    public string signature;
  

    // automatically called when game started
    void Start()
    {
        thisRenderer = this.GetComponent<Renderer>();
        StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently

        // the following will be called even before the load finished
        thisRenderer.material.color = Color.red;
    }


    public void updateImage(nft url)

    {
        
        this.url = url.uri;
        
        this.id = url.tokenid;
        
       
        string[] authorsList = this.url.Split('/');
        this.url = "https://ipfs.io/ipfs/" + authorsList[2] + "/" + authorsList[3];
    
      

        StartCoroutine(GetRequest(this.url));

    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()
    {
        
        WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        
        thisRenderer.material.color = Color.white;              // set white
        thisRenderer.material.mainTexture = wwwLoader.texture;  // set loaded image
    }
    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {

            data ab = JsonUtility.FromJson<data>("{\"dat\":" + uwr.downloadHandler.text + "}");
            string[] authorsList = ab.dat.image.Split('/');
            this.url = "https://ipfs.io/ipfs/" + authorsList[2] + "/" + authorsList[3];
            StartCoroutine(LoadFromLikeCoroutine());

        }
    }

   

}