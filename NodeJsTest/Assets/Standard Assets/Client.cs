using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
 
public class Client : MonoBehaviour 
{
    private Socket m_Socket;
    
    public string iPAdress = "127.0.0.1";
    public const int kPort = 1337;
    private float timer = 0.0f;
	
    private int SenddataLength;                     // Send Data Length. (byte)
    private int ReceivedataLength;                     // Receive Data Length. (byte)

    private byte[] Sendbyte;                        // Data encoding to send. ( to Change bytes)
    private byte[] Receivebyte = new byte[2000];    // Receive data by this array to save.
    private string ReceiveString;                     // Receive bytes to Change string. 
    
    void Awake ()
    {
        //=======================================================
        // Socket create.
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
        m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);
 
         //=======================================================
        // Socket connect.
          try
        {
            IPAddress ipAddr = System.Net.IPAddress.Parse(iPAdress); 
            IPEndPoint ipEndPoint = new System.Net.IPEndPoint(ipAddr, kPort);
            m_Socket.Connect(ipEndPoint);
        }
        catch (SocketException SCE)
        {
            Debug.Log("Socket connect error! : " + SCE.ToString() ); 
            return;
        }
        
        //=======================================================
        // Send data write.
        StringBuilder sb = new StringBuilder(); // String Builder Create
        sb.Append("Test 1 - By Mac!!");
        sb.Append("Test 2 - By Mac!!");
        
        try
         {
             //=======================================================
            // Send.
            SenddataLength = Encoding.Default.GetByteCount(sb.ToString());
            Sendbyte = Encoding.Default.GetBytes(sb.ToString());
            m_Socket.Send(Sendbyte, Sendbyte.Length, 0);
                 
            //=======================================================       
            // Receive.
            m_Socket.Receive(Receivebyte);
            ReceiveString = Encoding.Default.GetString(Receivebyte);
            ReceivedataLength = Encoding.Default.GetByteCount(ReceiveString.ToString());
            Debug.Log("Receive Data : " + ReceiveString + "(" + ReceivedataLength + ")");
        }
        catch (SocketException err)
        {
            Debug.Log("Socket send or receive error! : " + err.ToString() );
        }
    }
   
    void OnApplicationQuit ()
    {
        m_Socket.Close();
        m_Socket = null;
    }

} 

