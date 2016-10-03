var LogDetailsController = function ($scope, $http, $stateParams, $state, $window) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.details = {};
    $scope.integrationName = $stateParams.integrationName;
    $scope.error = false;
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

    getDetails();

    function getDetails() {
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Logs/getDetails?integrationId=' + $stateParams.integrationId + '&referenceCode=' + $stateParams.referenceCode, data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                $scope.details = resp;
            } else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
                $scope.error = true;
            }
            $scope.isloading = false;
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }
}
LogDetailsController.$inject = ['$scope', '$http', '$stateParams', '$state', '$window'];
