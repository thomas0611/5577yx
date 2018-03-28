function GetAllGame() {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetGamesIdName",
        success: function (text) {
            var jsonObj = eval(text);
            for (var i = 0; i < jsonObj.length; i++) {
                $("#Selyx").append("<option value=\"" + jsonObj[i].Id + "\">" + jsonObj[i].Name + "</option>");
            }
        }
    })
}

function GetAllServer(GameId) {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetServersIdName&GameId=" + GameId,
        success: function (text) {
            var jsonObj = eval(text);
            //var html = "<option value=\"0\">" + $("#Selfwq option").html() + "</option>";
            var html = "<option value=\"0\">" + "所有区服" + "</option>";
            for (var i = 0; i < jsonObj.length; i++) {
                html += "<option value=\"" + jsonObj[i].Id + "\">" + jsonObj[i].Name + "</option>";
            }
            $("#Selfwq").html(html);
        }
    })
}

function onyxchange(Code) {
    $("#Selfwq").val(0);
    GetAllServer($("#Selyx").val());
    InitPageContent(Code);
}

function onlxchange() {
    if ($("#czlx").val() == "2") {
        $("#txtqf").attr("style", "visibility:hidden;");
        $("#Selyx").attr("style", "visibility:hidden;");
        $("#Selfwq").attr("style", "visibility:hidden;");
    } else {
        $("#txtqf").attr("style", "visibility:visible;");
        $("#Selyx").attr("style", "visibility:visible;");
        $("#Selfwq").attr("style", "visibility:visible;");
    }
}

var usNum = 0;
function czNum() {
    usNum += 1;
    if (usNum >= 10) {
    } else {
        html = "<input id=\"UserName" + usNum + "\" type=\"text\" name=\"UserName\" onblur=\"uschange('" + usNum + "')\" /><span><img id=\"txtnameimg" + usNum + "\" src=\"/Images/Admin/tj.png\" onclick=\"czNum()\" style=\"width:15px;height:15px;margin:3px;\" alt=\"\"/><span id=\"txtnamelbl" + usNum + "\" style=\"color: #F7654B\"></span></span><br/>";
        $("#moreus").append(html);
    }
}

function uschange(a) {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=CheckUser",
        data: { UserName: $("#UserName" + a).val(), GameId: $("#Selyx").val(), ServerId: $("#Selfwq").val(), Type: $("#czlx").val(), Money: $("#Money").val() },
        success: function (text) {
            if (text != "cg") {
                $("#txtnameimg" + a).attr("src", "/Images/Admin/zhucedenglu1_06.png");
                $("#txtnamelbl" + a).text(text);
            } else {
                $("#txtnameimg" + a).attr("src", "/Images/Admin/onebit_34.png");
                $("#txtnameimg" + a).attr("style", "width:15px;height:15px;");
                $("#txtnamelbl" + a).text("");
            }
        }
    })
}

function tjOrder() {
    var usStr = "";
    for (var i = 0; i < 10; i++) {
        usStr += $("#UserName" + i).val() + "|";
    }
    if ($("#Bz").val() == "") {
        $("#BzTs").html("请填写备注！");
    } else {
        if ($("#Money").val() >= 10) {
            if (confirm("请确定您的充值金额：" + $("#Money").val() + "元/RMB")) {
                $.ajax({
                    type: "get",
                    url: "../Models/AdminFunction.ashx?method=DoCz",
                    data: { PWD: $("#PWD").val(), Bz: $("#Bz").val(), UserList: usStr, GameId: $("#Selyx").val(), ServerId: $("#Selfwq").val(), Type: $("#czlx").val(), Money: $("#Money").val() },
                    success: function (text) {
                        if (text == "fali") {
                            $("#PWDTs").html("您输入的密码不正确！");
                        } else if (text == "BzIsNull") {
                            $("#BzTs").html("请填写备注");
                        } else if (text == "outmoney"){
                            jsprint("超出限额", "", "Error");
                        }
                        else {
                            $("#czjg").append(text);
                        }
                    }
                })
            }
        } else {
            $.ajax({
                type: "get",
                url: "../Models/AdminFunction.ashx?method=DoCz",
                data: { PWD: $("#PWD").val(), Bz: $("#Bz").val(), UserList: usStr, GameId: $("#Selyx").val(), ServerId: $("#Selfwq").val(), Type: $("#czlx").val(), Money: $("#Money").val() },
                success: function (text) {
                    if (text == "fali") {
                        $("#PWDTs").html("您输入的密码不正确！");
                    } else if (text == "BzIsNull") {
                        $("#BzTs").html("请填写备注");
                    }
                    else {
                        $("#czjg").append(text);
                    }
                }
            })
        }
    }
}

function tjOrders() {
    if ($("#Bz").val() == "") {
        $("#BzTs").html("请填写备注！");
    }else {
        $("form").ajaxSubmit({
            url: "../Models/AdminFunction.ashx?method=DoBatCz",
            type: "post",
            success: function (text) {
                if (text == "fail") {
                    $("#PWDTs").html("您输入的密码不正确！");
                }
                else if (text == "BzIsNull") {
                    $("#BzTs").html("请填写备注");
                }
                else if (text == "GameIsNull") {
                    $("#BzTs").html("请选择游戏服");
                }
                else {
                    $("#czjg").append(text);
                }
            }
        });
        return false;
        }
}


