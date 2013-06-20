using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
 
public class Network : MonoBehaviour {
 
      private GUIText guiConsole;
      private Socket sock = null;
      private float timer = 0.0f;
      private int counter = 0;
      private byte[] recvBuffer = new byte[1024];
      private byte[] sendBuffer = new byte[1];
     
      // Use this for initialization
      void Start () {
             guiConsole = GameObject.Find("Console").guiText;                   
            
             sock = new Socket(
                        AddressFamily.InterNetwork,
         SocketType.Stream,
                 ProtocolType.Tcp );           
            
             sock.Connect( "localhost", 1337 );
             if( sock.Connected )
             {
                     ConsoleMessage( "Connected" );
                    
                     sock.BeginReceive( 
                         recvBuffer,
                         0,
                         recvBuffer.Length,
                         SocketFlags.None,
                         new AsyncCallback( ReceiveComplete ),
                         null );
             }
             else
             {
                     ConsoleMessage( "Fail to connect" );
             }             
      }      
     
      // Update is called once per frame
      void Update () {
            
      }
     
      void FixedUpdate()
      {             
             timer += Time.deltaTime;
             if( timer > 1.0f )
             {
                     String tmp = String.Format("{0} times try: ", counter);
                     ConsoleMessage( tmp);
                     if( sock != null )
                     {
                            try
                            {                                    
                                    sendBuffer[0] = 255;
                                    sock.BeginSend(
                                           sendBuffer,
                                           0,
                                           1,
                                           SocketFlags.None,
                                           new AsyncCallback( SendComplete),
                                           null );
                           
                                    tmp += "Success";
                                    ConsoleMessage( tmp );
                            }
                            catch( Exception e )
                            {
                                    tmp += "Exception: " + e.Message;
                                    ConsoleMessage( tmp );
                                   
                                    Shutdown();
                            }
                     }
                    
                     counter ++;
                     timer = 0.0f;
             }                            
      }
     
      void OnApplicationQuit()
      {
             Shutdown();
      }
 
      private void ReceiveComplete( IAsyncResult ar )
      {
             try
             {
                     if( null == sock )
                            return;
                    
                     int len = sock.EndReceive( ar );
                    
                     if( len == 0 )
                     {
                            Shutdown();
                     }
                     else
                     {
                            ConsoleMessage(String.Format(
"{0} received", recvBuffer[0] ) );
                            sock.BeginReceive(
               recvBuffer,
0,
                                      recvBuffer.Length,
                                      SocketFlags.None,
                                      new AsyncCallback( ReceiveComplete ),
                                      null );
                     }                     
             }
             catch( Exception e )
             {
                     ConsoleMessage( "Exception: " + e.Message );
                     Shutdown();
             }
      }      
     
      private void SendComplete( IAsyncResult ar )
      {
             try
             {
                     if( null == sock )
                            return;
                    
                     int len = sock.EndSend( ar );
                     if( len == 1 )
                     {
                            ConsoleMessage( "Send success" );
                     }
             }
             catch( Exception e )
             {
                     ConsoleMessage( "Exception: " + e.Message );
                     Shutdown();
             }
      }
            
      private void ConsoleMessage( string msg )
      {             
             guiConsole.text = msg;
      }
     
      private void Shutdown()
      {
             if( sock != null )
             {
                     sock.Shutdown( SocketShutdown.Both );
                     sock = null;
             }
      }
}
 