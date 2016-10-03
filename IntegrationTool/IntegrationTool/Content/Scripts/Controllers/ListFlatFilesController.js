var ListFlatFilesController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listFlatFiles = [];
    $scope.deleteFlatFile = deleteFlatFile;
    $scope.editFlatFile = editFlatFile;


    getListFlatFiles();

    function getListFlatFiles() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListFlatFiles', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listFlatFiles = resp;
              
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function deleteFlatFile(FlatFileParametersId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.delete('Configuration/deleteFlatFile?id=' + FlatFileParametersId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getListFlatFiles();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editFlatFile(id) {
        $location.url('/main/configuration/formFlatFile/' + id);
    }
}

ListFlatFilesController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
