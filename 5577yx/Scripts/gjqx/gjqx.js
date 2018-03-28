/**
 * 组件 — 渐变交错图片轮播
 * @param {Object} options
 * @param {String} imgWrap 轮播图片容器选择器
 * @param {String} controllWrap 轮播控制按钮容器选择器
 * @param {Number} timer 轮播计时器
 * @param {Number} gap 轮播间隔
 * @param {Number} fadeSpeed 渐变消失速度
 */
$.fn.slideShow = function(options){	
	var defaults = {
		imgWrap: '#slideShow .slide-pic',
		controllWrap: '#slideShow .slide-index',
		timer: 0,
		gap: 4000,
		fadeSpeed: 300
	};
	
	this.each(function(){
		var settings = $.extend(defaults, options);
		
		//图片轮播事件
		settings.timer = setTimeout(function(){
			$.fn.autoPlay();
		}, settings.gap);
		
		//图片索引鼠标事件
		$(settings.controllWrap).find('li').bind('mouseover', function(){
			if ($(this).hasClass('curr')) {
				return false;
			}
			else {
				$(this).autoPlay();
			}
		}).bind('click', function(){
			return false;
		});
		
		//自动播放
		$.fn.autoPlay = function(){
			clearTimeout(settings.timer);
			
			var currPic = $(settings.imgWrap).find('.curr');
			var nextIndex = this.changeIndex() + 1;
			var nextPic = $(settings.imgWrap).find('.slide-' + nextIndex);
			currPic.fadeOut(settings.fadeSpeed, function(){
				currPic.removeClass('curr');
			});
			
			nextPic.fadeIn(settings.fadeSpeed, function(){
				nextPic.addClass('curr');
			});
			
			settings.timer = setTimeout(function(){
				$.fn.autoPlay();
			}, settings.gap);
		};
		
		//为下一张图片匹配索引值
		$.fn.changeIndex = function(){
			var me = this,
				i = 0,
				index = 0;
			
			if(me.context == undefined){
				i = $(settings.controllWrap).find('li.curr').index();
				i = i >= $(settings.controllWrap).find('li').length - 1 ? 0 : i + 1;
				me = $(settings.controllWrap).find('li').eq(i);
			}
			$(settings.controllWrap).find('.curr').removeClass('curr');
			$(me).addClass('curr');
			
			i = $(settings.controllWrap).find('li.curr').index();
			return i;
		};
	});
	
};

/**
 * 组件 — 简单交互手风琴内容切换（图右标题左）
 * @param {Object} options
 * @param {String} accWrap “风琴”内容选择器
 * @param {String} controllWrap 控制按钮选择器
 * @param {String} offsetWidth 手风琴褶位置移动宽度
 */
$.fn.accordion = function(options){
	var defaults = {
		accWrap: '#accordian li.slide',
		controllWrap: '#accordian .slide-btn',
		offsetWidth: '500px'
	};
	
	this.each(function(){
		var settings = $.extend(defaults, options);
		
		$(settings.accWrap).bind('active', function(){
			var me = $(this);
			if(!me.hasClass('open')){
				me.addClass('open');
				me.animate({left: "-=" + settings.offsetWidth});
				me.prev().trigger('open');
				me.next().trigger('close');	
			}else{
				me.next().trigger('close');
			}
			me.siblings().removeClass('active');
			me.addClass('active');
		}).bind('open', function(){
			var me = $(this);
			if(!me.hasClass('open')){
				me.addClass('open');
				me.animate({left: "-=" + settings.offsetWidth});
				me.prev().trigger('open');
			}
		}).bind('close', function(){
			var me = $(this);
			if(me.hasClass('open')){
				me.removeClass('open');
				me.animate({left: "+=" + settings.offsetWidth});
				me.next().trigger('close');
			}
		});
		
		$(settings.controllWrap).click(function(){
			$(this).parent().trigger('active');
		});
	});
};

/**
 * 组件 — 内容向上卷动
 * @param {Object} options
 * @param {String} line 每次滚动的行数，默认为一屏，即父容器高度
 * @param {String} speed 卷动速度，数值越大，速度越慢（毫秒），默认为500
 * @param {String} timer 滚动的时间间隔（毫秒），默认为5000
 */
$.fn.carousel = function(options, callback){
	var defaults = {
		speed: 500,
		gap: 5000
	};
	var settings = $.extend(defaults, options);
	var timer = 0;
	
	var $this = this.eq(0).find("ul:first");
	var lineH = $this.find("li:first").height(), //获取行高
		line = settings.line ? parseInt(settings.line, 10) : parseInt($this.height()/lineH, 10), 
		speed = parseInt(settings.speed, 10),
		gap = parseInt(settings.gap, 10); 
	if(line == 0) line = 1;
	var upHeight = 0 - line * lineH;
	
	timer = setInterval(scrollUp, settings.gap);
	function scrollUp(){
		$this.animate({
			marginTop: upHeight
		},speed,function(){
			for(i = 1; i <= line; i++){
				$this.find("li:first").appendTo($this);
			}
			$this.css({marginTop:0});
		});
	}
	
	this.hover(function(){
		clearInterval(timer);
	}, function(){
		timer = setInterval(scrollUp, settings.gap);
	});
};


/**
 * 组件 - Tab & Content selector
 * @param {Object} options
 * @param {String} indexWrap tab菜单项选择器
 * @param {String} contentWrap tab内容外层容器选择器
 * @param {String} tabContent tab内容子容器选择器
 * @param {String} classname 显示隐藏classname
 * @param {Boolean} initSelect 是否根据url参数预选tab，默认为不预选
 * @param {String} indexName url参数预选名
 * @param {Number} switchMethod tab索引切换方式：click、mouseover
 */
$.fn.tabSwitch = function(options){
	var that = this;
	var opts = $.extend({ 
			indexWrap: '.tab-index',
			contentWrap: '.tab-content',
			tabContent: '.tab-box',
			cname: 'on',
			initSelect: false,
			indexName: 'no',
			switchMethod: 'click'
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
	
	function switchTab(){
		switch(opts.switchMethod){
			case 'click':
				indexWrap.find('li').bind('click', changeTabStatus);
				break;
			case 'mouseover':
				indexWrap.find('li').bind('mouseover', changeTabStatus);
				break;
		}
	}
	
	function changeTabStatus(el, flag){		
		var me = flag ? el : $(this);
		$(opts.indexWrap).find('.' + opts.cname).removeClass(opts.cname);
		me.addClass(opts.cname);
		selectContent(me);
	}
	
	function selectContent($el){
		contentWrap.find('.' + opts.cname).removeClass(opts.cname);
		contentWrap.find(opts.tabContent).eq(indexWrap.find('li').index($el)).addClass(opts.cname);
	}
	
	/*get URL parameter*/
	function getQueryString(name){
		var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
		var r = window.location.search.substr(1).match(reg);
		if (r != null) {
			return unescape(r[2]);
		}
		return null;
	}
	
	/*decide to select or not*/
	function changeTab(num){
		var index;
		if(num){
			index = $(opts.indexWrap).find('li').eq(num - 1);
			changeTabStatus(index, true);
		}
	}
};