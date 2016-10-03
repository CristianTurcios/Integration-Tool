var IntegrationToolApp = angular.module('IntegrationToolApp', ['ngRoute', 'ngMessages', 'ng-sweet-alert', 'ui.calendar', 'ui.bootstrap', 'ui.router', 'ngCookies']);

IntegrationToolApp.controller('MainController', MainController);

IntegrationToolApp.controller('FormActiveDirectoryController', FormActiveDirectoryController);
IntegrationToolApp.controller('FormDataBaseController', FormDataBaseController);
IntegrationToolApp.controller('FormFlatFileController', FormFlatFileController);
IntegrationToolApp.controller('FormPathReportController', FormPathReportController);
IntegrationToolApp.controller('FormQueryController', FormQueryController);
IntegrationToolApp.controller('FormServerSMTPController', FormServerSMTPController);
IntegrationToolApp.controller('FormWebServiceController', FormWebServiceController);

IntegrationToolApp.controller('ListActiveDirectoriesController', ListActiveDirectoriesController);
IntegrationToolApp.controller('ListDatabasesController', ListDatabasesController);
IntegrationToolApp.controller('ListFlatFilesController', ListFlatFilesController);
IntegrationToolApp.controller('ListPathReportsController', ListPathReportsController);
IntegrationToolApp.controller('ListQueriesController', ListQueriesController);
IntegrationToolApp.controller('ListServerSMTPController', ListServerSMTPController);
IntegrationToolApp.controller('ListWebServicesController', ListWebServicesController);
IntegrationToolApp.controller('ListKeysController', ListKeysController);

IntegrationToolApp.controller('ConfigIntegrationManualController', ConfigIntegrationManualController);
IntegrationToolApp.controller('ConfigIntegrationScheduledController', ConfigIntegrationScheduledController);
IntegrationToolApp.controller('ManualIntegrationController', ManualIntegrationController);
IntegrationToolApp.controller('SheduleIntegrationsController', SheduleIntegrationsController);

IntegrationToolApp.controller('ListIntegrationLogsController', ListIntegrationLogsController);
IntegrationToolApp.controller('LogDetailsController', LogDetailsController);
IntegrationToolApp.controller('ListSystemLogsController', ListSystemLogsController);

IntegrationToolApp.controller('LoginController', LoginController);
IntegrationToolApp.controller('LayoutController', LayoutController);

IntegrationToolApp.controller('ReportController', ReportController);

IntegrationToolApp.controller('ListUsersController', ListUsersController);
IntegrationToolApp.controller('FormLocalUserController', FormLocalUserController);
IntegrationToolApp.controller('FormADUserController', FormADUserController);
IntegrationToolApp.controller('ChangePasswordController', ChangePasswordController);

IntegrationToolApp.controller('AlertController', AlertController);
IntegrationToolApp.directive('feedback', feedBackMessagesDirective);

function runFunction($rootScope, $state, Authentication, $location) {
    $rootScope.$on('$stateChangeStart', stateChangeStart);
    $rootScope.$on('$stateChangeSuccess', stateChangeSuccess);

    function stateChangeStart(event, toState, toParams, fromState, fromParams) {
        if (toState.name == 'login' && !Authentication.user) {
            return;
        }

        if (toState.name == 'main.errors.forbidden' && Authentication.user) {
            return;
        }

        if (toState.name == 'main.errors.internalServerError' && Authentication.user) {
            return;
        }

        if (toState.name == 'main.index' && Authentication.user) {
            return;
        }

        if (toState.name == 'login' && Authentication.user) {
            event.preventDefault();
            $state.go('main.index').then(function () {
                storePreviousState(toState, toParams);
            });
            return;
        }

        if (Authentication.user) {
            for (index = 0; index < Authentication.user.Permissions.length; index++) {
                var regex = new RegExp(Authentication.user.Permissions[index].Resource.Name.toLowerCase());
                var havePermissions = toState.name.search(regex);
                if (havePermissions != -1) {
                    return;
                }
            }    
            event.preventDefault();
            $state.transitionTo('main.errors.forbidden');
        }
        else {
            event.preventDefault();
            $state.transitionTo('login').then(function () {
                storePreviousState(toState, toParams);
            });
            return;
        }
    }

    function stateChangeSuccess(event, toState, toParams, fromState, fromParams) {
        storePreviousState(fromState, fromParams);
    }

    function storePreviousState(state, params) {
        if (!state.data || !state.data.ignoreState) {
            $state.previous = {
                state: state,
                params: params,
                href: $state.href(state, params)
            };
        }
    }
}

runFunction.$inject = ['$rootScope', '$state', 'Authentication', '$location'];
IntegrationToolApp.run(runFunction);

