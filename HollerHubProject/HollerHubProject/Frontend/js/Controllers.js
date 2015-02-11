var app = angular.module("hollerhub", ["ngRoute"]);



app.controller("HomeController", function ($scope) {
    $scope.repo = {
        name: "Repo name"
    };

});

app.controller("RepoController", ["$scope", "$routeParams", "$http", function ($scope, $routeParams, $http) {

    $scope.repo = {
        stars: []
    };
    var owner = $routeParams["owner"];
    var repoName = $routeParams["repoName"];

    $http.get("/repo/" + owner + "/" + repoName).success(function (repoData) {
        console.log(repoData);
        $scope.repo.name = repoData.Name;

        var rating = 5;

        var updateStars = function(rating) {
            var stars = [];
            for (var i = 0; i < 10; i++) {
                stars[i] = {};
                var currentStar = i + 1;
                stars[i].lit = currentStar <= rating;
            }
            $scope.repo.stars = stars;
        }

        updateStars(5);

        $scope.changeRating = function (index) {
            updateStars(index);
        };

        $http.get("/api/reviews/" + repoData.Id).success(function (reviewsData) {
            console.log(reviewsData);
            var sum = reviewsData.reduce(function (previousValue, aReview) {
                return previousValue + aReview.ratingStars;
            }, 0.0);
            var average = sum / reviewsData.length;
            $scope.repo.communityRating = average.toFixed(1);
            console.log(average);
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