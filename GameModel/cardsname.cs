using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    [Serializable]
    public partial class cardsname
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _gameid;

        public int gameid
        {
            get { return _gameid; }
            set { _gameid = value; }
        }
        private string _cardname;

        public string cardname
        {
            get { return _cardname; }
            set { _cardname = value; }
        }
        private string _carddesc;

        public string carddesc
        {
            get { return _carddesc; }
            set { _carddesc = value; }
        }
        private string _urls;

        public string urls
        {
            get { return _urls; }
            set { _urls = value; }
        }
        private int _islock;

        public int islock
        {
            get { return _islock; }
            set { _islock = value; }
        }
        private string _gamename;

        public string gamename
        {
            get { return _gamename; }
            set { _gamename = value; }
        }
        private string _img;

        public string img
        {
            get { return _img; }
            set { _img = value; }
        }

        private int _serverid;

        public int serverid
        {
            get { return _serverid; }
            set { _serverid = value; }
        }

        private string _servername;

        public string servername
        {
            get { return _servername; }
            set { _servername = value; }
        }

    }
}
