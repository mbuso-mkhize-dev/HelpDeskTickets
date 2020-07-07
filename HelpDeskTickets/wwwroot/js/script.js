$(function() {

	

	$(window).on("scroll", function(e) {
		if ($(window).scrollTop() > 50) {
			$("body").addClass("sticky");
		} else {
			$("body").removeClass("sticky");
		}
	});
       

	$('.rad-chat-body').slimScroll({
		height: '450px',
		color: "#c6c6c6"
	});

	$('.rad-timeline-body').slimScroll({
		height: '450px',
		color: '#c6c6c6'
	});
	
	$('.rad-activity-body').slimScroll({
		height: '250px',
		color: '#c6c6c6'
	});


	$(".rad-logo-container").toggleClass("rad-nav-min");
		$(".rad-sidebar").toggleClass("rad-nav-min");
		$(".rad-body-wrapper").toggleClass("rad-nav-min");
	$(".rad-toggle-btn").on('click', function() {
		$(".rad-logo-container").toggleClass("rad-nav-min");
		$(".rad-sidebar").toggleClass("rad-nav-min");
		$(".rad-body-wrapper").toggleClass("rad-nav-min");
		
	});

	$("li.rad-dropdown > a.rad-menu-item").on('click', function(e) {
		e.preventDefault();
		e.stopPropagation();
		$(".rad-dropmenu-item").removeClass("active");
		$(this).next(".rad-dropmenu-item").toggleClass("active");
	});

	$(".fa-chevron-down").on("click", function() {
		var $ele = $(this).parents('.panel-heading');
		$ele.siblings('.panel-footer').toggleClass("rad-collapse");
		$ele.siblings('.panel-body').toggleClass("rad-collapse", function() {
			setTimeout(function() {
				initializeCharts();
			}, 200);
		});
	});

	$(".fa-close").on("click", function() {
		var $ele = $(this).parents('.panel');
		$ele.addClass('panel-close');
		setTimeout(function() {
			$ele.parent().remove();
		}, 210);
	});



	

	var colorMap = {
		crimson: "crimson", 
		teal: "#1fb5ad", 
		orange: "#ff503f", 
		purple: "rebeccapurple", 
		twitter: "#55acee"
	};

});
