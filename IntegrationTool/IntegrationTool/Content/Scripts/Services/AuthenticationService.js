angular.module('IntegrationToolApp').factory('Authentication', Authentication);

Authentication.$inject = ['$cookies'];

function Authentication($cookies) {
    var auth = {
        user: $cookies.getObject('user')
    };

    return auth;
}
