panelapp.controller("socialMediaController", function ($scope, $http, $rootScope, PanelServices,) {
    $scope.getSocialMediaList = function () {
        $http({
            method: "POST",
            url: "/Admin/SocialMedia/GetSocialMedia",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.socialMedia = response.data;
        });
    };

    $scope.saveSocialMedia = function () {
        $http({
            method: "POST",
            url: "/Admin/SocialMedia/SaveSocialMedia",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { social: $scope.socialMedia}
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
        });
    };

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getSocialMediaList();
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