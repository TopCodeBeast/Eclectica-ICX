using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class screen_helper : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("__Internal")]
    private static extern void join_presentation(string channel_name, System.IntPtr data);
    [DllImport("__Internal")]
    private static extern void do_presentation();
    [DllImport("__Internal")]
    private static extern void stop_presenting();
    [DllImport("__Internal")]
    private static extern string initial_contract();
    Renderer m_Renderer;
    Texture2D tx;
    System.IntPtr data;
    byte[] logo_image;
    void Start()
    {
        data = Marshal.AllocHGlobal(1280 * 720 * 4);
        m_Renderer = GetComponent<Renderer>();
        tx = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        m_Renderer.material.mainTexture = tx;
        m_Renderer.material.mainTextureScale = new Vector2(-1f, 1f);
        //logo_image = Resources.Load<TextAsset>("nft-vision-hack").bytes; 
        

        join_presentation("test", data);
        //tx.LoadImage(logo_image);
    }

    public void present_screen(){
       
        do_presentation();
    }

    public void set_logo()
    {
       
    }


    public void stop_present()
    {

        stop_presenting();
    }
    // Update is called once per frame
    void Update()
    {
        tx.LoadRawTextureData(data, 1280 * 720 * 4);
        tx.Apply();
    }
}
