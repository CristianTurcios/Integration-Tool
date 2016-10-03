var FormActiveDirectoryController = function ($scope, $http, $stateParams, $state) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.sendRequest = sendRequest;
    $scope.type = $stateParams.id;

    loadInfo();

    function loadInfo() {
        if ($stateParams.id == -1) {
            return;
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getActiveDirectory?id=' + $stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.request = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function sendRequest(req, form){
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

        if ($stateParams.id == -1)
            put(data, config, form);
        else
            post(data, config, form);
    }

    function put(data, config, form) {
        $http.put('Configuration/saveActiveDirectory', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $scope.request = {};
            form.$setPristine();
            form.$setUntouched();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function post(data, config, form) {
        $http.post('Configuration/updateActiveDirectory', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $scope.request = {};
            loadInfo();
            form.$setPristine();
            form.$setUntouched();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}
FormActiveDirectoryController.$inject = ['$scope', '$http', '$stateParams', '$state'];
