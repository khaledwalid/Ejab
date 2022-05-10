$(function () {

    "use stric";
    // Scroll Top 

    var scrollButton = $("#scroll-top");

    $(window).scroll(function () {

        if ($(this).scrollTop() >= 150) {
            scrollButton.show();
        } else {
            scrollButton.hide();
        }

    });


    // scroll top

    scrollButton.click(function () {

        $("html,body").animate({
            scrollTop: 0
        }, 1500);
    });

    // loading

    $(window).on('load', function () {
        if ($('.loader').length) {
            $('.loader').delay(200).fadeOut(500);
        }
    });

    // change bars

    $('#nav-icon1,#nav-icon2,#nav-icon3,#nav-icon4').click(function () {
        $(this).toggleClass('open');
    });


    //   resize window 

    $('#home').height($(window).height() - 100);

    $(window).resize(
        function () {
            $('header').height($(window).height() - 100);
        }
    );


    // active linkes header

    $('.navs li').click(function () {
        $(this).addClass('active').siblings().removeClass('active');
    });

    // smooth scroll to div 

    $('.navs li a').click(function () {
        $('html,body').animate({
            scrollTop: $('#' + $(this).data('value')).offset().top - 77
        }, 1000);

    });


    // change navbar top

    var nav = $(".navbar.navbar-default");

    $(window).scroll(function () {

        if ($(this).scrollTop() >= 60) {
            nav.addClass('navbar-fixed-top navbars ');
        } else {
            nav.removeClass('navbar-fixed-top navbars ');

        }

    });


    // remove collapse in mobile

    $('.navbar.navbar-default .navs').click(function () {
        $('.navbar.navbar-default .navbar-collapse').removeClass('in');
        $('.navbar-header .navbar-toggle').removeClass('open');
    });


    // remove active panel
    $('#accordion-1').on('show.bs.collapse', function (n) {
        $(n.target).siblings('.panel-heading').toggleClass('active');
    });
    $('#accordion-1').on('hide.bs.collapse', function (n) {
        $(n.target).siblings('.panel-heading').toggleClass('active');
    });

    // picture
    $(".pic").owlCarousel({
        items: 5,
        autoplay: 4000,
        loop: true,
        nav: false,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        responsive: {
            0: {
                items: 1
            },
            300: {
                items: 1
            },
            320: {
                items: 2
            },
            480: {
                items: 2
            },
            600: {
                items: 4
            },
            1200: {
                items: 5
            }
        }
    });

    // accordion
    $('#accordion-1').on('show.bs.collapse', function (n) {
        $(n.target).siblings('.panel-heading').find('.panel-title i').toggleClass('fa-angle-up fa-angle-down');
    });
    $('#accordion-1').on('hide.bs.collapse', function (n) {
        $(n.target).siblings('.panel-heading').find('.panel-title i').toggleClass('fa-angle-down fa-angle-up');
    });



   



});
