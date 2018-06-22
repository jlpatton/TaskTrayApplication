using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Windows.Forms;

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
            tcpPort = Convert.ToInt32(TaskTrayApplication.Properties.Settings.Default.tcpPort); //8083;

            tcpThread.WorkerReportsProgress = true;
            tcpThread.WorkerSupportsCancellation = true;

            tcpThread.DoWork += Thread_DoWork;
            tcpThread.ProgressChanged += tcpThread_ProgressChanged;
        }

        public void StartListening()
        {
            try
            {
                if (!tcpThread.IsBusy)
                {
                    tcpThread.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show("MessageReceiver", ex.Message);
            }
        }

        public void StopListening()
        {
            tcpThread.CancelAsync();
            Thread.Sleep(200);
        }




        void Thread_DoWork(Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            IPAddress ipLocalhost = IPAddress.Any;
            TcpListener tcpListener = new TcpListener(ipLocalhost, tcpPort);

            try
            {
                tcpListener.Start(100);
                while (!tcpThread.CancellationPending)
                {
                    while (!tcpListener.Pending() && !tcpThread.CancellationPending)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(10);
                    }

                    //check for cancel pending
                    if (tcpThread.CancellationPending)
                    {
                        break;
                    }

                    //Socket soc = tcpListener.AcceptSocket();

                    //open stream and receive data
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    //Stream netStream = new NetworkStream(soc);
                    NetworkStream netStream = tcpClient.GetStream();
                    StreamReader netStreamReader = new StreamReader(netStream);
                    StreamWriter netStreamWriter = new StreamWriter(netStream);
                    netStreamWriter.AutoFlush = true;
                    string stringData;

                    //report thread progress
                    stringData = netStreamReader.ReadToEnd();
                    tcpThread.ReportProgress(0, stringData);

                    //close stream
                    netStreamReader.Close();
                    netStream.Close();
                    //soc.Close();
                    tcpClient.Close();

                }
            }

            catch (Exception ex)
            {
                throw;
                //handle the exception outside the thread
                //MessageBox.Show("MessageReceiver",ex.Message);
                
            }
            finally
            {

                tcpThread.ReportProgress(100, e);
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
                    MessageRead(messageType, messageText);
                }
                catch (Exception ex)
                {
                    throw;
                    //string er = ex.Message;
                    //MessageBox.Show(ex.Message);
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
