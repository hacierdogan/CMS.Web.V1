panelapp.controller("galleryController", function ($scope, $http, $rootScope, PanelServices,) {
    var galleryTable;
    $scope.galleryList = {};
    $scope.isform = false;

    $scope.getGalleryList = function () {
        $http({
            method: "POST",
            url: "/Admin/Gallery/GetGalleryList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.galleryList = response.data;
        });
    };

    $scope.$on('ngRepeatGalleryListFinished', function (ngRepeatGalleryListFinishedEvent) {
        galleryTable = $('#gallery-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            //"paging": false,
            "ordering": false,
            //"searching": false,
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

    $scope.newGallery = function () {
        $scope.gallery = {
            Id: 0,
            Status: true,
            PicturePath: "/Content/images/image-file.jpg",
            DisplayOrder: $scope.galleryList.length + 1,
            detail: {}
        };
        $scope.isNew = true;
        $scope.isform = true;
    }

    $scope.updateGallery = function (gallery) {
        $scope.gallery = angular.copy(gallery);
        $scope.isNew = false;
        $scope.isform = true;
    }

    $scope.$on('ngRepeatDetailFinished', function (ngRepeatDetailFinishedEvent) {
        $(".nav-tabs .nav-item .nav-link").removeClass("active");
        $(".tab-content .tab-pane").removeClass("show active");
        $(".nav-tabs .nav-item:first-child() .nav-link").addClass("active");
        $(".tab-content .tab-pane:first-child()").addClass("show active");
    });

    $scope.saveGallery = function () {
        if ($scope.gallery.Id == 0 && $("#imageInput")[0].files.length == 0) {
            iziToast.error({
                type: "Error",
                message: "Resim seçiniz.",
                position: 'topCenter'
            });
        } else {
            $scope.newGalleryDetailList = [];
            angular.forEach($rootScope.languageList, function (value, key) {
                $scope.galleryDetail = {};
                $scope.galleryDetail.DetailId = $('#galleryTitle-' + value.Id).attr('data-detailId');
                $scope.galleryDetail.Title = $('#galleryTitle-' + value.Id).val();
                $scope.galleryDetail.LanguageId = $('#galleryTitle-' + value.Id).attr('data-langno');
                $scope.galleryDetail.ButtonText = $('#galleryButtonText-' + value.Id).val();
                $scope.newGalleryDetailList.push($scope.galleryDetail);
            });
            $scope.gallery.Detail = $scope.newGalleryDetailList;

            $http({
                method: "POST",
                url: "/Admin/Gallery/WriteGallery",
                headers: { "Content-Type": "Application/json;charset=utf-8" },
                data: { gallery: $scope.gallery }
            }).then(function (response) {
                var data = new FormData();
                data.append("fileSelected", $("#imageInput")[0].files[0]);
                $.ajax({
                    url: "/Admin/Gallery/SaveGallery",
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
                $scope.isform = false;
                $scope.getGalleryList();
                galleryTable.destroy();
                $scope.gallery = {
                    Id: 0,
                    Status: true,
                    PicturePath: "/Content/images/image-file.jpg",
                    DisplayOrder: $scope.galleryList.length + 1,
                    detail: {}
                };
            });
        }
    }

    $scope.deleteGallery = function (gallery) {
        swal({
            title: 'Resim Sil!',
            text: "Seçilen resmi silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'post',
                    url: '/Admin/Gallery/DeleteGallery',
                    data: { gallery: gallery },
                    success: function (data) {
                        iziToast.show({
                            message: data.Message,
                            position: 'topCenter',
                            color: data.Color,
                            icon: data.Icon
                        });
                        $scope.getGalleryList();
                        galleryTable.destroy();
                    }
                });
            }
        });
    }

    function ImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#fileImg').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        ImageChange(this);
    });

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getGalleryList();
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