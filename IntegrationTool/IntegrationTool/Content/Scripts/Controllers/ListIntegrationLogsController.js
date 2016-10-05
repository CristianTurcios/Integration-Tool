var ListIntegrationLogsController = function ($scope, $http, $location, $stateParams, $state, $window, $filter) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.viewDetails = viewDetails;

    $scope.manageLogs = manageLogs;
    $scope.nextGroupOfPages = nextGroupOfPages;
    $scope.prevGroupOfPages = prevGroupOfPages;
    $scope.methodSearch = methodSearch;

    $scope.search = '';
    $scope.numberOfPages = 0;
    $scope.groupOfPages = 0;
    $scope.page = 0;
    $scope.logs = [];
    $scope.searchLogs = [];
    $scope.showLogs = [];

    getListIntegrationLogs();

    function manageLogs(page) {
        $scope.page = page;
        $scope.searchLogs = $filter('filter')($scope.logs, $scope.search);
        $scope.numberOfPages = Math.floor($scope.searchLogs.length / 5) + 1;
        if ($scope.page + 1 > $scope.numberOfPages)
            $scope.page = 0;
        $scope.showLogs = $scope.searchLogs.slice(($scope.groupOfPages * 5 * 5) + ($scope.page * 5), ($scope.groupOfPages * 5 * 5) + ($scope.page * 5) + 5);
    }

    function nextGroupOfPages() {
        $scope.groupOfPages = $scope.groupOfPages + 1;
        manageLogs(0);
    }

    function prevGroupOfPages() {
        $scope.groupOfPages = $scope.groupOfPages - 1;
        manageLogs(0);
    }

    function methodSearch(search) {
        $scope.search = search;
        manageLogs($scope.page);
    }

    function getListIntegrationLogs() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Logs/getListIntegrationLogs?id='+$stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {              
                $scope.logs = resp;
                manageLogs(0);
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

ListIntegrationLogsController.$inject = ['$scope', '$http', '$location', '$stateParams', '$state', '$window', '$filter'];
