var ListPathReportsController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listPathReports = [];
    $scope.deletePathReport = deletePathReport;
    $scope.editPathReport = editPathReport;


    getListPathReports();

    function getListPathReports() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Configuration/getListPathReports', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.listPathReports = resp;            
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function deletePathReport(PathReportId) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.delete('Configuration/deletePathReport?id=' + PathReportId, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
            getListPathReports();
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editPathReport(id) {
        $location.url('/main/configuration/formPathReport/' + id);
    }
}

ListPathReportsController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
