using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace StarterAssets
{
    public class SitonMe : MonoBehaviour
    {
        public GameObject player;
        public Vector3 location;
        public bool iswalking = false;
        private StarterAssetsInputs _input;
        public GameObject CinemachineCameraTarget;
        public bool lookat = false;
        public GameObject follow;
        public bool issitting = false;
        public bool sit = false;
        public bool occupi = false;
        public bool isoccupied = false;
        public bool stand;
        public GameObject hel;
        public GameObject sign;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("PlayerArmature");
            stand = true;
            location = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            _input = player.GetComponent<StarterAssetsInputs>();
            follow = GameObject.FindGameObjectWithTag("MainCamera");
            CinemachineCameraTarget = GameObject.Find("PlayerArmature/PlayerCameraRoot");
            CinemachineCameraTarget.GetComponent<Camera>().enabled = false;

        }


        public void occupy(string name)
        {
            if (!isoccupied)
            {
                sign = new GameObject("player_label");
                TextMesh tm = sign.AddComponent<TextMesh>();
                tm.text = name;
                tm.color = new Color(0.8f, 0.8f, 0.8f);
                tm.fontStyle = FontStyle.Bold;
                tm.alignment = TextAlignment.Center;
                tm.anchor = TextAnchor.MiddleCenter;
                tm.characterSize = 0.065f;
                tm.fontSize = 30;
                Debug.Log("ocuupied called");
                var loadedObject = Resources.Load("remotes");
                hel =(GameObject) Instantiate(loadedObject, new Vector3(location.x,2.4f,location.z), Quaternion.identity);
                hel.name = name;
                sign.name = "sign_" + name;
                sign.transform.position = hel.transform.position + Vector3.up * 1.6f;
                sign.transform.eulerAngles = new Vector3(sign.transform.eulerAngles.x, 180+270 + transform.eulerAngles.y, sign.transform.eulerAngles.z);
                hel.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, 270 + transform.eulerAngles.y, player.transform.eulerAngles.z);
                hel.transform.position += new Vector3(hel.transform.forward.x, 0f, hel.transform.forward.z) * 0.4f;
                occupi = false;
                isoccupied = true;
                transform.parent.GetComponent<Trif>().sitt[int.Parse(transform.name.Substring(5, 1))-1] = true;
               
                


            }
        }

        public void deloc()
        {
            isoccupied = false;
            transform.parent.GetComponent<Trif>().sitt[int.Parse(transform.name.Substring(5, 1)) - 1] = false;
            Destroy(hel);
            Destroy(sign);
        }


        // Update is called once per frame
        void Update()
        {
           

            if (sit==true && issitting == false && stand == true)
            {
                iswalking = true;
                issitting = true;
                lookat = false;
                player.GetComponent<CreateConnection>().sendData(transform.parent.name, transform.name);
            }
            if (stand == false)
            {
                CinemachineCameraTarget.GetComponent<Camera>().enabled = false;
                follow.GetComponent<Camera>().enabled = true;
                Debug.Log("i am going to stand");
                player.GetComponent<Animator>().SetTrigger("stand");
                player.GetComponent<CreateConnection>().onStand();
                player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, player.transform.eulerAngles.z);
                player.GetComponent<CharacterController>().center = new Vector3(0f, 0.9f, 0f);
                issitting = false;
                sit = false;
                stand = true;

                
            }

            if (iswalking)
            {
                if (lookat == false)
                {
                    player.transform.LookAt(new Vector3(transform.position.x,2.3f,transform.position.y));

                    follow.transform.LookAt(player.transform);

                    lookat = true;
                }

                Vector3 targetDir;
              
                player.transform.position = Vector3.MoveTowards(player.transform.position,location, 2f*Time.deltaTime);
                player.GetComponent<ThirdPersonController>().enabled = false;
                player.GetComponent<Animator>().SetFloat("Speed", 2f);

                if (Vector3.Distance(player.transform.position,transform.position) < 0.9)
                {
                    Debug.Log("here");
                    iswalking = false;
                 

                    player.GetComponent<Animator>().SetTrigger("sit");

                    player.transform.position = location;
                    player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,270+ transform.eulerAngles.y, player.transform.eulerAngles.z);
                    player.transform.position += new Vector3(player.transform.forward.x,0f, player.transform.forward.z) * 0.4f;
                    issitting = true;
                    Invoke("startCamera", 3);
                    player.GetComponent<ThirdPersonController>().tablee = transform.parent.name;
                    player.GetComponent<ThirdPersonController>().chaire = transform.name;
                 
                    player.GetComponent<CharacterController>().center = new Vector3(0f,0.7f,0f);






                }




            }


        }

        public void startCamera()
        {
          
            player.GetComponent<ThirdPersonController>().enabled = true;
            Debug.Log("done");
            follow.GetComponent<Camera>().enabled = false;
         
            CinemachineCameraTarget.GetComponent<Camera>().enabled = true;
        }
    }
}