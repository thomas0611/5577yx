using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Common
{
    public partial class SendHelper
    {
        //发送邮件
        public void SendEmail(string sendemail, string title, string content)
        {
            string fromemail = ConfigurationManager.AppSettings["email"].ToString();
            string pwd = ConfigurationManager.AppSettings["password"].ToString();
            string serveraddress = ConfigurationManager.AppSettings["emailserver"].ToString();
            string fromtitle = ConfigurationManager.AppSettings["emailuser"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());

            Encoding encoding = Encoding.GetEncoding(936);
            MailMessage Message = new MailMessage(
            new MailAddress(fromemail, fromtitle, encoding),//第一个是发信人的地址，第二个参数是显示的发信人   
            new MailAddress(sendemail));//收信人邮箱
            Message.SubjectEncoding = encoding;
            Message.Subject = title;//标题
            Message.BodyEncoding = encoding;
            Message.IsBodyHtml = true;//邮箱主体识别html语言
            Message.Body = content;//邮件主体
            SmtpClient smtpClient;
            if (port == 0)
            {
                smtpClient = new SmtpClient(serveraddress);//信箱服务器
            }
            else
            {
                smtpClient = new SmtpClient(serveraddress, port);//信箱服务器
            }
            smtpClient.Credentials = new NetworkCredential(fromemail, pwd);//信箱的用户名和密码
            smtpClient.Timeout = 99999;
            smtpClient.Send(Message);
        }


        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpserver">SMTP服务器</param>
        /// <param name="userName">登录帐号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="nickName">发件人昵称</param>
        /// <param name="strfrom">发件人</param>
        /// <param name="strto">收件人</param>
        /// <param name="subj">主题</param>
        /// <param name="bodys">内容</param>
        public void sendMail(string strto, string subj, string bodys)
        {
            string userName = ConfigurationManager.AppSettings["email"].ToString();
            string pwd = ConfigurationManager.AppSettings["password"].ToString();
            string smtpserver = ConfigurationManager.AppSettings["emailserver"].ToString();
            string nickName = ConfigurationManager.AppSettings["emailuser"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["port"].ToString());

            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = smtpserver;//指定SMTP服务器
            _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码

            //MailMessage _mailMessage = new MailMessage(strfrom, strto);
            MailAddress _from = new MailAddress(userName, nickName);
            MailAddress _to = new MailAddress(strto);
            MailMessage _mailMessage = new MailMessage(_from, _to);
            _mailMessage.Subject = subj;//主题
            _mailMessage.Body = bodys;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式
            _mailMessage.Priority = MailPriority.Normal;//优先级
            _smtpClient.Send(_mailMessage);
        }
        #endregion

    }
}
