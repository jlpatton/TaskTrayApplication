using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Windows.Forms;
using CallParrotCtiACTIVELib;
using CallRecordingServer;
//using SHDocVw;
//using SSO;


namespace TaskTrayApplication
{
    public class TaskTrayApplicationContext : ApplicationContext
    {
        
        
        NotifyIcon notifyIcon = new NotifyIcon();
        ContextMenu CM;
        //MessageReceiver MsgRcvr;
        //POSTSSO SSO;//SSO = new POSTSSO();
        
        
        private CallParrotCtiConnect _objCPCC = null;
        private CallParrotCtiACTIVELib.ICallParrotCtiConnect2 _objCPCC2 = null;

        AKcomSettings AKcomSetWindow;// = new AKcomSettings(ref _objCPCC);
        LoginWnd LoginWindow;

        private bool lb_test = false;

        FileInfo myFile = new FileInfo(@"C:\TelStratTest\myfile.txt");
        StreamWriter sw;// = myFile.OpenWrite();
        string txtline = String.Empty;

        private string _loginID;
        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }
        
        private string _agentID;
        public string AgentID
        {
            get { return _agentID; }
            set { _agentID = value; }
        }

        private string _agentName;
        public string AgentName
        {
            get { return _agentName; }
            set { _agentName = value; }
        }

        private double _tstratUID;
        public double TstratUID
        {
            get { return _tstratUID; }
            //set { _tstratUID = value; }
        }

        private string _callPort;
        public string CallPort
        {
            get { return _callPort; }
            set
            {
                if (value != null)
                {
                    _callPort = value;
                }
                else
                {
                    _callPort = "";
                }
            }
        }

        private TimeSpan _timeoutInterval = new TimeSpan(0,5,0);
        private DateTime _dtBegin;
        private int _idleTime = 5;

        private bool _lb_Idle;
        public bool lb_Idle
        {
            get { return _lb_Idle; }
            set
            {
                _lb_Idle = value;
                if (_lb_Idle)
                {
                    _objCPCC.OnServerTimeChange += new _ICallParrotCtiConnectEvents_OnServerTimeChangeEventHandler(_objCPCC_OnServerTimeChange);
                    _dtBegin = DateTime.Now;
                }
                else
                {
                    _objCPCC.OnServerTimeChange -= new _ICallParrotCtiConnectEvents_OnServerTimeChangeEventHandler(_objCPCC_OnServerTimeChange);
                }
            }
        }


