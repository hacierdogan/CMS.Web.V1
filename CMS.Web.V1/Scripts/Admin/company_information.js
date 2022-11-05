panelapp.controller("companyInformationController", function ($scope, $http, $rootScope, PanelServices,) {

    $scope.getCompanyInformation = function () {
        $http({
            method: "POST",
            url: "/Admin/CompanyInformation/GetCompany",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            $scope.company = response.data;
            $('#map').attr('src', response.data.Location);
        });
    };

    $scope.saveCompanyInformation = function () {
        $scope.company.LogoBasePath = $('#baseLogo').val();
        $http({
            method: "POST",
            url: "/Admin/CompanyInformation/WriteCompany",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { company: $scope.company}
        }).then(function (response) {
            var data = new FormData();
            data.append("fileSelected", $("#imageInput")[0].files[0]);
            $.ajax({
                url: "/Admin/CompanyInformation/SaveCompany",
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
                $scope.getCompanyInformation();
                $("#imageInput").val('');
            });
        });
    };

    $scope.showPdf = function () {
        $http({
            method: "POST",
            url: "/Admin/CompanyInformation/SavePdf",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: {}
        }).then(function (response) {
            //$scope.frameUrl = response.data;
            //window.open($scope.frameUrl, '_blank');
            $('#pdfbox').attr('src', response.data);
            $('#mPdfShow').appendTo("body").modal('show');
        });
    }

    function FileImageChange(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#fileImg').attr('src', e.target.result);
                $('#baseLogo').val(e.currentTarget.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }

    $("#imageInput").change(function () {
        FileImageChange(this);
    });

    $(document).ready(function () {
        $rootScope.getLanguageList();
        $scope.getCompanyInformation();
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