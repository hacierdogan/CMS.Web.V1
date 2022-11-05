panelapp.controller("popupController", function ($scope, $http, $rootScope, PanelServices,) {
    //$scope.popup = {
    //    Type:1,
    //    Size:0,
    //    PicturePath : "/Content/images/image-file.jpg",
    //    detail : {}
    //};
    
    $scope.getPopupList = function () {
        $http({
            method: "POST",
            url: "/Admin/Popup/GetPopupList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.popup = response.data;
        });
    };

    $scope.$on('ngRepeatDetailFinished', function (ngRepeatDetailFinishedEvent) {
        $(".nav-tabs .nav-item .nav-link").removeClass("active");
        $(".tab-content .tab-pane").removeClass("show active");
        $(".nav-tabs .nav-item:first-child() .nav-link").addClass("active");
        $(".tab-content .tab-pane:first-child()").addClass("show active");
    });

    $scope.viewPopup = function () {
        $("#popupModal").modal();
    }

    $scope.savePopup = function () {
        $scope.popup.StartDate = $("#startDate").val();
        $scope.popup.EndDate = $("#endDate").val();

        $scope.newPopupDetailList = [];
        angular.forEach($rootScope.languageList, function (value, key) {
            $scope.popupDetail = {};
            $scope.popupDetail.DetailId = $('#popupTitle-' + value.Id).attr('data-detailId');
            $scope.popupDetail.Title = $('#popupTitle-' + value.Id).val();
            $scope.popupDetail.LanguageId = $('#popupTitle-' + value.Id).attr('data-langno');
            $scope.popupDetail.Description = $('#popupDescription-' + value.Id).val();
            $scope.newPopupDetailList.push($scope.popupDetail);
        });
        $scope.popup.Detail = $scope.newPopupDetailList;

        $http({
            method: "POST",
            url: "/Admin/Popup/WritePopup",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { popup: $scope.popup }
        }).then(function (response) {
            var data = new FormData();
            data.append("fileSelected", $("#imageInput")[0].files[0]);
            $.ajax({
                url: "/Admin/Popup/SavePopup",
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
            });
            $("#imageInput").val('');
        });

    }

    function ImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#fileImg').attr('src', e.target.result);
                $('#pfileImg').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        ImageChange(this);
    });

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getPopupList();
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