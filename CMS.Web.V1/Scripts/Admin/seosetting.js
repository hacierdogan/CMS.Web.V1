panelapp.controller("seoSettingController", function ($scope, $http, $rootScope, PanelServices,) {
   
    $scope.getSeoSetting = function () {
        $http({
            method: "POST",
            url: "/Admin/SeoSetting/GetSeoSettting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.seo = response.data;
        });
    };

    $scope.saveSeoSetting = function () {
        $http({
            method: "POST",
            url: "/Admin/SeoSetting/SaveSeoSetting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { seo: $scope.seo }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getSeoSetting();
        });
    }

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getSeoSetting();
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