using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.yeepay.icc;
using com.yeepay.utils;
using Common;
using Game.Manager;
using Game.Model;
using System.Configuration;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace Game.Controllers
{
    public class PayCenterController : Controller
    {
        //
        // GET: /PayCenter/
        public string result_info = string.Empty;
        public bool isok = false;
        Orders order = new Orders();
        OrderManager om = new OrderManager();
        GamesManager gm = new GamesManager();
        GameUserManager gum = new GameUserManager();
        LotteryManager lm = new LotteryManager();

        public ActionResult Index()
        {
            ViewData["WyOn"] = "chosen";
            return View();
        }

        public ActionResult Ptb()
        {
            ViewData["PtbOn"] = "chosen";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Dhq()
        {
            ViewData["DhqOn"] = "chosen";
            return View();
        }

        public ActionResult Zfb()
        {
            ViewData["ZfbOn"] = "chosen";
            return View();
        }

        public ActionResult Qb()
        {
            ViewData["QbOn"] = "chosen";
            return View();
        }

        public ActionResult Szx()
        {
            ViewData["SzxOn"] = "chosen";
            return View();
        }

        public ActionResult Lt()
        {
            ViewData["LtOn"] = "chosen";
            return View();
        }

        public ActionResult Dx()
        {
            ViewData["DxOn"] = "chosen";
            return View();
        }

        public ActionResult Jw()
        {
            ViewData["JwOn"] = "chosen";
            return View();
        }

        public ActionResult Hfb()
        {
            ViewData["HfbOn"] = "chosen";
            return View();
        }

        public ActionResult Sdk()
        {
            ViewData["SdkOn"] = "chosen";
            return View();
        }

        public ActionResult WyYkt()
        {
            ViewData["WyYktOn"] = "chosen";
            return View();
        }

        public ActionResult WmYkt()
        {
            ViewData["WmYktOn"] = "chosen";
            return View();
        }

        public ActionResult Rgcz()
        {
            ViewData["RgczOn"] = "chosen";
            return View();
        }

        /// <summary>
        /// 返利币
        /// </summary>
        /// <returns></returns>
        public ActionResult Flb()
        {
            ViewData["FlbOn"] = "chosen";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                ViewData["userName"] = gu.UserName;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult CallBack()
        {
            Buy.logstr(FormatQueryString.GetQueryString("r6_Order"), Request.Url.Query, "");
            BuyCallbackResult result = Buy.VerifyCallback(FormatQueryString.GetQueryString("p1_MerId"), FormatQueryString.GetQueryString("r0_Cmd"), FormatQueryString.GetQueryString("r1_Code"), FormatQueryString.GetQueryString("r2_TrxId"),
                FormatQueryString.GetQueryString("r3_Amt"), FormatQueryString.GetQueryString("r4_Cur"), FormatQueryString.GetQueryString("r5_Pid"), FormatQueryString.GetQueryString("r6_Order"), FormatQueryString.GetQueryString("r7_Uid"),
                FormatQueryString.GetQueryString("r8_MP"), FormatQueryString.GetQueryString("r9_BType"), FormatQueryString.GetQueryString("rp_PayDate"), FormatQueryString.GetQueryString("hmac"));

            if (string.IsNullOrEmpty(result.ErrMsg))
            {
                //在接收到支付结果通知后，判断是否进行过业务逻辑处理，不要重复进行业务逻辑处理
                if (result.R1_Code == "1")
                {
                    order = om.GetOrder(result.R6_Order);
                    if (result.R9_BType == "1")
                    {
                        try
                        {
                            if (order.State == 0)
                            {
                                if (om.UpdateOrder(result.R6_Order))
                                {
                                    result_info = "您的订单：" + result.R6_Order + "已支付成功！";
                                    isok = true;
                                    //gu = gum.GetGameUser(order.UserName);
                                    GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                    if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                    #region  返利券暂未开放
                                    //是否使用返利券
                                    //if (order.RebateId > 0)
                                    //{
                                    //    //返利券暂未开放
                                    //}
                                    //else
                                    //{
                                    //    if (order.ConvertId > 0)
                                    //    {
                                    //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                    //    }
                                    //    else
                                    //    {
                                    //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                    //        if (!string.IsNullOrEmpty(scale1))
                                    //        {
                                    //            //生成返利券
                                    //            Model.rebatenum rebatenum = new Model.rebatenum();
                                    //            Random ran = new Random();
                                    //            string code5 = ran.Next(10, 99).ToString();
                                    //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                    //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                    //            rebatenum.pwd = "000";
                                    //            rebatenum.num = int.Parse(scale1);
                                    //            rebatenum.userid = gu.Id;
                                    //            rebatenum.typeid = 0;
                                    //            new DAL.rebatenum().Add(rebatenum);
                                    //        }
                                    //    }
                                    //}
                                    #endregion
                                    //根据订单进行充值
                                    result_info = gm.PayManager(result.R6_Order);
                                }
                                else
                                {
                                    result_info = "您的订单：" + result.R6_Order + "更新订单状态失败！";
                                }
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        catch (Exception ex)
                        {
                            result_info = "出错啦！" + ex.Message;
                        }

                    }
                    else if (result.R9_BType == "2")
                    {
                        Response.Write("SUCCESS");
                        try
                        {
                            if (order.State == 0)
                            {
                                if (om.UpdateOrder(result.R6_Order))
                                {
                                    result_info = "您的订单：" + result.R6_Order + "已支付成功！";
                                    isok = true;
                                    //gu = gum.GetGameUser(order.UserName);
                                    GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                    if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                    #region  返利券暂未开放
                                    //是否使用返利券
                                    //if (order.RebateId > 0)
                                    //{
                                    //    //返利券暂未开放
                                    //}
                                    //else
                                    //{
                                    //    if (order.ConvertId > 0)
                                    //    {
                                    //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                    //    }
                                    //    else
                                    //    {
                                    //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                    //        if (!string.IsNullOrEmpty(scale1))
                                    //        {
                                    //            //生成返利券
                                    //            Model.rebatenum rebatenum = new Model.rebatenum();
                                    //            Random ran = new Random();
                                    //            string code5 = ran.Next(10, 99).ToString();
                                    //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                    //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                    //            rebatenum.pwd = "000";
                                    //            rebatenum.num = int.Parse(scale1);
                                    //            rebatenum.userid = gu.Id;
                                    //            rebatenum.typeid = 0;
                                    //            new DAL.rebatenum().Add(rebatenum);
                                    //        }
                                    //    }
                                    //}
                                    #endregion
                                    //根据订单进行充值
                                    result_info = gm.PayManager(result.R6_Order);
                                }
                                else
                                {
                                    result_info = "您的订单：" + result.R6_Order + "更新订单状态失败！";
                                }
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        catch (Exception ex)
                        {
                            result_info = "出错啦！" + ex.Message;
                        }
                    }
                }
                else
                {
                    result_info = "支付失败！";
                }
            }
            else
            {
                result_info = "交易签名无效!";
            }
            ViewData["ImgUrl"] = "../Images/onebit_33.png";
            if (isok)
            {
                ViewData["ImgUrl"] = "../Images/onebit_34.png";
            }
            ViewData["Msg"] = result_info;
            return View();
        }

        public void AirPayReq()
        {
            SortedDictionary<string, string> sPara = GetRequestGet();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);
                if (verifyResult)//验证成功
                {
                    //商户订单号
                    string out_trade_no = Request.QueryString["out_trade_no"];
                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];
                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];
                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        try
                        {
                            order = om.GetOrder(out_trade_no);
                            if (order.State == 0)
                            {
                                if (om.UpdateOrder(out_trade_no))
                                {
                                    result_info = "您的订单：" + out_trade_no + "已支付成功！";
                                    isok = true;
                                    GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                    if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                    else if (order.PayMoney >= 2000) UpdateUserVip(order.UserName);      //升级用户为VIP
                                    #region  返利券暂未开放
                                    //是否使用返利券
                                    //if (order.RebateId > 0)
                                    //{
                                    //    //返利券暂未开放
                                    //}
                                    //else
                                    //{
                                    //    if (order.ConvertId > 0)
                                    //    {
                                    //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                    //    }
                                    //    else
                                    //    {
                                    //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                    //        if (!string.IsNullOrEmpty(scale1))
                                    //        {
                                    //            //生成返利券
                                    //            Model.rebatenum rebatenum = new Model.rebatenum();
                                    //            Random ran = new Random();
                                    //            string code5 = ran.Next(10, 99).ToString();
                                    //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                    //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                    //            rebatenum.pwd = "000";
                                    //            rebatenum.num = int.Parse(scale1);
                                    //            rebatenum.userid = gu.Id;
                                    //            rebatenum.typeid = 0;
                                    //            new DAL.rebatenum().Add(rebatenum);
                                    //        }
                                    //    }
                                    //}
                                    #endregion
                                    //根据订单进行充值
                                    result_info = gm.PayManager(out_trade_no);
                                }
                                else
                                {
                                    result_info = "您的订单：" + out_trade_no + "更新订单状态失败！";
                                }
                            }
                            else
                            {
                                Response.Redirect("/Home/Index");
                            }
                        }
                        catch (Exception ex)
                        {
                            result_info = "出错啦！" + ex.Message;
                        }
                        Response.Write("success");  //请不要修改或删除
                        //成功状态
                    }
                }
                //失败状态
            }
            else
            {
                result_info = "无返回参数";
                Response.Write("无返回参数");
            }
            Session["AirPayinfo"] = isok + "|" + result_info;
            Response.Redirect("AirPay");
        }

        public void AirPayMsg()
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];


                    if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                        try
                        {
                            order = om.GetOrder(out_trade_no);
                            if (order.State == 0)
                            {
                                if (om.UpdateOrder(out_trade_no))
                                {
                                    result_info = "您的订单：" + out_trade_no + "已支付成功！";
                                    isok = true;
                                    GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                    if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                    else if (order.PayMoney >= 2000) UpdateUserVip(order.UserName);      //升级用户为VIP
                                    //gu = gum.GetGameUser(order.UserName);
                                    #region  返利券暂未开放
                                    //是否使用返利券
                                    //if (order.RebateId > 0)
                                    //{
                                    //    //返利券暂未开放
                                    //}
                                    //else
                                    //{
                                    //    if (order.ConvertId > 0)
                                    //    {
                                    //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                    //    }
                                    //    else
                                    //    {
                                    //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                    //        if (!string.IsNullOrEmpty(scale1))
                                    //        {
                                    //            //生成返利券
                                    //            Model.rebatenum rebatenum = new Model.rebatenum();
                                    //            Random ran = new Random();
                                    //            string code5 = ran.Next(10, 99).ToString();
                                    //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                    //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                    //            rebatenum.pwd = "000";
                                    //            rebatenum.num = int.Parse(scale1);
                                    //            rebatenum.userid = gu.Id;
                                    //            rebatenum.typeid = 0;
                                    //            new DAL.rebatenum().Add(rebatenum);
                                    //        }
                                    //    }
                                    //}
                                    #endregion
                                    //根据订单进行充值
                                    result_info = gm.PayManager(out_trade_no);
                                }
                                else
                                {
                                    result_info = "您的订单：" + out_trade_no + "更新订单状态失败！";
                                }
                            }
                            else
                            {
                                Response.Redirect("/Home/Index");
                            }
                        }
                        catch (Exception ex)
                        {
                            result_info = "出错啦！" + ex.Message;
                        }
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
                        result_info = "验证失败";
                        Response.Write("fail");
                    }
                }
                else//验证失败
                {
                    result_info = "无返回参数";
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
            Session["AirPayinfo"] = isok + "|" + result_info;
            Response.Redirect("AirPay");
        }

        public ActionResult PayOrder()
        {
            string error = string.Empty;
            string OrderNo = DESEncrypt.decryptstring1(BBRequest.GetQueryString("Order"));
            string Bank = BBRequest.GetQueryString("Bank");
            try
            {
                if (string.IsNullOrEmpty(OrderNo))
                {
                    error = "您的订单存在安全风险，请查看订单状态或稍后重试。";
                }
                order = om.GetOrder(OrderNo);
                ViewData["Money"] = order.PayMoney;
                string Type = "";
                if (order.Type == 1)
                {
                    Type = "游戏币";
                }
                else
                {
                    Type = "平台币";
                }
                string PayType = "";
                string Function = "";
                string Action = "";
                switch (order.PayTypeId)
                {
                    case 1:
                        //if (!string.IsNullOrEmpty(Bank))
                        //{
                        //    // PayType = Bank;
                        //}
                        //else
                        //{
                        //    error = "您的订单存在安全风险，请查看订单状态或稍后重试。";
                        //}
                        PayType = "0";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 2:
                        //PayType = "SZX-NET";
                        //Action = "action=\"PayReq\"";
                        //YeePayOrder(Type, PayType);
                        PayType = "13";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 3:
                        //PayType = "UNICOM-NET";
                        //Action = "action=\"PayReq\"";
                        //YeePayOrder(Type, PayType);
                        PayType = "14";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 4:
                        //PayType = "TELECOM-NET";
                        //Action = "action=\"PayReq\"";
                        //YeePayOrder(Type, PayType);
                        PayType = "15";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 5:
                        PayType = "QQCARD-NET";
                        Action = "action=\"PayReq\"";
                        YeePayOrder(Type, PayType);
                        break;
                    case 9:
                        //PayType = "JUNNET-NET";
                        //Action = "action=\"PayReq\"";
                        //YeePayOrder(Type, PayType);
                        PayType = "10";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 12:
                        PayType = "35";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 13:
                        PayType = "42";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 14:
                        PayType = "44";
                        Action = "action=\"HeePayReq\"";
                        HeePayOrder(Type, PayType);
                        break;
                    case 8:
                        Function = "onsubmit=\"return SubmitOrder()\"";
                        Action = "";
                        AirPayOrder(Type);
                        break;
                    default:
                        break;
                }

                ViewData["Function"] = Function;
                ViewData["Action"] = Action;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            if (string.IsNullOrEmpty(error))
            {
                ViewData["EVisible"] = "display:none";
            }
            else
            {
                ViewData["OVisible"] = "display:none";
            }
            return View();
        }

        public ActionResult PayReq()
        {
            string p1_MerId = Buy.GetMerId();
            string p2_Order;
            string p3_Amt;
            string p4_Cur;
            string p5_Pid;
            string p6_Pcat;
            string p7_Pdesc;
            string p8_Url;
            string p9_SAF;
            string pa_MP;
            string pd_FrpId;
            string pr_NeedResponse;
            string hmac;
            string reqURL_onLine = Buy.GetBuyUrl();

            // 设置 Response编码格式为GB2312
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            //1
            //p2_Order	商户平台订单号
            //若不为""，提交的订单号必须在自身账户交易中唯一;为""时，易宝支付会自动生成随机的商户订单号.
            p2_Order = Request.Form["p2_Order"];
            //p3_Amt	交易金额  精确两位小数，最小值为0.01,为持卡人实际要支付的金额.                 
            p3_Amt = Request.Form["p3_Amt"];
            //p3_Amt = "0.01";
            //交易币种,固定值"CNY".
            p4_Cur = "CNY";
            //商品名称
            //用于支付时显示在易宝支付网关左侧的订单产品信息.
            p5_Pid = Request.Form["p5_Pid"];
            //商品种类
            p6_Pcat = Request.Form["p6_Pcat"];
            //2
            //商品描述
            p7_Pdesc = Request.Form["p7_Pdesc"];
            //商户接收支付成功数据的地址,支付成功后易宝支付会向该地址发送两次成功通知.
            p8_Url = Request.Form["p8_Url"];
            //送货地址
            //为“1”: 需要用户将送货地址留在易宝支付系统;为“0”: 不需要，默认为 ”0”.
            p9_SAF = "0";
            //商户扩展信息
            //商户可以任意填写1K 的字符串,支付成功时将原样返回.	
            pa_MP = Request.Form["pa_MP"];
            //银行编码
            //默认为""，到易宝支付网关.若不需显示易宝支付的页面，直接跳转到各银行、神州行支付、骏网一卡通等支付页面，该字段可依照附录:银行列表设置参数值.
            pd_FrpId = Request.Form["pd_FrpId"];
            //3
            //应答机制
            //默认为"1": 需要应答机制;
            pr_NeedResponse = "1";
            hmac = Buy.CreateBuyHmac(p2_Order, p3_Amt, p4_Cur, p5_Pid, p6_Pcat, p7_Pdesc, p8_Url, p9_SAF, pa_MP, pd_FrpId, pr_NeedResponse);

            ViewData["reqURL_onLine"] = reqURL_onLine;
            ViewData["p1_MerId"] = p1_MerId;
            ViewData["p2_Order"] = p2_Order;
            ViewData["p3_Amt"] = p3_Amt;
            ViewData["p4_Cur"] = p4_Cur;
            ViewData["p5_Pid"] = p5_Pid;
            ViewData["p6_Pcat"] = p6_Pcat;
            ViewData["p7_Pdesc"] = p7_Pdesc;
            ViewData["p8_Url"] = p8_Url;
            ViewData["p9_SAF"] = p9_SAF;
            ViewData["pa_MP"] = pa_MP;
            ViewData["pd_FrpId"] = pd_FrpId;
            ViewData["pr_NeedResponse"] = pr_NeedResponse;
            ViewData["hmac"] = hmac;
            return View();
        }

        public ActionResult AirPay()
        {
            string Info = Session["AirPayinfo"].ToString();
            ViewData["ImgUrl"] = "../Images/onebit_33.png";
            string[] re = Info.Split('|');
            isok = Boolean.Parse(re[0]);
            result_info = re[1];
            if (isok)
            {
                ViewData["ImgUrl"] = "../Images/onebit_34.png";
            }
            ViewData["Msg"] = result_info;
            return View("CallBack");
        }

        public ActionResult HeePayReq()
        {
            string agent_id;
            string pay_type;
            string pay_amt;
            string remark;
            string key;
            string sign;
            int version;
            string agent_bill_id;
            string agent_bill_time;
            string notify_url;
            string return_url;
            string user_ip;
            string goods_name;
            string goods_num;
            string goods_note;
            int is_test;
            string pay_code = string.Empty;
            #region 获取参数值
            version = 1;                                                            //当前接口版本号 1  
            user_ip = BBRequest.GetIP();                                                      //用户所在客户端的真实ip。如 127.127.12.12
            goods_name = HttpUtility.UrlEncode(Request["goodsname"]);                                   //商品名称, 长度最长50字符
            //pay_flag = TypeConvert.GetString(Request["txtpayflag"]); ;                 //支付标记，1=容许其他高于指定支付类型折扣的支付，0=不容许（默认）(如传了此参数，则要参加MD5的验证)
            agent_bill_id = Request["agentbillid"];                              //商户系统内部的定单号（要保证唯一）。长度最长50字符
            goods_note = HttpUtility.UrlEncode(Request["goods_note"]);                                  //支付说明, 长度50字符
            remark = HttpUtility.UrlEncode(Request["remark"]);                                           //商户自定义 原样返回,长度最长50字符
            is_test = 0; //是否测试 1 为测试
            pay_type = Request["pay_type"];                                          //支付类型见7.1表                   
            if ("20".Equals(pay_type))
            {
                pay_code = Request["bank_type"];       //银行
            }
            pay_amt = Request["TradeAmt"];                                       //订单总金额 不可为空，单位：元，小数点后保留两位。12.37
            goods_num = "1";                                     //产品数量,长度最长20字符
            agent_bill_time = DateTime.Now.ToString("yyyyMMddHHmmss");              //提交单据的时间yyyyMMddHHmmss 20100225102000
            agent_id = "1684905";                                                      //商户编号
            key = "46057F3CCD3D4CE68B5C5F9C";                                                          //商户密钥

            /*
            //如果需要测试，请把取消关于is_test的注释  订单会显示详细信息

            is_test = "1";
            */
            Regex _Regex = new Regex("http((.|\n)*?)HeePayReq", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string _URL = _Regex.Match(Request.Url.AbsoluteUri).Value.Replace("HeePayReq", "").Replace("HeePayReq", "");
            notify_url = _URL + "Return";
            return_url = _URL + "Notify";
            string SignStr = "version=" + version + "&agent_id=" + agent_id + "&agent_bill_id=" + agent_bill_id + "&agent_bill_time=" + agent_bill_time + "&pay_type=" + pay_type + "&pay_amt=" + pay_amt + "&notify_url=" + notify_url + "&return_url=" + return_url + "&user_ip=" + user_ip;
            if (is_test == 1)
            {
                SignStr += "&is_test=" + is_test;
            }
            SignStr += "&key=" + key;
            sign = DESEncrypt.Md5(SignStr, 32).ToLower();

            #endregion

            ViewData["version"] = version;
            ViewData["agent_id"] = agent_id;
            ViewData["agent_bill_id"] = agent_bill_id;
            ViewData["agent_bill_time"] = agent_bill_time;
            ViewData["pay_type"] = pay_type;
            ViewData["pay_code"] = pay_code;
            ViewData["pay_amt"] = pay_amt;
            ViewData["notify_url"] = notify_url;
            ViewData["return_url"] = return_url;
            ViewData["user_ip"] = user_ip;
            ViewData["goods_name"] = goods_name;
            ViewData["goods_num"] = goods_num;
            ViewData["goods_note"] = goods_note;
            ViewData["is_test"] = is_test;
            ViewData["remark"] = remark;
            ViewData["sign"] = sign;
            return View();
        }

        /// <summary>
        /// 根据汇付宝返回的支付类型更改订单的支付类型
        /// </summary>
        /// <param name="payType"></param>
        /// <param name="orderNo"></param>
        private void UpdatePayType(string payType, string orderNo)
        {                                                              //修复支付类型的漏洞
            int payTypeId = order.PayTypeId;
            switch (payType)
            {
                case "10":
                    payTypeId = 9;                          //骏网卡支付
                    break;
                case "13":
                    payTypeId = 2;                          //神州行支付
                    break;
                case "14":
                    payTypeId = 3;                          //联通卡支付
                    break;
                case "15":
                    payTypeId = 4;                          //电信卡支付
                    break;
                case "35":
                    payTypeId = 12;                          //盛大卡支付
                    break;
                case "42":
                    payTypeId = 13;                          //网易一卡通
                    break;
                case "44":
                    payTypeId = 14;                          //完美一卡通
                    break;
                default:
                    break;                                    //保持不变
            }
            om.EditOrder(orderNo, payTypeId);
        }

        public void Notify()
        {
            string result = Request["result"];
            string pay_message = Request["pay_message"];
            string agent_id = Request["agent_id"];
            string jnet_bill_no = Request["jnet_bill_no"];
            string agent_bill_id = Request["agent_bill_id"];
            string pay_type = Request["pay_type"];
            string pay_amt = Request["pay_amt"];
            string remark = Request["remark"];
            string returnSign = Request["sign"];
            string SignStr = "result=" + result + "&agent_id=" + agent_id + "&jnet_bill_no=" + jnet_bill_no + "&agent_bill_id=" + agent_bill_id + "&pay_type=" + pay_type + "&pay_amt=" + pay_amt + "&remark=" + remark + "&key=" + "46057F3CCD3D4CE68B5C5F9C";
            string sign = DESEncrypt.Md5(SignStr, 32);

            if (sign.Equals(returnSign))
            {
                try
                {
                    order = om.GetOrder(agent_bill_id);
                    if (float.Parse(pay_amt) == order.PayMoney)   //验证订单金额是否与返回的支付金额相等
                    {
                        if (order.OrderNo.Substring(0, 1) == "V")
                        {
                            om.UpdateOrder(agent_bill_id);
                            UpdateUserVip(order.UserName);
                            return;
                        }
                        UpdatePayType(pay_type, agent_bill_id);       //根据汇付宝返回的支付类型更改订单的支付类型
                        if (order.State == 0)
                        {
                            if (om.UpdateOrder(agent_bill_id))
                            {
                                result_info = "您的订单：" + agent_bill_id + "已支付成功！";
                                isok = true;
                                // gu = gum.GetGameUser(order.UserName);
                                GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                else if (order.PayMoney >= 2000) UpdateUserVip(order.UserName);      //升级用户为VIP
                                #region  返利券暂未开放
                                //是否使用返利券
                                //if (order.RebateId > 0)
                                //{
                                //    //返利券暂未开放
                                //}
                                //else
                                //{
                                //    if (order.ConvertId > 0)
                                //    {
                                //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                //    }
                                //    else
                                //    {
                                //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                //        if (!string.IsNullOrEmpty(scale1))
                                //        {
                                //            //生成返利券
                                //            Model.rebatenum rebatenum = new Model.rebatenum();
                                //            Random ran = new Random();
                                //            string code5 = ran.Next(10, 99).ToString();
                                //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                //            rebatenum.pwd = "000";
                                //            rebatenum.num = int.Parse(scale1);
                                //            rebatenum.userid = gu.Id;
                                //            rebatenum.typeid = 0;
                                //            new DAL.rebatenum().Add(rebatenum);
                                //        }
                                //    }
                                //}
                                #endregion
                                //根据订单进行充值
                                result_info = gm.PayManager(agent_bill_id);
                                // Response.Write("ok");
                            }
                            else
                            {
                                result_info = "您的订单：" + agent_bill_id + "更新订单状态失败！";
                                //Response.Write("ok");
                            }
                        }
                    }
                    else
                    {
                        result_info = "支付金额与订单不匹配！";
                        Response.Write("error");
                    }
                }
                catch (Exception ex)
                {
                    result_info = "出错啦！" + ex.Message;
                }
                Response.Write("ok");
            }
            else
            {
                result_info = "验证失败！";
                Response.Write("error");
            }
            Session["HeePayinfo"] = isok + "|" + result_info;
            Response.Redirect("HeePay");
            Response.End();
        }

        public void Return()
        {
            string result = Request["result"];
            string pay_message = Request["pay_message"];
            string agent_id = Request["agent_id"];
            string jnet_bill_no = Request["jnet_bill_no"];
            string agent_bill_id = Request["agent_bill_id"];
            string pay_type = Request["pay_type"];
            string pay_amt = Request["pay_amt"];
            string remark = Request["remark"];
            string returnSign = Request["sign"];
            string SignStr = "result=" + result + "&agent_id=" + agent_id + "&jnet_bill_no=" + jnet_bill_no + "&agent_bill_id=" + agent_bill_id + "&pay_type=" + pay_type + "&pay_amt=" + pay_amt + "&remark=" + remark + "&key=" + "46057F3CCD3D4CE68B5C5F9C";
            string sign = DESEncrypt.Md5(SignStr, 32);

            if (sign.Equals(returnSign))
            { 
                try
                {
                    order = om.GetOrder(agent_bill_id);

                    if (float.Parse(pay_amt) == order.PayMoney)   //验证订单金额是否与返回的支付金额相等
                    {
                        if (order.OrderNo.Substring(0, 1) == "V")
                        {
                            om.UpdateOrder(agent_bill_id);
                            UpdateUserVip(order.UserName);
                            return;
                        }
                        UpdatePayType(pay_type, agent_bill_id);             //根据汇付宝返回的支付类型更改订单的支付类型
                        if (order.State == 0)
                        {
                            if (om.UpdateOrder(agent_bill_id))
                            {
                                result_info = "您的订单：" + agent_bill_id + "已支付成功！";
                                isok = true;
                                //gu = gum.GetGameUser(order.UserName);
                                GameUser gu = gum.GetGameUser(order.UserName);      //获得充值用户
                                if (gu.GradeId > 0) GetFlb(gu.UserName, order.PayMoney);             //充值用户获取返利币
                                else if (order.PayMoney >= 2000) UpdateUserVip(order.UserName);      //升级用户为VIP
                                #region  返利券暂未开放
                                //是否使用返利券
                                //if (order.RebateId > 0)
                                //{
                                //    //返利券暂未开放
                                //}
                                //else
                                //{
                                //    if (order.ConvertId > 0)
                                //    {
                                //        // new DAL.convertnum().UpdateField(order.convertid, " state=1,usergettime='" + DateTime.Now.ToString() + "'");
                                //    }
                                //    else
                                //    {
                                //        string scale1 = new DAL.rebatetype().GetRange(order.PayMoney);   //获得返利比
                                //        if (!string.IsNullOrEmpty(scale1))
                                //        {
                                //            //生成返利券
                                //            Model.rebatenum rebatenum = new Model.rebatenum();
                                //            Random ran = new Random();
                                //            string code5 = ran.Next(10, 99).ToString();
                                //            string no1 = DateTime.Now.ToString("yyyyMMddhhmmss") + DateTime.Now.Millisecond;
                                //            rebatenum.no = code5 + no1.Substring(2, no1.Length - 2);
                                //            rebatenum.pwd = "000";
                                //            rebatenum.num = int.Parse(scale1);
                                //            rebatenum.userid = gu.Id;
                                //            rebatenum.typeid = 0;
                                //            new DAL.rebatenum().Add(rebatenum);
                                //        }
                                //    }
                                //}
                                #endregion
                                //根据订单进行充值
                                result_info = gm.PayManager(agent_bill_id);
                                Response.Write("ok");
                            }
                            else
                            {
                                result_info = "您的订单：" + agent_bill_id + "更新订单状态失败！";
                                Response.Write("error");
                            }
                        }
                        else
                        {
                            Response.Write("error");
                        }
                    }
                    else
                    {
                        result_info = "支付金额与订单不匹配！";
                        Response.Write("error");
                    }
                }
                catch (Exception ex)
                {
                    result_info = "出错啦！" + ex.Message;
                    Response.Write("error");
                }
            }
            else
            {
                result_info = "验证失败";
                Response.Write("error");
            }
            Session["HeePayinfo"] = isok + "|" + result_info;
           // Response.Redirect("HeePay");
            Response.End();
        }

        public ActionResult HeePay()
        {
            string Info = Session["HeePayinfo"].ToString();
            ViewData["ImgUrl"] = "../Images/onebit_33.png";
            string[] re = Info.Split('|');
            isok = Boolean.Parse(re[0]);
            result_info = re[1];
            if (isok)
            {
                ViewData["ImgUrl"] = "../Images/onebit_34.png";
            }
            ViewData["Msg"] = result_info;
            return View("CallBack");
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        private void YeePayOrder(string Type, string PayType)
        {
            ViewData["PayHtml"] = " <div id=\"dingdan1\"><div id=\"dingdan1left\"></div><p class=\"p0\">订单提交成功，请您尽快付款！</p><p class=\"p1\"><span class=\"s0\">订单号：<label id=\"OrderNo\">" + order.OrderNo + "</label></span><span class=\"s1\"> 应付金额：<a><label id=\"PayMoney\"></label>" + order.PayMoney + "元</a></span><input type=\"hidden\" name=\"p3_Amt\" id=\"p3_Amt\" value=\"" + order.PayMoney + "\" /><input type=\"hidden\" name=\"p2_Order\" id=\"p2_Order\" value=\"" + order.OrderNo + "\" /><input type=\"hidden\" name=\"p5_Pid\" id=\"p5_Pid\" value=\"5577yx平台充值\" /><input type=\"hidden\" name=\"p8_Url\" id=\"p8_Url\" value=\"" + ConfigurationManager.AppSettings["ybreurl"].ToString() + "\" /><input type=\"hidden\" name=\"pd_FrpId\" id=\"pd_FrpId\" value=\"" + PayType + "\" /></p><br /><p class=\"p11\"><span class=\"s0\">充值类型：<label id=\"Type\">" + Type + "</label></span><span class=\"s1\">充值游戏及服务器:<a><label id=\"yxQf\">" + order.GameName + "-" + order.ServerName + "</label></a></span><input type=\"hidden\" name=\"p6_Pcat\" id=\"p6_Pcat\" value=\"" + Type + "\"/><input type=\"hidden\" name=\"p7_Pdesc\" id=\"p7_Pdesc\" value=\"" + order.GameName + "-" + order.ServerName + "\" /></p></div>";
        }

        private void HeePayOrder(string Type, string PayType)
        {
            ViewData["PayHtml"] = "<div id=\"dingdan1\"><div id=\"dingdan1left\"></div><p class=\"p0\">订单提交成功，请您尽快付款！</p><p class=\"p1\"><span class=\"s0\">订单号：<label id=\"OrderNo\">" + order.OrderNo + "</label></span><span class=\"s1\"> 应付金额：<a><label id=\"PayMoney\">" + order.PayMoney + "</label>元</a></span><input type=\"hidden\" name=\"goodsname\" id=\"goodsname\" value=\"" + "5577yx平台" + Type + "充值" + "\" /><input type=\"hidden\" name=\"agentbillid\" id=\"agentbillid\" value=\"" + order.OrderNo + "\"><input type=\"hidden\" name=\"remark\" id=\"remark\" value=\"\" /> <input type=\"hidden\" name=\"pay_type\" id=\"pay_type\" value=\"" + PayType + "\" /><input type=\"hidden\" name=\"TradeAmt\" id=\"TradeAmt\" value=\"" + order.PayMoney + "\" /></p><br /><p class=\"p11\"><span class=\"s0\">充值类型：<label id=\"Type\">" + Type + "</label></span><span class=\"s1\">充值游戏及服务器:<a><label id=\"yxQf\">" + order.GameName + "-" + order.ServerName + "</label></a></span> <input type=\"hidden\" name=\"goodsnum\" id=\"goodsnum\" value=\"\" /><input type=\"hidden\" name=\"goods_note\" id=\"goods_note\" value=\"" + order.GameName + " - " + order.ServerName + "\" /></p></div>";
        }

        private void VipOrder(Orders order)
        {
            ViewData["PayHtml"] = "<input type=\"hidden\" name=\"goodsname\" id=\"goodsname\" value=\"" + "5577yx平台VIP充值" + "\" /> " +
                "<input type=\"hidden\" name=\"agentbillid\" id=\"agentbillid\" value=\"" + order.OrderNo + "\">" +
                "<input type=\"hidden\" name=\"remark\" id=\"remark\" value=\"\" />" +
                "<input type=\"hidden\" name=\"pay_type\" id=\"pay_type\" value=\"" + 0 + "\" />" +
                "<input type=\"hidden\" name=\"TradeAmt\" id=\"TradeAmt\" value=\"" + order.PayMoney + "\" />"+
                "<input type=\"hidden\" name=\"goodsnum\" id=\"goodsnum\" value=\"\" />"+
                "<input type=\"hidden\" name=\"goods_note\" id=\"goods_note\" value=\"" + order.GameName + " - " + order.ServerName + "\" />";
        }

        private void AirPayOrder(string Type)
        {
            ViewData["PayHtml"] = "<div id=\"dingdan1\"><div id=\"dingdan1left\"></div><p class=\"p0\">订单提交成功，请您尽快付款！</p><p class=\"p1\"><span class=\"s0\">订单号：<label id=\"OrderNo\">" + order.OrderNo + "</label></span><span class=\"s1\"> 应付金额：<a><label id=\"PayMoney\"></label>" + order.PayMoney + "元</a></span></p><br /><p class=\"p11\"><span class=\"s0\">充值类型：<label id=\"Type\">" + Type + "</label></span><span class=\"s1\">充值游戏及服务器:<a><label id=\"yxQf\">" + order.GameName + "-" + order.ServerName + "</label></a></span></p></div>";
        }

        public ActionResult Lottery()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                ViewData["UserName"] = gu.UserName;
                ViewData["Points"] = gu.Points;
                ViewData["LotteryInfo"] = GetLotteryInfo();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public string GetLottery()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (lm.DeducUserPoints(gu.UserName))
                {
                    return lm.LotteryRes(gu.UserName);
                }
                else
                {
                    return "0|扣除积分失败！请检查积分余额是否充足！";
                }
            }
            else
            {
                return "0|您还未登陆，或登陆信息已经超时，请重新登陆！";
            }
        }

        public string GetLotteryInfo()
        {
            List<LotteryLog> list = new List<LotteryLog>();
            list = lm.GetLotteryLog();
            string LotteryInfoHtml = "";
            foreach (LotteryLog ll in list)
            {
                LotteryInfoHtml += " <span>恭喜" + ll.UserName.Substring(0, 4) + "****获得" + ll.LotterName + "</span> ";
            }
            return LotteryInfoHtml;
        }

        public ActionResult Vip()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                ViewData["Money"] = "50";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult PayVip()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                Orders order = om.GetOrder("V", "0", 0, 2, 1, gu.UserName, 50, "升级VIP");
                if (om.AddOrder(order))
                {
                    Orders o = om.GetOrder(order.OrderNo);
                    VipOrder(o);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        private void UpdateUserVip(string userName)
        {
            GameUser gu = gum.GetGameUser(userName);
            gu.GradeId = 1;
            try
            {
                if (gum.UpdateUser(gu))
                {
                    Response.Write("ok");
                    Session["HeePayinfo"] = true + "|" + "升级成功";
                    Response.Redirect("HeePay");
                    Response.End();
                }
            }
            catch (Exception)
            {
                Response.Write("error");
            }
        }

        /// <summary>
        /// 获取返利币
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="orderMoney"></param>
        private void GetFlb(string UserName,float orderMoney)
        {
            GameUserManager gum = new GameUserManager();
            float money = 0;
            if (orderMoney > 0 && orderMoney <= 100) money = orderMoney * 0.03F;
            else if (orderMoney > 100 && orderMoney <= 499) money = orderMoney * 0.05F;
            else if (orderMoney > 500 && orderMoney <= 999) money = orderMoney * 0.08F;
            else if (orderMoney > 1000 && orderMoney <= 1999) money = orderMoney * 0.1F;
            else if (orderMoney > 2000 && orderMoney <= 4999) money = orderMoney * 0.12F;
            else money = orderMoney * 0.15F;
            float flb = (float) Math.Floor(money * 10);
            Orders order = new Orders();
            order = om.GetOrder("F", "0", 0, 2, 6, UserName, flb, "VIP返利系统");
            order.State = 2;
            try
            {
                if (om.AddOrder(order))
                {
                    gum.UpdateUserFlMoney(UserName, flb, "+");
                }
            }
            catch (Exception e)
            {
                throw new Exception (e.Message);
            }
        }


    }
}
