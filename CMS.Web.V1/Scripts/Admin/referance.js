panelapp.controller("referanceController", function ($scope, $http, $rootScope, PanelServices,) {
    var referanceTable;
    $scope.referance = {};
    $scope.form = false;
    $scope.getReferanceList = function () {
        $http({
            method: "POST",
            url: "/Admin/Referance/GetReferanceList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.referanceList = response.data;
        });
    };

    $scope.$on('ngRepeatReferanceListFinished', function (ngRepeatReferanceListFinishedEvent) {
        referanceTable = $('#referance-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            //"paging": false,
            "ordering": false,
            //"searching": false,
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

   
    $scope.newReferance = function () {
        $scope.form = true;
        $scope.referance.Id = 0;
        $scope.referance.Title = "";
        $scope.referance.Url = "";
        $scope.referance.PicturePath = "/Content/images/image-file.jpg";
        $scope.referance.Status = true;
        $scope.referance.DisplayOrder = $scope.referanceList.length+1;
    }

    $scope.updateReferance = function (referance) {
        $scope.referance = angular.copy(referance);
        $scope.form = true;
    }

    $scope.saveReferance = function () {
        if ($scope.referance.Title == "") {
            iziToast.error({
                type: "Error",
                message: "Tanım bilgisi giriniz.",
                position: 'topCenter'
            });
        }
        else if ($scope.referance.Id == 0 && $("#imageInput")[0].files.length == 0) {
            iziToast.error({
                type: "Error",
                message: "Resim seçiniz.",
                position: 'topCenter'
            });
        }
        else {          
            $http({
                method: "POST",
                url: "/Admin/Referance/WriteReferance",
                headers: { "Content-Type": "Application/json;charset=utf-8" },
                data: { referance: $scope.referance }
            }).then(function (response) {
                var data = new FormData();
                data.append("fileSelected", $("#imageInput")[0].files[0]);
                $.ajax({
                    url: "/Admin/Referance/SaveReferance",
                    type: 'POST',
                    dataType: 'json',
                    data: data,
                    contentType: false,
                    processData: false
                }).then(function successCallback(response) {
                    iziToast.show({
                        message: response.Message,
                        position: 'topCenter',
                        color: response.Color,
                        icon: response.Icon
                    });
                    $scope.getReferanceList();
                    referanceTable.destroy();
                });
                $scope.referance = {};
                $scope.form = false;
            });
        }
    }

    $scope.deleteReferance = function (referance) {
        swal({
            title: 'Refrans Sil!',
            text: "Seçilen referansı silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $http({
                    method: "POST",
                    url: "/Admin/Referance/DeleteReferance",
                    headers: { "Content-Type": "Application/json;charset=utf-8" },
                    data: { referance: referance }
                }).then(function (response) {
                    iziToast.show({
                        message: response.data.Message,
                        position: 'topCenter',
                        color: response.data.Color,
                        icon: response.data.Icon
                    });
                    $scope.getReferanceList();
                    referanceTable.destroy();
                });
            }
        });
    }

    function FileImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#fileImg').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        FileImageChange(this);
    });

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getReferanceList();
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