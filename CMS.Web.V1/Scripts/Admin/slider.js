panelapp.controller("sliderController", function ($scope, $http, $rootScope, PanelServices,) {
    var sliderTable;
    $scope.slider = {};

    $scope.getSliderList = function () {
        $http({
            method: "POST",
            url: "/Admin/Slider/GetSliderList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.sliderList = response.data;
        });
    };

    $scope.$on('ngRepeatSliderListFinished', function (ngRepeatSliderListFinishedEvent) {
        sliderTable = $('#slider-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            //"paging": false,
            "ordering": false,
            //"searching": false,
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

    $scope.$on('ngRepeatDetailFinished', function (ngRepeatDetailFinishedEvent) {
        $(".nav-tabs .nav-item .nav-link").removeClass("active");
        $(".tab-content .tab-pane").removeClass("show active");
        $(".nav-tabs .nav-item:first-child() .nav-link").addClass("active");
        $(".tab-content .tab-pane:first-child()").addClass("show active");
    });

    $scope.clearForm = function () {
        angular.forEach($rootScope.languageList, function (value) {
            $('#sliderTitle-' + value.Id).val('');
            $('#sliderSubTitle-' + value.Id).val('');
            $('#sliderDescription-' + value.Id).val('');
        });
    }

    $scope.newSlider = function () {
        $scope.newslider = true;
        $scope.slider.Id = 0;
        $scope.slider.Name = "";
        $scope.slider.PicturePath = "/Content/images/image-file.jpg";
        var date = new Date();
        $scope.slider.StartDateStr = date.toLocaleDateString();
        $scope.slider.EndDateStr = date.toLocaleDateString();
        $scope.slider.Status = true;
        $scope.slider.DisplayOrder = $scope.sliderList.length+1;
        $("#sliderModal").modal();
        $scope.clearForm();
    }

    $scope.updateSlider = function (slider) {
        $scope.clearForm();
        $scope.newslider = false;
        $scope.slider = angular.copy(slider);
        $("#sliderModal").modal();
    }

    $scope.saveSlider = function () {
        if ($scope.slider.Name == "") {
            iziToast.error({
                type: "Error",
                message: "Tanım bilgisi giriniz.",
                position: 'topCenter'
            });
        }
        else if ($scope.slider.Id == 0 && $("#imageInput")[0].files.length == 0) {
            iziToast.error({
                type: "Error",
                message: "Resim seçiniz.",
                position: 'topCenter'
            });
        }
        else {
            $scope.slider.StartDate = $("#startDate").val();
            $scope.slider.EndDate = $("#endDate").val();

            $scope.newSliderDetailList = [];
            angular.forEach($rootScope.languageList, function (value, key) {
                $scope.sliderDetail = {};
                $scope.sliderDetail.SliderId = $scope.slider.Id;
                $scope.sliderDetail.Title = $('#sliderTitle-' + value.Id).val();
                $scope.sliderDetail.LanguageId = $('#sliderTitle-' + value.Id).attr('data-langno');
                $scope.sliderDetail.SubTitle = $('#sliderSubTitle-' + value.Id).val();
                $scope.sliderDetail.Description = $('#sliderDescription-' + value.Id).val();
                $scope.newSliderDetailList.push($scope.sliderDetail);
            });
            $scope.slider.Detail = $scope.newSliderDetailList;

            $http({
                method: "POST",
                url: "/Admin/Slider/WriteSlider",
                headers: { "Content-Type": "Application/json;charset=utf-8" },
                data: { slider: $scope.slider }
            }).then(function (response) {
                var data = new FormData();
                data.append("fileSelected", $("#imageInput")[0].files[0]);
                $.ajax({
                    url: "/Admin/Slider/SaveSlider",
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
                    $scope.getSliderList();
                    sliderTable.destroy();
                });
                $("#imageInput").val('');
                $scope.slider = {};
                $scope.slider.Detail = {}
                $scope.newslider = true;
                $scope.clearForm();
                $("#sliderModal").modal("hide");
            });
        }
    }

    $scope.deleteSlider = function (slider) {
        swal({
            title: 'Slider Sil!',
            text: "Seçilen slider'ı silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $http({
                    method: "POST",
                    url: "/Slider/DeleteSlider",
                    headers: { "Content-Type": "Application/json;charset=utf-8" },
                    data: { slider: slider }
                }).then(function (response) {
                    iziToast.show({
                        message: response.data.Message,
                        position: 'topCenter',
                        color: response.data.Color,
                        icon: response.data.Icon
                    });
                    $scope.getSliderList();
                    sliderTable.destroy();
                });
            }
        });
    }

    function sliderImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#sliderImg').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        sliderImageChange(this);
    });

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getSliderList();
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