using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    [Serializable]
    public partial class sys_onepage
    {
        private int _id;
        private string _title;
        private string _contents;
        private int _sort_id;
        private string _seo_title;
        private string _img_url;
        private string _modelname;

        public string modelname
        {
            get { return _modelname; }
            set { _modelname = value; }
        }

        public string img_url
        {
            get { return _img_url; }
            set { _img_url = value; }
        }
        public string seo_title
        {
            get { return _seo_title; }
            set { _seo_title = value; }
        }
        private string _seo_keyword;

        public string seo_keyword
        {
            get { return _seo_keyword; }
            set { _seo_keyword = value; }
        }
        private string _seo_desc;

        public string seo_desc
        {
            get { return _seo_desc; }
            set { _seo_desc = value; }
        }
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string contents
        {
            get { return _contents; }
            set { _contents = value; }
        }


        public int sort_id
        {
            get { return _sort_id; }
            set { _sort_id = value; }
        }


    }
}
