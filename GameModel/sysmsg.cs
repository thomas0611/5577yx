using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    public partial class sysmsg
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _type;

        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _title;

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _msg;

        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        private int _state;

        public int state
        {
            get { return _state; }
            set { _state = value; }
        }
        private int _userid;

        public int userid
        {
            get { return _userid; }
            set { _userid = value; }
        }
        private int _fromid;

        public int fromid
        {
            get { return _fromid; }
            set { _fromid = value; }
        }
        private DateTime _addtime;

        public DateTime addtime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        private int _msgid;

        public int msgid
        {
            get { return _msgid; }
            set { _msgid = value; }
        }


    }
}