var configFunction = function ($routeProvider, $stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/main/index');
    // Login
    $stateProvider
    .state('login', {
        url: '/account/login',
        templateUrl: '/Account/viewLogin',
        controller: 'LoginController'
    })
    // Rutas para usuarios logeados.
    .state('main', {
        abstract: true,
        url: '/main',
        templateUrl: 'Main/layout',
        controller: 'LayoutController'
    })
    //Main index
    .state('main.index', {
        url: '/index',
        templateUrl: '/Main/main',
        controller: 'MainController'
    })
    // Pestaña de configuración
    .state('main.configuration', {
        abstract: true,
        url: '/configuration',
        template: '<div ui-view></div>'
    })
    .state('main.configuration.listActiveDirectories', {
        url: '/listActiveDirectories',
        templateUrl: '/Configuration/listActiveDirectories',
        controller: 'ListActiveDirectoriesController'
    })
    .state('main.configuration.listDatabases', {
        url: '/listDatabases',
        templateUrl: '/Configuration/listDatabases',
        controller: 'ListDatabasesController'
    })
    .state('main.configuration.listFlatFiles', {
        url: '/listFlatFiles',
        templateUrl: '/Configuration/listFlatFiles',
        controller: 'ListFlatFilesController'
    })
   .state('main.configuration.listPathReports', {
        url: '/listPathReports',
        templateUrl: '/Configuration/listPathReports',
        controller: 'ListPathReportsController'
    })
    .state('main.configuration.listQueries', {
        url: '/listQueries',
        templateUrl: '/Configuration/listQueries',
        controller: 'ListQueriesController'
    })
    .state('main.configuration.listServerSMTP', {
        url: '/listServerSMTP',
        templateUrl: '/Configuration/listServerSMTP',
        controller: 'ListServerSMTPController'
    })
    .state('main.configuration.listWebServices', {
        url: '/listWebServices',
        templateUrl: '/Configuration/listWebServices',
        controller: 'ListWebServicesController'
    })
    .state('main.configuration.listKeys', {
        url: '/listKeys',
        templateUrl: '/Configuration/listKeys',
        controller: 'ListKeysController'
    })
    .state('main.configuration.formActiveDirectory', {
        url: '/formActiveDirectory/:id',
        templateUrl: '/Configuration/formActiveDirectory',
        controller: 'FormActiveDirectoryController'
    })
    .state('main.configuration.formDatabase', {
        url: '/formDatabase/:id',
        templateUrl: '/Configuration/formDatabase',
        controller: 'FormDataBaseController'
    })
    .state('main.configuration.formFlatFile', {
        url: '/formFlatFile/:id',
        templateUrl: '/Configuration/formFlatFile',
        controller: 'FormFlatFileController'
    })
    .state('main.configuration.formPathReport', {
        url: '/formPathReport/:id',
        templateUrl: '/Configuration/formPathReport',
        controller: 'FormPathReportController'
    })
    .state('main.configuration.formQuery', {
        url: '/formQuery/:id',
        templateUrl: '/Configuration/formQuery',
        controller: 'FormQueryController'
    })
    .state('main.configuration.formServerSMTP', {
        url: '/formServerSMTP/:id',
        templateUrl: '/Configuration/formServerSMTP',
        controller: 'FormServerSMTPController'
    })
    .state('main.configuration.formWebService', {
        url: '/formWebService/:id',
        templateUrl: '/Configuration/formWebService',
        controller: 'FormWebServiceController'
    })
    // Pestaña de integraciones
    .state('main.integrations', {
        abstract: true,
        url: '/integrations',
        template: '<div ui-view></div>'
    })
    .state('main.integrations.configurationManual', {
        url: '/configurationManual/:id',
        templateUrl: '/Integration/configurationManual',
        controller: 'ConfigIntegrationManualController'
    })
    .state('main.integrations.configurationScheduled', {
        url: '/configurationScheduled/:id',
        templateUrl: '/Integration/configurationScheduled',
        controller: 'ConfigIntegrationScheduledController'
    })
    .state('main.integrations.manual', {
        url: '/manual',
        templateUrl: '/Integration/manual',
        controller: 'ManualIntegrationController'
    })
    .state('main.integrations.calendar', {
        url: '/calendar',
        templateUrl: '/Integration/calendar',
        controller: 'SheduleIntegrationsController'
    })
    // Pestaña de logs
    .state('main.logs', {
        abstract: true,
        url: '/logs',
        template: '<div ui-view></div>'
    })
    .state('main.logs.listSystemLogs', {
        url: '/listSystemLogs/:id',
        templateUrl: '/Logs/listSystemLogs',
        controller: 'ListSystemLogsController'
    })
    .state('main.logs.listIntegrationLogs', {
        url: '/listIntegrationLogs/:id',
        templateUrl: '/Logs/listIntegrationLogs',
        controller: 'ListIntegrationLogsController'
    })
    .state('main.logs.viewDetails', {
        url: '/viewDetails/:integrationId/:integrationName/:referenceCode',
        templateUrl: '/Logs/viewDetails',
        controller: 'LogDetailsController'
    })
    // Rutas de errores.
    .state('main.errors', {
        abstract: true,
        url: '/errors',
        template: '<div ui-view></div>'
    })
    .state('main.errors.forbidden', {
        url: '/forbiden',
        templateUrl: '/Errors/forbidden403'
    })
    .state('main.errors.internalServerError', {
        url: '/internalServerError',
        templateUrl: '/Errors/internalServerError500'
    })
    // Pestaña de Reports
    .state('main.reports', {
        abstract: true,
        url: '/reports',
        template: '<div ui-view></div>'
    })
    .state('main.reports.generate', {
        url: '/generate',
        templateUrl: '/Report/getReport',
        controller: 'ReportController'
    })
    // Pestaña de Users
    .state('main.users', {
        abstract: true,
        url: '/users',
        template: '<div ui-view></div>'
    })
    .state('main.users.list', {
        url: '/list',
        templateUrl: '/Users/listUsers',
        controller: 'ListUsersController'
    })
    .state('main.users.formLocalUser', {
        url: '/formLocalUser/:id',
        templateUrl: '/Users/formLocalUser',
        controller: 'FormLocalUserController'
    })
    .state('main.users.formADUser', {
        url: '/formADUser/:id',
        templateUrl: '/Users/formADUser',
        controller: 'FormADUserController'
    })
    .state('main.users.changePassword', {
        url: '/changePassword',
        templateUrl: '/Users/changePassword',
        controller: 'ChangePasswordController'
    });

}
configFunction.$inject = ['$routeProvider', '$stateProvider', '$urlRouterProvider'];

IntegrationToolApp.config(configFunction);