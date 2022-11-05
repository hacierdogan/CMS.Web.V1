siteapp.controller("maintenancecontroller", function ($scope, $http, $rootScope, CMSServices,) {

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

    $(document).ready(function () {
        $scope.getCompanyInfo();
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