function InitNews() {
    var Type = $("#ddltype").val() == "0" ? "1=1" : "Type=" + $("#ddltype").val();
    var Game = $("#Selyx").val() == "0" ? "1=1" : "GameId=" + $("#Selyx").val();
    var Code = "";
    if ($("#ddlProperty").length > 0) {
        Code = "News";
        var Property = $("#ddlProperty").val() == "0" ? "1=1" : $("#ddlProperty").val() + "=1";
        WhereStr = " where " + Type + " and " + Game + " and " + Property + " and Title like '%" + $("#txtKeywords").val() + "%'";
    } else {
        WhereStr = " where " + Type + " and " + Game;
        Code = "IndexNews";
    }
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetNewsCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, Code);
        }
    })
}

function InitGames() {
    var Property = $("#ddlProperty").val() == "0" ? "1=1" : $("#ddlProperty").val() + "=1";
    WhereStr = " where " + Property;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetGamesCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Game");
        }
    })
}

function InitServers() {
    var Game = $("#Selyx").val() == "0" ? "1=1" : "GameId=" + $("#Selyx").val();
    var Property = $("#ddlProperty").val() == "0" ? "1=1" : "State=" + $("#ddlProperty").val();
    WhereStr = " where " + Game + " and " + Property;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetServerCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Server");
        }
    })
}

function InitCards() {
    var Game = "";
    var Code = "";
    if ($("#Selyx").length > 0) {
        Game = $("#Selyx").val() == "0" ? "1=1" : "GameId=" + $("#Selyx").val();
        Code = "Card";
    } else {
        Game = "1=1";
        Code = "Gift";
    }
    WhereStr = " where " + Game;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetCardCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, Code);
        }
    })
}

function InitOrders() {
    var PayMoney = "PayMoney >=" + $("#PayMoney").val();
    var OrderNo = "OrderNo like'%" + $("#OrderNo").val() + "%'";
    var UserName = "UserName like'%" + $("#UserName").val() + "%'";
    var Time = "";
    if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
        Time = "1=1";
    } else {
        Time = "PayTime>='" + $("#StartTime").val() + "' and PayTime<='" + $("#EndTime").val() + "'";
    }
    var Type = $("#Type").val() == "0" ? "1=1" : "Type=" + $("#Type").val();
    var PayType = $("#PayType").val() == "0" ? "1=1" : "PayType=" + $("#PayType").val();
    var State = $("#State").val() == "" ? "1=1" : "State=" + $("#State").val();
    var Game = $("#Selyx").val() == "0" ? "1=1" : "GameId=" + $("#Selyx").val();
    var Server = $("#Selfwq").val() == "0" ? "1=1" : "ServerId=" + $("#Selfwq").val();
    WhereStr = " where " + PayMoney + " and " + OrderNo + " and " + UserName + " and " + Time + " and " + Type + " and " + PayType + " and " + State + " and " + Game + " and " + Server;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetOrderCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Order");
        }
    })
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetOrderInfo",
        data: { WhereStr: WhereStr },
        success: function (text) {
            var re = text.split('|');
            $("#totalCount").html(re[0]);
            $("#CompleteCount").html(re[1]);
            $("#UnfinishedCount").html(re[2]);
            $("#SumOrderMoney").html(re[3]);
            $("#SdOrderMoney").html(re[4]);
            $("#PtOrderMoney").html(re[5]);
        }
    });
}

function InitSourceChange() {
    WhereStr = "where 1=1";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSourceChangeCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "SourceChange");
        }
    })
}

function GetAllCollect(PageSize, PageNum) {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetAllCollect",
        data: { PageSize: PageSize, PageNum: PageNum, WhereStr: WhereStr, MinMoney: $("#MinMoney").val(), MaxMoney: $("#MaxMoney").val() },
        success: function (text) {
            $("#Collect").html(text);
        }
    })
}

function InitCollect() {
    var Game = $("#Selyx").val() == "0" ? "1=1" : "GameId=" + $("#Selyx").val();
    WhereStr = " where " + Game;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetServerCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Collect");
        }
    })
}

function GetAllPromo(PageSize, PageNum) {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetAllPromo",
        data: { PageSize: PageSize, PageNum: PageNum, WhereStr: WhereStr, LinkName: $("#LinkName").val() },
        success: function (text) {
            $("#PromoAnalysis").html(text);
        }
    })
}

function InitPromo() {
    var UserName = "UserName like'%" + $("#UserName").val() + "%'";
    WhereStr = " where " + UserName;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetPromoCount",
        data: { WhereStr: WhereStr, LinkName: $("#LinkName").val() },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Promo");
        }
    })
}

function InitGameUser() {
    var Require = $("#Require").val() + " like'%" + $("#RequireText").val() + "%'";
    var SourceIsNull = " 1=1 ";
    switch ($("#SourceIsNull").val()) {
        case "1":
            SourceIsNull = " Source is not null ";
            break;
        case "2":
            SourceIsNull = " Source is null ";
            break;
        default:
    }
    var Time = "1=1";
    if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
        Time = "1=1";
    } else {
        Time = "" + $("#Time").val() + ">='" + $("#StartTime").val() + "' and " + $("#Time").val() + "<='" + $("#EndTime").val() + "'";
    }
    WhereStr = " where " + SourceIsNull + " and " + Require + " and " + Time;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetGameUserCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "GameUser");
        }
    })
}

function InitMasterLog() {
    WhereStr = " where 1=1 ";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetMasterLogCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "MasterLog");
        }
    })
}

function InitLinks() {
    WhereStr = " where 1=1 ";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetLinksCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Links");
        }
    })
}

function InitCardLog() {
    var Game = $("#Selyx").val() == "0" ? " 1=1 " : "GameId=" + $("#Selyx").val();
    var CarNum = "CardNum like'%" + $("#CardNum").val() + "'";
    WhereStr = " where " + Game + " and " + CarNum;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetCardLogCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "CardLog");
        }
    })
}

