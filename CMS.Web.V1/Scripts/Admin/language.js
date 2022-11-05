panelapp.controller("languageController", function ($scope, $http, $rootScope, PanelServices,) {
    var languageTable;
    $scope.language = {};

    $scope.$on('ngRepeatLanguageListFinished', function (ngRepeatLanguageListFinishedEvent) {
        languageTable = $('#language-table').DataTable({
            order: [[0, 'asc']],
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            "paging": false,
            "ordering": false,
            "searching": false,
            "columnDefs": [
                { "targets": 2, "orderable": false, "className": "text-center" },
                { "targets": 3, "orderable": false, "className": "text-center" },
                { "targets": 4, "orderable": false, "className": "text-center" }
            ],
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" }
        });
    });

    $scope.newLanguage = function () {
        $scope.popupTitle = "Dil Ekle";
        $scope.language = {};
        $("#languageModal").modal();
    }

    $scope.updateLanguage = function (lang) {
        $scope.popupTitle = "Dil Güncelle";
        $scope.language = angular.copy(lang);
        $("#languageModal").modal();
    }

    $scope.saveLanguage = function (language) {
        $http({
            method: "POST",
            url: "/Admin/Language/SaveLanguage",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { lang: language }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $("#languageModal").modal("hide");
            $rootScope.getLanguageList();
            languageTable.destroy();
            $scope.language = {};
        });
    }

    $scope.deleteLanguage = function (language) {
        swal({
            title: language.LangName+' dili Silinecek!',
            text: "Dili ve kaynakları silmek istediğinize emin misiniz?",
            type: 'warning',
            buttons: ["İptal", "Sil"],
            closeOnConfirm: false
        }).then(function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: 'post',
                    url: '/Admin/Language/DeleteLanguage',
                    data: { lang: language },
                    success: function (data) {
                        iziToast.show({
                            message: data.Message,
                            position: 'topCenter',
                            color: data.Color,
                            icon: data.Icon
                        });
                        $rootScope.getLanguageList();
                        languageTable.destroy();
                    }
                });
            }
        });
    }
 
    $(document).ready(function () {
        $rootScope.getLanguageList();
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