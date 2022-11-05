panelapp.controller("ourserviceController", function ($scope, $http, $rootScope, PanelServices,) {
    var ourserviceTable;
    $scope.ourservice = {};

    $scope.getOurServiceList = function () {
        $http({
            method: "POST",
            url: "/Admin/OurService/GetOurServiceList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.ourserviceList = response.data;
        });
    };

    $scope.$on('ngRepeatOurServiceListFinished', function (ngRepeatOurServiceListFinishedEvent) {
        ourserviceTable = $('#ourservice-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            //"paging": false,
            "ordering": false,
            //"searching": false,
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

    $scope.newOurService = function () {
        $scope.ourservice = {};
        $scope.ourservice.Detail = {};
        $scope.ourservice.Id = 0;
        $scope.ourservice.DisplayOrder = $scope.ourserviceList.length + 1;
        $scope.newourservice = true;
        $scope.ourservice.Icon = "fa fa-glass";
        $("#ourserviceModal").modal();
    }

    $scope.updateOurService = function (ourService) {
        $scope.newourservice = false;
        $scope.ourservice = angular.copy(ourService);
        $("#ourserviceModal").modal();
    }

    $scope.$on('ngRepeatDetailFinished', function (ngRepeatDetailFinishedEvent) {
        $(".nav-tabs .nav-item .nav-link").removeClass("active");
        $(".tab-content .tab-pane").removeClass("show active");
        $(".nav-tabs .nav-item:first-child() .nav-link").addClass("active");
        $(".tab-content .tab-pane:first-child()").addClass("show active");
    });

    $scope.saveOurService = function (ourService) {
        $scope.newOurServiceDetailList = [];
        angular.forEach($rootScope.languageList, function (value, key) {
            $scope.ourserviceDetail = {};
            $scope.ourserviceDetail.OurServiceId = $scope.ourservice.Id;
            $scope.ourserviceDetail.Title = $('#ourserviceTitle-' + value.Id).val();
            $('#ourserviceTitle-' + value.Id).val("");
            $scope.ourserviceDetail.LanguageId = $('#ourserviceTitle-' + value.Id).attr('data-langno');
            $scope.ourserviceDetail.Description = $('#ourserviceDescription-' + value.Id).val();
            $('#ourserviceDescription-' + value.Id).val("");
            $scope.newOurServiceDetailList.push($scope.ourserviceDetail);
        });
        $scope.ourservice.Detail = $scope.newOurServiceDetailList;
        $scope.ourservice.Icon = $('#icon-class').val();
        $http({
            method: "POST",
            url: "/Admin/OurService/SaveOurService",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { ourService: $scope.ourservice }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $("#ourserviceModal").modal("hide");
            $scope.getOurServiceList();
            ourserviceTable.destroy();
            $scope.ourservice = {};
            $scope.newourservice = true;
        });
    }

    $scope.deleteOurService = function (ourService) {
        swal({
            title: 'Hizmet Sil!',
            text: "Seçilen hizmeti silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'post',
                    url: '/Admin/OurService/DeleteOurService',
                    data: { ourService: ourService },
                    success: function (data) {
                        iziToast.show({
                            message: data.Message,
                            position: 'topCenter',
                            color: data.Color,
                            icon: data.Icon
                        });
                        $scope.getOurServiceList();
                        ourserviceTable.destroy();
                    }
                });
            }
        });
    }

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getOurServiceList();
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