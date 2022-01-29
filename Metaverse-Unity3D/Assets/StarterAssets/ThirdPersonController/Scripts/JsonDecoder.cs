using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;


namespace StarterAssets
{
   
    public class remoteuser : MonoBehaviour
    {
        public string username;
        public GameObject usergame
        { get; set; }

        public string table;

        public string chair;

      
        public GameObject myObject;



        public remoteuser(string name,string table,
                 string chair)
        {
            Debug.Log("why am i called");
            this.username = name;
            this.table = table;
            this.chair = chair;
          
            myObject = GameObject.Find(table+"/"+chair);
            myObject.GetComponent<SitonMe>().occupy(name);
        }
        public remoteuser(string name, string table,
                 string chair,bool self)
        {
            this.username = name;
            this.table = table;
            this.chair = chair;


        }


        public void update(string table,
                 string chair, bool self)
        {
            this.username = name;
            this.table = table;
            this.chair = chair;

        }
        public void update(string table,
                 string chair)
        {
            this.username = name;
            this.table = table;
            this.chair = chair;

        }


    }
    
    public class remoteUserList
    {
        public Dictionary<string, remoteuser> users;


        public remoteUserList()
        {
            this.users = new Dictionary<string, remoteuser>();
        }
        public void add(string username, remoteuser remote)
        {
            this.users.Add(username, remote);
        }
    }

    [Serializable]
    public class nft
    {
        public string uri;
        public string tokenid;
    }
    [Serializable]
    public class nftlist
    {
        public nft[] nfts;
    }

    [Serializable]
    public class dataa
    {
        public string image;
    }
    [Serializable]
    public class data
    {
        public dataa dat;
    }


    public class JsonDecoder : MonoBehaviour
    {


        public static void userdata(Dictionary<string, object> user, remoteUserList remotelist)
        {

            string username = user["username"] as string;
            Debug.Log(username);

            string table = user["table"] as string;
            string chair = user["chair"] as string;
            
            remoteuser remote = new remoteuser(username, table, chair);
            remotelist.add(remote.username, remote);
        }
        public static void usersdata(Dictionary<string, object> users, remoteUserList remote)
        {
            List<System.Object> userlist = users["users"] as List<System.Object>;
            Debug.Log("this is main " + users["users"]);
            foreach (System.Object i in userlist)
            {
                Debug.Log("times");
                Dictionary<string, object> ui = i as Dictionary<string, object>;
                userdata(ui, remote);

            }

        }
        public static void updateData(Dictionary<string, object> user, remoteUserList remotelist)
        {

            string username = user["username"] as string;
            string table = user["table"] as string;
            string chair = user["chair"] as string;
            remoteuser updateuser = remotelist.users[username];
            updateuser.update(table,chair);
        }

        public static void removeuser(Dictionary<string, object> user, remoteUserList remotelist)
        {
            string username = user["username"] as string;
            remotelist.users.Remove(username);

        }

    }
}