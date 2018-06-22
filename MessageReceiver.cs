using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Cryptography;

namespace CallRecordingServer
{
    public class MessageReceiver
    {
        int tcpPort;

        private BackgroundWorker tcpThread = new BackgroundWorker();
        public delegate void ReadHandler(string MessageType, string MessageText);

        public event ReadHandler MessageRead;


        public MessageReceiver()
        {
             tcpPort = 8083;  //TODO: pull from config or system table somewhere

             tcpThread.WorkerReportsProgress = true;
             tcpThread.WorkerSupportsCancellation = true;

             tcpThread.DoWork += Thread_DoWork;
             tcpThread.ProgressChanged += tcpThread_ProgressChanged;
        }



        public void StartListening()
        {
            if (!tcpThread.IsBusy)
            {
                tcpThread.RunWorkerAsync();
            }
            SymmetricAlgorithm crypto;
            DES DESKey = new DES();
            crypto.Key = DESKey;
            //Triple
        }

        public void StopListening()
        {
            tcpThread.CancelAsync();
            Thread.Sleep(200);
        }

        void Thread_DoWork(Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var ipLocalhost = IPAddress.Any;
            var tcpListener = new TcpListener(ipLocalhost, tcpPort);

            try
            {
                tcpListener.Start(100);
                while (!tcpThread.CancellationPending)
                {
                    while (!tcpListener.Pending() && !tcpThread.CancellationPending)
                    {
                        Thread.Sleep(10);
                    }

                    //check for cancel pending
                    if (tcpThread.CancellationPending)
                    {
                        break;
                    }

                    //open stream and receive data
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    NetworkStream netStream = tcpClient.GetStream();
                    StreamReader netStreamReader = new StreamReader(netStream);
                    StreamWriter netStreamWriter = new StreamWriter(netStream);
                    string stringData;

                    //report thread progress
                    stringData = netStreamReader.ReadToEnd();
                    tcpThread.ReportProgress(0, stringData);

                    //close stream
                    netStreamReader.Close();
                    netStream.Close();
                    tcpClient.Close();

                }
            }

            catch (Exception ex)
            {

                //handle the exception outside the thread
                tcpThread.ReportProgress(100, ex);
            }
            finally
            {


                //stop the listener
                tcpListener.Stop();

            }

        }



        void tcpThread_ProgressChanged(Object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                //convert the xml to an object and fire public event
                try
                {
                    string xmlData = e.UserState.ToString();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlData);
                    XmlNode root = doc.FirstChild;
                    string messageType = root.ChildNodes[0].InnerText;
                    string messageText = root.ChildNodes[1].InnerText;
                    MessageRead(messageType,messageText);
                }
                catch
                {
                    //do nothing because we're disposing
                }
            }
            else
            {
                if (e.ProgressPercentage == 100)
                {
                    //could not connect
                    //displayError("Error with TCP connection: " & CType(e.UserState, Exception).Message)
                    tcpThread.CancelAsync();
                    Thread.Sleep(200);
                }
            }

        }
        }
}
