var FormDataBaseController = function ($scope, $http, $stateParams, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.regexIP = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    $scope.regexPort = /^([0-9]*)$/;
    $scope.sendRequest = sendRequest;
    $scope.dataBaseEngines = [];
    $scope.type = $stateParams.id;

    getDataBaseEngines();

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

        $http.get('Configuration/getDataBase?id=' + $stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                resp.EngineId =  resp.EngineId + ''
                console.log(resp);
                $scope.request = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getDataBaseEngines() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getDataBaseEngines', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.dataBaseEngines = resp;
            }else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function sendRequest(req, form) {
        if (!form.$valid) {
            $scope.message = "Error: Invalid form, please try again.";
            $scope.typeMessage = "danger";
            $window.scrollTo(0, 0);
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
        $http.put('Configuration/saveDataBase', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $scope.request = {};
            form.$setPristine();
            form.$setUntouched();
            $window.scrollTo(0, 0);
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function post(data, config, form) {
        $http.post('Configuration/updateDataBase', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $scope.request = {};
            loadInfo();
            form.$setPristine();
            form.$setUntouched();
            $window.scrollTo(0, 0);
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}

FormDataBaseController.$inject = ['$scope', '$http', '$stateParams', '$state', '$window'];
