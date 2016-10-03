var ChangePasswordController = function ($scope, $http, $stateParams, $state, Authentication) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.sendRequest = sendRequest;
    $scope.name = Authentication.user.Name;

    function sendRequest(req, form) {
        if (!form.$valid) {
            $scope.message = "Error: Invalid form, please try again.";
            $scope.typeMessage = "danger";
            return false;
        }
        if (req.NewPassword != req.RepeatNewPassword) {
            $scope.message = "Error: New password don´t match.";
            $scope.typeMessage = "danger";
            return false;
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        req.UserId = Authentication.user.UserId;

        var data = $.param(req);

        $http.post('Users/updatePassword', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $scope.request = {};
            form.$setPristine();
            form.$setUntouched();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}

ChangePasswordController.$inject = ['$scope', '$http', '$stateParams', '$state', 'Authentication'];