        //IPhoneStatus pStat;
        private bool lb_CSRmode;
        private bool lb_Ready;
        int j = 0;
        cstErrorCodes cst;
        IRecordedCallList RecCallList;

        
        // Menu Items
        MenuItem exitMenuItem;
        MenuItem CSRmodeOnMenuItem;
        MenuItem CSRmodeOffMenuItem;
        MenuItem ReadyMenuItem;
        MenuItem NotReadyMenuItem;
        MenuItem Ready1MenuItem;
        MenuItem NotReady1MenuItem;
        MenuItem SettingsMenuItem;
        MenuItem AnswerKeyMenuItem;
        MenuItem BenefitConnectMenuItem;
        MenuItem CollectGarbageMenuItem;
        
        
        public TaskTrayApplicationContext()
        {
            exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));
            ReadyMenuItem = new MenuItem("Set CSR Ready", new EventHandler(CsrReady));
            NotReadyMenuItem = new MenuItem("Set CSR NotReady", new EventHandler(CsrNotReady));

            Ready1MenuItem = new MenuItem("CSR Ready", new EventHandler(CsrReady));
            NotReady1MenuItem = new MenuItem("CSR NotReady", new EventHandler(CsrNotReady));
            
            CSRmodeOnMenuItem = new MenuItem("CSR Mode On");
            CSRmodeOffMenuItem = new MenuItem("CSR Mode Off", new EventHandler(CSRmodeOff));
            SettingsMenuItem = new MenuItem("Settings", new EventHandler(AKcomSettings));
            AnswerKeyMenuItem = new MenuItem("AnswerKey", new EventHandler(AnswerKeyFire));
            BenefitConnectMenuItem = new MenuItem("BenefitConnect", new EventHandler(BenefitConnectFire));
            #if DEBUG
            CollectGarbageMenuItem = new MenuItem("CollectGarbageFire", new EventHandler(CollectGarbageEH)); 
            #endif

            CM = new ContextMenu();

            CM.MenuItems.Add(CSRmodeOnMenuItem);
            CM.MenuItems.Add(Ready1MenuItem);
            CM.MenuItems.Add(NotReady1MenuItem);
            CM.MenuItems.Add(CSRmodeOffMenuItem);
            CM.MenuItems.Add("-");
            CM.MenuItems.Add(AnswerKeyMenuItem);
            CM.MenuItems.Add(BenefitConnectMenuItem);
            CM.MenuItems.Add("-");
            CM.MenuItems.Add(SettingsMenuItem);
            CM.MenuItems.Add(exitMenuItem);

            #if DEBUG
            CM.MenuItems.Add(CollectGarbageMenuItem); 
            #endif

            CSRmodeOnMenuItem.MenuItems.Add(ReadyMenuItem);
            CSRmodeOnMenuItem.MenuItems.Add(NotReadyMenuItem);

            Ready1MenuItem.Visible = false;
            NotReady1MenuItem.Visible = false;

            notifyIcon.ContextMenu = CM;
            
            notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Hungup;
            notifyIcon.Visible = true;

            notifyIcon.MouseMove += new MouseEventHandler(NotifyIcon_MouseMove);
            //notifyIcon.BalloonTipShown += new EventHandler(notifyIcon_BalloonTipShown);
            //notifyIcon.BalloonTipClosed += new EventHandler(notifyIcon_BalloonTipClosed);
            //notifyIcon.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);

            AgentID = TaskTrayApplication.Properties.Settings.Default.AgentID;

            _tstratUID = 0;

            CallPort = String.Empty;

            if (CreateNewApi())
            {
                try
                {
                    _objCPCC.Logoff();

                    cst = _objCPCC.Logon(TaskTrayApplication.Properties.Settings.Default.iDVRserver.ToString(),
                        TaskTrayApplication.Properties.Settings.Default.iDVRuser.ToString(),
                        TaskTrayApplication.Properties.Settings.Default.iDVRpwd.ToString()); //"benpidvr.ebs.fedex.com", "681593", "Anythin9");//186.97", "681593", "Anythin9");
                    
                    if (cst == cstErrorCodes.ErrorIntegratedLogonRequired)
                    {
                        cst = _objCPCC2.IntegratedSecurityLogon(TaskTrayApplication.Properties.Settings.Default.iDVRserver.ToString());
                        if (cst != cstErrorCodes.ErrorSuccess && Convert.ToDouble(cst) != 65535)
                        {
                            double d_cst = Convert.ToDouble(cstErrorCodes.ErrorSecurity);

                            MessageBox.Show("The Error is: " + cst.ToString(), "Telstrat Error!");
                        }
                        else
                        {
                            RecCallList = _objCPCC.GetRecentCall();
                            RecCallList = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("The Error is: " + ex.Message, "Error!");
                    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                    throw;
                }
            }

        }



        private Boolean CreateNewApi()
        {
            Boolean bRetVal = true;
            try
            {
                // Remove any existing one
                DeleteApi();

                // create a new one and hook up the events...
                _objCPCC = new CallParrotCtiConnect();
                _objCPCC2 = (CallParrotCtiACTIVELib.ICallParrotCtiConnect2)_objCPCC;
                //_objCPCC.OnServerTimeChange += new _ICallParrotCtiConnectEvents_OnServerTimeChangeEventHandler(_objCPCC_OnServerTimeChange);
                _objCPCC.OnServerDown += new _ICallParrotCtiConnectEvents_OnServerDownEventHandler(_objCPCC_OnServerDown);
                //_objCPCC.OnCallRecorded += new _ICallParrotCtiConnectEvents_OnCallRecordedEventHandler(_objCPCC_OnCallRecorded);
                _objCPCC.OnCallStart += new _ICallParrotCtiConnectEvents_OnCallStartEventHandler(_objCPCC_OnCallStart);
                _objCPCC.OnCallEnd += new _ICallParrotCtiConnectEvents_OnCallEndEventHandler(_objCPCC_OnCallEnd);
                _objCPCC.OnCallResume += new _ICallParrotCtiConnectEvents_OnCallResumeEventHandler(_objCPCC_OnCallResume);
                _objCPCC.OnCallHold += new _ICallParrotCtiConnectEvents_OnCallHoldEventHandler(_objCPCC_OnCallHold);
                _objCPCC.OnAgentLogon += new _ICallParrotCtiConnectEvents_OnAgentLogonEventHandler(_objCPCC_OnAgentLogon);
                _objCPCC.OnAgentLogoff += new _ICallParrotCtiConnectEvents_OnAgentLogoffEventHandler(_objCPCC_OnAgentLogoff);

                //MsgRcvr = new MessageReceiver();
                //MsgRcvr.MessageRead += new MessageReceiver.ReadHandler(MsgRcvr_MessageRead);
                //MsgRcvr.StartListening();


                LoginWindow = new LoginWnd();//ref LoginID);

                if (!myFile.Exists) sw = myFile.CreateText();

                DialogResult DlgRslt = LoginWindow.ShowDialog();
                if (DlgRslt == DialogResult.OK || DlgRslt == DialogResult.Yes)
                {
                    if (DlgRslt == DialogResult.Yes) lb_test = true;
                    if (MessageBox.Show("Do you wish to be placed in CSR Mode?", "CSR Mode?", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        lb_CSRmode = false;
                        if (MessageBox.Show("Do you wish to use AnswerKey?", "AnswerKey?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //// Be prepared to comment out this block 

                        ////InternetExplorer ie = new InternetExplorer();

                            ////build a header for a form post
                            //Object vHeaders = "Content-Type: application/x-www-form-urlencoded" + "\n" + "\r";

                            ////load the form data into a byte array
                            //Object vPost = null;// System.Text.ASCIIEncoding.ASCII.GetBytes(postData);

                            //Object vTarget = null;
                            //Object vFlags = null;

                            //try
                            //{
                            //    //do the post here
                            //    ie.Navigate("http://www.yahoo.com", ref vFlags, ref vTarget, ref vPost, ref vHeaders);
                            //}
                            //catch (Exception ex)
                            //{
                            //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString());
                            //    throw;
                            //}

                            ////display the window
                            //ie.Visible = true;

                            //+++++++++++++++++++++++++++++++++++++++++++++++++

                            //SSO = new POSTSSO();
                            //SSO.lb_test = lb_test;
                            //SSO.LoginID = TaskTrayApplication.Properties.Settings.Default.LoginID;
                            //SSO.TargetScreen = "";
                            //SSO.RecordingID = "";// AgentID.ToString().Trim() + DateTime.UtcNow.ToString("yyyyMMddHHmmssff"); //"66064372009082715530558";
                            //SSO.ClientID = TaskTrayApplication.Properties.Settings.Default.KeyClientID.ToString(); //"3"; //TODO: Pull from settings?
                            //SSO.ServerID = TaskTrayApplication.Properties.Settings.Default.KeyServerID.ToString(); //"1";
                            //SSO.SenderKeyVersion = TaskTrayApplication.Properties.Settings.Default.SenderKeyVersion.ToString(); //"1.0";
                            //SSO.ReceiverKeyVersion = TaskTrayApplication.Properties.Settings.Default.ReceiverKeyVersion.ToString(); //"1.0";
                            //SSO.DoInit();
                            //string postData = AssemblePostData();

                            //SSO = null;

                            //string url = TaskTrayApplication.Properties.Settings.Default.NonCsrTestURL.ToString(); //"https://ak-uat.ehr.com/FedEx/default.ashx?classname=recordingsso";
                            //try
                            //{
                            //    PostToIE("AK EBA", postData, url);
                            //}
                            //catch (Exception ex)
                            //{
                            //    //TODO: Can't get to AK
                            //    #if DEBUG
                            //    txtline = "CreateNewApi - PostToIE; Time: " + DateTime.Now.ToShortTimeString().ToString() + ";";
                            //    sw = myFile.AppendText();
                            //    sw.WriteLine(txtline);
                            //    sw.Close(); 
                            //    #endif

                            //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                            //    throw;
                            //}
                            //MessageBox.Show("Functionality to be developed. Please start BenefitConnect manually.", "Under construction", MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        lb_CSRmode = true;
                        if (MessageBox.Show("Are you ready to answer phone calls?", "Ready State?", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            lb_Ready = false;
                            lb_Idle = true;
                            MessageBox.Show("When you wish to be in 'Ready State' please right-click on the AKcom tray icon and choose 'Set CSR Ready'.");
                        }
                        else
                        {
                            lb_Idle = false;
                            lb_Ready = true;
                        }
                    }

                }
                else
                {
                    MessageBox.Show("The login procedure has failed and the application is shutting down.", "Login Error", MessageBoxButtons.OK);
                    notifyIcon.Visible = false;
                    //_objCPCC.Logoff();

                    Application.Exit();

                }

                
                if (lb_CSRmode)
                {
                    if (lb_Ready)
                    {
                        Ready1MenuItem.Checked = true;
                        NotReady1MenuItem.Checked = false;
                        CSRmodeOffMenuItem.Checked = false;

                        Ready1MenuItem.Visible = true;
                        NotReady1MenuItem.Visible = true;
                        CSRmodeOnMenuItem.Visible = false;
                        //CSRmodeOnMenuItem.Checked = true;
                        
                        notifyIcon.Visible = true;
                        notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Ready3;
                        lb_Idle = false;
                    }
                    else
                    {
                        Ready1MenuItem.Checked = false;
                        NotReady1MenuItem.Checked = true;
                        CSRmodeOffMenuItem.Checked = false;

                        Ready1MenuItem.Visible = true;
                        NotReady1MenuItem.Visible = true;
                        CSRmodeOnMenuItem.Visible = false;
                        
                        notifyIcon.Visible = true; 
                        notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Wait;
                        lb_Idle = true;
                    }
                }
                else
                {
                    Ready1MenuItem.Checked = false;
                    NotReady1MenuItem.Checked = false;
                    CSRmodeOffMenuItem.Checked = true;

                    Ready1MenuItem.Visible = false;
                    NotReady1MenuItem.Visible = false;
                    CSRmodeOnMenuItem.Visible = true;
                    
                    notifyIcon.Visible = true;
                    notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Hungup;
                }

                notifyIcon.Visible = true;
                
            }
            catch (Exception ex)
            {
                DeleteApi();
                //Trace.WriteLine("CreateNewApi Failed with exception: " + ex.Message);
                bRetVal = false;
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;

            }

            return bRetVal;
        }

        void _objCPCC_OnAgentLogoff(string bsPort)
        {
            #if DEBUG
            txtline = "OnAgentLogon; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
            sw = myFile.AppendText();
            sw.WriteLine(txtline);
            sw.Close(); 
            #endif
        }

        void _objCPCC_OnAgentLogon(string bsAgentID, string bsFirstName, string bsLastName, string bsMName, string bsPort, string bsDN)
        {
            #if DEBUG
            txtline = "OnAgentLogon; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
            sw = myFile.AppendText();
            sw.WriteLine(txtline);
            sw.Close(); 
            #endif
        }

        private void DeleteApi()
        {
            if (_objCPCC != null)
            {
                _objCPCC.OnServerDown -= new _ICallParrotCtiConnectEvents_OnServerDownEventHandler(_objCPCC_OnServerDown);
                _objCPCC.OnServerTimeChange -= new _ICallParrotCtiConnectEvents_OnServerTimeChangeEventHandler(_objCPCC_OnServerTimeChange);
                //_objCPCC.OnCallRecorded -= new _ICallParrotCtiConnectEvents_OnCallRecordedEventHandler(_objCPCC_OnCallRecorded);
                _objCPCC.OnCallStart -= new _ICallParrotCtiConnectEvents_OnCallStartEventHandler(_objCPCC_OnCallStart);
                _objCPCC.OnCallEnd -= new _ICallParrotCtiConnectEvents_OnCallEndEventHandler(_objCPCC_OnCallEnd);
                _objCPCC.OnCallResume -= new _ICallParrotCtiConnectEvents_OnCallResumeEventHandler(_objCPCC_OnCallResume);
                _objCPCC.OnCallHold -= new _ICallParrotCtiConnectEvents_OnCallHoldEventHandler(_objCPCC_OnCallHold);
                _objCPCC.OnAgentLogon -= new _ICallParrotCtiConnectEvents_OnAgentLogonEventHandler(_objCPCC_OnAgentLogon);
                _objCPCC.OnAgentLogoff -= new _ICallParrotCtiConnectEvents_OnAgentLogoffEventHandler(_objCPCC_OnAgentLogoff);


                //MsgRcvr.StopListening();
                //MsgRcvr.MessageRead -= new MessageReceiver.ReadHandler(MsgRcvr_MessageRead);
                //MsgRcvr = null;

                AKcomSetWindow = null;

                _objCPCC = null;
                _objCPCC2 = null;
            }
        }


        //void MsgRcvr_MessageRead(string MessageType, string MessageText)
        //{
        //    string mType = MessageType;
        //    string mText = MessageText;

        //    if (mType == "PlayRecording")
        //    {
        //        _objCPCC.EnableDecode(true);
        //        _objCPCC.GetRecentCall();

        //        RecordingCriteria recCrit = new RecordingCriteria();
        //        RecordingCriteriaList CritList = new RecordingCriteriaList();

        //        recCrit.Remark = mText.ToString().Trim();


        //        IRecordedCallList RecCallList = _objCPCC.GetRecordedCall("Hard Disk", recCrit);
        //        int cnt = RecCallList.Count;
        //        int i = 0;
        //        foreach (IRecordedCall RecCall in RecCallList)
        //        {
        //            if (!Directory.Exists("C:\\Program Files\\AKcom\\DownloadedCall"))
        //            {
        //                Directory.CreateDirectory("C:\\Program Files\\AKcom\\DownloadedCall");
        //            }
        //            i++;
        //            //RecCall.DownloadVoiceFile("C:\\Program Files\\AKcom\\DownloadedCall\\" + recCrit.Remark.ToString() + ".wav");

        //            RecCall.PlayVoiceFile("C:\\Program Files\\AKcom\\DownloadedCall\\" + recCrit.Remark.ToString() + ".wav");
        //            bool notDone = true;
        //            while (notDone)
        //            {
        //                switch (RecCall.GetDownloadStatus())
        //                {
        //                    case cstDownloadStatus.DownloadInProcess:
        //                        notDone = true;
        //                        break;
        //                    case cstDownloadStatus.DownloadComplete:
        //                        notDone = false;
        //                        break;
        //                    case cstDownloadStatus.DownloadFileNotFound:
        //                        notDone = false;
        //                        break;
        //                }
        //            }


        //        }
        //    }

        //}



        #region NotifyIcon events
        private void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            notifyIcon.MouseMove -= new MouseEventHandler(NotifyIcon_MouseMove);
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipText = "Test";
            notifyIcon.BalloonTipTitle = "Alert!";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.ShowBalloonTip(20000);//, "Alert", "Test text", ToolTipIcon.Info);
            notifyIcon.MouseMove += new MouseEventHandler(NotifyIcon_MouseMove);
        }

        //void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        //{
        //    string tiptxt;
            
        //    notifyIcon.Visible = true;
        //    notifyIcon.BalloonTipTitle = "AKcom";
        //    if (!lb_CSRmode)
        //    { tiptxt = "Non CSR Mode"; }
        //    else
        //    {
        //        if (!lb_Ready)
        //        {
        //            tiptxt = "NOT Ready State";
        //        }
        //        else
        //        {
        //            tiptxt = "Ready State";
        //        }
        //    }
        //    notifyIcon.BalloonTipText = tiptxt;
        //    notifyIcon.ShowBalloonTip(10000);
        //    notifyIcon.MouseMove -= new MouseEventHandler(notifyIcon_MouseMove);

        //}

        //void notifyIcon_BalloonTipShown(object sender, EventArgs e)
        //{
        //    notifyIcon.MouseMove -= new MouseEventHandler(notifyIcon_MouseMove);
        //}

        //void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        //{
        //    notifyIcon.MouseMove += new MouseEventHandler(notifyIcon_MouseMove);
        //}

        //void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        //{
        //    notifyIcon.MouseMove += new MouseEventHandler(notifyIcon_MouseMove);
        //} 
        #endregion



        void _objCPCC_OnServerTimeChange(ref DateTime pDate)
        {
            if (lb_Idle)
            {
                if (DateTime.Now - _dtBegin > _timeoutInterval)
                {
                    MessageBox.Show("You have been in 'Not Ready State' in excess of " + _idleTime.ToString() + " minutes!", "Attention ", MessageBoxButtons.OK);
                    _dtBegin = DateTime.Now;
                    _idleTime = _idleTime + 5;
                }
            }
            System.GC.Collect();
            //Trace.WriteLine("OnServerTimeChange Event");
            //Trace.WriteLine(String.Format("OnServerTimeChange - {0}", pDate.ToString()));
        }


        void _objCPCC_OnServerDown()
        {
            #if DEBUG
            txtline = "OnServerDown; Time: " + DateTime.Now.ToShortTimeString().ToString() + ";";
            sw = myFile.AppendText();
            sw.WriteLine(txtline);
            sw.Close(); 
            #endif
        }


        //void _objCPCC_OnCallRecorded(IRecordedCall IRecCall)
        //{
        //    //string ls_agID = IRecCall.AgentID;

        //    if (pStat != null)
        //    {
        //        string ls_pstat = pStat.Status.ToString();

        //        cstPhoneStatus cst = pStat.Status;
        //        string ls_statPH = cst.ToString();
        //    }
            
        //    //Trace.WriteLine("OnCallRecorded Event");
        //    //throw new Exception("The method or operation is not implemented.");
        //}


        void _objCPCC_OnCallEnd(string s1, string s2)
        {
            try
            {
                if (CallPort.ToString().Trim() == s1.ToString().Trim() && Convert.ToDouble(s2) == _tstratUID)
                {

                    if (!lb_CSRmode)
                    {
                        notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Hungup;
                    }
                    else
                    {
                        if (!lb_Ready)
                        {
                            notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Wait;
                            lb_Idle = true;
                        }
                        else
                        {
                            notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Ready3;
                            lb_Idle = false;
                        }
                    }

                    //if (pStat != null)
                    //{
                    //    txtline = "OnCallEnd -- AgentID: " + pStat.AgentID.ToString() + "; UID: "
                    //                    + pStat.UniqueID.ToString() + "; CallPort:" + s1.ToString().Trim()
                    //                    + "; ANI: " + pStat.ANI + "; DN: " + pStat.DN + "; DNIS: " + pStat.DNIS
                    //                    + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                    //    sw = myFile.AppendText();
                    //    sw.WriteLine(txtline);
                    //    sw.Close();
                    //}

                    //pStat = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }

        }


        void _objCPCC_OnCallResume(string s1, string s2)
        {
            try
            {
                if (CallPort.ToString().Trim() == s1.ToString().Trim() && Convert.ToDouble(s2) == _tstratUID)
                {
                    //if (pStat != null)
                    //{
                    //    txtline = "OnCallResume -- AgentID: " + pStat.AgentID.ToString() + "; UID: "
                    //                    + pStat.UniqueID.ToString() + "; CallPort:" + s1.ToString().Trim()
                    //                    + "; ANI: " + pStat.ANI + "; DN: " + pStat.DN + "; DNIS: " + pStat.DNIS
                    //                    + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                    //    sw = myFile.AppendText();
                    //    sw.WriteLine(txtline);
                    //    sw.Close(); 
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }
        }

        void _objCPCC_OnCallHold(string s1, string s2)
        {
            try
            {
                if (CallPort.ToString().Trim() == s1.ToString().Trim() && Convert.ToDouble(s2) == _tstratUID)
                {
                    //if (pStat != null)
                    //{
                    //    txtline = "OnCallHold -- AgentID: " + pStat.AgentID.ToString() + "; UID: "
                    //                    + pStat.UniqueID.ToString() + "; CallPort:" + s1.ToString().Trim()
                    //                    + "; ANI: " + pStat.ANI + "; DN: " + pStat.DN + "; DNIS: " + pStat.DNIS
                    //                    + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                    //    sw = myFile.AppendText();
                    //    sw.WriteLine(txtline);
                    //    sw.Close(); 
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }
        }

        void _objCPCC_OnCallStart(IPhoneStatus pStatus) 
        {
            try
            {
                AgentID = TaskTrayApplication.Properties.Settings.Default.AgentID;
                if (AgentID.ToString().Equals(pStatus.AgentID.ToString()))
                {

#if DEBUG
                    txtline = "OnCallStart -- AgentID: " + pStatus.AgentID.ToString() + "; UID: " + pStatus.UniqueID.ToString()
                        + "; CallPort: " + pStatus.Port.ToString()
                        //+ "; ANI: " + pStatus.ANI + "; DN: " + pStatus.DN + "; DNIS: " + pStatus.DNIS
                        + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                    sw = myFile.AppendText();
                    sw.WriteLine(txtline);
                    sw.Close();
#endif

                    double remNum = 0;


                    try
                    {
                        if (!double.TryParse(pStatus.UniqueID, out remNum))
                        {
                            remNum = -1;
                        }

                    }
                    catch (AccessViolationException axe)
                    {
                        Debug.WriteLine("TryParse AccessViolation Error: " + axe.Message);

                        txtline = "OnCallStart -- " + axe.Message + "; Source: " + axe.Source
                        + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                        sw = myFile.AppendText();
                        sw.WriteLine(txtline);
                        sw.Close();

                        return;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("TryParse Error: " + e.Message);
                        return;
                    }

                    if (_tstratUID != remNum)
                    {
                        //pStat = null;
                        //pStat = pStatus;

                        try
                        {
                            notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Ready;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(String.Format("Caught Exception - {0}", ex.ToString()));
                            throw;
                        }

                        CallPort = pStatus.Port.ToString();
                        lb_Idle = false;

                        ////MessageBox.Show(txtline, "OnCallStart", MessageBoxButtons.OK);

                        ////+++++++++++++++++++++++++++++++++++++++++++++++++
                        //// Be prepared to comment out this block 

                        //InternetExplorer ie = new InternetExplorer();

                        ////build a header for a form post
                        //Object vHeaders = "Content-Type: application/x-www-form-urlencoded" + "\n" + "\r";

                        ////load the form data into a byte array
                        //Object vPost = null;// System.Text.ASCIIEncoding.ASCII.GetBytes(postData);

                        //Object vTarget = null;
                        //Object vFlags = null;

                        //try
                        //{
                        //    //do the post here
                        //    ie.Navigate("http://www.yahoo.com", ref vFlags, ref vTarget, ref vPost, ref vHeaders);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                        //    throw;
                        //}

                        ////display the window
                        //ie.Visible = true;

                        ////+++++++++++++++++++++++++++++++++++++++++++++++++

//                        SSO = new POSTSSO();

//                        SSO.lb_test = lb_test;
//                        SSO.LoginID = TaskTrayApplication.Properties.Settings.Default.LoginID;
//                        SSO.TargetScreen = "History";
//                        SSO.RecordingID = AgentID.ToString().Trim() + DateTime.UtcNow.ToString("yyyyMMddHHmmssff"); //"66064372009082715530558";
//                        SSO.ClientID = TaskTrayApplication.Properties.Settings.Default.KeyClientID.ToString(); //"3"; //TODO: Pull from settings?
//                        SSO.ServerID = TaskTrayApplication.Properties.Settings.Default.KeyServerID.ToString(); //"1";
//                        SSO.SenderKeyVersion = TaskTrayApplication.Properties.Settings.Default.SenderKeyVersion.ToString(); //"1.0";
//                        SSO.ReceiverKeyVersion = TaskTrayApplication.Properties.Settings.Default.ReceiverKeyVersion.ToString(); //"1.0";
//                        SSO.DoInit();
//                        string postData = AssemblePostData();

//                        string url = TaskTrayApplication.Properties.Settings.Default.TestURL.ToString(); //"https://ak-uat.ehr.com/FedEx/default.ashx?classname=recordingsso";
//                        try
//                        {
//                            PostToIE("AK EBA", postData, url);
//                        }
//                        catch (Exception ex)
//                        {
//#if DEBUG
//                            txtline = "PostToIE Error: " + ex.Message
//                                + "; Time: " + DateTime.Now.ToShortTimeString().ToString() + ";";
//                            sw = myFile.AppendText();
//                            sw.WriteLine(txtline);
//                            sw.Close();
//#endif

//                            Debug.WriteLine(String.Format("Caught Exception - {0}", ex.ToString()));
//                            throw;
//                            //TODO: Can't get to AK so prompt user and save to XML
//                        }

//                        string rem1 = SSO.RecordingID.ToString();
//                        string rem2 = pStatus.AgentID.ToString();

//                        SSO = null;

//                        cstErrorCodes ec = pStatus.AddRemark(rem1, rem2);

                        _tstratUID = Convert.ToDouble(pStatus.UniqueID);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}", ex.ToString()));
                //throw;
                txtline = "OnCallStart -- " + ex.Message + "; Source: " + ex.Source
                        + "; Time: " + DateTime.Now.Day.ToString() + ": " + DateTime.Now.ToShortTimeString().ToString() + ";";
                sw = myFile.AppendText();
                sw.WriteLine(txtline);
                sw.Close();
            }
            finally
            {
                pStatus = null;
            }

        }

        void CollectGarbageEH(object sender, EventArgs e)
        {
            GC.Collect();

        }


        void AnswerKeyFire(object sender, EventArgs e)
        {
            ////Using this method as a test for downloading a call recording.

            //cstErrorCodes errCd = _objCPCC.EnableDecode(true);
            //_objCPCC.GetRecentCall();

            //RecordingCriteria recCrit = new RecordingCriteria();
            ////RecordingCriteriaList CritList = new RecordingCriteriaList();

            //recCrit.Remark = "66064412009090419585697";// mText.ToString().Trim();


            //IRecordedCallList RecCallList = _objCPCC.GetRecordedCall("Hard Disk", recCrit);
            //int cnt = RecCallList.Count;
            //int i = 0;
            //foreach (IRecordedCall RecCall in RecCallList)
            //{
            //    if (!Directory.Exists("C:\\Program Files\\AKcom\\DownloadedCall"))
            //    {
            //        Directory.CreateDirectory("C:\\Program Files\\AKcom\\DownloadedCall");
            //    }
            //    i++;
                
            //    string parm = "C:\\Program Files\\AKcom\\DownloadedCall\\" + recCrit.Remark.ToString() + ".wav";
            //    //errCd = RecCall.DownloadVoiceFile(parm);
            //    //bool notDone = true;
            //    //while (notDone)
            //    //{
            //    //    switch (RecCall.GetDownloadStatus())
            //    //    {
            //    //        case cstDownloadStatus.DownloadInProcess:
            //    //            notDone = true;
            //    //            break;
            //    //        case cstDownloadStatus.DownloadComplete:
            //    //            notDone = false;
            //    //            break;
            //    //        case cstDownloadStatus.DownloadFileNotFound:
            //    //            notDone = false;
            //    //            break;
            //    //    }
            //    //}

            //    errCd = RecCall.PlayVoiceFile(parm);
            //    string ls_err = errCd.ToString();
            //}

            ////++++++++++++++++++++++++++++++++++++

            //InternetExplorer ie = new InternetExplorer();
            ////build a header for a form post
            //Object vHeaders = "Content-Type: application/x-www-form-urlencoded" + "\n" + "\r";

            ////load the form data into a byte array
            //Object vPost = null;

            //Object vTarget = null;
            //Object vFlags = null;

            //try
            //{
            //    //do the post here
            //    ie.Navigate("http://www.yahoo.com", ref vFlags, ref vTarget, ref vPost, ref vHeaders);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString());
            //    throw;
            //}

            ////display the window
            //ie.Visible = true;

            ////++++++++++++++++++++++++++++++++++++

            //if (pStat != null)
            //{
            //    cstPhoneStatus phoneStat = pStat.Status;
            //    string ls_stat = phoneStat.ToString();
            //}

            //SSO = new POSTSSO();
            //SSO.LoginID = TaskTrayApplication.Properties.Settings.Default.LoginID;
            //SSO.TargetScreen = "";
            //SSO.RecordingID = "";// AgentID.ToString().Trim() + DateTime.UtcNow.ToString("yyyyMMddHHmmssff"); //"66064372009082715530558";
            //SSO.ClientID = TaskTrayApplication.Properties.Settings.Default.KeyClientID.ToString(); //"3"; //TODO: Pull from settings?
            //SSO.ServerID = TaskTrayApplication.Properties.Settings.Default.KeyServerID.ToString(); //"1";
            //SSO.SenderKeyVersion = TaskTrayApplication.Properties.Settings.Default.SenderKeyVersion.ToString(); //"1.0";
            //SSO.ReceiverKeyVersion = TaskTrayApplication.Properties.Settings.Default.ReceiverKeyVersion.ToString(); //"1.0";
            //SSO.DoInit();
            //string postData = AssemblePostData();

            //SSO = null;
            //string url = TaskTrayApplication.Properties.Settings.Default.NonCsrTestURL.ToString(); //"https://ak-uat.ehr.com/FedEx/default.ashx?classname=recordingsso";
            //try
            //{
            //    PostToIE("AK EBA", postData, url);
            //}
            //catch (Exception ex)
            //{
            //    //TODO: Can't get to AK
            //    txtline = "AKFire - PostToIE Error: " + ex.Message
            //                + "; Time: " + DateTime.Now.ToShortTimeString().ToString() + ";";
            //    sw = myFile.AppendText();
            //    sw.WriteLine(txtline);
            //    sw.Close();
            //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
            //    throw;
            //}
            //////MessageBox.Show("This functionality has not yet been developed.", "Not Implemented", MessageBoxButtons.OK);
        }


        void BenefitConnectFire(object sender, EventArgs e)
        {
            //SSO.LoginID = TaskTrayApplication.Properties.Settings.Default.LoginID;
            //SSO.TargetScreen = "";
            //SSO.RecordingID = "";// AgentID.ToString().Trim() + DateTime.UtcNow.ToString("yyyyMMddHHmmssff"); //"66064372009082715530558";
            //SSO.ClientID = TaskTrayApplication.Properties.Settings.Default.KeyClientID.ToString(); //"3"; //TODO: Pull from settings?
            //SSO.ServerID = TaskTrayApplication.Properties.Settings.Default.KeyServerID.ToString(); //"1";
            //SSO.SenderKeyVersion = TaskTrayApplication.Properties.Settings.Default.SenderKeyVersion.ToString(); //"1.0";
            //SSO.ReceiverKeyVersion = TaskTrayApplication.Properties.Settings.Default.ReceiverKeyVersion.ToString(); //"1.0";
            //SSO.DoInit();
            //string postData = AssemblePostData();

            //string url = TaskTrayApplication.Properties.Settings.Default.NonCsrTestURL.ToString(); //"https://ak-uat.ehr.com/FedEx/default.ashx?classname=recordingsso";
            //try
            //{
            //    PostToIE("AK EBA", postData, url);
            //}
            //catch (Exception ex)
            //{
            //    //TODO: Can't get to AK
            //    Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString());
            //    Trace.WriteLine("CreateNewApi Failed with exception: " + ex.Message);
            //}
            MessageBox.Show("This functionality has not yet been developed.", "Not Implemented", MessageBoxButtons.OK);
        }

        void CSRmodeOff(object sender, EventArgs e)
        {
            lb_CSRmode = false;
            lb_Idle = false;

            try
            {
                Ready1MenuItem.Visible = false;
                NotReady1MenuItem.Visible = false;
                CSRmodeOnMenuItem.Visible = true;

                CSRmodeOffMenuItem.Checked = true;
                
                //CSRmodeOnMenuItem.Checked = false;

                notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Hungup;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }
        }

        void CSRmodeOn(object sender, EventArgs e)
        {
            //lb_CSRmode = true;
            //try
            //{
            //    if (lb_Ready)
            //    {
            //        //if (null != CM) CM.Dispose(); 
            //        //CM = new ContextMenu();
            //        //CM.MenuItems.Add(ReadyMenuItem);
            //        //CM.MenuItems.Add(CSRmodeOffMenuItem);
            //        //CM.MenuItems.Add("-");
            //        //CM.MenuItems.Add(AnswerKeyMenuItem);
            //        //CM.MenuItems.Add(BenefitConnectMenuItem);
            //        //CM.MenuItems.Add("-");
            //        //CM.MenuItems.Add(SettingsMenuItem);
            //        //CM.MenuItems.Add(exitMenuItem);

            //        //notifyIcon.ContextMenu = CM;
            //        Ready1MenuItem.Visible = true;
            //        NotReady1MenuItem.Visible = true;
            //        CSRmodeOnMenuItem.Visible = false;

            //        Ready1MenuItem.Checked = true;
            //        NotReady1MenuItem.Checked = false;
            //        CSRmodeOffMenuItem.Checked = false;
            //        //CSRmodeOnMenuItem.Checked = true;

            //        notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Ready3;
            //        notifyIcon.Visible = true;
            //        lb_Idle = false;
            //    }
            //    else
            //    {
            //        //if (null != CM) CM.Dispose(); 
            //        //CM = new ContextMenu();
            //        //CM.MenuItems.Add(NotReadyMenuItem);
            //        //CM.MenuItems.Add(CSRmodeOffMenuItem);
            //        //CM.MenuItems.Add("-");
            //        //CM.MenuItems.Add(AnswerKeyMenuItem);
            //        //CM.MenuItems.Add(BenefitConnectMenuItem);
            //        //CM.MenuItems.Add("-");
            //        //CM.MenuItems.Add(SettingsMenuItem);
            //        //CM.MenuItems.Add(exitMenuItem);

            //        //notifyIcon.ContextMenu = CM;
            //        Ready1MenuItem.Visible = true;
            //        NotReady1MenuItem.Visible = true;
            //        CSRmodeOnMenuItem.Visible = false;

            //        Ready1MenuItem.Checked = false;
            //        NotReady1MenuItem.Checked = true;
            //        CSRmodeOffMenuItem.Checked = false;
            //        //CSRmodeOnMenuItem.Checked = true;

            //        notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Wait;
            //        notifyIcon.Visible = true;
            //        lb_Idle = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //   Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString());
            //   throw;
            //}
            
        }

        
        void CsrReady(object sender, EventArgs e)
        {
            lb_Ready = true;
            lb_Idle = false;
            lb_CSRmode = true;

            try
            {
                Ready1MenuItem.Visible = true;
                NotReady1MenuItem.Visible = true;
                CSRmodeOnMenuItem.Visible = false;

                Ready1MenuItem.Checked = true;
                NotReady1MenuItem.Checked = false;
                CSRmodeOffMenuItem.Checked = false;
                
                notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Ready3;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }
            
        }
        
        void CsrNotReady(object sender, EventArgs e)
        {
            lb_Ready = false;
            lb_Idle = true;
            lb_CSRmode = true;

            try
            {
                Ready1MenuItem.Visible = true;
                NotReady1MenuItem.Visible = true;
                CSRmodeOnMenuItem.Visible = false;

                Ready1MenuItem.Checked = false;
                NotReady1MenuItem.Checked = true;
                CSRmodeOffMenuItem.Checked = false;
                //CSRmodeOnMenuItem.Checked = false;

                notifyIcon.Icon = TaskTrayApplication.Properties.Resources.Wait;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                throw;
            }
        }
        
        void AKcomSettings(object sender, EventArgs e)
        {
            AKcomSetWindow = new AKcomSettings(ref _objCPCC);
            if (AKcomSetWindow.Visible)
                AKcomSetWindow.Focus();
            else
            {
                AKcomSetWindow.ShowDialog();
                AKcomSetWindow.Dispose();
            }
        }

        //InternetExplorer FindOrCreateIE(string titleString)
        //{
        //    //first try to find a running instance with the titleString in the title
        //    try
        //    {
        //        SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();
        //        foreach (Object cls in shellWindows)
        //        {
        //            if (cls is SHDocVw.InternetExplorerClass)
        //            {
        //                InternetExplorer ie = (InternetExplorer)cls;
        //                if (ie.LocationName.IndexOf(titleString) > -1)
        //                {
        //                    return ie;
        //                }
        //            }
        //        }
        //        //no running instance found - create a new one
        //        return new InternetExplorer();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
        //        throw;
        //    }
         
        //}


        //void PostToIE(string titleString, string postData, string URL)
        //{
        //    InternetExplorer ie = FindOrCreateIE(titleString);

        //    //build a header for a form post
        //    Object vHeaders = "Content-Type: application/x-www-form-urlencoded" + "\n" + "\r";

        //    //load the form data into a byte array
        //    Object vPost = System.Text.ASCIIEncoding.ASCII.GetBytes(postData);

        //    Object vTarget = null;
        //    Object vFlags = null;

        //    try
        //    {
        //        //do the post here
        //        ie.Navigate(URL, ref vFlags, ref vTarget, ref vPost, ref vHeaders);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
        //        throw;
        //    }

        //    //display the window
        //    ie.Visible = true;

        //}

        

        //private string AssemblePostData()
        //{
        //    try
        //    {
        //        StringBuilder postData = new StringBuilder();

        //        postData.Append("Payload=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.Payload));
        //        postData.Append("&");
        //        postData.Append("ClientID=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.ClientID));
        //        postData.Append("&");
        //        postData.Append("ServerID=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.ServerID));
        //        postData.Append("&");
        //        postData.Append("Signature=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.Signature));
        //        postData.Append("&");
        //        postData.Append("Cipher=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.Cipher));
        //        postData.Append("&");
        //        postData.Append("SenderKeyVersion=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.SenderKeyVersion));
        //        postData.Append("&");
        //        postData.Append("ReceiverKeyVersion=");
        //        postData.Append(HttpUtility.UrlEncode(SSO.ReceiverKeyVersion));


        //        return postData.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(String.Format("Caught Exception - {0}",ex.ToString()));
                
        //        throw;
        //    }
        //}

        void CloseTelstrat()
        {
            if (_objCPCC != null) _objCPCC.Logoff();
            _objCPCC2 = null;
            _objCPCC = null;
            
        }

        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            DeleteApi();

            if (sw != null) sw.Close();
            if (CM != null) CM.Dispose();
            if (notifyIcon != null) notifyIcon.Dispose();
            
            CloseTelstrat();
            Application.DoEvents();

            Application.Exit();
        }
    }
}
