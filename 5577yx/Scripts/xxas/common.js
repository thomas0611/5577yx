/**
 * @author jiangcunyun
 * @date 2012-11-30
 */
var Xianxia = {};

Xianxia.Common = {
    initialize: function () {
        var that = Xianxia;
        if ($.browser.msie) { //only to IE
            $('#LoginForm .input-bar input').addClass('watermark');
            $('#Username').val('请输入用户名');
            $('#Password').val('请输入密码');
            that.Watermark.init();
        }

        that.SlideShow.init();
        that.ShowMask.init();
    }
};

Xianxia.Watermark = {
    init: function () {
        $('input[type="text"], input[type="password"]').focus(function () {
            $(this)[0].value = '';
            $(this).removeClass('watermark');
        });
    }
};

/**
 * 页头背景图渐变轮播效果
 */
Xianxia.SlideShow = {
    init: function () {
        var me = this;

        //图片轮播事件
        var slideWrap = $(me.param.imgWrap).find('div.role:eq(0)');
        me.param.timer = setTimeout(function () {
            me.autoPlay();
        }, me.param.gap);

        //图片索引圆点鼠标事件
        $(me.param.controllWrap + ' li').bind('mouseover', function () {
            if ($(this).hasClass('on')) {
                return false;
            }
            else {
                me.autoPlay($(this));
            }
        }).click(function () {
            return false;
        });
    },
    //自动播放
    autoPlay: function (obj) {
        var me = this;
        clearTimeout(me.param.timer);

        var currPic = $(me.param.imgWrap).find('.on');
        var nextIndex = this.changeIndex(obj) + 1;
        var nextPic = $(me.param.imgWrap).find('.r' + nextIndex);
        currPic.fadeOut(me.param.fadeSpeed, function () {
            currPic.removeClass('on');
        });

        nextPic.fadeIn(me.param.fadeSpeed, function () {
            nextPic.addClass('on');
        });

        me.param.timer = setTimeout(function () {
            me.autoPlay();
        }, me.param.gap);
    },
    //为下一张图片匹配索引值
    changeIndex: function (obj) {
        var me = this,
			i = 0,
			index = 0;

        if (typeof (obj) == 'undefined') {
            i = $(me.param.controllWrap).find('.on').index();
            i = i >= $(me.param.controllWrap).find('li').length - 1 ? 0 : i + 1;
            obj = $(me.param.controllWrap).find('li').eq(i);
        }
        $(me.param.controllWrap).find('.on').removeClass('on');
        $(obj).addClass('on');
        $(obj).find('span').addClass('on');

        i = $(me.param.controllWrap).find('li.on').index();
        return i;
    },
    param: {
        imgWrap: '#Header .role-pic',
        controllWrap: '#Header .role-index',
        timer: 0,
        gap: 7000,
        fadeSpeed: 300
    }
};

/**
 * Tab & Content selector
 * @param {Object} options
 * @param {String} indexWrap tab菜单项
 * @param {String} contentWrap tab内容外层容器
 * @param {String} tabContent tab内容子容器
 * @param {String} classname 显示隐藏classname
 * @param {boolean} initSelect 是否根据url参数预选tab，默认为不预选
 * @param {boolean} indexName url参数预选名
 */
$.fn.TabSwitch = function (options) {
    var that = this;
    var opts = $.extend({
        indexWrap: '.tab-index',
        contentWrap: '.tab-con',
        tabContent: '.wrap',
        cname: 'on',
        initSelect: false,
        indexName: 'no'
    }, options);
    var indexWrap = $(opts.indexWrap),
		contentWrap = $(opts.contentWrap);

    if (opts.initSelect) {
        var num = getQueryString(opts.indexName);
        if (num) {
            changeTab(num);
        }
    }

    switchTab();

    function switchTab() {
        indexWrap.find('li').bind('mouseover', changeTabStatus);
    }

    function changeTabStatus(el, flag) {
        var me = flag ? el : $(this);
        $(opts.indexWrap).find('.' + opts.cname).removeClass(opts.cname);
        me.addClass(opts.cname);
        selectContent(me);
    }

    function selectContent($el) {
        contentWrap.find('.' + opts.cname).removeClass(opts.cname);
        contentWrap.find(opts.tabContent).eq(indexWrap.find('li').index($el)).addClass(opts.cname);
    }

    /*get URL parameter*/
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return unescape(r[2]);
        }
        return null;
    }

    /*decide to select or not*/
    function changeTab(num) {
        var index;
        if (num) {
            index = $(opts.indexWrap).find('li').eq(num - 1);
            changeTabStatus(index, true);
        }
    }
};


/**
 * 遮罩弹出层
 */
Xianxia.ShowMask = {
    init: function () {
        var me = this;
        $(me.param.showBtn).click(function () {
            if ($(me.param.box).children().size() == 0) {
                content = '<object id="VideoPlayer" width="600" height="400" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"> \
				  <param value="../Content/xxas/mediaplayer.swf?file=' + FLASH_URL + '&amp;autostart=true" name="movie"> \
				  <param value="high" name="quality"> \
				  <param value="true" name="allowFullScreen"> \
				  <param value="always" name="allowscriptaccess"> \
				  <param value="transparent" name="wmode"> \
				  <embed name="videoPlayer" width="600" height="400" src="../Content/xxas/mediaplayer.swf?file=' + FLASH_URL + '&amp;autostart=true" allowfullscreen="true" wmode="transparent" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" quality="high"> \
				</object>';
                me.showVideo(content);
            }
            me.showAll();
            return false;
        });
        $(me.param.closeBtn).live('click', me.closeAll);

        $(window).resize(function () {
            if ($(me.param.wrap).children().css('display') == 'block') {
                me.resizeBox();
            }
        });
    },
    showVideo: function (content) {
        var me = this;
        var html = '<a href="#" id="CloseBtn"></a>' + content;
        $(me.param.box).html(html);
    },
    //显示遮罩层
    showMask: function () {
        var me = Xianxia.ShowMask;
        $(me.param.mask).css({
            'height': $(document).height(),
            'width': $(document).width()
        }).show();
    },
    //显示内容层
    showBox: function () {
        var me = Xianxia.ShowMask;
        var top = ($(window).height() - $(me.param.box).height()) / 2;
        var left = ($(window).width() - $(me.param.box).width()) / 2;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();
        $(me.param.box).css({
            'position': 'absolute',
            'top': top + scrollTop,
            'left': left + scrollLeft
        }).show();
    },
    showAll: function () {
        var me = Xianxia.ShowMask;
        me.showMask();
        me.showBox();
    },
    closeAll: function () {
        var me = Xianxia.ShowMask;
        $(me.param.mask).hide();
        $(me.param.box).children().remove().andSelf().hide();
        return false;
    },
    resizeBox: function () {
        Xianxia.ShowMask.showAll();
    },
    param: {
        wrap: '#ShowMask',
        showBtn: '#Video a',
        mask: '#Mask',
        box: '#PlayerBox',
        closeBtn: '#CloseBtn'
    }
};

$(document).ready(function () {
    Xianxia.Common.initialize();
});


/*请按示例填写完整flash文件路径*/
var FLASH_URL = 'http://www.xxxx.com/flash/xxx.swf';