function InitMaster() {
    WhereStr = " where 1=1";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetMasterCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Master");
        }
    })
}

function InitSysMsg() {
    WhereStr = "";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSysMsgCount",
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "SysMsg");
        }
    })
}

function InitPayHistory(Code) {
    WhereStr = "";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=Get" + Code + "Count",
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, Code);
        }
    })
}

function InitSpreadUser(C) {
    SpreadCode = C;
    WhereStr = "";
    if ($("#Selyx").length > 0) {
        if ($("#Selyx").val() > 0) {
            WhereStr += $("#Selyx").val() + "|"
        } else {
            WhereStr += "|";
        }
    }
    //if ($("#Selfwq").length > 0) {
    //    if ($("#Selfwq").val > 0) {
    //        WhereStr += $("#Selfwq").val() + "|"
    //    } else {
    //        WhereStr += "|";
    //    }
    //}
    if ($("#StartTime").length > 0) {
        if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
            WhereStr += "|";
        } else {
            WhereStr += $("#StartTime").val() + "|" + $("#EndTime").val();
        }
    }
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSpreadUserCount",
        data: { WhereStr: WhereStr, Code: SpreadCode },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "SpreadUser");
        }
    })
}

function GetAllSpreadUser(PageSize, PageNum) {
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetAllSpreadUser",
        data: { PageSize: PageSize, PageNum: PageNum, WhereStr: WhereStr, Code: SpreadCode },
        success: function (text) {
            if (SpreadCode == "All") {
                $("#SpreadUser").html(text);
            } else if (SpreadCode == "Under") {
                $("#UnderUser").html(text);
            } else {
                $("#UnderUserDetail").html(text);
            }
        }
    })
}

function InitNextSpreadUser(b) {
    WhereStr = "";
    if ($("#Selyx").length > 0) {
        if ($("#Selyx").val() > 0) {
            WhereStr += $("#Selyx").val() + "|"
        } else {
            WhereStr += "|";
        }
    }
    if ($("#StartTime").length > 0) {
        if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
            WhereStr += "|";
        } else {
            WhereStr += $("#StartTime").val() + "|" + $("#EndTime").val();
        }
    }
    WhereStr += "|" + b;
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSpreadUserCount",
        data: { WhereStr: WhereStr, Code: "All" },
        success: function (text) {
            NextPageSize = $("#NexttxtPageNum").val();
            NextPageCount = Math.ceil(text / NextPageSize);
            Nextgopage(1, "NextSpreadUser");
        }
    })
}

function InitSpreadPay() {
    WhereStr = "";
    if ($("#Selyx").val() > 0) {
        WhereStr += $("#Selyx").val() + "|"
    } else {
        WhereStr += "|";
    }
    
    //if ($("#Selfwq").val > 0) {
    //    WhereStr += $("#Selfwq").val() + "|"
    //} else {
    //    WhereStr += "|";
    //}
       
    if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
        WhereStr += "|";
    } else {
        WhereStr += $("#StartTime").val() + "|" + $("#EndTime").val();
    }
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSpreadPayCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "SpreadPay");
        }
    })
}

function InitSpreadGame() {
    WhereStr = "";
    if ($("#Selyx").length > 0) {
        if ($("#Selyx").val() > 0) {
            WhereStr += $("#Selyx").val() + "|"
        } else {
            WhereStr += "|";
        }
    }
    if ($("#StartTime").length > 0) {
        if ($("#StartTime").val() == null || $("#StartTime").val() == "" || $("#EndTime").val() == null || $("#EndTime").val() == "") {
            WhereStr += "|";
        } else {
            WhereStr += $("#StartTime").val() + "|" + $("#EndTime").val();
        }
    }
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetSpreadGameCount",
        data: { WhereStr: WhereStr },
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "SpreadGame");
        }
    })
}

function InitMasterRole() {
    WhereStr = "";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetMasterRoleCount",
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "MasterRole");
        }
    })
}

function InitLock() {
    WhereStr = "";
    $.ajax({
        type: "get",
        url: "../Models/AdminFunction.ashx?method=GetLockCount",
        success: function (text) {
            PageSize = $("#txtPageNum").val();
            PageCount = Math.ceil(text / PageSize);
            gopage(1, "Lock");
        }
    })
}

function GetAllData(PageSize, PageNum, Code) {
    switch (Code) {
        case "Collect":
            GetAllCollect(PageSize, PageNum);
            break;
        case "Promo":
            GetAllPromo(PageSize, PageNum);
            break;
        case "SpreadUser":
            GetAllSpreadUser(PageSize, PageNum);
            break;
        default:
            $.ajax({
                type: "get",
                url: "../Models/AdminFunction.ashx?method=GetAll" + Code,
                data: { PageSize: PageSize, PageNum: PageNum, WhereStr: WhereStr },
                success: function (text) {
                    $("#" + Code + "").html(text);
                }
            })
            break;
    }

}

