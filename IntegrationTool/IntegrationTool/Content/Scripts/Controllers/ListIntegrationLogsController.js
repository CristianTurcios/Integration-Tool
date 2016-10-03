var ListIntegrationLogsController = function ($scope, $http, $location, $stateParams, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listIntegrationLogs = [];
    $scope.viewDetails = viewDetails;

    getListIntegrationLogs();

    function getListIntegrationLogs() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Logs/getListIntegrationLogs?id='+$stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {              
                $scope.listIntegrationLogs = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function viewDetails(integrationId, integrationName, referenceCode) {
        $location.url('/main/logs/viewDetails/' + integrationId + '/' + integrationName + '/' + referenceCode);
    }
}

ListIntegrationLogsController.$inject = ['$scope', '$http', '$location', '$stateParams', '$state', '$window'];
