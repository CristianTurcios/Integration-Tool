var AlertController = function ($scope, $http) {
    $scope.sweet = {};
    $scope.sweet.option = {
        title: "Are you sure?",
        text: "Press confirm to continue.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#0e5555",
        confirmButtonText: "Confirm",
        cancelButtonText: "Cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    }
}

AlertController.$inject = ['$scope', '$http'];