function InitPageContent(Code) {
    switch (Code) {
        case "News":
            InitNews();
            break;
        case "Game":
            InitGames();
            break;
        case "Server":
            InitServers();
            break;
        case "Card":
            InitCards();
            break;
        case "Order":
            InitOrders();
            break;
        case "SourceChange":
            InitSourceChange();
            break;
        case "Collect":
            InitCollect();
            break;
        case "PromoAnalysis":
            InitPromo();
            break;
        case "GameUser":
            InitGameUser();
            break;
        case "MasterLog":
            InitMasterLog();
            break;
        case "Links":
            InitLinks();
            break;
        case "CardLog":
            InitCardLog();
            break;
        case "Master":
            InitMaster();
            break;
        case "SysMsg":
            InitSysMsg();
            break;
        case "PayHistory1":
            InitPayHistory('PayHistory1');
            break;
        case "PayHistory2":
            InitPayHistory('PayHistory2');
            break;
        case "SpreadUser":
            InitSpreadUser("All");
            GetSpreadMoney();
            break;
        case "UnderUser":
            InitSpreadUser("Under");
            GetSpreadMoney();
            break;
        case "UnderUserDetail":
            InitSpreadUser("UnderDetail");
            GetSpreadMoney();
            break;
        case "SpreadPay":
            InitSpreadPay();
            GetSpreadMoney();
            break;
        case "SpreadGame":
            InitSpreadGame();
            GetSpreadMoney();
            break;
        case "MasterRole":
            InitMasterRole();
            break;
        case "Lock":
            InitLock();
            break;
        default:
    }
}

