using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/*
 * https://github.com/Fulviuus/unity-network-client
 * 
 * E:\Game for Software
 */

public class networkSocket : MonoBehaviour, IPointerDownHandler
{


    public String hostTCP = "localhost";
    public Int32 port = 8000;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

    public Text MessageText;
    public GameObject GamePosition;
    public GameObject GameActor;
    public GameObject TootTips;
    private Vector3 velocity = Vector3.zero;

    private enum _ind
    {
        message = 0,
        actor = 1,
        position = 2,
        intent = 3
    };

    List<string> received_actor = new List<string>();
    List<string> received_position = new List<string>();
    List<string> received_intent = new List<string>();
    List<string> data = new List<string>();

    void Update()
    {
        string received_data = readSocket();

        if (received_data != "")
        {
            // print it in the log for now
           // Debug.Log(received_data);

            string received_message = "";
            received_actor.Clear();
            received_position.Clear();
            received_intent.Clear();
            data.Clear();
            
            //message value#Actor value, ... #position value, ...#intent value, ...
            Debug.Log(received_data);
            data.AddRange(received_data.Split('#'));
            // her wer is the p robleme
            Debug.Log(" Count data "+data.Count);
            //PrintArray(data);
            if (data.Count == 4)
            {
                received_message = data[(int)_ind.message];
                received_actor.AddRange(data[(int)_ind.actor].Split(','));
                received_position.AddRange(data[(int)_ind.position].Split(','));
                received_intent.AddRange(data[(int)_ind.intent].Split(','));
            }
            /*
            PrintArray(received_actor);
            PrintArray(received_position);
            PrintArray(received_intent);
             */

            if (received_message.Equals("NaN"))
                received_message = "";

            if (!received_message.Equals(""))
            {
                TootTips.GetComponent<BotTooltipHandle>().Activate(received_message);
                TootTips.GetComponent<BotTooltipHandle>().closeAfter = 3f;
            }

            Transform target = null;
            if (received_position.Count != 0)
            {
                if (!received_position[0].Equals("NaN"))
                {
                    target = GamePosition.transform.FindChild("Pos_" + received_position[0].Trim());
                }
            }
            if (target != null)
            {
                Debug.Log("Position to go is " + target.position);
                if (received_actor.Count != 0)
                {
                    for (int i = 0; i < received_actor.Count; i++)
                    {
                        if (!received_actor[i].Equals("NaN"))
                        {
                            GameObject source = GameActor.transform.FindChild(received_actor[i]).gameObject;
                            if (received_intent.Count != 0)
                            {
                                if (!received_intent[0].Equals("NaN"))
                                {
                                    if (received_intent[0].Equals("Moving"))
                                    {
                                        source.GetComponent<ActorAction>().Move();
                                        source.GetComponent<ActorAction>().GoTo(target.position);
                                    }
                                }// default moving just for test 
                                else
                                {
                                    source.GetComponent<ActorAction>().Move();
                                    source.GetComponent<ActorAction>().GoTo(target.position);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                Debug.Log("Target Position not find");
                TootTips.GetComponent<BotTooltipHandle>().Activate("Target Position not find");
                TootTips.GetComponent<BotTooltipHandle>().closeAfter = 1f;
            }

        }
    }


    void Awake()
    {
        setupSocket();
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    public void setupSocket()
    {
        try
        {
            tcp_socket = new TcpClient(hostTCP, port);

            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            socket_ready = true;
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;

        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }

    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (MessageText.text != "")
        {
            string msg = "";
            if (BotSelected.CurrentItemCount() > 0)
            {
                //foreach (GameObject botref in BotSelected.GetCurrentAll())
                //{
                //    msg += botref.name + " and ";
                //}
                //  Debug.Log(msg + MessageText.text);
                GameObject[] botrefs = BotSelected.GetCurrentAll().ToArray();
                for (int i = 0; i < botrefs.Length; i++)
                {
                    msg += botrefs[i].name + " ";
                    if (i < botrefs.Length - 1)
                        msg += " and ";
                }
                writeSocket(msg + MessageText.text);
                TootTips.GetComponent<BotTooltipHandle>().Activate("Message send " + msg + MessageText.text);
            }
            else {
                TootTips.GetComponent<BotTooltipHandle>().Activate("Select at less one actor");
                TootTips.GetComponent<BotTooltipHandle>().closeAfter = 2f;
            
            }
        }
    }
    private void PrintArray(List<string> ar)
    {

        for (int i = 0; i < ar.Count; i++)
            Debug.Log(ar[i] + " ");
        Debug.Log(" ");
    }
}
