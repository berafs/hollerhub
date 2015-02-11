var app = angular.module("hollerhub", ["ngRoute"]);



app.controller("HomeController", function ($scope) {
    $scope.repo = {
        name: "Repo name"
    };
});

app.controller("RepoController", ["$scope", "$routeParams", function ($scope, $routeParams) {
    $scope.repo = {
        name: $routeParams["repoId"]
    };
}]);


app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.
        when("/repo/:repoId", {
            "templateUrl": "Repo.html",
            "controller": "RepoController"
        }).
        when("/home", {
            "templateUrl": "Home.html",
            "controller": "HomeController"
        }).
        otherwise({
            "redirectTo": "/home"
        });
}]);