function gopage(PageNum, Code) {
    if (PageNum == 0) {
        PageNum = $("#tpageindex").val();
        if (PageNum > PageCount) {
            PageNum = PageCount;
        } else if (PageNum < 1) {
            PageNum = 1;
        }
    }
    var html = "";
    var html_F = "";
    var html_P = "";
    var html_N = "";
    var html_L = "";
    var html_F_of = "<span class=\"disabled\">首页</span>";
    var html_F_on = "<a href=\"javascript:;\" onclick=\"gopage(1,'" + Code + "')\">首页</a>";
    var html_P_of = "<span class=\"disabled\">上一页</span>";
    var html_P_on = "<a href=\"javascript:;\" onclick=\"gopage(" + parseInt(PageNum - 1) + ",'" + Code + "')\">上一页</a>";
    var html_N_of = "<span class=\"disabled\">下一页</span>";
    var html_N_on = "<a href=\"javascript:;\" onclick=\"gopage(" + parseInt(PageNum + 1) + ",'" + Code + "')\">下一页</a>";
    var html_L_of = "<span class=\"disabled\">末页</span>";
    var html_L_on = "<a href=\"javascript:;\" onclick=\"gopage(" + PageCount + ",'" + Code + "')\">末页</a>";
    var html_M = "<input id=\"tpageindex\" style=\"width: 30px;\" type=\"text\" onkeyup=\"this.value=this.value.replace(/\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\D/g,'')\" /><a href=\"javascript:;\" onclick=\"gopage(0,'" + Code + "')\">GO</a>";
    var html_ON = "<span class=\"current\">" + PageNum + "</span>";

    if (PageNum > 1) {
        html_F = html_F_on;
        html_P = html_P_on;
    } else {
        html_F = html_F_of;
        html_P = html_P_of;
    }
    if (PageNum == PageCount) {
        html_L = html_F_of;
        html_N = html_N_of;
    } else {
        html_L = html_L_on;
        html_N = html_N_on;
    }
    if (PageCount <= 1) {
        html = html_F_of + html_P_of + html_ON + html_N_of + html_L_of;
    } else if (PageCount > 1 && PageCount <= 5) {
        html = html_F + html_P;
        for (var i = 1; i <= PageCount; i++) {
            if (i == PageNum) {
                html += html_ON;
            } else {
                html += "<a href=\"javascript:;\" onclick=\"gopage(" + i + ",'" + Code + "')\">" + i + "</a>";
            }
        }
        html += html_N + html_L;
    } else if (PageCount > 5) {
        html = html_F + html_P;
        if (PageNum <= 4) {
            for (var i = 1; i < PageNum; i++) {
                html += "<a href=\"javascript:;\" onclick=\"gopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        } else {
            html += "<a href=\"javascript:;\" onclick=\"gopage(1,'" + Code + "')\">1</a><span>....</span>";
            for (var i = PageNum - 2; i < PageNum; i++) {
                html += "<a href=\"javascript:;\" onclick=\"gopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        }
        html += html_ON;
        if (PageNum + 3 < PageCount) {
            for (var i = PageNum + 1; i <= PageNum + 2; i++) {
                html += "<a href=\"javascript:;\" onclick=\"gopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
            html += "<span>....</span><a href=\"javascript:;\" onclick=\"gopage(" + PageCount + ",'" + Code + "')\">" + PageCount + "</a>";
        } else {
            for (var i = PageNum + 1; i <= PageCount; i++) {
                html += "<a href=\"javascript:;\" onclick=\"gopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        }
        html += html_N + html_L + html_M;
    }
    $("#NextPageContent").html("");
    $("#PageContent").html(html);
    GetAllData(PageSize, PageNum, Code);
}

function Nextgopage(NextPageNum, Code) {
    if (NextPageNum == 0) {
        NextPageNum = $("#tpageindex").val();
        if (NextPageNum > NextPageCount) {
            NextPageNum = NextPageCount;
        } else if (NextPageNum < 1) {
            NextPageNum = 1;
        }
    }
    var html = "";
    var html_F = "";
    var html_P = "";
    var html_N = "";
    var html_L = "";
    var html_F_of = "<span class=\"disabled\">首页</span>";
    var html_F_on = "<a href=\"javascript:;\" onclick=\"Nextgopage(1,'" + Code + "')\">首页</a>";
    var html_P_of = "<span class=\"disabled\">上一页</span>";
    var html_P_on = "<a href=\"javascript:;\" onclick=\"Nextgopage(" + parseInt(NextPageNum - 1) + ",'" + Code + "')\">上一页</a>";
    var html_N_of = "<span class=\"disabled\">下一页</span>";
    var html_N_on = "<a href=\"javascript:;\" onclick=\"Nextgopage(" + parseInt(NextPageNum + 1) + ",'" + Code + "')\">下一页</a>";
    var html_L_of = "<span class=\"disabled\">末页</span>";
    var html_L_on = "<a href=\"javascript:;\" onclick=\"Nextgopage(" + NextPageCount + ",'" + Code + "')\">末页</a>";
    var html_M = "<input id=\"tpageindex\" style=\"width: 30px;\" type=\"text\" onkeyup=\"this.value=this.value.replace(/\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\D/g,'')\" /><a href=\"javascript:;\" onclick=\"Nextgopage(0,'" + Code + "')\">GO</a>";
    var html_ON = "<span class=\"current\">" + NextPageNum + "</span>";

    if (NextPageNum > 1) {
        html_F = html_F_on;
        html_P = html_P_on;
    } else {
        html_F = html_F_of;
        html_P = html_P_of;
    }
    if (NextPageNum == NextPageCount) {
        html_L = html_F_of;
        html_N = html_N_of;
    } else {
        html_L = html_L_on;
        html_N = html_N_on;
    }
    if (NextPageCount <= 1) {
        html = html_F_of + html_P_of + html_ON + html_N_of + html_L_of;
    } else if (NextPageCount > 1 && NextPageCount <= 5) {
        html = html_F + html_P;
        for (var i = 1; i <= NextPageCount; i++) {
            if (i == NextPageNum) {
                html += html_ON;
            } else {
                html += "<a href=\"javascript:;\" onclick=\"Nextgopage(" + i + ",'" + Code + "')\">" + i + "</a>";
            }
        }
        html += html_N + html_L;
    } else if (NextPageCount > 5) {
        html = html_F + html_P;
        if (NextPageNum <= 4) {
            for (var i = 1; i < NextPageNum; i++) {
                html += "<a href=\"javascript:;\" onclick=\"Nextgopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        } else {
            html += "<a href=\"javascript:;\" onclick=\"Nextgopage(1,'" + Code + "')\">1</a><span>....</span>";
            for (var i = NextPageNum - 2; i < NextPageNum; i++) {
                html += "<a href=\"javascript:;\" onclick=\"Nextgopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        }
        html += html_ON;
        if (NextPageNum + 3 < NextPageCount) {
            for (var i = NextPageNum + 1; i <= NextPageNum + 2; i++) {
                html += "<a href=\"javascript:;\" onclick=\"Nextgopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
            html += "<span>....</span><a href=\"javascript:;\" onclick=\"Nextgopage(" + NextPageCount + ",'" + Code + "')\">" + NextPageCount + "</a>";
        } else {
            for (var i = NextPageNum + 1; i <= NextPageCount; i++) {
                html += "<a href=\"javascript:;\" onclick=\"Nextgopage(" + i + ",'" + Code + "')\">" + i + "</a>";;
            }
        }
        html += html_N + html_L + html_M;
    }
    $("#NextPageContent").html(html);
    GetAllData(NextPageSize, NextPageNum, Code);
}

function Del(DelUrl) {
    if (confirm("确定删除？")) {
        $.ajax({
            type: "get",
            url: DelUrl,
            success: function (text) {
                if (text == "True") {
                    jsprint("删除成功！", "", "Success");
                } else {
                    jsprint("删除失败！", "", "Error");
                }
            }
        })
    };
}

function Reset(ResetUrl) {
    if (confirm("确定重置？")) {
        $.ajax({
            type: "get",
            url: ResetUrl,
            success: function (text) {
                if (text == "True") {
                    jsprint("重置成功！", "", "Success");
                } else {
                    jsprint("重置失败！", "", "Error");
                }
            }
        })
    };
}

function UpdateData(UpdateUrl) {
    if (UpdateUrl == "/News/UpdateNews" || UpdateUrl == "/Card/UpdateCard") {
        editor.sync();
    }
    $("form").ajaxSubmit({
        url: UpdateUrl,
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                jsprint("修改成功！", "", "Success");
            } else {
                jsprint("修改失败！", "", "Error");
            }
        }
    });
    return false;
}

function AddData(AddUrl) {
    if (AddUrl == "/News/DoAddNews" || AddUrl == "/Card/DoAddCard") {
        editor.sync();
    }
    $("form").ajaxSubmit({
        url: AddUrl,
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                jsprint("添加成功！", "back", "Success");
            } else {
                jsprint("添加失败！", "", "Error");
            }
        }
    });
    return false;
}

$(document).ready(function () {
    //if ($("#HeadInfo").length > 0) {
    //    $.ajax({
    //        type: "get",
    //        url: "../Models/Function.ashx?method=InitHeadInfo",
    //        success: function (text) {
    //            $("#HeadInfo").html(text);
    //        }
    //    });
    //}
    //if ($("#logined").length > 0) {
    //    InitLoginInfo();
    //}
    //if ($("#NewsServer").length > 0) {
    //    $.ajax({
    //        type: "get",
    //        url: "../Models/Function.ashx?method=GetNewsServer",
    //        success: function (text) {
    //            $("#NewsServer").html(text);
    //        }
    //    });
    //}
    if ($("#xzyx").length > 0) {
        Type = 1;
        $(".clearfix  li").each(function (i, k) {
            $(k).click(function () {
                $(".clearfix li").removeClass('current');
                $(k).addClass('current');
                if (i > 0) {
                    Type = 2;
                    $("#xzyx").hide().attr("style", 'display: none;');
                } else {
                    $("#xzyx").hide().attr("style", '');
                    Type = 1;
                }
                CheckMoney();
            });
        });
    }
    if ($("#winpop").length > 0) {
        $.ajax({
            type: "get",
            url: "../Models/Function.ashx?method=GetXiaoxi",
            success: function (text) {
                if (text != "" && text != null) {
                    $("#Xiaoxi").html(text);
                    document.getElementById('winpop').style.height = '0px';//我不知道为什么要初始化这个高度,CSS里不是已经初始化了吗,知道的告诉我一下
                    setTimeout("tips_pop()", 800);     //3秒后调用tips_pop()这个函数
                }
            }
        });
    }
    if ($("#online_qq_layer").length > 0) {
        $("#floatShow").bind("click", function () {
            $("#onlineService").animate({ width: "show", opacity: "show" }, "normal", function () { $("#onlineService").show() });
            $("#floatShow").attr("style", "display:none");
            $("#floatHide").attr("style", "display:block");
            return false
        });
        $("#floatHide").bind("click", function () {
            $("#onlineService").animate({ width: "hide", opacity: "hide" }, "normal", function () { $("#onlineService").hide() });
            $("#floatShow").attr("style", "display:block");
            $("#floatHide").attr("style", "display:none");
            return false
        });
    }
});

function tips_pop() {
    var MsgPop = document.getElementById("winpop");//获取窗口这个对象,即ID为winpop的对象
    var popH = parseInt(MsgPop.style.height);//用parseInt将对象的高度转化为数字,以方便下面比较
    if (popH == 0) {         //如果窗口的高度是0
        MsgPop.style.display = "block";//那么将隐藏的窗口显示出来
        show = setInterval("changeH('up')", 2);//开始以每0.002秒调用函数changeH("up"),即每0.002秒向上移动一次
    }
    else {         //否则
        hide = setInterval("changeH('down')", 2);//开始以每0.002秒调用函数changeH("down"),即每0.002秒向下移动一次
    }
}

function changeH(str) {
    var MsgPop = document.getElementById("winpop");
    var popH = parseInt(MsgPop.style.height);
    if (str == "up") {     //如果这个参数是UP
        if (popH <= 100) {    //如果转化为数值的高度小于等于100
            MsgPop.style.height = (popH + 4).toString() + "px";//高度增加4个象素

        }
        else {
            clearInterval(show);//否则就取消这个函数调用,意思就是如果高度超过100象度了,就不再增长了

        }
    }
    if (str == "down") {

        if (popH >= 4) {       //如果这个参数是down

            MsgPop.style.height = (popH - 4).toString() + "px";//那么窗口的高度减少4个象素

        }
        else {        //否则
            clearInterval(hide);    //否则就取消这个函数调用,意思就是如果高度小于4个象度的时候,就不再减了

            MsgPop.style.display = "none";  //因为窗口有边框,所以还是可以看见1~2象素没缩进去,这时候就把DIV隐藏掉

        }
    }
}

function InitLoginInfo() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=InitLeftInfo",
        success: function (text) {
            if (text.length > 0) {
                $("#logined").attr("style", "display: block;");
                $("#login_form").attr("style", "display: none;");
                var obj = jQuery.parseJSON(text);
                $("#UserNameed").html(obj.UserName);
                $("#Money").html(obj.Money);
                GetLastLogin();

            } else {
                $("#logined").attr("style", "display: none;");
                $("#login_form").attr("style", "display: block;");
                GetCookie();
            }
        }
    });
}

function GetCookie() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=GetCookie",
        success: function (text) {
            if (text.length > 0) {
                var re = text.split("|");
                $("#UserName").val(re[0]);
                $("#PWD").val(re[1]);
            }
        }
    });
}

