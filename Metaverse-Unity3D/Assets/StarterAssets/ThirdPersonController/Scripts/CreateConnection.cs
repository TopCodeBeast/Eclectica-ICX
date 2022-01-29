using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using BestHTTP.SocketIO;
using BestHTTP;
using BestHTTP.SocketIO.JsonEncoders;
using BestHTTP.JSON;
using System.ComponentModel;

using System.Runtime.InteropServices;

namespace StarterAssets
{

    public class CreateConnection : MonoBehaviour
    {
        public GameObject stage;
        public static remoteUserList remotelist = new remoteUserList();
        SocketManager manager = new SocketManager(new Uri("https://eclectica-meta.herokuapp.com/socket.io/"));

        private StarterAssetsInputs _input;
        private remoteuser remote;
        public string contract;
        [DllImport("__Internal")]
        private static extern string initial_contract();
        [DllImport("__Internal")]
        private static extern string get_nfts();

        // Start is called before the first frame update
        void Start()
        {
            string contract = initial_contract();
            Debug.Log(contract);
            StartCoroutine(GetRequest("https://participatehack.herokuapp.com/api/v1/nft?contract="+contract));
            Invoke("getnft", 10f);
            var root = manager.Socket;

            manager.Open();
            _input = GetComponent<StarterAssetsInputs>();
            manager.Socket.On("connect", onConnect);
            manager.Socket.On("updateUser", OnUpdate);
            manager.Socket.On("newUser", newUser);
            manager.Socket.On("initUser", onInit);
            manager.Socket.On("leaveUser", onLeave);
            manager.Socket.On("removeUser", onRemove);
            manager.Socket.On("teamtable", teamtable);
            manager.Socket.On("present", presento);
            manager.Socket.On("presentstop", presentstop);
          




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
               
                nftlist ab= JsonUtility.FromJson<nftlist>("{\"nfts\":"+ uwr.downloadHandler.text + "}");
                //GameObject.Find("MainFrame").GetComponent<framehandler>().initiate(ab.nfts);


            }
        }
        public void getnft()
        {
            string nfts = get_nfts();
            nftlist ab = JsonUtility.FromJson<nftlist>(nfts);
            GameObject.Find("MainFrame").GetComponent<framehandler>().initiate(ab.nfts);
        }

       
        public void presento(Socket socket, Packet packet, params object[] args)
        {
            string name = args[0] as string;
            GameObject.Find("hallroomfinal").transform.Find("stage").GetComponent<Stage>().onPresent();
         }

        public void presentstop(Socket socket, Packet packet, params object[] args)
        {
            string name = args[0] as string;
            GameObject.Find("hallroomfinal").transform.Find("stage").GetComponent<Stage>().onStopPresent();
        }

        public void presentation()
        {
            this.GetComponent<ThirdPersonController>().amipresenting = true;
            manager.Socket.Emit("onpresent", "{}");

        }

        public void stopPresent()
        {
            manager.Socket.Emit("onstoppresent", "{}");
        }

        public void sendData(string table,string chair)
        {

            manager.Socket.Emit("onsit", "{\"table\":\""+table+"\",\"chair\":\""+chair+"\"}");

        }
    
        private string CreateRandomString(int stringLength = 5)
        {
            int _stringLength = stringLength - 1;
            string randomString = "";
            string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            for (int i = 0; i <= _stringLength; i++)
            {
                randomString = randomString + characters[UnityEngine.Random.Range(0, characters.Length)];
            }
            return randomString;
        }
        public void onStand()
        {

            manager.Socket.Emit("onstand", "{}");

        }


        void teamtable(Socket socket,Packet packet,params object[] args)
        {

            GetComponent<ThirdPersonController>().tablee = args[0] as string;
        }


        void onRemove(Socket socket, Packet packet, params object[] args)
        {

            Dictionary<string, object> ab = args[0] as Dictionary<string, object>;
            string username = ab["username"] as string;
            remotelist.users[username].myObject.GetComponent<SitonMe>().deloc();
            Destroy(GameObject.Find(username));
            Debug.Log("remove");
            remotelist.users.Remove(username);
        }

        void OnUpdate(Socket socket, Packet packet, params object[] args)
        {

            Dictionary<string, object> ab = args[0] as Dictionary<string, object>;
            string username = ab["username"] as string;
            if (!remotelist.users.ContainsKey(username))
            {
                Debug.Log("the following username was not init " + username);
                JsonDecoder.userdata(ab, remotelist);
            }
            JsonDecoder.updateData(ab, remotelist);

        }
        void onConnect(Socket socket, Packet packet, params object[] args)
        {
            // Debug.Log(StateController.usernameo);
            GameObject.Find("hallroomfinal").transform.Find("stage").GetComponent<Stage>().onStopPresent();
            Debug.Log("i am onconnect and i am called");
            Dictionary<string, string> remotee= new Dictionary<string, string>();
            remotee.Add("username", "hola");
            remotee.Add("room", "eroom");
            Debug.Log(JsonUtility.ToJson(remotee));
            socket.Emit("joinroom", "{\"username\":\""+CreateRandomString()+"\",\"room\":\""+contract+"\"}");
            socket.Emit("jointeam", "{\"team\":\"chapo\"}");
        }
        void onInit(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log("On init Called");
            Debug.Log(JsonUtility.ToJson(args[0]));
            Dictionary<string, object> ab = args[0] as Dictionary<string, object>;
            string present = ab["present"] as string;
            if (present == "1")
            {
                GameObject.Find("hallroomfinal").transform.Find("stage").GetComponent<Stage>().onPresent();
            }
            JsonDecoder.usersdata(ab, remotelist);
        }
        void newUser(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log("new user ccalled");
            Dictionary<string, object> ab = args[0] as Dictionary<string, object>;
            string username = ab["username"] as string;
            if (!remotelist.users.ContainsKey(username))
            {
                Debug.Log("the following username was not init " + username);
                JsonDecoder.userdata(ab, remotelist);
            }



        }
        void onLeave(Socket socket, Packet packet, params object[] args)
        {
            Dictionary<string, object> ab = args[0] as Dictionary<string, object>;
            
            socket.Emit("move", JsonUtility.ToJson(remotelist.users));
            Debug.Log(remotelist.users["hello1"]);
            Debug.Log(remotelist.users.Count);

        }
        void DumpToConsole(object obj)
        {
            var output = JsonUtility.ToJson(obj, true);
            Debug.Log(output);
        }




        // Update is called once per frame
        void Update()
        {

        }
    }
}
