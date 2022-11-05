panelapp.controller("mailSettingController", function ($scope, $http, $rootScope, PanelServices,) {
   
    $scope.getMailSetting = function () {
        $http({
            method: "POST",
            url: "/Admin/MailSetting/GetMailSettting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.mail = response.data;
        });
    };

    $scope.saveMailSetting = function () {
        $http({
            method: "POST",
            url: "/Admin/MailSetting/SaveMailSetting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { mail: $scope.mail }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getMailSetting();
        });
    }

    $scope.mailTest = function () {
        $http({
            method: "POST",
            url: "/Admin/MailSetting/MailTest",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { mail: $scope.mail }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
        });
    }
   
    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getMailSetting();
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