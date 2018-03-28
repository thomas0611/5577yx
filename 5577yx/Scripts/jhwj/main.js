
//
$(function(){
	$('.Menubox a').click(function(){
		$('.Menubox a').removeClass('current');
		$(this).addClass('current');
		$('.yin').hide();
	})	
	
	$('#one1').click(function(){
		$('#con_one_1').show();
	});
	$('#one2').click(function(){
		$('#con_one_2').show();
	});
	$('#one3').click(function(){
		$('#con_one_3').show();
	});
	
	$(".box1").click(function () {
		$(this).toggleClass("box2");
	});
	
});