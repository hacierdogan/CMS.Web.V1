var panelapp = angular.module('ngPanelApp', []).service("PanelServices", ['$http', function ($http) {
}]);
panelapp.run(['$rootScope', '$http', function ($rootScope, $http) {
     $rootScope.getLanguageList = function () {
        $http({
            method: "POST",
            url: "/Language/GetLanguageList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $rootScope.languageList = response.data;
        });
    };
    $rootScope.getMessaceCount = function () {
        $http({
            method: "POST",
            url: "/Admin/Message/GetMessageCount",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $rootScope.messageCount = response.data;
        });
    };
    $rootScope.getAuthorityUser = function () {
        $http({
            method: "POST",
            url: "/Admin/Home/GetAuthorityUser",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $rootScope.authorityUser = response.data;
        });
    };
    $(document).ready(function () {
        $rootScope.getMessaceCount();
        $rootScope.getAuthorityUser();
    });

}]);

var siteapp = angular.module('ngApp', ['ngSanitize']).service("CMSServices", ['$http', function ($http) {
    this.getLanguageCount = function () {
        $http({
            method: "POST",
            url: "/Home/GetLanguageList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $('#adet').html(response.data.length);
        });
    };
}]);

siteapp.run(['$rootScope', '$http', function ($rootScope, $http) {
    $rootScope.getLanguageList = function () {
        $http({
            method: "POST",
            url: "/Home/GetLanguageList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $rootScope.languageList = response.data;
            // $scope.selectedLanguage = response.data.filter(x => x.selected == true);
        });
    };

    //$rootScope.getPagePopup = function () {
    //    $http({
    //        method: "POST",
    //        url: "/Home/GetLanguageList",
    //        headers: { "Content-Type": "Application/json;charset=utf-8" },
    //        data: {}
    //    }).then(function (response) {
    //        $rootScope.pagePopup = response.data;
    //        // $scope.selectedLanguage = response.data.filter(x => x.selected == true);
    //    });
    //};
}]);

