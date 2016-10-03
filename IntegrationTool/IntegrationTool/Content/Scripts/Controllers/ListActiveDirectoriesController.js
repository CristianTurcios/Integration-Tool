var ListActiveDirectoriesController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listActiveDirectories = [];
    $scope.deleteActiveDirectory = deleteActiveDirectory;
    $scope.editActiveDirectory = editActiveDirectory;

    getListActiveDirectories();

    function getListActiveDirectories() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getlistActiveDirectories', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listActiveDirectories = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function deleteActiveDirectory(ActiveDirectoryId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.delete('Configuration/deleteActiveDirectory?id=' + ActiveDirectoryId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getListActiveDirectories();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editActiveDirectory(id) {
        $location.url('/main/configuration/formActiveDirectory/' + id);
    }
}
ListActiveDirectoriesController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
