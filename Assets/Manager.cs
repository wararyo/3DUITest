using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class Manager : MonoBehaviour
{
    public string host = "192.168.11.22";
    public int port = 3333;
    UdpClient client;
    // Start is called before the first frame update
    void Start()
    {
        client = new UdpClient();
        client.Connect(host, port);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendToClient(string data){
        byte[] dgram = Encoding.UTF8.GetBytes(data);
        client.Send(dgram, dgram.Length);
    }
    public void SendToClient(byte[] data){
        client.Send(data, data.Length);
    }

    void OnApplicationQuit()
    {
        client.Close();
    }
}
