using System;
using System.Collections.Generic;
using System.Text;

namespace TaskTrayApplication
{
    class AKcomObj
    {

        private string _loginID;
        public string LoginID
        {
            get { return _loginID; }
            set { _loginID = value; }
        }


        private bool _lbCSRMode;
        public bool bln_CSRMode
        {
            get { return _lbCSRMode; }
            set { _lbCSRMode = value; }
        }


        private bool _lbReadyMode;
        public bool bln_ReadyMode
        {
            get { return _lbReadyMode; }
            set { _lbReadyMode = value; }
        }



    }
}
