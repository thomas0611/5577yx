(function($){
	$.fn.setTab = function(o){
		o = $.extend({
			btn:".btn a",
			div:".tab",
			tag:"show"
		},o||{});
		return this.each(function(){
			var btn = $(o.btn,this),tab =$(o.div,this),that=this;
				btn.click(function(){

					var tag ="."+$(this).attr(o.tag);

					tab.hide();
					$(tag,that).show();

				})
		})
	}
})(jQuery)

	