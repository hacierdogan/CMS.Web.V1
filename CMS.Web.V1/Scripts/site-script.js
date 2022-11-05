// Progress Bar
$('.experience').bind('inview', function (event, visible, visiblePartX, visiblePartY) {
    if (visible) {
        $.each($('div.progress-bar'), function () {
            $(this).css('width', $(this).attr('aria-valuemax') + '%');
        });
        $(this).unbind('inview');
    }
});


// Preloader
function pageCustomLoader(status) {
    if (status===undefined||status===null||status==="") {
        status = false;
    }
    if (status) {
        $('#preloader').show('slow');
    }else {
        $('#preloader').delay(1000).hide('slow');
    }
};

//WOW Javascripts 
new WOW().init();


//Smooth Scroll
$(function () {
    $('a.scroll').click(function () {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html,body').animate({
                    scrollTop: target.offset().top - 50
                }, 1000);
                return false;
            }
        }
    });
});


//Navbar-Fixed-Top
$(window).bind('scroll', function () {
    var navHeight = $(window).height() - 100;
    if ($(window).scrollTop() > navHeight) {
        $('.navbar').addClass('on');
    } else {
        $('.navbar').removeClass('on');
    }
});


//ToolTip
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

//Count
$('#fun-facts').bind('inview', function (event, visible, visiblePartX, visiblePartY) {
    if (visible) {
        $(this).find('.timer').each(function () {
            var $this = $(this);
            $({ Counter: 0 }).animate({ Counter: $this.text() }, {
                duration: 2000,
                easing: 'swing',
                step: function () {
                    $this.text(Math.ceil(this.Counter));
                }
            });
        });
        $(this).unbind('inview');
    }
});

//Fullscreen burger menu
$(".menu-trigger, .mobilenav").click(function () {
    $(".mobilenav").fadeToggle(500);
});
$(".menu-trigger, .mobilenav").click(function () {
    $(".top-menu").toggleClass("top-animate");
    $(".mid-menu").toggleClass("mid-animate");
    $(".bottom-menu").toggleClass("bottom-animate");
});


//On click menu item animate to the section
$(".mobilenav li, .back-to-top").on('click', function () {
    var target = $(this).data('rel');
    var $target = $(target);
    $('html, body').stop().animate({
        'scrollTop': $target.offset().top
    }, 900, 'swing');
});

