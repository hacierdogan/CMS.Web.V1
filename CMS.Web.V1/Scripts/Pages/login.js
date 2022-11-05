siteapp.controller("logincontroller", function ($scope, $http, $rootScope, CMSServices) {
   
    $scope.resetPassword = function () {
        swal({
            text: 'Tanımlı e-posta adresinizi girin.',
            content: "input",
            button: {
                text: "Şifre Yenileme Bağlantısı Gönder",
                closeModal: false,
            },
        })
            .then(value => {
                if (!value) {
                    return swal("Mail adresinizi girin.", { buttons: [,] });
                }
                else {
                    $http({
                        method: "POST",
                        url: "/Login/ResetPassword",
                        headers: { "Content-Type": "Application/json;charset=utf-8" },
                        data: { email: value }
                    }).then(function (response) {
                        swal("", response.data.Message, response.data.Statu, {buttons: [,"Tamam"]});
                    });
                }
            });
    }

    $(document).ready(function () {
        $rootScope.getLanguageList(); //Language List
        CMSServices.getLanguageCount(); //Language Count
        $scope.getSelectedLanguage();
    });
})


