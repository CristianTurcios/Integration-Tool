var ListUsersController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listUsers = [];
    $scope.disableUser = disableUser;
    $scope.enableUser = enableUser;
    $scope.editUser = editUser;

    getUsers();

    function getUsers() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/getUsers', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listUsers = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function disableUser(UserId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/disableUser?id=' + UserId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getUsers();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function enableUser(UserId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/enableUser?id=' + UserId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getUsers();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editUser(id, usersType) {
        if(usersType == 'Local')
            $location.url('/main/users/formLocalUser/' + id);
        else
            $location.url('/main/users/formADUser/' + id);
    }
}
ListUsersController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
