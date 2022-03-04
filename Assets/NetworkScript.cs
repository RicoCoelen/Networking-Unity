using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://forum.unity.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/

public class NetworkScript : MonoBehaviour {
    private UdpConnection connection;
    public bool isServer = true;
    int myID;

    bool upKey, downKey, leftKey, rightKey, spaceBar;

    private int lastBullet = -1;
    private int bulletCache = 100;
    private GameObject[] bullets;
    [SerializeField]
    private GameObject bullet;

    public PlayerScript[] players = new PlayerScript[2];
    Sendable sendData = new Sendable();

    void Start() {
        
        string sendIp = "127.0.0.1";
        
        int sendPort, receivePort;
        if (isServer) {
            sendPort = 8881;
            receivePort = 11000;
            myID = 0;
        } else {
            sendPort = 11000;
            receivePort = 8881;
            myID = 1;
        }

        connection = new UdpConnection();
        connection.StartConnection(sendIp, sendPort, receivePort);
    }
 
    void FixedUpdate() {
        //Check input...
        if (upKey) {
            players[myID].transform.Translate(0, .1f, 0);
            UpdatePositions(myID);
        }
        if (downKey)
        {
            players[myID].transform.Translate(0, -.1f, 0);
            UpdatePositions(myID);
        }
        if (leftKey)
        {
            players[myID].transform.Translate(-.1f, 0, 0);
            UpdatePositions(myID);
        }
        if (rightKey)
        {
            players[myID].transform.Translate(.1f, 0, 0);
            UpdatePositions(myID);
        }
        if (spaceBar)
        {
            var go = bullets[GetNextBullet()];
            go.transform.position = players[myID].transform.position;
            Debug.Log(myID);
            if (myID == 0)
            {
                go.GetComponent<BulletScript>().velocity = (players[1].transform.position - players[myID].transform.position).normalized;
            }
            else
            {
                go.GetComponent<BulletScript>().velocity = (players[0].transform.position - players[myID].transform.position).normalized;
            }
            UpdatePositions(myID);
        }

        //network stuff:
        CheckIncomingMessages();
            
    }

    public void Update()
    {
        //handling keyboard (in Update, because FixedUpdate isnt meant for that(!))
        if (Input.GetKeyDown("w")) upKey = true;       
        if (Input.GetKeyUp("w")) upKey = false;
        if (Input.GetKeyDown("s")) downKey = true;
        if (Input.GetKeyUp("s")) downKey = false;
        if (Input.GetKeyDown("a")) leftKey = true;
        if (Input.GetKeyUp("a")) leftKey = false;
        if (Input.GetKeyDown("d")) rightKey = true;
        if (Input.GetKeyUp("d")) rightKey = false;
        if (Input.GetKeyDown("space")) spaceBar = true;
        if (Input.GetKeyUp("space")) spaceBar = false;
    }

    void CheckIncomingMessages()
    {
        //Do the networkStuff:
        string[] o = connection.getMessages();
        if (o.Length > 0)
        {
            foreach (var json in o)
            {
                JsonUtility.FromJsonOverwrite(json, sendData);
                //now, check its id..
                int i = sendData.id;
                //..and update the right player_entity:
                players[i].transform.position = new Vector3(sendData.x, sendData.y, 0);
            }

            for (int j = 0; j < bulletCache; j++)
            {
                bullets[j].transform.position = sendData.bulletPos[j];
                bullets[j].GetComponent<BulletScript>().velocity = sendData.bulletVel[j];
            }
        }

    }
    public void UpdatePositions(int id)
    {
        //update sendData-object
        sendData.id = id;
        sendData.x = players[id].transform.position.x;
        sendData.y = players[id].transform.position.y;

        Vector3[] bulletPos = new Vector3[bulletCache];
        Vector3[] bulletVel = new Vector3[bulletCache];

        for (int i = 0; i<bulletCache; i++)
        {
            bulletPos[i] = bullets[i].transform.position;
            bulletVel[i] = bullets[i].GetComponent<BulletScript>().velocity;
        }

        sendData.bulletPos = bulletPos;
        sendData.bulletVel = bulletVel;
        sendData.framenumber = Time.frameCount;
        //sendData.time = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(sendData); //Convert to String
        Debug.Log(json);
        
        connection.Send(json); //send the string

    }

    void Awake()
    {
        bullets = new GameObject[bulletCache];
        for (int i = 0; i<bullets.Length; i++){
            bullets[i] = Instantiate(bullet, new Vector3(0,0,0), Quaternion.identity);
        }
    }

    int GetNextBullet()
    {
        lastBullet += 1;
        if (lastBullet > bulletCache - 1){
            lastBullet = 0;//reset the loop
        }
        return lastBullet;
    }

    void OnDestroy() {
        connection.Stop();
    }
}

