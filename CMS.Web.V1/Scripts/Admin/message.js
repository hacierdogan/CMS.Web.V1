panelapp.controller("messageController", function ($scope, $http, $rootScope, PanelServices,) {

    $scope.messageList = {};
    $scope.messageDetail = {};
    $scope.chkAll = false;

    $scope.getMessageList = function (type) {
        if (type == 1) {
            $scope.messageListType = 1;
            $scope.messageTypeTitle = "Gelen Kutusu";
        } else if (type == 2) {
            $scope.messageListType = 2;
            $scope.messageTypeTitle = "Arşiv";
        } else {
            $scope.messageListType = 3;
            $scope.messageTypeTitle = "Silinen";
        }
        $http({
            method: "POST",
            url: "/Admin/Message/GetMessageList",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { type: type }
        }).then(function (response) {
            $scope.chkAll = false;
            $scope.messageList = response.data;
            $rootScope.getMessaceCount();
        });
    };

    $scope.updateMessageType = function (type) {
        $http({
            method: "POST",
            url: "/Admin/Message/UpdateMessageType",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { messageList: $scope.messageList.filter(w => w.Selected), type: type }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getMessageList($scope.messageListType);
        });
    };

    $scope.updateMessageRead = function () {
        $http({
            method: "POST",
            url: "/Admin/Message/UpdateMessageRead",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { messageList: $scope.messageList.filter(w => w.Selected) }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getMessageList($scope.messageListType);
        });
    };

    $scope.showMessageDetail = function (row) {
        $http({
            method: "POST",
            url: "/Admin/Message/GetMessageDetail",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { id: row.Id }
        }).then(function (response) {
            $scope.messageDetail = response.data;
            $("#messageModal").modal();
            $scope.getMessageList($scope.messageListType);
        });
    };

    $scope.deleteMessage = function () {
        $http({
            method: "POST",
            url: "/Admin/Message/DeleteMessage",
            headers: { "Content-Type": "Application/json;charset=utf-8" },
            data: { messageList: $scope.messageList.filter(w => w.Selected) }
        }).then(function (response) {
            iziToast.show({
                message: response.data.Message,
                position: 'topCenter',
                color: response.data.Color,
                icon: response.data.Icon
            });
            $scope.getMessageList($scope.messageListType);
        });
    };

    $scope.checkAllMessageList = function (checked) {
        $scope.messageList.forEach(function (item) {
            item.Selected = !checked
        });
        //angular.forEach($scope.messageList, function (value) {
        //    value.Selected = checked;
        //});
    };

    $(document).ready(function () {
        $scope.getMessageList(1);
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