function GetLastLogin() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=GetGetOnlineLog",
        success: function (text) {
            $("#OnlineLog").html(text);
        }
    });
}

function GetNewsCard() {
    $.ajax({
        type: "get",
        url: "/" + $("#Gn").val() + "/DoGetGift?G=" + $("#G").val(),
        success: function (text) {
            UpdateCardCount();
            $("#Msg").html(text);
        }
    });
}

function UpdateCardCount() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=GetCardCount",
        data: { CardId: $("#G").val() },
        success: function (text) {
            $("#CardCount").html(text);
        }
    });
}

function UpdateUserInfo() {
    var RealName = "";
    var Card = "";
    var Email = "";
    var PWD = "";
    if ($("#RealName").length > 0) {
        if (VerifyRealName()) {
            RealName = $("#RealName").val();
        } else {
            return;
        }
    }
    if ($("#Card").length > 0) {
        if (VerifyCard()) {
            Card = $("#Card").val();
        } else {
            return;
        }
    }
    if ($("#Email").length > 0) {
        if (VerifyEmail()) {
            Email = $("#Email").val();
        } else {
            return;
        }
    }
    if ($("#PWD").length > 0) {
        if (VerifyPWD() && VerifyyPWD() && VerifyPWDAgain()) {
            PWD = $("#PWD").val();
        } else {
            return;
        }
    }
    $.ajax({
        type: "get",
        url: "/UserCenter/DoInfoEdit",
        data: { Photo: $("#Photo").val(), RealName: RealName, Card: Card, Birthday: $("#Birthday").val(), Sex: $("#Sex").val(), Email: Email, PWD: PWD },
        success: function (text) {
            if (text == "True") {
                jsprint("修改成功！", "", "Success");
            } else {
                jsprint("修改失败！", "", "Error");
            }
        }
    });
}

