siteapp.controller("layoutController", function ($scope, $http, $rootScope, CMSServices) {
   
    $scope.selectedLanguage = {};
  
    $scope.getSelectedLanguage = function () {
        $http({
            method: "POST",
            url: "/Home/GetSelectedLanguage",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.selectedLanguage = response.data;
        });
    };

  

    $(document).ready(function () {
        $rootScope.getLanguageList(); //Language List
        CMSServices.getLanguageCount(); //Language Count
        $scope.getSelectedLanguage();
    });
})


siteapp.controller("footerController", function ($scope, $http, $rootScope, CMSServices) {

    $scope.getSocialList = function () {
        $http({
            method: "POST",
            url: "/Home/GetSocialMediaList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.socialMedia = response.data;
        });
    };

    $(document).ready(function () {
        $scope.getSocialList();
    });
});