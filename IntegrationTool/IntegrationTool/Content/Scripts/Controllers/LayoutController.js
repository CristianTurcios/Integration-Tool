var LayoutController = function ($http, $scope, $state, Authentication, $cookies, $state) {
    $scope.sendRequest = sendRequest;
    $scope.permissions = [];
    $scope.name = Authentication.user.Name;
    $scope.userType = Authentication.user.UsersType.Type;

    for (index = 0; index < Authentication.user.Permissions.length; index++) {
        $scope.permissions.push(Authentication.user.Permissions[index].Resource.Name.toLowerCase());
    }

    function sendRequest() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.post('Account/logout', data, config).success(function (resp) {
            Authentication.user = null;
            $cookies.remove('user');
            $state.go('login');
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}

LayoutController.$inject = ['$http', '$scope', '$state', 'Authentication', '$cookies', '$state'];