function UpdateUserSource(Ud, UserId) {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=UpdateUserSource",
        data: { Ud: Ud, Id: UserId },
        success: function (text) {
            if (text == "True") {
                jsprint("修改成功！", "", "Success");
            } else {
                jsprint("修改失败！", "", "Error");
            }
        }
    });
}

function FindPwd() {
    $.ajax({
        type: "get",
        url: "/UserCenter/DoFindPwd",
        data: { UserName: $("#UserName").val(), Code: $("#Code").val() },
        success: function (text) {
            var re = text.split('|');
            jsprint(re[0], "", re[1]);
        }
    });
}

function UpadatePwd() {
    $.ajax({
        type: "get",
        url: "/UserCenter/DoUpadatePwd",
        data: { Pwd: $("#txtpwd1").val(), APwd: $("#txtpwd").val() },
        success: function (text) {
            if (text == "" || text == null) {
                $("#Msg").html("密码修改成功");
                $("#MsgText").html("修改密码已成功,请您使用新密码登录！");
                $(".email2").attr("style", "");
                $(".changecode").attr("style", "display: none;");
            } else {
                jsprint(text, "", "Error");
            }
        }
    });
}

function GetSpreadMoney() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=GetSpreadMoney",
        data: { WhereStr: WhereStr },
        success: function (text) {
            $("#SpreadMoney").html(text);
        }
    });
}

function UpdateMasterRole() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=GetSpreadMoney",
        data: { WhereStr: WhereStr },
        success: function (text) {
            $("#SpreadMoney").html(text);
        }
    });
}

var t = n = 0, count;
$(function ($) {
    /*图片轮播*/
    count = $("#slider ul li").length;
    var t = setInterval(showPic, 5000);

    $("#controls li").hover(function () {
        clearInterval(t);
        var index = $(this).index() - 1;
        n = index;
        showPic();
        t = setInterval(showPic, 3000);
    });
    $("#slider .prev").click(function () {
        clearInterval(t);
        var index = $("#controls li.current").index() - 1 - 1;
        if (index == -2) {
            index = count - 2;
        }
        n = index;
        showPic();
        t = setInterval(showPic, 3000);
    });
    $("#slider .next").click(function () {
        clearInterval(t);
        var index = $("#controls li.current").index() - 1 + 1;
        n = index;
        showPic();
        t = setInterval(showPic, 3000);
    });


    $(".spanlink1").click(function () {
        $(".moreLink").slideToggle(1000);
    });


    $(".userlist_menu li").click(function () {
        var index = $(this).index();
        $(".userlistgroup").hide();
        $(".userlist_menu li").removeClass("current");
        $(this).addClass("current");
        $(".userlistgroup" + index).show();
    });


    $(".listTable tr:odd td").attr("style", "background-color:#FFF4E1");


    $(".newsUl li").hover(function () {
        var index = $(this).index();
        if (index != 2) {
            $(".area").hide();
            $(".area" + index).show();
            $(".newsUl li").removeClass("current");
            $(this).addClass("current");
        }
    });

});

function showPic() {
    n = n >= (count - 1) ? 0 : ++n;
    $("#slider ul").stop().animate({ "margin-left": -688 * n + "px" }, 500);
    $("#controls li").removeClass("current");
    $("#controls li").eq(n).addClass("current");
}

function TjOrder() {
    if (Type == "2" || (Type == "1" && $("#Selyx").val() != "0")) {
        if ($("#UserName").val() == null || $("#UserName").val() == "") {
            alert("请输入用户名！");
        } else {
            if ($("#UserName").val() != $("#UserNameAG").val()) {
                alert("您两次输入的账号不一致！");
            } else {
                $.ajax({
                    type: "get",
                    url: "../Models/Function.ashx?method=MakeOrder",
                    data: { Type: Type, GameId: $("#Selyx").val(), ServerId: $("#Selfwq").val(), SelMoney: $("#SelMoney").val(), TxtMoney: $("#TxtMoney").val(), UserName: $("#UserName").val(), Bank: $("input:radio[name='chongzhi']:checked").val(), PayType: $("#PayType").val() },
                    success: function (text) {
                        var re = text.split("|");
                        if (re[1] == "Error") {
                            alert(re[0]);
                        } else {
                            location.href = re[0];
                        }
                    }
                })
            }
        }
    } else {
        alert("请选择游戏区服！");
    }
}

function CheckMoney() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=CheckMoney",
        data: { Type: Type, GameId: $("#Selyx").val(), SelMoney: $("#SelMoney").val(), TxtMoney: $("#TxtMoney").val(), PayType: $("#PayType").val() },
        success: function (text) {
            var re = text.split("|");
            $("#YfMoney").html(re[0]);
            $("#HdMoney").html(re[1]);
        }
    })
}

function SubmitOrder() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=zfbeOrder",
        data: { OrderNo: $("#OrderNo").html() },
        success: function (text) {
            if (text == "NoZfb") {
            } else {
                $("body").html(text);
            }
        }
    })
    return false;
}

//=============================切换验证码======================================
function ToggleCode(obj, codeurl) {
    $(obj).attr("src", codeurl + "?time=" + Math.random());
}

//Tab控制函数
function tabs(tabId, tabNum) {
    //设置点击后的切换样式
    $(tabId + " .tab_nav li").removeClass("selected");
    $(tabId + " .tab_nav li").eq(tabNum).addClass("selected");
    //根据参数决定显示内容
    $(tabId + " .tab_con").hide();
    $(tabId + " .tab_con").eq(tabNum).show();
}

