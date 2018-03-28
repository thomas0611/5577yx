using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Common
{
    public class XmlHelper
    {
        #region 增、删、改操作==============================================

        /// <summary>
        /// 追加节点
        /// </summary>
        /// <param name="filePath">XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="xmlNode">XmlNode节点</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, XmlNode xmlNode)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNode n = doc.ImportNode(xmlNode, true);
                xn.AppendChild(n);
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从XML文档中读取节点追加到另一个XML文档中
        /// </summary>
        /// <param name="filePath">需要读取的XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="toFilePath">被追加节点的XML文档绝对路径</param>
        /// <param name="toXPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, string toFilePath, string toXPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(toFilePath);
                XmlNode xn = doc.SelectSingleNode(toXPath);

                XmlNodeList xnList = ReadNodes(filePath, xPath);
                if (xnList != null)
                {
                    foreach (XmlElement xe in xnList)
                    {
                        XmlNode n = doc.ImportNode(xe, true);
                        xn.AppendChild(n);
                    }
                    doc.Save(toFilePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改节点的InnerText的值
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="value">节点的值</param>
        /// <returns></returns>
        public static bool UpdateNodeInnerText(string filePath, string xPath, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = value;
                doc.Save(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion 增、删、改操作

        #region 扩展方法===================================================
        /// <summary>
        /// 读取XML的所有子节点
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static XmlNodeList ReadNodes(string filePath, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNodeList xnList = xn.ChildNodes;  //得到该节点的子节点
                return xnList;
            }
            catch
            {
                return null;
            }
        }


        //解析Xml

        public static Dictionary<string, string> ReadXml(string xml)
        {
            XmlDocument MyXml = new XmlDocument();
            MyXml.LoadXml(xml);
            XmlNode ShowList = MyXml.DocumentElement;
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (XmlNode node in ShowList.ChildNodes)
            {
                foreach (XmlNode item in node.Attributes)
                {
                    list.Add(item.Name, item.InnerText);
                }
            }
            return list;
        }

        public static Dictionary<string, string> ReadXml2(string xml)
        {
            XmlDocument MyXml = new XmlDocument();
            MyXml.LoadXml(decodeString(xml));
            XmlNode ShowList = MyXml.DocumentElement;
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (XmlNode node in ShowList.ChildNodes)
            {
                if (node.HasChildNodes)
                {
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        list.Add(item.Name, item.InnerText);
                    }
                }
            }
            return list;
        }
        public static String encodeString(String strData)
        {
            if (strData == null)
            {
                return "";
            }
            strData = replaceString(strData, "&", "&amp;");
            strData = replaceString(strData, "<", "&lt;");
            strData = replaceString(strData, ">", "&gt;");
            strData = replaceString(strData, "&apos;", "&apos;");
            strData = replaceString(strData, "\"", "&quot;");
            return strData;
        }
        public static String decodeString(String strData)
        {
            strData = replaceString(strData, "&lt;", "<");
            strData = replaceString(strData, "&gt;", ">");
            strData = replaceString(strData, "&apos;", "&apos;");
            strData = replaceString(strData, "&quot;", "\"");
            strData = replaceString(strData, "&amp;", "&");
            return strData;
        }
        public static String replaceString(String strData, String regex,
         String replacement)
        {
            if (strData == null)
            {
                return null;
            }
            int index;
            index = strData.IndexOf(regex);
            String strNew = "";
            if (index >= 0)
            {
                while (index >= 0)
                {
                    strNew += strData.Substring(0, index) + replacement;
                    strData = strData.Substring(index + regex.Length);
                    index = strData.IndexOf(regex);
                }
                strNew += strData;
                return strNew;
            }
            return strData;
        }
        #endregion 扩展方法
    }
}
