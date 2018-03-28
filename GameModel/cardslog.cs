using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    [Serializable]
    public partial class cardslog
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
        private DateTime _curtime;

        public DateTime curtime
        {
            get { return _curtime; }
            set { _curtime = value; }
        }
        private int _cardid;

        public int cardid
        {
            get { return _cardid; }
            set { _cardid = value; }
        }
        private int _cardsid;

        public int cardsid
        {
            get { return _cardsid; }
            set { _cardsid = value; }
        }

    }
}
