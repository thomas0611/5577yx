using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Web.Mvc;
using System.Xml;

namespace Game.Controllers.Admin
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        WebInfoManager wim = new WebInfoManager();
        LinksManager lm = new LinksManager();

        public ActionResult DelWebInfo(int CompetenceId, int WebInfoId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, CompetenceId))
                {
                    sys_onepage wi = wim.GetWebInfo(WebInfoId);
                    ViewData["modelname"] = wi.modelname;
                    ViewData["title"] = wi.title;
                    ViewData["contents"] = wi.contents;
                    ViewData["sort_id"] = wi.sort_id;
                    ViewData["seo_title"] = wi.seo_title;
                    ViewData["seo_keyword"] = wi.seo_keyword;
                    ViewData["seo_desc"] = wi.seo_desc;
                    ViewData["img_url"] = wi.img_url;
                    ViewData["id"] = wi.id;
                    ViewData["style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, int.Parse(CompetenceId + "1")))
                    {
                        ViewData["style"] = "display:block";
                    }
                    return View("WebInfo");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult IndexPic()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1117))
                {
                    string[] pic = new string[5];
                    string filePath = Utils.GetXmlMapPath("Picpath");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode xn = doc.SelectSingleNode("models");
                    int i = 0;
                    foreach (XmlElement xe in xn.ChildNodes)
                    {
                        if (xe.NodeType != XmlNodeType.Comment && xe.Name.ToLower() == "model")
                        {
                            pic[i] = xe.Attributes["name"].Value + "|" + xe.Attributes["url"].Value + "|" + xe.Attributes["url1"].Value + "|" + xe.Attributes["link"].Value;
                            i++;
                        }
                    }
                    ViewData["txtImgUrl1"] = pic[0].Split('|')[1];
                    ViewData["TextBox1"] = pic[0].Split('|')[3];
                    ViewData["txtImgUrl2"] = pic[1].Split('|')[1];
                    ViewData["TextBox2"] = pic[1].Split('|')[3];
                    ViewData["txtImgUrl3"] = pic[2].Split('|')[1];
                    ViewData["TextBox3"] = pic[2].Split('|')[3];
                    ViewData["txtImgUrl4"] = pic[3].Split('|')[1];
                    ViewData["TextBox4"] = pic[3].Split('|')[3];
                    ViewData["txtImgUrl5"] = pic[4].Split('|')[1];
                    ViewData["TextBox5"] = pic[4].Split('|')[3];

                    ViewData["style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 11171))
                    {
                        ViewData["style"] = "display:block";
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult SysConfig()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 141))
                {
                    SiteConfig sc = new SiteConfigManager().loadConfig(Utils.GetXmlMapPath("Configpath"));
                    ViewData["webname"] = sc.webname;
                    ViewData["webcompany"] = sc.webcompany;
                    ViewData["weburl"] = sc.weburl;
                    ViewData["webtel"] = sc.webtel;
                    ViewData["webfax"] = sc.webfax;
                    ViewData["webmail"] = sc.webmail;
                    ViewData["webcrod"] = sc.webcrod;
                    ViewData["webtitle"] = sc.webtitle;
                    ViewData["webkeyword"] = sc.webkeyword;
                    ViewData["webdescription"] = sc.webdescription;
                    ViewData["webcopyright"] = sc.webcopyright;

                    ViewData["attachpath"] = sc.attachpath;
                    ViewData["attachextension"] = sc.attachextension;
                    ViewData["attachsave"] = sc.attachsave;
                    ViewData["attachfilesize"] = sc.attachfilesize;
                    ViewData["attachimgsize"] = sc.attachimgsize;
                    ViewData["attachimgmaxheight"] = sc.attachimgmaxheight;
                    ViewData["attachimgmaxwidth"] = sc.attachimgmaxwidth;
                    ViewData["thumbnailheight"] = sc.thumbnailheight;
                    ViewData["thumbnailwidth"] = sc.thumbnailwidth;
                    ViewData["watermarktype"] = sc.watermarktype;
                    ViewData["watermarkposition"] = sc.watermarkposition;
                    ViewData["watermarkimgquality"] = sc.watermarkimgquality;
                    ViewData["watermarkpic"] = sc.watermarkpic;
                    ViewData["watermarktransparency"] = sc.watermarktransparency;
                    ViewData["watermarktext"] = sc.watermarktext;
                    ViewData["watermarkfont"] = sc.watermarkfont;
                    ViewData["watermarkfontsize"] = sc.watermarkfontsize;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult Links()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 143))
                {
                    ViewData["style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 1432))
                    {
                        ViewData["style"] = "display:block";
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        [ValidateInput(false)]
        public bool UpdateWebInfo()
        {
            sys_onepage wi = new sys_onepage();
            wi.modelname = Request["modelname"];
            wi.title = Request["title"];
            wi.contents = Request["contents"];
            wi.sort_id = int.Parse(Request["sort_id"]);
            wi.seo_title = Request["seo_title"];
            wi.seo_keyword = Request["seo_keyword"];
            wi.seo_desc = Request["seo_desc"];
            wi.img_url = Request["img_url"];
            wi.id = int.Parse(Request["id"]);
            return wim.UpdateWebInfo(wi);
        }

        public bool UpdateIndexPic()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1117))
                {
                    string filePath = Utils.GetXmlMapPath("Picpath");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.SelectSingleNode("models");
                    root.RemoveAll();
                    XmlElement xe1 = doc.CreateElement("model");
                    //    xe1.SetAttribute("name", txttitle1.Text);
                    xe1.SetAttribute("name", "");
                    xe1.SetAttribute("url", Request["txtImgUrl1"]);
                    //    xe1.SetAttribute("url1", txtsmallimg1.Text);
                    xe1.SetAttribute("url1", "");
                    xe1.SetAttribute("link", Request["TextBox1"]);
                    root.AppendChild(xe1);
                    XmlElement xe2 = doc.CreateElement("model");
                    //     xe2.SetAttribute("name", txttitle2.Text);
                    xe2.SetAttribute("name", "");
                    xe2.SetAttribute("url", Request["txtImgUrl2"]);
                    //     xe2.SetAttribute("url1", txtsmallimg2.Text);
                    xe2.SetAttribute("url1", "");
                    xe2.SetAttribute("link", Request["TextBox2"]);
                    root.AppendChild(xe2);
                    XmlElement xe3 = doc.CreateElement("model");
                    //     xe3.SetAttribute("name", txttitle3.Text);
                    xe3.SetAttribute("name", "");
                    xe3.SetAttribute("url", Request["txtImgUrl3"]);
                    //     xe3.SetAttribute("url1", txtsmallimg3.Text);
                    xe3.SetAttribute("url1", "");
                    xe3.SetAttribute("link", Request["TextBox3"]);
                    root.AppendChild(xe3);
                    XmlElement xe4 = doc.CreateElement("model");
                    //     xe4.SetAttribute("name", txttitle4.Text);
                    xe4.SetAttribute("name", "");
                    xe4.SetAttribute("url", Request["txtImgUrl4"]);
                    //      xe4.SetAttribute("url1", txtsmallimg4.Text);
                    xe4.SetAttribute("url1", "");
                    xe4.SetAttribute("link", Request["TextBox4"]);
                    root.AppendChild(xe4);
                    XmlElement xe5 = doc.CreateElement("model");
                    //     xe5.SetAttribute("name", txttitle5.Text);
                    xe5.SetAttribute("name", "");
                    xe5.SetAttribute("url", Request["txtImgUrl5"]);
                    //     xe5.SetAttribute("url1", txtsmallimg5.Text);
                    xe5.SetAttribute("url1", "");
                    xe5.SetAttribute("link", Request["TextBox5"]);
                    root.AppendChild(xe5);
                    doc.Save(filePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult SysCompetence()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 146))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult SendMsg()
        {
            return View();
        }

        public ActionResult LinkEdit(int Id)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1431))
                {
                    link l = new link();
                    l = lm.GetLink(Id);
                    ViewData["LinkTitle"] = l.Title;
                    ViewData["Is_lock"] = l.Is_lock;
                    ViewData["Is_red"] = l.Is_red;
                    ViewData["SortId"] = l.Sort_id;
                    ViewData["UserName"] = l.User_name;
                    ViewData["Tel"] = l.User_tel;
                    ViewData["Email"] = l.Email;
                    ViewData["Url"] = l.Site_url;
                    ViewData["ImgUrl"] = l.Img_url;
                    ViewData["LinkId"] = l.Id;
                    ViewData["Function"] = "UpdateData('/Settings/UpdateLink')";
                    return View("Link");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean UpdateLink()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1431))
                {
                    link l = new link();
                    l.Title = Request["LinkTitle"];
                    l.Id = int.Parse(Request["LinkId"]);
                    l.Is_lock = int.Parse(Request["Is_lock"]);
                    l.Is_red = int.Parse(Request["Is_red"]);
                    l.Sort_id = int.Parse(Request["SortId"]);
                    l.User_name = Request["UserName"];
                    l.User_tel = Request["Tel"];
                    l.Email = Request["Email"];
                    l.Site_url = Request["Url"];
                    l.Img_url = Request["ImgUrl"];
                    l.Is_image = string.IsNullOrEmpty(l.Img_url) ? 0 : 1;
                    return lm.UpdateLink(l);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult AddLink()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1432))
                {
                    ViewData["Function"] = "AddData('/Settings/DoAddLink')";
                    ViewData["SortId"] = 99;
                    return View("Link");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoAddLink()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1432))
                {
                    link l = new link();
                    l.Title = Request["LinkTitle"];
                    l.Is_lock = int.Parse(Request["Is_lock"]);
                    l.Is_red = int.Parse(Request["Is_red"]);
                    l.Sort_id = int.Parse(Request["SortId"]);
                    l.User_name = Request["UserName"];
                    l.User_tel = Request["Tel"];
                    l.Email = Request["Email"];
                    l.Site_url = Request["Url"];
                    l.Img_url = Request["ImgUrl"];
                    return lm.AddLink(l);
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DelLink(int Id)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1433))
                {
                    return lm.DelLink(Id);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
