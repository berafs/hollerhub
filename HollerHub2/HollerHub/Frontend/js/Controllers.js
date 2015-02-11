var app = angular.module("hollerhub", ["ngRoute"]);



app.controller("HomeController", function ($scope) {
    $scope.repo = {
        name: "Repo name"
    };
})

app.controller("RepoController", function ($scope) {
    $scope.repo = {
        name: "Repo name"
    };
})


app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.
        when("/repo", {
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