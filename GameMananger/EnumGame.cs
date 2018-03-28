using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Manager
{
    public class EnumGame
    {
        public string GetGameByNo(string GameNo)
        { 
            switch (GameNo)
            {
                case "dxz":
                    return "Game_Dxz";
                case "sjsg":
                    return "Game_Sjsg";
                case "tj":
                    return "Game_Qh";
                case "nz":
                    return "Game_Nz";
                case "djj":
                    return "Game_Djj";
                case "jlc":
                    return "Game_Jlc";
                case "ly":
                    return "Game_Ly";
                case "xyb":
                    return "Game_Xyb";
                case "dhz":
                    return "Game_Dhz";
                case "wz":
                    return "Game_Wz";
                case "dpqk":
                    return "Game_Dpqk";
                case "lm":
                    return "Game_Lm";
                case "mxqy":
                    return "Game_Mxqy";
                case "zsg":
                    return "Game_Zsg";
                case "rxhzw":
                    return "Game_Rxhzw";
                case "klsg":
                    return "Game_Klsg";
                case "gjqx":
                    return "Game_Gjqx";
                case "txj":
                    return "Game_Txj";
                case "wdqk":
                    return "Game_Wdqk";
                case "chcq":
                    return "Game_Chcq";
                case "jjsg":
                    return "Game_Jjsg";
                case "qh":
                    return "Game_Qh";
                case "yjxy":
                    return "Game_Yjxy";
                case "xxas":
                    return "Game_Xxas";
                case "tgzt":
                    return "Game_Tgzt";
                case "nslm":
                    return "Game_Nslm";
                case "ftz":
                    return "Game_Ftz";
                case "sbcs":
                    return "Game_Sbcs";
                case "zwj":
                    return "Game_Zwj";
                case "dqqyz":
                    return "Game_Dqqyz";
                case "yxy":
                    return "Game_Yxy";
                case "jhwj":
                    return "Game_Jhwj";
                case "tyjy":
                    return "Game_Tyjy";
                case "shmc":
                    return "Game_Shmc";
                case "qjll":
                    return "Game_Qjll";
                case "fyws":
                    return "Game_Fyws";
                case "ahxx":
                    return "Game_Ahxx";
                case "jstm":
                    return "Game_Jstm";
                case "cssg":
                    return "Game_Cssg";
                default:
                    return "";
            }
        }
    }
}
