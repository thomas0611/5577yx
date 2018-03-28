using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    public partial class validatecode
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _userid;

        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _type;

        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _code;

        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        private DateTime _sendtime;

        public DateTime sendtime
        {
            get { return _sendtime; }
            set { _sendtime = value; }
        }

        private string _email;

        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _phone;

        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

    }
}
