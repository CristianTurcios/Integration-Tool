var ListSystemLogsController = function ($scope, $http, $location, $stateParams, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listSystemLogs = [];

    getListSystemLogs();

    function getListSystemLogs() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Logs/getListSystemLogs?id=' + $stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listSystemLogs = resp;
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

ListSystemLogsController.$inject = ['$scope', '$http', '$location', '$stateParams', '$state', '$window'];
