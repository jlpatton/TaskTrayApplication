using System;
using System.Windows.Forms;
using CallParrotCtiACTIVELib;

namespace TaskTrayApplication
{
    public partial class AKcomSettings : Form
    {
        private CallParrotCtiConnect _objCPCC;
        private AgentList CSRs;
        private string curAgent;
        private string _savedAgent;
        private string _serverPath;
        private string _searchPath;
        private string _userAttr;
        

        public AKcomSettings(ref CallParrotCtiConnect objCPCC)
        {
            InitializeComponent();
            _objCPCC = objCPCC;
        }


        #region Win32Dlls
        //// The Win32 API methods
        //[DllImport("kernel32", SetLastError = true)]
        //static extern int WritePrivateProfileString(string section, string key, string value, string fileName);
        //[DllImport("kernel32", SetLastError = true)]
        //static extern int WritePrivateProfileString(string section, string key, int value, string fileName);
        //[DllImport("kernel32", SetLastError = true)]
        //static extern int WritePrivateProfileString(string section, int key, string value, string fileName);
        //[DllImport("kernel32")]
        //static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder result, int size, string fileName);
        //[DllImport("kernel32")]
        //static extern int GetPrivateProfileString(string section, int key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        //[DllImport("kernel32")]
        //static extern int GetPrivateProfileString(int section, string key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        #endregion 


        private void AKcomSettings_Load(object sender, EventArgs e)
        {
            //if (File.Exists("c:\\Program Files\\AKcom\\AKcom.ini"))
            //{
            //    // Loop until the buffer has grown enough to fit the value
            //    for (int maxSize = 250; true; maxSize *= 2)
            //    {
            //        StringBuilder result = new StringBuilder(maxSize);

            //        int size = GetPrivateProfileString("CurrentAgent", "Name", "", result, maxSize, "c:\\Program Files\\AKcom\\AKcom.ini");

            //        if (size < maxSize - 1)
            //        {
            //            if (size == 0)
            //            {
            //                savedAgent = "";
            //            }
            //            else
            //            {
            //                savedAgent = result.ToString();
            //                break;
            //            }
            //        }
            //    }
            //}

            _serverPath = TaskTrayApplication.Properties.Settings.Default.LDAPserver;
            _searchPath = TaskTrayApplication.Properties.Settings.Default.SearchPath;
            _userAttr = TaskTrayApplication.Properties.Settings.Default.UserAttr;
            _savedAgent = TaskTrayApplication.Properties.Settings.Default.AgentID;

            CSRs = null;
            CSRs = _objCPCC.GetAgents();

            foreach (Agent agnt in CSRs)
            {
                lstBxAgents.Items.Add(agnt.ID.ToString() + " -- " + agnt.FirstName + " " + agnt.LastName);
            }

            int fnd = lstBxAgents.FindString(_savedAgent);
            lstBxAgents.SelectedIndex = fnd;
            txtBxServerPath.Text = _serverPath;
            txtBxSearchPath.Text = _searchPath;
            txtBxUserAttr.Text = _userAttr;
        }

        private void lstBxAgents_SelectedIndexChanged(object sender, EventArgs e)
        {
            curAgent = lstBxAgents.SelectedItem.ToString().Substring(0,lstBxAgents.SelectedItem.ToString().IndexOf(" -- "));
            TaskTrayApplication.Properties.Settings.Default.AgentID = curAgent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _serverPath = txtBxServerPath.Text;
            _searchPath = txtBxSearchPath.Text;
            _userAttr = txtBxUserAttr.Text;
            _savedAgent = curAgent;

            TaskTrayApplication.Properties.Settings.Default.LDAPserver = _serverPath;
            TaskTrayApplication.Properties.Settings.Default.SearchPath = _searchPath;
            TaskTrayApplication.Properties.Settings.Default.UserAttr = _userAttr;
            TaskTrayApplication.Properties.Settings.Default.AgentID = _savedAgent;
            
        }

        private void AKcomSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            _objCPCC = null;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //int size = WritePrivateProfileString("CurrentAgent", "Name", curAgent, "c:\\Program Files\\AKcom\\AKtest\\AKcom.ini");
        //    config.Write("CurrentAgent", curAgent);
        //}


    }
}