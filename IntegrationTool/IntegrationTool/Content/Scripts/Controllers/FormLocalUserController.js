var FormLocalUserController = function ($scope, $http, $stateParams, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.request = {};
    $scope.sendRequest = sendRequest;
    $scope.someSelected = someSelected;
    $scope.type = $stateParams.id;
    $scope.userTypes = [];
    $scope.resources = [];

    getUserTypes();
    getResources();

    if ($stateParams.id != -1) {
        loadInfo();
    }

    function loadInfo() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/getUser?id=' + $stateParams.id, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                var permissions = {};
                for (var index = 0; index < resp.Permissions.length; index++) {
                    permissions[resp.Permissions[index].Resource.ResourceId] = true;
                }
                resp.Permissions = permissions;
                resp.UserTypeId = resp.UsersType.UserTypeId +'';
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

    function getUserTypes() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/getUserTypes', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.userTypes = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function getResources() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Users/getResources', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.resources = resp;
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
            $window.scrollTo(0, 0);
            return false;
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var permissions = '';
        for (var index = 0; index < $scope.resources.length; index++) {
            if (req.Permissions[$scope.resources[index].ResourceId] == true)
                permissions += $scope.resources[index].ResourceId + '|';
        }

        permissions = permissions.replace(/\|$/, '');

        req.Permissions = permissions;

        for (var index = 0; index < $scope.userTypes.length; index++) {
            if ($scope.userTypes[index].Type == 'Local')
                req.UserTypeId = $scope.userTypes[index].UserTypeId;
        }

        var data = $.param(req);

        if ($stateParams.id == -1)
            saveUser(data, config, form);
        else
            updateUser(data, config, form);
    }

    function saveUser(data, config, form) {
        $http.put('Users/saveUser', data, config).success(function (resp) {
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

    function updateUser(data, config, form) {
        $http.post('Users/updateUser', data, config).success(function (resp) {
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

    function someSelected(object) {
        if (!object) return false;
        return Object.keys(object).some(function (key) {
            return object[key];
        });
    }
}

FormLocalUserController.$inject = ['$scope', '$http', '$stateParams', '$state', '$window'];
