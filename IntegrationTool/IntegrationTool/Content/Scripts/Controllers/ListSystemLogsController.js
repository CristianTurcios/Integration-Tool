var ListSystemLogsController = function ($scope, $http, $location, $stateParams, $state, $window, $filter) {
    $scope.typeMessage = 0;
    $scope.message = "";

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

    getListSystemLogs();

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

    function getListSystemLogs() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Logs/getListSystemLogs?id=' + $stateParams.id, data, config).success(function (resp) {
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
}

ListSystemLogsController.$inject = ['$scope', '$http', '$location', '$stateParams', '$state', '$window', '$filter'];
