panelapp.controller("loggeneralController", function ($scope, $http, $rootScope, PanelServices,) {
    $scope.logExplanation = "";
    $scope.logType = 0;
    
    $scope.getLogList = function (typ) {
        $scope.logType = typ;
        $http({
            method: "POST",
            url: "/Admin/LogGeneral/GetLogList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { type: typ}
        }).then(function (response) {
            $scope.logList = response.data;
        });
    };

    $scope.logDetail = function (detail) {
        $scope.logExplanation = detail;
        $('#logModal').modal();
    };

    $(document).ready(function () {
        $scope.getLogList(0);
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