//可以自动关闭的提示
function jsprint(msgtitle, url, msgcss, callback) {
    $("#msgprint").remove();
    var cssname = "";
    switch (msgcss) {
        case "Success":
            cssname = "pcent success";
            break;
        case "Error":
            cssname = "pcent error";
            break;
        default:
            cssname = "pcent warning";
            break;
    }
    var str = "<div id=\"msgprint\" class=\"" + cssname + "\">" + msgtitle + "</div>";
    $("body").append(str);
    $("#msgprint").show();

    if (url == "back") {
        location.href = document.referrer;
    } else if (url != "") {
        location.href = url;
    }
    //2秒后清除提示
    setTimeout(function () {
        $("#msgprint").fadeOut(500);
        //如果动画结束则删除节点
        if (!$("#msgprint").is(":animated")) {
            $("#msgprint").remove();
        }
    }, 2000);
    //执行回调函数
    if (typeof (callback) == "function") callback();
}

//提示
function jsprint1(objmsg) {
    $.ligerDialog.warn(objmsg);
}
function jsprint2(objmsg) {
    $.ligerDialog.success(objmsg);
}
//全选取消按钮函数，调用样式如：
function checkAll(chkobj) {
    if ($(chkobj).find("span b").text() == "全选") {
        $(chkobj).find("span b").text("取消");
        $(".checkall input").attr("checked", true);
    } else {
        $(chkobj).find("span b").text("全选");
        $(".checkall input").attr("checked", false);
    }
}

//执行回传函数
function ExegetBack(objId, objmsg) {
    if ($(".checkall input:checked").size() < 1) {
        $.ligerDialog.warn("对不起，请选中您要操作的记录！");
        return false;
    }
    var msg = "删除记录后不可恢复，您确定吗？";
    if (arguments.length == 2) {
        msg = objmsg;
    }
    $.ligerDialog.confirm(msg, "提示信息", function (result) {
        if (result) {
            __dogetBack(objId, '');
        }
    });
    return false;
}


//关闭提示窗口
function CloseTip(objId) {
    $("#" + objId).hide();
}

//===========================系统管理JS函数结束================================

//================上传文件JS函数开始，需和jquery.form.js一起使用===============
//文件上传
function Upload(action, repath, uppath, iswater, isthumbnail, filepath) {
    var sendUrl = "../Models/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath;
    //判断是否打水印
    if (arguments.length == 4) {
        sendUrl = "../Models/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath + "&IsWater=" + iswater;
    }
    //判断是否生成宿略图
    if (arguments.length == 5) {
        sendUrl = "../Models/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath + "&IsWater=" + iswater + "&IsThumbnail=" + isthumbnail;
    }
    //自定义上传路径
    if (arguments.length == 6) {
        sendUrl = filepath + "tools/upload_ajax.ashx?action=" + action + "&ReFilePath=" + repath + "&UpFilePath=" + uppath + "&IsWater=" + iswater + "&IsThumbnail=" + isthumbnail;
    }
    //开始提交

    //开始提交
    $("form").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //隐藏上传按钮
            $("#" + repath).nextAll(".files").eq(0).hide();
            //显示LOADING图片
            $("#" + repath).nextAll(".uploading").eq(0).show();
        },
        success: function (data, textStatus) {
            if (data.msg == 1) {
                $("#" + repath).val(data.msbox);
            } else {
                alert(data.msbox);
            }
            $("#" + repath).nextAll(".files").eq(0).show();
            $("#" + repath).nextAll(".uploading").eq(0).hide();
        },
        error: function (data, status, e) {
            alert("上传失败，错误信息：" + e);
            $("#" + repath).nextAll(".files").eq(0).show();
            $("#" + repath).nextAll(".uploading").eq(0).hide();
        },
        url: sendUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};
//附件上传
function AttachUpload(repath, uppath) {
    var submitUrl = "../Models/upload_ajax.ashx?action=AttachFile&UpFilePath=" + uppath;
    //开始提交
    $("form1").ajaxSubmit({
        beforeSubmit: function (formData, jqForm, options) {
            //隐藏上传按钮
            $("#" + uppath).parent().hide();
            //显示LOADING图片
            $("#" + uppath).parent().nextAll(".uploading").eq(0).show();
        },
        success: function (data, textStatus) {
            if (data.msg == 1) {
                var listBox = $("#" + repath + " ul");
                var newLi = '<li>'
                + '<input name="hidFileName" type="hidden" value="0|' + data.mstitle + "|" + data.msbox + '" />'
                + '<b title="删除" onclick="DelAttachLi(this);"></b>附件：' + data.mstitle
                + '</li>';
                listBox.append(newLi);
                //alert(data.mstitle);
            } else {
                alert(data.msbox);
            }
            $("#" + uppath).parent().show();
            $("#" + uppath).parent().nextAll(".uploading").eq(0).hide();
        },
        error: function (data, status, e) {
            alert("上传失败，错误信息：" + e);
            $("#" + uppath).parent().show();
            $("#" + uppath).parent().nextAll(".uploading").eq(0).hide();
        },
        url: submitUrl,
        type: "post",
        dataType: "json",
        timeout: 600000
    });
};
//===========================上传文件JS函数结束================================
//提示是否删除
function ckdel(url) {
    var submit = function (v, h, f) {
        if (v == 'ok')
            window.location.href = url;
        return true; //close
    };
    $.jBox.confirm("删除后不可恢复,确定删除吗？", "提示", submit);
}
function ckdel1(url, msg) {
    var submit = function (v, h, f) {
        if (v == 'ok')
            window.location.href = url;
        return true; //close
    };
    $.jBox.confirm(msg, "提示", submit);
}