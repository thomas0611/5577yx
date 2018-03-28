/// <reference path="messages_cn.js" />
function VerifyIsNull(obj) {
    var re = $(obj).val();
    if (re == null || re == undefined || re == "") {
        return false;
    }
    return true;
}
function VerifySortId() {
    if ($("#SortId").val() == null || $("#SortId").val() == "" || $("#SortId").val() <= 0) {
        return false;
    } else {
        return true;
    }
}
function VerifyGameId() {
    if ($("#Selyx").val() == null || $("#Selyx").val() == "" || $("#Selyx").val() <= 0) {
        return false;
    } else {
        return true;
    }
}

function VerifyUserName() {
    var obj = $("#UserName");
    if (VerifyIsNull(obj)) {
        var nameRegEx = /^[a-zA-Z]\w{5,17}$/;
        if (nameRegEx.test(obj.val())) {
            $.ajax({
                type: "Post",
                url: "/Home/IsGameUser",
                data: { UserName: obj.val() },
                success: function (text) {
                    if (text == "True") {
                        $("#UserNameMsg").html("您输入的用户名已经被注册了，换一个试试吧！");
                        $("#UserNameMsg").attr("Class", "msgN");
                        obj.addClass("inputB");
                        return false;
                    } else {
                        $("#UserNameMsg").html("这个还没注册，赶紧抢注吧！");
                        $("#UserNameMsg").attr("Class", "msgS");
                        obj.removeClass("inputB");
                        return true;
                    }
                }
            });
        } else {
            $("#UserNameMsg").html("您输入的用户名不行哟！(长度为6～16位，由字母、数字组成，且只能以字母开头)");
            $("#UserNameMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        }
    } else {
        $("#UserNameMsg").html("您还没输入用户名哟！");
        $("#UserNameMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyTgUserName() {
    var obj = $("#UserName");
    if (VerifyIsNull(obj)) {
        var nameRegEx = /^[a-zA-Z]\w{5,17}$/;
        if (nameRegEx.test(obj.val())) {
            $.ajax({
                type: "Post",
                url: "/Home/IsGameUser",
                data: { UserName: obj.val() },
                success: function (text) {
                    if (text == "True") {
                        $("#TgMsg").html("您输入的用户名已经被注册了，换一个试试吧！");
                        $("#TgMsg").attr("Class", "msgN");
                        obj.addClass("inputB");
                        return false;
                    } else {
                        $("#TgMsg").html("这个还没注册，赶紧抢注吧！");
                        $("#TgMsg").attr("Class", "msgS");
                        obj.removeClass("inputB");
                        return true;
                    }
                }
            });
        } else {
            $("#TgMsg").html("您输入的用户名不行哟！(长度为6～16位，由字母、数字组成，且只能以字母开头)");
            $("#TgMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        }
    } else {
        $("#UserNameMsg").html("您还没输入用户名哟！");
        $("#UserNameMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

//CharMode函数 
//测试某个字符是属于哪一类. 
function CharMode(iN) {
    if (iN >= 48 && iN <= 57) //数字 
        return 1;
    if (iN >= 65 && iN <= 90) //大写字母 
        return 2;
    if (iN >= 97 && iN <= 122) //小写 
        return 4;
    else
        return 8; //特殊字符 
}

//bitTotal函数 
//计算出当前密码当中一共有多少种模式 
function bitTotal(num) {
    modes = 0;
    for (i = 0; i < 4; i++) {
        if (num & 1) modes++;
        num >>>= 1;
    }
    return modes;
}

//checkStrong函数 
//返回密码的强度级别 
function checkStrong(sPW) {
    if (sPW.length <= 4)
        return 0; //密码太短 
    Modes = 0;
    for (i = 0; i < sPW.length; i++) {
        //测试每一个字符的类别并统计一共有多少种模式. 
        Modes |= CharMode(sPW.charCodeAt(i));
    }

    return bitTotal(Modes);

}

function VerifyPWDStrong() {
    var obj = $("#PWD");
    if (VerifyIsNull(obj)) {
        switch (checkStrong(obj.val())) {
            case 0:
                $("#PWDMsg").html("<img src=\"/Images/zhucedenglu_32.png\" />");
                break;
            case 1:
                $("#PWDMsg").html("<img src=\"/Images/zhucedenglu_34.png\" />");
                break;
            case 2:
                $("#PWDMsg").html("<img src=\"/Images/zhucedenglu_36.png\" />");
                break;
            case 3:
                $("#PWDMsg").html("<img src=\"/Images/zhucedenglu_38.png\" />");
                break;
            default:
                break;
        }
    }
}

function VerifyTgPWDStrong() {
    var obj = $("#PWD");
    if (VerifyIsNull(obj)) {
        switch (checkStrong(obj.val())) {
            case 0:
                $("#TgMsg").html("<img src=\"/Images/zhucedenglu_32.png\" />");
                break;
            case 1:
                $("#TgMsg").html("<img src=\"/Images/zhucedenglu_34.png\" />");
                break;
            case 2:
                $("#TgMsg").html("<img src=\"/Images/zhucedenglu_36.png\" />");
                break;
            case 3:
                $("#TgMsg").html("<img src=\"/Images/zhucedenglu_38.png\" />");
                break;
            default:
                break;
        }
    }
}

function VerifyPWD() {
    var obj = $("#PWD");
    if (VerifyIsNull(obj)) {
        if ($("#yPWD").length > 0) {
            if (obj.val() == $("#yPWD").val()) {
                $("#PWDMsg").html("您输入的密码不行哟！(不能与原密码相同)");
                $("#PWDMsg").attr("Class", "msgE");
                obj.addClass("inputB");
                return false;
            }
        }
        if (obj.val().length < 6 || obj.val().length > 16 || obj.val() == $("#UserName").val()) {
            $("#PWDMsg").html("您输入的密码不行哟！(长度为6～16位，且不能与账号相同)");
            $("#PWDMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        } else {
            $("#PWDMsg").html("");
            $("#PWDMsg").attr("Class", "msgS");
            obj.removeClass("inputB");
            return true;
        }
    } else {
        $("#PWDMsg").html("您还没输入密码哟！");
        $("#PWDMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyTgPWD() {
    var obj = $("#PWD");
    if (VerifyIsNull(obj)) {
        if ($("#yPWD").length > 0) {
            if (obj.val() == $("#yPWD").val()) {
                $("#PWDMsg").html("您输入的密码不行哟！(不能与原密码相同)");
                $("#PWDMsg").attr("Class", "msgE");
                obj.addClass("inputB");
                return false;
            }
        }
        if (obj.val().length < 6 || obj.val().length > 16 || obj.val() == $("#UserName").val()) {
            $("#TgMsg").html("您输入的密码不行哟！(长度为6～16位，且不能与账号相同)");
            $("#TgMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        } else {
            $("#TgMsg").html("");
            $("#TgMsg").attr("Class", "msgS");
            obj.removeClass("inputB");
            return true;
        }
    } else {
        $("#TgMsg").html("您还没输入密码哟！");
        $("#TgMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyyPWD() {
    var obj = $("#yPWD");
    if (VerifyIsNull(obj)) {
        if (hex_md5(obj.val()) != $("#YMima").val()) {
            $("#yPWDMsg").html("您输入的密码跟原始密码不一致！");
            $("#yPWDMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        } else {
            $("#yPWDMsg").html("");
            $("#yPWDMsg").attr("Class", "msgS");
            obj.removeClass("inputB");
            return true;
        }
    } else {
        $("#yPWDMsg").html("您还没输入密码哟！");
        $("#yPWDMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyPWDAgain() {
    var obj = $("#PWDAgain");
    if (obj.val() == $("#PWD").val()) {
        $("#PWDAgainMsg").html("");
        $("#PWDAgainMsg").attr("Class", "msgS");
        obj.removeClass("inputB");
        return true;
    } else {
        $("#PWDAgainMsg").html("两次输入的密码不一致！");
        $("#PWDAgainMsg").attr("Class", "msgE");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyTgPWDAgain() {
    var obj = $("#PWDAgain");
    if (obj.val() == $("#PWD").val()) {
        $("#TgMsg").html("");
        $("#TgMsg").attr("Class", "msgS");
        obj.removeClass("inputB");
        return true;
    } else {
        $("#TgMsg").html("两次输入的密码不一致！");
        $("#TgMsg").attr("Class", "msgE");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyEmail() {
    var obj = $("#Email");
    if (VerifyIsNull(obj)) {
        var reg = /^\w{3,}@\w+(\.\w+)+$/;
        if (reg.test(obj.val())) {
            $("#EmailMsg").html("您输入的邮箱可以使用哟！");
            $("#EmailMsg").attr("Class", "msgS");
            obj.removeClass("inputB");
            return true;
        } else {
            $("#EmailMsg").html("您输入的邮箱不正确哟！");
            $("#EmailMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        }
    } else {
        $("#EmailMsg").html("您还没输入邮箱哟！");
        $("#EmailMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyRealName() {
    var obj = $("#RealName");
    if (VerifyIsNull(obj)) {
        $("#RealNameMsg").html("");
        $("#RealNameMsg").attr("Class", "msgS");
        obj.removeClass("inputB");
        return true;
    } else {
        $("#RealNameMsg").html("您还没输入真实姓名哟！");
        $("#RealNameMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyCard() {
    var obj = $("#Card");
    if (VerifyIsNull(obj)) {
        if (IdCardValidate(obj.val())) {
            $("#CardMsg").html("");
            $("#CardMsg").attr("Class", "msgS");
            obj.removeClass("inputB");
            $("#Sex").val(maleOrFemalByIdCard(obj.val()));
            return true;
        } else {
            $("#CardMsg").html("请输入真实且正确的身份证卡号哟！");
            $("#CardMsg").attr("Class", "msgE");
            obj.addClass("inputB");
            return false;
        }
    } else {
        $("#CardMsg").html("您还没输入身份证卡号哟！");
        $("#CardMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyCode() {
    var obj = $("#Code");
    if (VerifyIsNull(obj)) {
        $("#CodeMsg").html("");
        $("#CodeMsg").attr("Class", "msgS");
        obj.removeClass("inputB");
        return true;
    } else {
        $("#CodeMsg").html("您还没输入验证码哟！");
        $("#CodeMsg").attr("Class", "msgN");
        obj.addClass("inputB");
        return false;
    }
}

function VerifyCk() {
    if ($("#Ck").attr("checked")) {
        $("#CkMsg").html("");
        return true;
    } else {
        $("#CkMsg").html("请务必确认您已经阅读服务条款！");
        $("#CkMsg").attr("Class", "msgN");
        return false;
    }
}

var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1]; // 加权因子   
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2]; // 身份证验证位值.10代表X   
function IdCardValidate(idCard) {
    if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard);
    } else if (idCard.length == 18) {
        var a_idCard = idCard.split(""); // 得到身份证数组   
        if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}
//判断身份证号码为18位时最后的验证位是否正确  
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0; // 声明加权求和变量   
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10; // 将最后位为x的验证码替换为10方便后续操作   
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i]; // 加权求和   
    }
    valCodePosition = sum % 11; // 得到验证码所位置   
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    } else {
        return false;
    }
}
// 通过身份证判断是男是女  
function maleOrFemalByIdCard(idCard) {
    if (idCard.length == 15) {
        if (idCard.substring(14, 15) % 2 == 0) {
            return '1';
        } else {
            return '0';
        }
    } else if (idCard.length == 18) {
        if (idCard.substring(14, 17) % 2 == 0) {
            return '1';
        } else {
            return '0';
        }
    } else {
        return null;
    }
}
//验证18位数身份证号码中的生日是否是有效生日  

function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    document.getElementById("Birthday").value = year + "/" + month + "/" + day;
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    if (temp_date.getFullYear() != parseFloat(year)
          || temp_date.getMonth() != parseFloat(month) - 1
          || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}
//验证15位数身份证号码中的生日是否是有效生日  
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    document.getElementById("Birthday").value = year + "/" + month + "/" + day;
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    if (temp_date.getYear() != parseFloat(year)
              || temp_date.getMonth() != parseFloat(month) - 1
              || temp_date.getDate() != parseFloat(day)) {
        return false;
    } else {
        return true;
    }
}

function GameUserReg() {
    if (VerifyPWD() && VerifyEmail() && VerifyRealName() && VerifyCard() && VerifyCode() && VerifyCk()) {
        $("form").ajaxSubmit({
            url: "/Home/DoReg",
            type: "post",
            success: function (data, textStatus) {
                if (data == "True") {
                    location.href = "/UserCenter/";
                } else {
                    jsprint(data, "", "Error");
                }
            }
        });
    }
    return false;
}

function SpreadUserReg() {
    if (VerifyPWD()) {
        $("form").ajaxSubmit({
            url: "/SpreadCenter/AddUnderSpreader",
            type: "post",
            success: function (data, textStatus) {
                if (data == "True") {
                    //location.href = "/SpreadCenter";
                    jsprint("添加成功","","Success");
                } else {
                    jsprint(data, "", "Error");
                }
            }
        });
    }
    return false;
}

function GameUserKsReg() {
    if (VerifyTgPWD()) {
        $("form").ajaxSubmit({
            url: "/Tg/DoTg",
            type: "post",
            success: function (data, textStatus) {
                var re = data.split("|");
                if (re.length == 2) {
                    location.href = "/" + re[0] + "/" + "LoginGame?S=" + re[1];
                } else {
                    jsprint(re[0], "", "Error");
                    return false;
                }
            }
        });
    }
    return false;
}

function GameUserLogin() {
    if (!VerifyIsNull($("#UserName"))) {
        jsprint("用户名不能为空！", "", "Error");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        jsprint("密码不能为空！", "", "Error");
        return false;
    }
    if (!VerifyIsNull($("#Code"))) {
        jsprint("验证码不能为空！", "", "Error");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/DoLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "/UserCenter/";
            } else {
                jsprint(data, "", "Error");
            }
        }
    });
    return false;
}

function GameSpreadUserLogin() {
    if (!VerifyIsNull($("#UserName"))) {
        jsprint("用户名不能为空！", "", "Error");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        jsprint("密码不能为空！", "", "Error");
        return false;
    }
    if (!VerifyIsNull($("#Code"))) {
        jsprint("验证码不能为空！", "", "Error");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/DoLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "/SpreadCenter/";
            } else {
                jsprint(data, "", "Error");
            }
        }
    });
    return false;
}

function KsLogin() {
    if (!VerifyIsNull($("#UserName"))) {
        jsprint("用户名不能为空！", "", "Error");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        jsprint("密码不能为空！", "", "Error");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/KsLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "/Home";
            } else {
                jsprint(data, "", "Error");
            }
        }
    });
    return false;
}

function KsLogin2() {
    if (!VerifyIsNull($("#UserName"))) {
        alert("用户名不能为空！");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        alert("密码不能为空！");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/KsLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "/UserCenter/";
            } else {
                alert(data);
            }
        }
    });
    return false;
}

function KsLogin3() {
    if (!VerifyIsNull($("#UserName"))) {
        alert("用户名不能为空！");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        alert("密码不能为空！");
        return false;
    }
    if (!VerifyIsNull($("#Code"))) {
        alert("验证码不能为空！");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/DoLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "WdServers";
            } else {
                alert(data);
            }
        }
    });
    return false;
}

function KsLogin4() {
    if (!VerifyIsNull($("#UserName"))) {
        alert("用户名不能为空！");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        alert("密码不能为空！");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/KsLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "WdServers";
            } else {
                alert(data);
            }
        }
    });
    return false;
}

function KsLogin5() {
    if (!VerifyIsNull($("#UserName"))) {
        $("#errorinfo").show().html("用户名不能为空！").css("color","#c00");
        return false;
    }
    if (!VerifyIsNull($("#PWD"))) {
        $("#errorinfo").show().html("密码不能为空！").css("color", "#c00");
        return false;
    }
    $("form").ajaxSubmit({
        url: "/Home/KsLogin",
        type: "post",
        success: function (data, textStatus) {
            if (data == "True") {
                location.href = "WdServers";
            } else {
                $("#errorinfo").show().html(data).css("color","#c00");
            }
        }
    });
    return false;
}


function AdminLogin() {
    if (!VerifyIsNull($("#txtUserName"))) {
        $(".login_tip").html("<span>用户名不能为空！</span>");
        return false;
    }
    if (!VerifyIsNull($("#txtUserPwd"))) {
        $(".login_tip").html("<span>密码不能为空！</span>");
        return false;
    }
    if (!VerifyIsNull($("#txtCode"))) {
        $(".login_tip").html("<span>验证码不能为空！</span>");
        return false;
    }
    $.ajax({
        url: "/Admin/DelAdminLogin",
        type: "POST",
        data: {
            UserName: $("#txtUserName").val(),
            PassWord: $("#txtUserPwd").val(),
            VerifyCode: $("#txtCode").val()
        },
        success: function (text) {
            if (text == null || text == "") {
                location.href = "/Admin/Index";
            } else {
                $(".login_tip").html("<span>" + text + "</span>");
            }
            return false;
        },
        error: function () {
            return false;
        }
    });
    return false;
}
