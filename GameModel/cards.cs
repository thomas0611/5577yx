using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    [Serializable]
    public partial class cards
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _cardnum;

        public string cardnum
        {
            get { return _cardnum; }
            set { _cardnum = value; }
        }
        private int _cardnameid;

        public int cardnameid
        {
            get { return _cardnameid; }
            set { _cardnameid = value; }
        }
        private int _state;

        public int state
        {
            get { return _state; }
            set { _state = value; }
        }
        private DateTime _addtime;

        public DateTime addtime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
    }
}
