siteapp.controller("homeController", function ($scope, $http, $rootScope, CMSServices) {

    $scope.sliderList = [];
    $scope.about = [];
    $scope.ourServiceList = [];
    $scope.tabList = [];
    $scope.blogOrNewsList = [];
    $scope.galleryList = [];
    $scope.buttonList = [];
    $scope.socialMedia = [];
    $scope.company = [];
    $scope.message = {
        Title: "",
        Mail: "",
        Subject:"",
        MessageStr:""
    };

    $scope.getSliderList = function () {
        $http({
            method: "POST",
            url: "/Home/GetSliderList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.sliderList = response.data;
        });
    };
    $scope.getAbout = function () {
        $http({
            method: "POST",
            url: "/About/GetAbout",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.about = response.data;
        });
    };
    $scope.getOurServiceList = function () {
        $http({
            method: "POST",
            url: "/OurService/GetOurServiceList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.ourServiceList = response.data;
        });
    };
    $scope.getTabList = function () {
        $http({
            method: "POST",
            url: "/Home/GetTabList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.tabList = response.data;
        });
    };
    $scope.getBlogOrNews = function () {
        $http({
            method: "POST",
            url: "/News/GetBlogOrNews",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.blogOrNewsList = response.data;
        });
    };
    $scope.$on('ngNewsFinished', function (ngNewsFinishedEvent) {
        $('#owl-news').owlCarousel({
            itemsCustom: [
                [0, 1],
                [450, 2],
                [600, 3],
                [700, 3],
                [1000, 3],
                [1200, 3],
                [1400, 4],
                [1600, 4]
            ],
            pagination: false,
            autoPlay: 3000,
        });

    });



    $scope.getGalleryList = function () {
        $http({
            method: "POST",
            url: "/Gallery/GetGalleryList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.galleryList = response.data.GalleryList;
            $scope.buttonList = response.data.ButtonList;
        });
    };

    $scope.$on('ngRepeatGalleryFinished', function (ngRepeatGalleryFinishedEvent) {
        'use strict';
        var $portfolio = $('.portfolio-items');
        //$portfolio[0].clientHeight = 496;
        $portfolio.isotope({
            itemSelector: '.portfolio-item',
            layoutMode: 'fitRows'
        });

        var $portfolio_selectors = $('.portfolio-filter >li>a');
        $portfolio_selectors.on('click', function () {
            $portfolio_selectors.removeClass('active');
            $(this).addClass('active');
            var selector = $(this).attr('data-filter');
            $portfolio.isotope({ filter: selector });
            return false;
        });

        //Pretty Photo
        $("a[data-rel^='prettyPhoto']").prettyPhoto({
            social_tools: false
        });

    });
    const myTimeout = setTimeout(openGallery, 2000);
    function openGallery() {
        $('.portfolio-filter >li>a')[0].click();
    }

    $scope.getReferenceList = function () {
        $http({
            method: "POST",
            url: "/Reference/GetReferenceList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.referenceList = response.data;
        });
    };
    $scope.$on('ngRepeatReferenceFinished', function (ngRepeatReferenceFinishedEvent) {
        $("#reference-slider").owlCarousel({
            itemsCustom: [
                [0, 2],
                [450, 3],
                [600, 3],
                [700, 4],
                [1000, 5],
                [1200, 5],
                [1400, 5],
                [1600, 5]
            ],
            pagination: false,
            autoPlay: 3000,
        });
    });


    $scope.getCompanyInfo = function () {
        $http({
            method: "POST",
            url: "/Home/GetCompanyInfo",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.company = response.data;
        });
    };


    $scope.getPopup = function () {
        $http({
            method: "POST",
            url: "/Home/GetPopup",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.popup = response.data.Popup;
            $scope.Ispopup = response.data.PopupStatus;

            if ($scope.Ispopup) {
                $("#popupModal").modal();
            }
        });
    };
    

    $scope.sendMessage = function () {
        $http({
            method: "POST",
            url: "/Contact/SendMessage",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { message: $scope.message}
        }).then(function (response) {
            swal(response.data.Message, "", response.data.Statu);
            $scope.message = {
                Title: "",
                Mail: "",
                Subject: "",
                MessageStr: ""
            };
        });
    };


    $(document).ready(function () {
        pageCustomLoader(true);
        $scope.getSliderList();
        $scope.getAbout();
        $scope.getOurServiceList();
        $scope.getTabList();
        $scope.getBlogOrNews();
        $scope.getGalleryList();
        $scope.getReferenceList();
        $scope.getCompanyInfo();
        $scope.getPopup();

        $("#owl-testi").owlCarousel({
            autoPlay: 3000,
            singleItem: true,
            transitionStyle: "fade"
        });
        pageCustomLoader(false);
    });
}).directive('onFinishRender', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    scope.$emit(attr.onFinishRender);
                });
            }
        }
    };
});


