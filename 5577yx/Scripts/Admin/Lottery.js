var StartPosition = 1;              //定义开始位置，默认从1号开始
var ElmCount = 0;                   //定义当前元素数量
var CurrentIndex = 1;               //定义当前位置，默认从1号开始
var PreviousIndex = 1;              //当前位置的前一位置，默认为1，最小不能为小于1
var Speed = 0;                      //定义转动速度
var Timeout;                        //定义定时器
var LotteryNum = 0;
var LotteryText;

$(document).ready(function () {
    ElmCount = $("div[id^='C']").length;        //获取当前元素数量
    $("#C" + CurrentIndex).attr("style", "border:3px solid red;");      //当前元素选中
});

function StartLottery() {
    if (CurrentIndex > 1) {
        PreviousIndex = CurrentIndex - 1;
    } else if (CurrentIndex == 1) {
        PreviousIndex = 16;
    }
    $("#C" + CurrentIndex).attr("style", "border:3px solid red;");
    $("#C" + PreviousIndex).attr("style", "border:3px solid #fff;");
    if (CurrentIndex == ElmCount) {
        CurrentIndex = 0;
    }
    if (Speed > 500 && CurrentIndex == LotteryNum) {
        clearTimeout(Timeout);
        Speed = 0;                  //重置速度
        ShowLottery();
    } else {
        Speed = Speed + 11;
        Timeout = setTimeout("StartLottery()", Speed);
    }
    CurrentIndex++;
}

function GetLottery() {
    $.ajax({
        type: "post",
        url: "../PayCenter/GetLottery",
        success: function (text) {
            var re = text.split("|");
            LotteryNum = re[0];
            LotteryText = re[1];
        }, complete: function () {
            if (LotteryNum > 0) {
                StartLottery();
            } else {
                ShowLottery();
            }
        }
    });
}

function GetUserPoints() {
    $.ajax({
        type: "get",
        url: "../Models/Function.ashx?method=InitLeftInfo",
        success: function (text) {
            if (text.length > 0) {
                var obj = jQuery.parseJSON(text);
                $("#Points").html(obj.Points);
            }
        }
    })
}

function ShowLottery() {
    $.ajax({
        type: "get",
        url: "/payCenter/GetLotteryInfo",
        success: function (text) {
            $("#LotteryInfo").html(text);
        }
    })
    alert(LotteryText);
    $("#Start").attr("onclick", "Start()");
}

function Start() {
    $("#Start").attr("onclick", "");
    GetLottery();
    GetUserPoints();
}