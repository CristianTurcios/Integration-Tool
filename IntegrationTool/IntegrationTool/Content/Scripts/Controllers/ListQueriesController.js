var ListQueriesController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listQueries = [];
    $scope.deleteQuery = deleteQuery;
    $scope.editQuery = editQuery;

    getlistQueries();

    function getlistQueries() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getlistQueries', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listQueries = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function deleteQuery(QueryId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.delete('Configuration/deleteQuery?id=' + QueryId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getlistQueries();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editQuery(id) {
        $location.url('/main/configuration/formQuery/' + id);
    }
}
ListQueriesController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
