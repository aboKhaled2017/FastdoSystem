/// <reference path="jquery-3.2.1.min.js" />
(function () {
	"use strict";

	var treeviewMenu = $('.app-menu');

	// Toggle Sidebar
	$('[data-toggle="sidebar"]').click(function(event) {
		event.preventDefault();
		$('.app').toggleClass('sidenav-toggled');
	});

	// Activate sidebar treeview toggle
	$("[data-toggle='treeview']").click(function(event) {
		event.preventDefault();
		if(!$(this).parent().hasClass('is-expanded')) {
			treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
		}
		$(this).parent().toggleClass('is-expanded');
	});

	// Set initial active toggle
	$("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

	//Activate bootstrip tooltips
	$("[data-toggle='tooltip']").tooltip();

   //handle sidePage activce liks on navigating
	$('.app-sidebar .app-menu li').each(function(){
		var $el = $(this);
		var $a = $el.find('.app-menu__item').eq(0);
		//$a.removeClass('active');
		if (location.href.includes($a.attr('href'))) {
			$a.addClass('active');
			$a.parent().siblings().find('.app-menu__item').removeClass('active')
		}
	})

})();
