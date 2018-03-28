using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public class DevRegHel
    {
        //验证字母和数字组合且必须以字母开头
        public static bool RegName(string str)
        {
            string regp = @"^[a-zA-Z]\w{5,17}$";
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
        // 验证字符长度
        public static bool RegStrLength(string str, int minlen, int maxlen)
        {
            string regp = "^.{" + minlen + "," + maxlen + "}$";
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
        // 验证字符串是否是数字
        public static bool RegNum(string str, int type)
        {
            string regp = "";
            if (type == 1)
            {
                regp = @"^\+?[1-9][0-9]*$"; //非0正整数
            }
            else if (type == 2)
            {
                regp = @"^\d+$"; //正整数
            }
            else if (type == 3)
            {
                regp = @"^[0-9]+(.[0-9]{2})?$";  //有两位小数的正实数
            }
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
        //数字取值范围
        public static bool RegNum(int value, int minvalue, int maxvalue)
        {
            if (value < minvalue)
            {
                return false;
            }
            else if (value > maxvalue)
            {
                return false;
            }
            return true;
        }
        // 验证邮箱格式
        public static bool RegEmail(string str)
        {
            string regp = @"^\w{3,}@\w+(\.\w+)+$";
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
        // 验证身份证格式
        public static bool RegCard(string str)
        {
            int[] Wi = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 }; // 加权因子   
            int[] ValideCode = { 1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2 }; // 身份证验证位值.10代表X   
            str = str.Trim().ToLower();
            string year;
            string month;
            string day;
            string date;
            DateTime dt;
            if (str.Length == 15)
            {
                year = "19" + str.Substring(6, 2);
                month = str.Substring(8, 2);
                day = str.Substring(10, 2);
                date = year + "-" + month + "-" + day + " 00:00:00";
                if (!DateTime.TryParse(date, out dt))
                {
                    return false;
                }
                return true;
            }
            else if (str.Length == 18)
            {
                year = str.Substring(6, 4);
                month = str.Substring(10, 2);
                day = str.Substring(12, 2);
                date = year + "-" + month + "-" + day + " 00:00:00";
                if (!DateTime.TryParse(date, out dt))
                {
                    return false;
                }
                int sum = 0;
                char[] a = str.ToCharArray();
                for (int i = 0; i < 17; i++)
                {
                    sum += Wi[i] * int.Parse(a[i].ToString());
                }
                int vcpos = sum % 11;
                if (a[17] == 'x' || a[17] == 'X')
                {
                    if (10 == ValideCode[vcpos])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (a[17].ToString() == ValideCode[vcpos].ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        //验证手机号码
        public static bool RegPhone(string str)
        {
            string regp = @"^(13[0-9]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$";
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
        public static bool RegPwd(string str)
        {
            string regp = @"^.{6,16}$";
            if (!Regex.IsMatch(str, regp))
            {
                return false;
            }
            return true;
        }
    }
}
