using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Net;
using System.Web.Security;
using System.Xml;
using SHDocVw;
using TelstratTest;

namespace SSO
{

    public class POSTSSO //: clsBase_Page
    {
        protected XmlDocument xmlPrivateKey = new XmlDocument();
        protected XmlDocument xmlPublicKey = new XmlDocument();

        protected string additionalParameters = String.Empty;
        

        private string _clientID;
        public string ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        private string _serverID;
        public string ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }

        private string _senderKeyVersion;
        public string SenderKeyVersion
        {
            get { return _senderKeyVersion; }
            set { _senderKeyVersion = value; }
        }

        private string _receiverKeyVersion;
        public string ReceiverKeyVersion
        {
            get { return _receiverKeyVersion; }
            set { _receiverKeyVersion = value; }
        }

        private string _payload;
        public string Payload
        {
            get { return _payload; }
            set { _payload = value; }
        }

        private string _signature;
        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        private string _cipher;
        public string Cipher
        {
            get { return _cipher; }
            set { _cipher = value; }
        }


        private string transactionID;
        public string TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }

        private string loginID;
        public string LoginID
        {
            get { return loginID; }
            set { loginID = value; }
        }
        private string employeeID;
        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }


        public bool DoInit()
        {
            bool returnValue = false;
            //if (base.DoInit())
            //{
                BuildSSO();
                returnValue = true;
            //}

            return returnValue;
        }

        protected void BuildSSO()
        {
            try
            {

                xmlPublicKey = GetPublicKey();
                xmlPrivateKey = GetPrivateKey();

                if (xmlPublicKey == null)
                {
                    LogError("Unable to retrieve Public Key");
                    return;
                }

                if (xmlPrivateKey == null)
                {
                    LogError("Unable to retrieve Private Key");
                    return;
                }

                CspParameters Sendercp = new CspParameters();
                RSACryptoServiceProvider SenderRSA = null;

                try
                {
                    Sendercp.Flags = CspProviderFlags.UseMachineKeyStore;
                    SenderRSA = new RSACryptoServiceProvider(Sendercp);
                    SenderRSA.FromXmlString(xmlPrivateKey.OuterXml);
                }
                catch (Exception ex)
                {
                    SenderRSA.Clear();
                    LogError("Error while constructing the PrivateKey: " + ex.Message);
                    return;
                }

                CspParameters Receivercp = new CspParameters();
                RSACryptoServiceProvider ReceiverRSA = null;

                try
                {
                    Receivercp.Flags = CspProviderFlags.UseMachineKeyStore;
                    ReceiverRSA = new RSACryptoServiceProvider(Receivercp);
                    ReceiverRSA.FromXmlString(xmlPublicKey.OuterXml);
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    LogError("Error while constructing the PublicKey: " + ex.Message);
                    return;
                }

                string tpayload = AssemblePayload();

                byte[] TripleDESKey = null;
                byte[] encryptedData = SSOEncryption.TripleDESEncrypt(Encoding.ASCII.GetBytes(tpayload), out TripleDESKey);

                string base64enc = Convert.ToBase64String(encryptedData);

                string plainsig = base64enc + _clientID + _serverID;

                MD5 md = null;
                byte[] buffer = null;
                byte[] hash = null;

                try
                {
                    md = MD5CryptoServiceProvider.Create();

                    //Convert the string into an array of bytes.
                    buffer = Encoding.ASCII.GetBytes(plainsig);

                    //Create the hash value from the array of bytes.
                    hash = md.ComputeHash(buffer);
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    SenderRSA.Clear();
                    LogError("Error Computing Hash: " + ex.Message);
                    return;
                }

                byte[] signed;
                byte[] cipher;

                try
                {
                    signed = SenderRSA.SignHash(hash, CryptoConfig.MapNameToOID("MD5"));
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    SenderRSA.Clear();
                    LogError("Error Signing Hash: " + ex.Message);
                    return;
                }

                try
                {
                    cipher = ReceiverRSA.Encrypt(TripleDESKey, false);
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    SenderRSA.Clear();
                    LogError("Error Encrypting Symmetric Key: " + ex.Message);
                    return;
                }

                string base64sign = Convert.ToBase64String(signed);
                string base64cipher = Convert.ToBase64String(cipher);


                _payload = webcode(base64enc);
                _signature = webcode(base64sign);

                //byte[] EncBinarySig = Convert.FromBase64String(webDecode(_signature));

                _cipher = webcode(base64cipher);

                WebClient wc = new WebClient();
                NameValueCollection vars = new NameValueCollection();
                vars.Add("Payload", webcode(base64enc));
                vars.Add("Signature", webcode(base64sign));
                vars.Add("Cipher", webcode(base64cipher));
                vars.Add("ClientID", _clientID);
                vars.Add("ServerID", _serverID);
                vars.Add("SenderKeyVersion", _senderKeyVersion);
                vars.Add("ReceiverKeyVersion", _receiverKeyVersion);

                ShellWindows shWin = new ShellWindows(); 

                object o = null;
                object v = (object)vars;
                SHDocVw.InternetExplorer IE = new InternetExplorerClass();
                //IWebBrowserApp wb = (IWebBrowserApp)IE;
                ////wb.FullName = "AnswerKey";
                //wb.Visible = true;
                //wb.Navigate("http://www.c-sharpcorner.com/", ref o, ref o, ref o, ref o);
                //IE.Name
                bool b = false;

                foreach (InternetExplorer ies in shWin)
                {
                    if (ies.HWND == IE.HWND)
                    {
                        IE.Visible = true;
                        IE.Navigate("http://www.c-sharpcorner.com/", ref o, ref o, ref o, ref o);

                    }
                    else
                    {
                        IE = new InternetExplorerClass();
                        IE.Visible = true;
                        IE.Navigate("http://www.codeproject.com/", ref o, ref o, ref o, ref o);
                    }
                }



                //IE.Visible = true;
                //IE.Navigate("https://sso.ehr.com/Destination.aspx", ref o, ref o, ref v, ref o);
                ////MessageBox.Show(IE.Name.ToString());

                //int hndl = IE.HWND;
                


                //byte[] retBytes = wc.UploadValues("https://sso.ehr.com/Destination.aspx", vars);

                //string retASCII = Encoding.ASCII.GetString(retBytes);

                ReceiverRSA.Clear();
                SenderRSA.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while Building the SSO Package: \n" + ex.Message);
            }
        }




        public void DisAssemble()
        {
        
            try
            {

                xmlPublicKey = GetPublicKey();
                xmlPrivateKey = GetPrivateKey();

                if (xmlPublicKey == null)
                {
                    LogError("Unable to retrieve Public Key");
                    return;
                }

                if (xmlPrivateKey == null)
                {
                    LogError("Unable to retrieve Private Key");
                    return;
                }


                CspParameters Sendercp = new CspParameters();
                RSACryptoServiceProvider SenderRSA = null;

                try
                {
                    Sendercp.Flags = CspProviderFlags.UseMachineKeyStore;
                    SenderRSA = new RSACryptoServiceProvider(Sendercp);
                    SenderRSA.FromXmlString(xmlPublicKey.OuterXml);
                }
                catch (Exception ex)
                {
                    SenderRSA.Clear();
                    LogError("Error while constructing the PublicKey: " + ex.Message);
                    return;
                }

                CspParameters Receivercp = new CspParameters();
                RSACryptoServiceProvider ReceiverRSA = null;

                try
                {
                    Receivercp.Flags = CspProviderFlags.UseMachineKeyStore;
                    ReceiverRSA = new RSACryptoServiceProvider(Receivercp);
                    ReceiverRSA.FromXmlString(xmlPrivateKey.OuterXml);
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    LogError("Error while constructing the PrivateKey: " + ex.Message);
                    return;
                }

                // This is where I am having problems. SenderRSA.Decrypt returns an error of "Bad key".
                byte[] EncBinarySig = Convert.FromBase64String(webDecode(_signature));
                //byte[] transmittedHash = SenderRSA.Decrypt(EncBinarySig,false);

                string nameValuePairText = webDecode(_payload);
                string transmittedMsg = nameValuePairText + _clientID + _serverID;
                string calculatedHash = nameValuePairText + ClientID + ServerID;
                byte[] hash;
                byte[] signed = null;

                signed = Convert.FromBase64String(webDecode(_signature));
                 
                MD5 md2 = null;

                try
                {
                    md2 = MD5CryptoServiceProvider.Create();

                    //Convert the string into an array of bytes.
                    byte[] buffer = Encoding.ASCII.GetBytes(calculatedHash);

                    //Create the hash value from the array of bytes.
                    hash = md2.ComputeHash(buffer);
                }
                catch (Exception ex)
                {
                    ReceiverRSA.Clear();
                    SenderRSA.Clear();
                    LogError("Error Computing Hash: " + ex.Message);
                    return;
                }

                if (SenderRSA.VerifyHash(hash, CryptoConfig.MapNameToOID("MD5"), signed))
                {
                    
                    byte[] TripleDESKey = null;

                    string encryptedKeyText = webDecode(_cipher);
                    byte[] encryptedKeyBinary = Convert.FromBase64String(encryptedKeyText);
                    TripleDESKey = ReceiverRSA.Decrypt(encryptedKeyBinary,false);

                    byte[] nameValuePairBinary = Convert.FromBase64String(nameValuePairText);
                    byte[] nameValuePair = SSOEncryption.TripleDESDecrypt(nameValuePairBinary, TripleDESKey,null);
                    string ClearTextPayload = Encoding.ASCII.GetString(nameValuePair);

                    //LogError("Error matching Computed and Transmitted Hash");
                    //return;
                }

                

                ReceiverRSA.Clear();
                SenderRSA.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while Building the SSO Package: \n" + ex.Message);
            }
        }


        

        protected string webcode(string st)
        {
            return HttpUtility.UrlEncode(st) + "decode";
        }


        protected string webDecode(string st)
        {
            string wrk = st.Substring(0, (st.IndexOf("decode")));
            return HttpUtility.UrlDecode(wrk);
        }



        public string AdditionalParameters
        {
            get
            {
                return additionalParameters;
            }
            set
            {
                if (!value.EndsWith(";"))
                {
                    value += ";";
                }

                additionalParameters = value;
            }
        }

        public virtual string AssemblePayload()
        {
            Random RandomClass = new Random();
            string salt = (RandomClass.Next()).ToString();

            StringBuilder payLoad = new StringBuilder();

            payLoad.Append("XXXXXXXX");
            payLoad.Append("TransactionID=");
            payLoad.Append(transactionID);
            payLoad.Append(";");
            payLoad.Append("LoginID=");
            payLoad.Append(loginID);
            payLoad.Append(";");
            payLoad.Append("EmployeeID=");
            payLoad.Append(employeeID);
            payLoad.Append(";");
            payLoad.Append("DOB=1/1/2005;salt=");
            payLoad.Append(salt);
            payLoad.Append(";");
            payLoad.Append(AdditionalParameters);
            payLoad.Append("StampedAt=");
            payLoad.Append(DateTime.UtcNow.ToString("yyyyMMdd:HHmmss"));

            return payLoad.ToString();
        }

        protected virtual XmlDocument GetPublicKey()
        {
            xmlPublicKey.Load("C:\\Documents and Settings\\681593\\My Documents\\CTIAPI\\WW_SSO\\PublicKey.xml");
            return xmlPublicKey;
        }

        protected virtual XmlDocument GetPrivateKey()
        {
            xmlPrivateKey.Load("C:\\Documents and Settings\\681593\\My Documents\\CTIAPI\\WW_SSO\\PrivateKey.xml");
            return xmlPrivateKey;
        }


        protected virtual void LogError(string message)
        {
            //LOG(message);
        }


    }



    public class SSOEncryption
    {


        const int CHUNKSIZE = 1024;

        public static byte[] TripleDESEncrypt(byte[] bytesData, out byte[] key)
        {
            TripleDES des3 = new TripleDESCryptoServiceProvider();
            des3.Mode = CipherMode.CBC;
            // See if a key was provided
            byte[] TripleDESKey = des3.Key;
            des3.GenerateIV();
            byte[] TripleDESinitVec = des3.IV;

            MemoryStream memStreamEncryptedData = new MemoryStream();
            ICryptoTransform transform = des3.CreateEncryptor();
            CryptoStream encStream = new CryptoStream(memStreamEncryptedData, transform, CryptoStreamMode.Write);
            try
            {
                //Encrypt the data, write it to the memory stream.
                encStream.Write(bytesData, 0, bytesData.Length);
            }
            catch (Exception ex)
            {
                //oLib.LogError("Error while Encrypting Data:  " + ex.Message, new System.Object());
                throw new Exception("Error while Encrypting Data: \n" + ex.Message);
            }
            encStream.FlushFinalBlock();
            encStream.Close();

            key = TripleDESKey;

            //Send the data back.
            return memStreamEncryptedData.ToArray();
        }

        public static byte[] TripleDESDecrypt(byte[] bytesData, byte[] key, byte[] iv)
        {
            MemoryStream memStreamDecryptedData = new MemoryStream();
            TripleDES des3 = new TripleDESCryptoServiceProvider();
            des3.Mode = CipherMode.CBC;
            des3.Key = key;
            // des3.GenerateIV();
            ICryptoTransform transform = des3.CreateDecryptor();

            CryptoStream decStream = new CryptoStream(memStreamDecryptedData,
                transform,
                CryptoStreamMode.Write);

            try
            {
                decStream.Write(bytesData, 0, bytesData.Length);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while writing encrypted data to the stream: \n" + ex.Message);
            }
            decStream.FlushFinalBlock();
            decStream.Close();
             //Send the data back.
            byte[] bloated = memStreamDecryptedData.ToArray();
            byte[] normal = new byte[bloated.Length - 8];

            for (int a = 0; a < normal.Length; a++)
            {
                normal[a] = bloated[8 + a];
            }
            return normal;
            
        }


    }

}



