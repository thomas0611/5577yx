using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Model
{
    /// <summary>
    //友情链接:实体类
    /// </summary>
    [Serializable]
    public partial class link
    {
        private int _id;    //标识列
        private string _title;    //标题
        private string _user_name;    //名称
        private string _user_tel;    //用户电话
        private string _email;    //邮箱
        private string _site_url;    //连接地址
        private string _img_url;    //图片地址
        private int _is_image;    //是否有图片
        private int _sort_id;    //排序
        private int _is_red;    //是否推荐
        private int _is_lock;    //是否可用
        private DateTime _add_time;    //添加时间


        /// <summary>
        /// 标识列
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get { return _title; } set { _title = value; } }

        /// <summary>
        /// 名称
        /// </summary>
        public string User_name { get { return _user_name; } set { _user_name = value; } }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string User_tel { get { return _user_tel; } set { _user_tel = value; } }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get { return _email; } set { _email = value; } }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Site_url { get { return _site_url; } set { _site_url = value; } }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string Img_url { get { return _img_url; } set { _img_url = value; } }

        /// <summary>
        /// 是否有图片
        /// </summary>
        public int Is_image { get { return _is_image; } set { _is_image = value; } }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort_id { get { return _sort_id; } set { _sort_id = value; } }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public int Is_red { get { return _is_red; } set { _is_red = value; } }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int Is_lock { get { return _is_lock; } set { _is_lock = value; } }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Add_time { get { return _add_time; } set { _add_time = value; } }
    }
}
