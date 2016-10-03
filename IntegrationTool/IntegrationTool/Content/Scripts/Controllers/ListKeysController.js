var ListKeysController = function ($scope, $http, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listKeys = [];

    getListKeys();

    function getListKeys() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListKeys', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listKeys = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}
ListKeysController.$inject = ['$scope', '$http', '$state', '$window'];
