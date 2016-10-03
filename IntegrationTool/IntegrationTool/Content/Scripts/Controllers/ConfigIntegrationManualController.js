var ConfigIntegrationManualController = function ($scope, $http, $stateParams, $state, Authentication, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.integrationsType = [];
    $scope.queries = [];
    $scope.webServices = [];
    $scope.operationsWebServices = [];
    $scope.databases = [];
    $scope.flatFiles = [];
    $scope.params = [];
    $scope.query;
    $scope.database;
    $scope.sendRequest = sendRequest;
    $scope.getQueries = getQueries;
    $scope.getQuery = getQuery;
    $scope.getDatabase = getDatabase;

    getIntegrationsType();
    getWebServices();
    getOperationsWebServices();
    getDatabases();
    getFlatFiles();

    if ($stateParams.id != -1) {
        getIntegration($stateParams.id);
    }

    function getIntegration(id) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/getIntegration?id=' + id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                integration(resp);
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function integration(resp) {
        resp.IntegrationTypeId = resp.IntegrationTypeId + '';
        resp.WebServiceId = resp.WebServiceId + '';
        resp.OperationWebServiceId = resp.OperationWebServiceId + '';
        resp.DatabaseParametersId = resp.DatabaseParametersId + '';
        resp.FlatFileParameterId = resp.FlatFileParameterId + '';
        resp.QueryId = resp.QueryId + '';

        getDatabase(resp.DatabaseParametersId);
        getQueries(resp.IntegrationTypeId);
        var Interval = setInterval(function () {
            if ($scope.queries.length != 0) {
                clearInterval(Interval);
                getQuery(resp.QueryId);
            }
        }, 50);

        $scope.request = resp;

        for (index = 0; index < resp.QueryParameters.length; index++) {
            var param = resp.QueryParameters[index].Value.replace(/\'/g, '');
            param = param.replace(/,/g, '|');
            $scope.request[resp.QueryParameters[index].Name] = param;
        }
    }

    function getDatabase(id) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getDataBase?id=' + id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.database = resp;
            } else {
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
            $window.scrollTo(0,0);
            return false;
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        req.UserId = Authentication.user.UserId;

        var data = $.param(req);

        $http.put('Integration/saveIntegrationManual', data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            if ($stateParams.id == -1) {
                $scope.request = {};
                $scope.queries = [];
                $scope.params = [];
                $scope.query = null;
            }
            form.$setPristine();
            form.$setUntouched();
            $window.scrollTo(0, 0);
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getIntegrationsType() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getIntegrationsType', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.integrationsType = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getQueries(integrationTypeId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/getQueriesByIntegrationType?id=' + integrationTypeId, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.queries = resp;
                $scope.query = null;
                $scope.params = [];
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getWebServices() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListWebServices', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.webServices = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getOperationsWebServices() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/getOperationsWebServices', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.operationsWebServices = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getQuery(queryId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getQuery?id=' + queryId, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.query = resp;
                getParams();
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getParams() {
        params = $scope.query.Query1.match(/\[\[.*\]\]/g);
        requestParams = [];
        for (index = 0; index < params.length; index++) {
            property = 'param' + index;
            requestParams[index] = { nameParam: params[index], valueParam: '' }
        }

        $scope.params = requestParams;
    }

    function getDatabases() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListDatabases', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.databases = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getFlatFiles() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListFlatFiles', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.flatFiles = resp;
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

ConfigIntegrationManualController.$inject = ['$scope', '$http', '$stateParams', '$state', 'Authentication', '$window'];
