panelapp.controller("contentController", function ($scope, $http, $rootScope, PanelServices,) {
    var contentTable;
    $scope.content = {};
    $scope.content.PicturePath = "/Content/images/image-file.jpg";
    $scope.isform = false;

    function getParameterByName(name) {
        var url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)");
        results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    $scope.getContentList = function () {
        if ($scope.page == "Tab") {
            $("#subtitle").html("Tab Menü İçerikleri");
            $("#subtitle2").html("Tab Menü Listesi");
        }
        else if ($scope.page == "Blog") {
            $("#subtitle").html("Blog & Haber");
            $("#subtitle2").html("Blog & Haber Listesi");
        }
        else if ($scope.page == "About") {
            $("#subtitle").html("Hakkımızda ");
            $("#subtitle2").html("Hakkımızda İçeriği");
        }
        else {
            window.location.href("/admin");
        };
        $http({
            method: "POST",
            url: "/Admin/Content/GetContentList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { type: $scope.page }
        }).then(function (response) {
            if ($scope.page == "About") {
                $scope.newcontent = false;
                $scope.content = response.data[0];
                $scope.isform = true;
            } else {
                $scope.contentList = response.data;
            }
        });
    };

    $scope.$on('ngRepeatContentListFinished', function (ngRepeatContentListFinishedEvent) {
        contentTable = $('#content-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            //"paging": false,
            "ordering": false,
            //"searching": false,
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

    $scope.newContent = function () {
        $scope.content = {}
        $scope.newcontent = true;
        $scope.content.DisplayOrder = $scope.contentList.length + 1;
        $scope.content.PicturePath = "/Content/images/image-file.jpg";

        $scope.content.TabButtonText = "fa fa-adjust";
        $('.icon-picker').qlIconPicker({
        });

        $scope.isform = true;
    }

    $scope.updateContent = function (content) {
        $scope.newcontent = false;
        $scope.content = angular.copy(content);
        $scope.isform = true;

        $('.icon-picker').qlIconPicker({
        });
    }

    $scope.$on('ngRepeatDetailFinished', function (ngRepeatDetailFinishedEvent) {
        $(".nav-tabs .nav-item .nav-link").removeClass("active");
        $(".tab-content .tab-pane").removeClass("show active");
        $(".nav-tabs .nav-item:first-child() .nav-link").addClass("active");
        $(".tab-content .tab-pane:first-child()").addClass("show active");

        $(".summerNoteInput").summernote({
            height: 300,
            focus: false,
            lang: "tr-TR",
            disableDragAndDrop: true
        });

        if ($scope.newcontent) {
            angular.forEach($rootScope.languageList, function (value, key) {
                $('#contentDescription-' + value.Id).summernote().code(" ");
            });
        } else {
            angular.forEach($scope.content.Detail, function (value, key) {
                $('#contentDescription-' + value.Id).summernote().code(value.Description);
            });
        }
    });

    $scope.deleteContent = function (content) {
        swal({
            title: 'İçeriği Sil!',
            text: "Seçilen içeriği silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $http({
                    method: "POST",
                    url: "/Content/DeleteContent",
                    headers: { "Content-Type": "Application/json;charset=utf-8" },
                    data: { content: content }
                }).then(function (response) {
                    iziToast.show({
                        message: response.data.Message,
                        position: 'topCenter',
                        color: response.data.Color,
                        icon: response.data.Icon
                    });
                    $scope.getContentList();
                    contentTable.destroy();
                });

            }
        });
    }

    $scope.saveContent = function () {
        if ($scope.content.Id == 0 && $("#imageInput")[0].files.length == 0) {
            iziToast.error({
                type: "Error",
                message: "Resim seçiniz.",
                position: 'topCenter'
            });
        } else {
            var page = getParameterByName("page");
            $scope.newContentList = [];
            angular.forEach($rootScope.languageList, function (value, key) {
                $scope.contentDetail = {};
                $scope.contentDetail.ContentId = $scope.content.Id;
                $scope.contentDetail.DetailId = parseInt($('#contentTitle-' + value.Id).attr('data-detailId'));
                $scope.contentDetail.Title = $('#contentTitle-' + value.Id).val();
                $('#contentTitle-' + value.Id).val("");
                $scope.contentDetail.LanguageId = parseInt($('#contentTitle-' + value.Id).attr('data-langno'));
                $scope.contentDetail.Description = $('#contentDescription-' + value.Id).summernote().code();
                $('#contentDescription-' + value.Id).summernote().code(" ");
                $scope.newContentList.push($scope.contentDetail);
            });
            $scope.content.Detail = $scope.newContentList;
            $scope.content.TabButtonText = $('#icon-class').val();;
            $http({
                method: "POST",
                url: "/Admin/Content/WriteContent",
                headers: { "Content-Type": "Application/json;charset=utf-8" },
                data: { content: $scope.content, type: page }
            }).then(function (response) {
                var data = new FormData();
                data.append("fileSelected", $("#imageInput")[0].files[0]);
                $.ajax({
                    url: "/Admin/Content/SaveContent",
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
                    $scope.getContentList();
                    contentTable.destroy();
                });
                $("#imageInput").val('');
                $("#contentModal").modal("hide");
                $("#imageInput").val(null);
                $scope.content = {};
                $scope.newcontent = true;
                $scope.isform = false;
            });
        }
    }

    function contentImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#contentImg').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        contentImageChange(this);
    });

    $(document).ready(function () {
        $scope.page = getParameterByName("page");
        $rootScope.getLanguageList();
        $scope.getContentList();
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