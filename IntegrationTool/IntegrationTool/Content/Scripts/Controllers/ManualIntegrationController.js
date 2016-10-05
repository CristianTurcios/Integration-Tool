var ManualIntegrationController = function ($scope, $http, $location, $state, $window, $filter) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.editManualIntegration = editManualIntegration;
    $scope.executeIntegration = executeIntegration;
    $scope.viewIntegrationLog = viewIntegrationLog;
    $scope.viewSystemLog = viewSystemLog;

    $scope.manageIntegrations = manageIntegrations;
    $scope.nextGroupOfPages = nextGroupOfPages;
    $scope.prevGroupOfPages = prevGroupOfPages;
    $scope.methodSearch = methodSearch;

    $scope.search = '';
    $scope.numberOfPages = 0;
    $scope.groupOfPages = 0;
    $scope.page = 0;
    $scope.manualIntegrations = [];
    $scope.searchIntegrations = [];
    $scope.showIntegrations = [];

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

    function manageIntegrations(page) {
        $scope.page = page;
        $scope.searchIntegrations = $filter('filter')($scope.manualIntegrations, $scope.search);
        $scope.numberOfPages = Math.floor($scope.searchIntegrations.length / 5) + 1;
        if ($scope.page + 1 > $scope.numberOfPages)
            $scope.page = 0;
        $scope.showIntegrations = $scope.searchIntegrations.slice(($scope.groupOfPages * 5 * 5) + ($scope.page * 5), ($scope.groupOfPages * 5 * 5) + ($scope.page * 5) + 5);
    }

    function nextGroupOfPages() {
        $scope.groupOfPages = $scope.groupOfPages + 1;
        manageIntegrations(0);
    }

    function prevGroupOfPages() {
        $scope.groupOfPages = $scope.groupOfPages - 1;
        manageIntegrations(0);
    }

    function methodSearch(search) {
        $scope.search = search;
        manageIntegrations($scope.page);
    }

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
                manageIntegrations(0);
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

ManualIntegrationController.$inject = ['$scope', '$http', '$location', '$state', '$window', '$filter'];
