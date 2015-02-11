var app = angular.module("hollerhub", ["ngRoute"]);



app.controller("HomeController", function ($scope) {
    $scope.repo = {
        name: "Repo name"
    };
});

app.controller("RepoController", ["$scope", "$routeParams", "$http", function ($scope, $routeParams, $http) {

    $scope.repo = {
       
    };
    var owner = $routeParams["owner"];
    var repoName = $routeParams["repoName"];

    $http.get("/repo/" + owner + "/" + repoName).success(function (repoData) {
        console.log(repoData);
        $scope.repo.name = repoData.Name;

        $http.get("/api/reviews/" + repoData.Id).success(function (reviewsData) {
            console.log(reviewsData);
        });
    });

}]);


app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.
        when("/repo/:owner/:repoName", {
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