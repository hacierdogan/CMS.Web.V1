panelapp.controller("excelLanguageController", function ($scope, $http, $rootScope, PanelServices,) {

    $scope.selectedLanguage = "tr";
   
    $scope.dowlandFile = function () {
        $http({
            method: "POST",
            url: "/Admin/Language/GetResourceListByLangCode",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.resourceList = response.data;
            var newData = [];
            $scope.resourceList.forEach(function (item) {
                newData.push({
                    "ANAHTAR": item.ResourceKey,
                    "TANIM": item.ResourceValue,
                    "YENI_DIL_DEGERI": ''
                });
            });
            var ws = XLSX.utils.json_to_sheet(newData, {
                header: [
                    "ANAHTAR",
                    "TANIM",
                    "YENI_DIL_DEGERI"
                ]
            });
            var wscols = [{ wch: 50 }, { wch: 50 }, { wch: 100 }];
            ws['!cols'] = wscols;
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Dil_ceviri_listesi");

            XLSX.utils.shee;
            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });
            saveAs(new Blob([fireGenerateBlobData(wbout)], { type: "application/octet-stream" }), "Dil_ceviri_listesi.xlsx");
        });
    };

    function fireGenerateBlobData(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i !== s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    $scope.uploadResourceExcelFile = function () {
        $scope.selectedLanguageId = $rootScope.languageList.filter(function (item) {
            if (item.LangCode == $scope.selectedLanguage) {
                return item.Id;
            }
        });

        var fileUpload = document.getElementById('excel-input');
        var files = fileUpload.files;
        if (files.length == 0) {
            iziToast.error({
                message: "Yüklenecek dosya bulunamadı.",
                position: 'topCenter',
            });
            return;
        }
        var f = files[0];
        {
            var reader = new FileReader();
            var name = f.name;
            reader.onload = function (e) {
                var data = e.target.result;
                var workbook = XLSX.read(data, { type: 'binary' });
                var jsonData;
                workbook.SheetNames.forEach(function (sheetName) {
                    jsonData = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName], { raw: true });
                });
                jsonData.forEach(function (e) {
                    e.LangId = $scope.selectedLanguageId[0].Id;
                    e.LangCode = $scope.selectedLanguage;
                    e.ResourceKey = e.ANAHTAR;
                    e.ResourceValue = e.YENI_DIL_DEGERI;
                });
                $http({
                    method: "POST",
                    url: "/Admin/Language/UploadResourceList",
                    headers: { "Content-Type": "Application/json;charset=utf-8" },
                    data: { uploadList: jsonData }
                }).then(function successCallback(response) {
                    $scope.uploadlist = response.data;
                });
            }
        }
        reader.readAsBinaryString(f);
    }

    $scope.saveResourceExcel = function () {
        $http({
            method: "POST",
            url: "/Admin/Language/SaveExcelResource",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { resourceList: $scope.uploadlist.SuccessList }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.uploadlist = {};
        });
    };

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