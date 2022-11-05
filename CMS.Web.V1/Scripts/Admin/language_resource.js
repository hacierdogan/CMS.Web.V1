panelapp.controller("resourceController", function ($scope, $http, $rootScope, PanelServices,) {
    var resourceTable;
    $scope.selectedLanguage = "tr";
    $scope.isResourceAvailable = false;

    $scope.getResourceList = function () {
        $http({
            method: "POST",
            url: "/Admin/Language/GetLanguageResourceList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.resourceList = response.data;
        });
    };

    $scope.$on('ngRepeatLanguageResourceListFinished', function (ngRepeatLanguageResourceListFinishedEvent) {
        resourceTable = $('#resource-table').DataTable({
            pageLength: 25,
            lengthMenu: [[25, 50, 100, -1], [25, 50, 100, 'Tümü']],
            order: [[0, 'asc']],
            "paging": true,
            "ordering": false,
          
            "columnDefs": [
                { "targets": 2, "orderable": false, "className": "text-center" },
                { "targets": 3, "orderable": false, "className": "text-center" }
            ],
            "language": { "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Turkish.json" },
            buttons: [
                {
                    text: 'Reload',
                    action: function (e, dt, node, config) {
                        dt.ajax.reload();
                    }
                }
            ]
        });
    });

    $scope.addResource = function () {
        $('#ResourceKey').prop('disabled', false);
        $scope.newResource = true;
        $scope.ResourceKey = "";
        $scope.resourceValueList = {};
        $rootScope.languageList = {}
        $rootScope.getLanguageList();
        $('#resourceModal').modal();
    }

    $scope.saveResourceList = function () {
        $scope.newResourceList = [];
        angular.forEach($rootScope.languageList, function (value, key) {
            $scope.resourceItem = {};
            $scope.resourceItem.ResourceId = $('#lang-id-' + value.LangCode).val();
            $scope.resourceItem.ResourceKey = $scope.ResourceKey;
            $scope.resourceItem.ResourceValue = $('#lang-val-' + value.LangCode).val();
            $scope.resourceItem.LangCode = value.LangCode;
            $scope.resourceItem.LangId = $('#lang-val-' + value.LangCode).attr('data-langno');
            $scope.newResourceList.push($scope.resourceItem);
        });
        $http({
            method: "POST",
            url: "/Admin/Language/SaveResource",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { resourceList: $scope.newResourceList }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $("#resourceModal").modal("hide");
            $scope.getResourceList();
            resourceTable.destroy();
        });
    }

    $scope.isResourceControl = function () {
        if ($scope.ResourceKey.match(/[^a-zA-Z.]/g)) {
            $scope.ResourceKey = $scope.ResourceKey.replace(/[^a-zA-Z.]/g, '');
        }
        $scope.isResourceAvailable = false;
        $scope.dbResource = $scope.resourceList.filter(function (item) {
            if (item.ResourceKey.toUpperCase() == $scope.ResourceKey.toUpperCase()) {
                $scope.isResourceAvailable = true;
            }
        });
    };

    $scope.updateResource = function (id, key, val) {
        $scope.ResourceKey = key;
        $scope.getResourceValues(key);
        $('#ResourceKey').attr("disabled", "disabled");
        $scope.newResource = false;
        $('#resourceModal').modal();
    }

    $scope.getResourceValues = function (rkey) {
        $http({
            method: "POST",
            url: "/Admin/Language/GetResourceValues",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { resourceKey: rkey }
        }).then(function (response) {
            $scope.resourceValueList = response.data;
            resourceTable.destroy();
        });
    };

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getResourceList();
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