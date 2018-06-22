using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using TaskTrayApplication;
//using SHDocVw;
//using TelstratTest;

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

        private string _edu;
        public string EDU
        {
            get { return _edu; }
            set { _edu = value; }
        }

        private string _recordingID;
        public string RecordingID
        {
            get { return _recordingID; }
            set { _recordingID = value; }
        }


        private string _ivrExit;
        public string IVRExit
        {
            get { return _ivrExit; }
            set { _ivrExit = value; }
        }

        private string _targetScreen;
        public string TargetScreen
        {
            get { return _targetScreen; }
            set { _targetScreen = value; }
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

        private bool _lb_test;
        public bool lb_test
        {
            get { return _lb_test; }
            set { _lb_test = value; }
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
                    throw;
                    //return;
                }
                //finally
                //{
                //    SenderRSA.Clear();
                //    //LogError("Error while constructing the PrivateKey: " + ex.Message);
                //}

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
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    //LogError("Error while constructing the PublicKey: " + ex.Message);
                //    //return;
                //}

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
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    SenderRSA.Clear();
                //    //LogError("Error Computing Hash: " + ex.Message);

                //}

                byte[] signed;
                byte[] cipher;

                try
                {
                    signed = SenderRSA.SignHash(hash, CryptoConfig.MapNameToOID("MD5"));
                }
                catch (Exception ex)
                {
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    SenderRSA.Clear();
                //    //LogError("Error Signing Hash: " + ex.Message);
                //    //return;
                //}

                try
                {
                    cipher = ReceiverRSA.Encrypt(TripleDESKey, false);
                }
                catch (Exception ex)
                {
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    SenderRSA.Clear();
                //    //LogError("Error Encrypting Symmetric Key: " + ex.Message);
                //    //return;
                //}

                string base64sign = Convert.ToBase64String(signed);
                string base64cipher = Convert.ToBase64String(cipher);


                _payload = webcode(base64enc);
                _signature = webcode(base64sign);

                //byte[] EncBinarySig = Convert.FromBase64String(webDecode(_signature));

                _cipher = webcode(base64cipher);



                ReceiverRSA.Clear();
                SenderRSA.Clear();
            }
            catch (Exception ex)
            {
                //Trace.WriteLine(ex.Message);
                throw;
                //throw new Exception("Error while Building the SSO Package: \n" + ex.Message);
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
                    throw;
                }
                //finally
                //{
                //    SenderRSA.Clear();
                //    //LogError("Error while constructing the PublicKey: " + ex.Message);
                //    //return;
                //}

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
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    //LogError("Error while constructing the PrivateKey: " + ex.Message);
                //    //return;
                //}

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
                    throw;
                }
                //finally
                //{
                //    ReceiverRSA.Clear();
                //    SenderRSA.Clear();
                //    //LogError("Error Computing Hash: " + ex.Message);
                //    //return;
                //}

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
                throw;
                //Trace.WriteLine(ex.Message);
                //throw new Exception("Error while Building the SSO Package: \n" + ex.Message);
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
            string tmStamp = String.Empty;

            StringBuilder payLoad = new StringBuilder();

            payLoad.Append("XXXXXXXX");
            payLoad.Append("EmployeeID=");
            payLoad.Append(employeeID);
            payLoad.Append(";");
            payLoad.Append("LoginID=");
            payLoad.Append(loginID);
            payLoad.Append(";");
            payLoad.Append("EDU=");
            payLoad.Append(EDU);
            payLoad.Append(";");
            payLoad.Append("IVRExit=");
            payLoad.Append(IVRExit);
            payLoad.Append(";");
            payLoad.Append("TargetScreen=");
            payLoad.Append(TargetScreen);
            payLoad.Append(";");
            payLoad.Append("salt=");
            payLoad.Append(salt);
            payLoad.Append(";");
            payLoad.Append(AdditionalParameters);
            //payLoad.Append(";");
            tmStamp = DateTime.UtcNow.ToString("yyyyMMdd:HHmmss");
            payLoad.Append("StampedAt=");
            payLoad.Append(tmStamp);
            payLoad.Append(";");
            payLoad.Append("RecordingID=");
            payLoad.Append(RecordingID);
            payLoad.Append(";");
            payLoad.Append("RecordedOn=");
            payLoad.Append(tmStamp);

            return payLoad.ToString();
        }
        //EmployeeID=;LoginID=659150;EDU=;IVRExit=;TargetScreen=History;StampedAt=20090824:172531;
        //    RecordingID=1002;RecordedOn=20090824:172531



        protected virtual XmlDocument GetPublicKey()
        {
            try
            {
                if (lb_test)
                {
                    xmlPublicKey.Load("C:\\Program Files\\AKcom\\Keys\\UATPublicKey.xml");
                }
                else
                {
                    xmlPublicKey.Load("C:\\Program Files\\AKcom\\Keys\\PRODPublicKey.xml");
                }
                //xmlPublicKey.Load("C:\\Program Files\\AKcom\\Keys\\PublicKey.xml");
                return xmlPublicKey;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected virtual XmlDocument GetPrivateKey()
        {
            try
            {
                xmlPrivateKey.Load("C:\\Program Files\\AKcom\\Keys\\PrivateKey.xml");
                return xmlPrivateKey;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected virtual void LogError(string message)
        {
            MessageBox.Show("The Error is: " + message, "Error!");
            
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
                throw;
                //Trace.WriteLine(ex.Message);
                //throw new Exception("Error while Encrypting Data: \n" + ex.Message);
            }
            finally
            {
                encStream.FlushFinalBlock();
                encStream.Close();
            }

            key = TripleDESKey;

            //Send the data back.
            byte[] rtn = memStreamEncryptedData.ToArray();
            memStreamEncryptedData.Close(); 
            return rtn;
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
                transform, CryptoStreamMode.Write);

            try
            {
                decStream.Write(bytesData, 0, bytesData.Length);
            }
            catch (Exception ex)
            {
                throw;
                //Trace.WriteLine(ex.Message); 
                //throw new Exception("Error while writing encrypted data to the stream: \n" + ex.Message);
            }
            finally
            {
                decStream.FlushFinalBlock();
                decStream.Close();
            }

             //Send the data back.
            byte[] bloated = memStreamDecryptedData.ToArray();
            byte[] normal = new byte[bloated.Length - 8];

            for (int a = 0; a < normal.Length; a++)
            {
                normal[a] = bloated[8 + a];
            }

            memStreamDecryptedData.Close();
            return normal;
            
        }
    }

}



