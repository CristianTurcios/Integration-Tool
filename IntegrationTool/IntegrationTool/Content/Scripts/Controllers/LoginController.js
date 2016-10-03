var LoginController = function ($scope, $http, $state, Authentication, $cookies, $state) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.sendRequest = sendRequest;
    var authentication = Authentication;

    function sendRequest(req, form) {
        if (!form.$valid) {
            $scope.message = "Error: Invalid form, please try again.";
            $scope.typeMessage = "danger";
            return false;
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param(req);

        $http.post('Account/login', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                var expireDate = new Date();
                expireDate.setMinutes(expireDate.getMinutes() + 30);
                $cookies.putObject('user', resp, { 'expires': expireDate });
                Authentication.user = resp;
                $state.go('main.index');
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $scope.request.Password = '';
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}

LoginController.$inject = ['$scope', '$http', '$state', 'Authentication', '$cookies', '$state'];
