var ManualIntegrationController = function ($scope, $http, $location, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.manualIntegrations = [];
    $scope.editManualIntegration = editManualIntegration;
    $scope.executeIntegration = executeIntegration;
    $scope.viewIntegrationLog = viewIntegrationLog;
    $scope.viewSystemLog = viewSystemLog;

    $scope.isloading = true;

    $("#Preloader").empty();
    $("#Preloader").append("<center>" +
                        "<div id='fountainTextG'>" +
                          "<div id='fountainTextG_1' class='fountainTextG'>L</div>" +
                          "<div id='fountainTextG_2' class='fountainTextG'>a</div>" +
                          "<div id='fountainTextG_3' class='fountainTextG'>u</div>" +
                          "<div id='fountainTextG_4' class='fountainTextG'>r</div>" +
                          "<div id='fountainTextG_5' class='fountainTextG'>e</div>" +
                          "<div id='fountainTextG_6' class='fountainTextG'>a</div>" +
                          "<div id='fountainTextG_7' class='fountainTextG'>t</div>" +
                          "<div id='fountainTextG_8' class='fountainTextG'>e</div>" +
                        "</div></center>");

    getManualIntegrations();

   

    function getManualIntegrations() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/getManualIntegrations', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.manualIntegrations = resp;
                $scope.isloading = false;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function executeIntegration(id) {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/executeIntegration?id=' + id, data, config).success(function (resp) {
            $scope.message = resp.message;
            $scope.typeMessage = resp.type;
            $window.scrollTo(0, 0);
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function editManualIntegration(id) {
        $location.url('/main/integrations/configurationManual/' + id);
    }

    function viewIntegrationLog(id)
    {
        $location.url('/main/logs/listIntegrationLogs/' + id);
    }

    function viewSystemLog(id)
    {
        $location.url('/main/logs/listSystemLogs/' + id);
    }
}

ManualIntegrationController.$inject = ['$scope', '$http', '$location', '$state', '$window'];
