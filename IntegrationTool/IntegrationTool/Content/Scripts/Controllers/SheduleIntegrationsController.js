var SheduleIntegrationsController = function ($scope, $http, $location, $window, $state) {
    $scope.typeMessage = 0;
    $scope.message = "";
    $scope.listIntegrationShedule = [];
    $scope.integrationId = '';

    getIntegrationsShedule();

    function getIntegrationsShedule()
    {

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var data = $.param({});

        $http.get('Integration/getSheduleIntegrations', data, config).success(function (resp) {
            if (resp.type !== 'danger') {
                buildCalendar(resp);
            }
            else {
                $scope.message = resp.message;
                $scope.typeMessage = resp.type;
                $window.scrollTo(0, 0);
            }
        }).error(function (resp) {
            $state.transitionTo('main.errors.internalServerError');
        });
    }

    function buildCalendar(resp)
    {
        var colors = ['#f56954', '#f39c12', '#0073b7', '#00c0ef', '#00a65a', '#3c8dbc'];
        var count = 0;

        for (index = 0; index < resp.length; index++)
        {
            if (count == colors.length)
                count = 0;

            var dateDatabase = resp[index].Calendars[0].NextExecutionDate;
            var fecha = dateDatabase.split("T");
            var fechaSplit = fecha[0].split("-");
            var horaSplit = fecha[1].split(":");

            $scope.listIntegrationShedule[index] =
            {
                id: resp[index].IntegrationId,
                title: resp[index].Status.Name + '; ' + resp[index].IntegrationName,
                start: new Date(fechaSplit[0], fechaSplit[1] - 1, fechaSplit[2], horaSplit[0], horaSplit[1], horaSplit[2]),
                url: '/#/main/integrations/configurationScheduled/' + resp[index].IntegrationId,
                backgroundColor: colors[count],
                borderColor: colors[count]
            }
            count++;
        }
        loadCallendar($scope.listIntegrationShedule);
    }

    function loadCallendar(integrationShedule) {
        /* initialize the external events
         -----------------------------------------------------------------*/
        function ini_events(ele) {
        }

        /* initialize the calendar
         -----------------------------------------------------------------*/
        $('#calendar').fullCalendar(
        {
            header:
            {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listDay'
            },
            buttonText:
            {
                today: 'today',
                month: 'month',
                week: 'week',
                day: 'day',
                list: 'list'
            },
            //Random default events
            events: integrationShedule,
            eventClick: function (event) {
                if (event.url) {
                    $window(event.url);
                }
            }
        });
    }
}
SheduleIntegrationsController.$inject = ['$scope', '$http', '$location', '$window', '$state'];
