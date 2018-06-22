using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using CallParrotCtiACTIVELib;
using SSO;
//using IDVRCTIACTIVELib;
//using Limaf.Tools.Web.Encryption;

namespace TelstratTest
{
    public partial class Form1 : Form
    {
        private CallParrotCtiConnect _objCPCC = null;
        private CallParrotCtiACTIVELib.ICallParrotCtiConnect2 _objCPCC2 = null;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
            CreateNewApi();

            try
            {
                _objCPCC.Logoff();
                cst = _objCPCC.Logon("benpidvr.ebs.fedex.com", "681593", "Anythin9");//186.97", "681593", "Anythin9");
                if (cst != cstErrorCodes.ErrorSuccess)
                {
                    if (cst == cstErrorCodes.ErrorIntegratedLogonRequired)
                    {
                        cst = _objCPCC2.IntegratedSecurityLogon("benpidvr.ebs.fedex.com");
                        if (cst != cstErrorCodes.ErrorSuccess && Convert.ToDouble(cst) != 65535)
                        {
                            double d_cst = Convert.ToDouble(cstErrorCodes.ErrorSecurity);
                            
                            MessageBox.Show("The Error is: " + cst.ToString());
                        }
                        else
                        {
                            RecCallList = _objCPCC.GetRecentCall();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Error is: " + ex.Message);
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
                //_objCPCC.OnCallEnd += new _ICallParrotCtiConnectEvents_OnCallEndEventHandler(_objCPCC_OnCallEnd);
                //_objCPCC.OnCallResume += new _ICallParrotCtiConnectEvents_OnCallResumeEventHandler(_objCPCC_OnCallResume);
                //_objCPCC.OnCallHold += new _ICallParrotCtiConnectEvents_OnCallHoldEventHandler(_objCPCC_OnCallHold);
                //_objCPCC.OnCallBlocked += new _ICallParrotCtiConnectEvents_OnCallBlockedEventHandler(_objCPCC_OnCallBlocked);

            }
            catch (Exception ex)
            {
                DeleteApi();
                Trace.WriteLine("CreateNewApi Failed with exception: " + ex.Message);
                bRetVal = false;

            }

            return bRetVal;
        }

        private void DeleteApi()
        {
            if (_objCPCC != null)
            {
                //_objCPCC.OnServerTimeChange -= new _ICallParrotCtiConnectEvents_OnServerTimeChangeEventHandler(_objCPCC_OnServerTimeChange);
                _objCPCC.OnServerDown -= new _ICallParrotCtiConnectEvents_OnServerDownEventHandler(_objCPCC_OnServerDown);

                //_objCPCC.OnCallRecorded -= new _ICallParrotCtiConnectEvents_OnCallRecordedEventHandler(_objCPCC_OnCallRecorded);
                _objCPCC.OnCallStart -= new _ICallParrotCtiConnectEvents_OnCallStartEventHandler(_objCPCC_OnCallStart);
                //_objCPCC.OnCallEnd -= new _ICallParrotCtiConnectEvents_OnCallEndEventHandler(_objCPCC_OnCallEnd);
                //_objCPCC.OnCallResume -= new _ICallParrotCtiConnectEvents_OnCallResumeEventHandler(_objCPCC_OnCallResume);
                //_objCPCC.OnCallHold -= new _ICallParrotCtiConnectEvents_OnCallHoldEventHandler(_objCPCC_OnCallHold);
                //_objCPCC.OnCallBlocked -= new _ICallParrotCtiConnectEvents_OnCallBlockedEventHandler(_objCPCC_OnCallBlocked);
                _objCPCC = null;
                _objCPCC2 = null;
            }
        }




        //ICallParrotCtiConnect iDVR;// = new ICallParrotCtiConnect();
        //ICallParrotCtiConnect2 CallParrot2;// = new ICallParrotCtiConnect2();
        ////CallParrot2 = iDVR;
        int j = 0;
        cstErrorCodes cst;
        IRecordedCallList RecCallList;
        ////Asymmetric ASM;

        //public Form1()
        //{
        //    InitializeComponent();
        //    //IDVRConnect iDVR = new IDVRConnect();
        //    //iDVR.OnCallStart += new _IIDVRCtiConnectEvents_OnCallStartEventHandler(iDVR_OnCallStart);
        //    iDVR.OnCallStart += new _IIDVRCtiConnectEvents_OnCallStartEventHandler(iDVR_OnCallStart);
        //    iDVR.OnServerDown += new _IIDVRCtiConnectEvents_OnServerDownEventHandler(iDVR_OnServerDown);
        //}

        void _objCPCC_OnCallStart(IPhoneStatus pStatus)
        {
            try
            {
                FileStream fs = new FileStream(@"C:\TelStratTest\logfile.log", FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter swrtr = new StreamWriter(fs);

                string nextline = "PortName: " + pStatus.PortFirstName + pStatus.PortLastName + "\r\n";

                //nextline += "PortMiddleName: " + pStatus.PortMiddleName + "\r\n";

                //nextline += "PortLastName: " + pStatus.PortLastName + "\r\n";

                nextline += "AgentId: " + pStatus.AgentID.ToString() + "\r\n";

                nextline += "AgentFirstName: " + pStatus.AgentFirstName.ToString() + "\r\n";

                nextline += "AgentLastName: " + pStatus.AgentLastName.ToString() + "\r\n";

                nextline += "ANI: " + pStatus.ANI.ToString() + "\r\n";

                nextline += "ChannelNumber: " + pStatus.ChannelNumber.ToString() + "\r\n";

                nextline += "Port: " + pStatus.Port.ToString() + "\r\n";

                string rem11 = System.DateTime.Today.ToShortDateString() + pStatus.PortLastName.Substring(2);
                nextline += "Remark1: " + rem11 + "\r\n"; //pStatus.Remark1 + "\r\n";

                
                nextline += "StartTime: " + pStatus.StartTime.ToString() + "\r\n";

                nextline += "Status: " + pStatus.Status.ToString() + "\r\n";

                nextline += "UniqueId: " + pStatus.UniqueID.ToString() + "\r\n";

                nextline += "\r\n\r\n\r\n";

                swrtr.WriteLine(nextline);
                
                swrtr.Close();
                j++;
                //throw new Exception("The method or operation is not implemented.");    
            }
            catch (Exception ev)
            {
                MessageBox.Show("Error in FileStream: " + ev.Message.ToString());
            }

            string rem1 = System.DateTime.Today.ToShortDateString() + pStatus.PortLastName.Substring(2); //"This is rem1 test data. Time: ";
            string rem2 = System.DateTime.Now.ToShortTimeString() + pStatus.PortLastName.Substring(2);

            cstErrorCodes ec = pStatus.AddRemark(rem1, rem2);

        }

        void _objCPCC_OnServerDown()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(@"C:\TelStratTest\logfile2.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter swrtr2 = new StreamWriter(fs);

            IRecordedCallList RecCallList = _objCPCC.GetRecentCall();
            //IRecordedCall RecCall;
            string sDetails;
            for (int i = 0; i < RecCallList.Count - 1; i++)
            {
                foreach (IRecordedCall RecCall in RecCallList)
                {
                    sDetails = "CallRec Number: " + i.ToString() + "; Port: " + RecCall.Port + "; Channel: " + RecCall.ChannelNumber + "; Remark1: " + RecCall.Remark1 + "; Remark2: " + RecCall.Remark2 + RecCall.AgentFirstName.ToString()
                        + " " + RecCall.AgentLastName.ToString() + " StartTime: " + RecCall.StartTime.ToShortDateString() + " " + RecCall.StartTime.ToShortTimeString() + " Duration: " + RecCall.Duration.ToString() + " EndTime: " + RecCall.EndTime.ToShortTimeString() + "\r\n";
                    swrtr2.WriteLine(sDetails);
                    //txtBoxLogging.Text += nextline;
                }
            }

           

            swrtr2.Close();

            //iDVR.GetRecentCall();
            //RecordedCall RecCall = new RecordedCall();


            ////RecordingCriteria recCrit = new RecordingCriteria();
            //RecordingCriteriaList CritList = new RecordingCriteriaList();

            //try{
            //    CritList = iDVR.GetRecordingCriteria();
            //    foreach(RecordingCriteria recCrit in CritList)
            //    {
            //        foreach(object Item in CritList)
            //        {
            //            lBx1.Items.Add(recCrit.NameOrSQLSortFilter);
            //            lBx1.Items.Add("\n");
            //        }
                    
            //    }
            //}
            //catch(Exception ev)
            //{
            //    MessageBox.Show("Error in 'GetRecordingCriteria': " + ev.Message.ToString());
            //}
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    //IDVRConnect iDVR = new IDVRConnect();
            
            
        //    try
        //    {
        //        cst = _objCPCC.Logon("146.18.194.202", "681593", "Anythin9");//186.97", "681593", "Anythin9");
        //        if (cst < cstErrorCodes.ErrorSuccess)
        //        {
        //            MessageBox.Show("The Error is: " + cst.ToString());
        //        }
        //        else
        //        {
        //            RecCallList = _objCPCC.GetRecentCall();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show("The Error is: " + ex.Message);
        //    }

            
            
            //IRecordedCallList callList = iDVR.GetRecentCall();


            //long cnt = callList.Count;

            //AgentList AgList = iDVR.GetAgents();
            // foreach (Agent agnt in AgList)
            // {
            //    string fname = agnt.FirstName;
            //    string lname = agnt.LastName;
            //    string uid = agnt.ID;

            // }

        //}

        //private void 

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _objCPCC.Logoff();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cst = _objCPCC.EnableDecode(true);
            _objCPCC.GetRecentCall();
            //IRecordedCall RecCall = new IRecordedCall();
            //IRecordedCallList RecCallList = iDVR.GetRecordedCall();


            //RecordingCriteria recCrit = new RecordingCriteria();
            RecordingCriteriaList CritList = new RecordingCriteriaList();

            try
            {
                CritList = _objCPCC.GetRecordingCriteria();
                foreach (RecordingCriteria recCrit in CritList)
                {
                    //foreach (object Item in CritList)
                    //{
                    //    lBx1.Items.Add("NameOrSQLFilter:\n");
                    //    lBx1.Items.Add(recCrit.NameOrSQLSortFilter.ToString());
                    //    lBx1.Items.Add("Mask:\n");
                    //    lBx1.Items.Add(recCrit.Mask.ToString());
                    //    lBx1.Items.Add("PortFilterList:\n");
                    //    lBx1.Items.Add(recCrit.PortFilterList.ToString());
                    //    lBx1.Items.Add("DayMask:\n");
                    //    lBx1.Items.Add(recCrit.DayMask.ToString());
                    //    lBx1.Items.Add("AdministratorOwner:\n");
                    //    lBx1.Items.Add(recCrit.AdministerOwner.ToString());
                    //    lBx1.Items.Add("Remark:\n");
                    //    lBx1.Items.Add(recCrit.Remark.ToString());
                    //    lBx1.Items.Add("\n");

                    //}

                    IRecordedCallList RecCallList = _objCPCC.GetRecordedCall("Hard Disk", recCrit);
                    int cnt = RecCallList.Count;
                    int i = 0;
                    foreach (IRecordedCall RecCall in RecCallList)
                    {
                        i++;
                        RecCall.DownloadVoiceFile("C:\\TelStratTest\\DownloadedCall" + i.ToString() + ".wav");
                        bool notDone = true;
                        while (notDone)
                        {
                            switch (RecCall.GetDownloadStatus())
                            {
                            	case cstDownloadStatus.DownloadInProcess:
                                    notDone = true;
                                    break;
                                case cstDownloadStatus.DownloadComplete:
                                    notDone = false;
                                    break;
                                case cstDownloadStatus.DownloadFileNotFound:
                            		notDone = false;
                                    break;
                            }
                        }

                        RecCall.PlayVoiceFile("C:\\TelStratTest\\DownloadedCall" + i.ToString() + ".wav");
                    }


                }
            }
            catch (Exception ev)
            {
                MessageBox.Show("Error in 'GetRecordingCriteria': " + ev.Message.ToString());
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            //iDVR.EnableDecode = true;
            _objCPCC.GetRecentCall();
            //IRecordedCall RecCall = new IRecordedCall();
            

            RecordingCriteria recCrit = new RecordingCriteria();
            RecordingCriteriaList CritList = new RecordingCriteriaList();

            recCrit.Remark = "This is rem1 test data. Time: 9/24/2008 2:30:02 PM"; // "This is rem1 test data. Time: 9/24/2008 12:18:51 PM";


            IRecordedCallList RecCallList = _objCPCC.GetRecordedCall("Hard Disk", recCrit);
                int cnt = RecCallList.Count;
                int i = 0;
                foreach (IRecordedCall RecCall in RecCallList)
                {
                    i++;
                    RecCall.DownloadVoiceFile("C:\\TelStratTest\\DownloadedCall" + i.ToString() + ".wav");
                    bool notDone = true;
                    while (notDone)
                    {
                        switch (RecCall.GetDownloadStatus())
                        {
                            case cstDownloadStatus.DownloadInProcess:
                                notDone = true;
                                break;
                            case cstDownloadStatus.DownloadComplete:
                                notDone = false;
                                break;
                            case cstDownloadStatus.DownloadFileNotFound:
                                notDone = false;
                                break;
                        }
                    }

                    lBx1.Items.Add(RecCall.AgentFirstName.ToString());
                    lBx1.Items.Add(RecCall.AgentLastName.ToString());
                    lBx1.Items.Add(RecCall.ANI.ToString());
                    lBx1.Items.Add(RecCall.BoardNumber.ToString());
                    lBx1.Items.Add(RecCall.ChannelNumber.ToString());
                    lBx1.Items.Add(RecCall.Duration.ToString());
                    lBx1.Items.Add(RecCall.Remark1.ToString());
                    lBx1.Items.Add(RecCall.Remark2.ToString());
                    lBx1.Items.Add(RecCall.Port.ToString());


                    RecCall.PlayVoiceFile("C:\\TelStratTest\\DownloadedCall" + i.ToString() + ".wav");
                }

            

        }

        private void btnDisassemble_Click(object sender, EventArgs e)
        {
            //ASM = new Asymmetric();
            ////XmlTextReader xmlRdr = new XmlTextReader(
            //Asymmetric.PublicKey RSApublicKey = new Asymmetric.PublicKey();
            //RSApublicKey.ImportFromXmlFile("");
            //Asymmetric.PrivateKey RSAprivateKey = new Asymmetric.PrivateKey("");
            //RSAprivateKey.ImportFromXmlFile("");

            //string encryptedSigText = HttpUtility.UrlDecode(txtBxSignature.Text.Substring(0,txtBxSignature.Text.IndexOf("decode")));
            //byte[] signatureBinary = Convert.FromBase64String(encryptedSigText);
            //string transmittedHash = ASM.Decrypt(signatureBinary, RSApublicKey);

            //string nameValuePairText = HttpUtility.UrlDecode(txtBxPayload.Text.Substring(0,txtBxPayload.Text.IndexOf("decode")));
            //string transmittedMessage = nameValuePairText + txtBxClientID.Text + txtBxClientID.Text;
            //Hash hsh = new Hash(Hash.Provider.MD5);
            //string calculatedHash = hsh.Calculate(transmittedMessage);

            //if (transmittedHash != transmittedMessage)
            //{
            //    //
            //}

            //string encryptedSymmetricKeyText = HttpUtility.UrlDecode(txtBxCipher.Text.Substring(0, txtBxCipher.Text.IndexOf("decode")));
            //byte[] encryptedSymKeyBinary = Convert.FromBase64String(encryptedSymmetricKeyText);

            POSTSSO objSSO = new POSTSSO();
            objSSO.ClientID = txtBxClientID.Text;
            objSSO.ServerID = txtBxServerID.Text;
            objSSO.SenderKeyVersion = txtBxSenderKeyVer.Text;
            objSSO.ReceiverKeyVersion = txtBxRecieverKeyVer.Text;

            objSSO.EmployeeID = txtBxEmpID.Text;
            objSSO.LoginID = txtBxLoginID.Text;
            objSSO.TransactionID = txtBxTransID.Text;
            objSSO.AdditionalParameters = txtBxAddParms.Text;


            objSSO.Payload = txtBxPayload.Text;
            objSSO.Cipher = txtBxCipher.Text;
            objSSO.Signature = txtBxSignature.Text;


            objSSO.DisAssemble();


        }

        private void btnAssemble_Click(object sender, EventArgs e)
        {
            POSTSSO objSSO = new POSTSSO();
            objSSO.ClientID = txtBxClientID.Text;
            objSSO.ServerID = txtBxServerID.Text;
            objSSO.SenderKeyVersion = txtBxSenderKeyVer.Text;
            objSSO.ReceiverKeyVersion = txtBxRecieverKeyVer.Text;

            objSSO.EmployeeID = txtBxEmpID.Text;
            objSSO.LoginID = txtBxLoginID.Text;
            objSSO.TransactionID = txtBxTransID.Text;
            objSSO.AdditionalParameters = txtBxAddParms.Text;


            objSSO.DoInit();

            txtBxPayload.Text = objSSO.Payload;
            txtBxCipher.Text = objSSO.Cipher;
            txtBxSignature.Text = objSSO.Signature;

        }
     
    }
}