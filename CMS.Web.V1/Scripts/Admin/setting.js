panelapp.controller("themeSettingController", function ($scope, $http, $rootScope, PanelServices,) {

    $scope.getThemeSetting = function () {
        $http({
            method: "POST",
            url: "/Admin/ThemeSetting/GetThemeSetting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.themeSetting = response.data;

            $('#Color1').minicolors({
                theme: "bootstrap",
                control: $(this).attr("data-control") || "hue",
                format: $(this).attr('data-format') || 'hex',
                opacity: $(this).attr("data-opacity"),
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                position: $(this).attr('data-position') || 'bottom left',
                defaultValue: $scope.themeSetting.Color1
            });
            $('#Color2').minicolors({
                theme: "bootstrap",
                control: $(this).attr("data-control") || "hue",
                format: $(this).attr('data-format') || 'hex',
                opacity: $(this).attr("data-opacity"),
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                position: $(this).attr('data-position') || 'bottom left',
                defaultValue: $scope.themeSetting.Color2
            });
            $('#Color3').minicolors({
                theme: "bootstrap",
                control: $(this).attr("data-control") || "hue",
                format: $(this).attr('data-format') || 'hex',
                opacity: $(this).attr("data-opacity"),
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                position: $(this).attr('data-position') || 'bottom left',
                defaultValue: $scope.themeSetting.Color3
            });
            $('#Color4').minicolors({
                theme: "bootstrap",
                control: $(this).attr("data-control") || "hue",
                format: $(this).attr('data-format') || 'hex',
                opacity: $(this).attr("data-opacity"),
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                position: $(this).attr('data-position') || 'bottom left',
                defaultValue: $scope.themeSetting.Color4
            });
        });
    };

    $scope.saveThemeSetting = function () {
        $scope.themeSetting.Color1 = $('#Color1').val();
        $scope.themeSetting.Color2 = $('#Color2').val();
        $scope.themeSetting.Color3 = $('#Color3').val();
        $scope.themeSetting.Color4 = $('#Color4').val();
        $http({
            method: "POST",
            url: "/Admin/ThemeSetting/SaveThemeSetting",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { setting: $scope.themeSetting }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getThemeSetting();
        });
    }

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getThemeSetting();
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