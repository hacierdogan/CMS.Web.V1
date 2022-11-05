panelapp.controller("homeController", function ($scope, $http, $rootScope, PanelServices,) {

    $scope.dashboard = {};
    $scope.webInfo = {};
    $scope.mission = {};


    $scope.getDashboard = function () {
        $http({
            method: "POST",
            url: "/Admin/Home/GetDashboard",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.dashboard = response.data.dashboard;
            $scope.webInfoList = response.data.webInfoList;
            $scope.missionList = response.data.missionList;
        });
    };

    $scope.updateMission = function (mission, rmv) {
        $http({
            method: "POST",
            url: "/Admin/Home/UpdateMission",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { id: mission.Id, status: mission.Status, delete: rmv }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getDashboard();
        });
    };


    $scope.newMission = function () {
        swal({
            text: 'Görev bilgisi girin.',
            content: "input",
            button: {
                text: "Ekle",
                closeModal: false,
            },
        })
            .then(value => {
                if (!value) {
                    return swal("Görev bilgisi girin.");
                }
                else {
                    $http({
                        method: "POST",
                        url: "/Admin/Home/SaveMission",
                        headers: { "Content-Type": "Application/json;charset=utf-8" },
                        data: { description: value }
                    }).then(function (response) {

                        iziToast.show({
                            message: response.data.Message,
                            position: 'topCenter',
                            color: response.data.Color,
                            icon: response.data.Icon
                        });
                        $scope.getDashboard();
                        //swal("Başarı", "Görev eklendi", "success");
                        swal.close();
                    });
                }
            });
    }


    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getDashboard();
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