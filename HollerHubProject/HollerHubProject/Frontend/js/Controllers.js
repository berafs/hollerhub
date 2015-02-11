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
        $scope.repo.imageUrl = repoData.AvatarUrl;
        $scope.repo.description = repoData.Description;
        $scope.repo.userReview = {

        };
        
        var updateStars = function (hasRating, rating) {
            if (!hasRating) {
                rating = 0;
            }

            var stars = [];
            for (var i = 0; i < 10; i++) {
                stars[i] = {};
                var currentStar = i + 1;
                stars[i].lit = currentStar <= rating;
            }

            $scope.repo.userReview.stars = stars;
            $scope.repo.userReview.starRating = rating;
            $scope.repo.userReview.hasUserRating = hasRating
        }

        $http.get("/api/reviews/benrafshoon/" + repoData.Id).success(function (reviewData) {
            console.log(reviewData);
            if (reviewData.length == 0) {
                updateStars(false, 0);
            } else {
                updateStars(true, reviewData[0].ratingStars);
                $scope.repo.userReview.id = reviewData[0].id;
                $scope.repo.userReview.title = reviewData[0].title;
                $scope.repo.userReview.text = reviewData[0].text;
                console.log(reviewData[0].id);
            }
            
        });

        $http.get("/api/metarating/" + $scope.repo.name).success(function(metaratingData) {
            $scope.repo.metarating = metaratingData.toFixed(1);
        });


        $scope.changeRating = function (newRating) {
            $scope.repo.userReview.starRating = newRating;

            $scope.saveUserReview();
        };

        $scope.saveUserReview = function () {
            var requestObj = {};

            requestObj.data = {
                reviewerAlias: "benrafshoon",
                ratingStars: $scope.repo.userReview.starRating,
                repoId: repoData.Id,
                id: $scope.repo.userReview.id,
                title: $scope.repo.userReview.title,
                text: $scope.repo.userReview.text
            };

            if ($scope.repo.userReview.id) {
                requestObj.method = "PUT",
                requestObj.url = "/api/reviews/" + $scope.repo.userReview.id
            } else {
                requestObj.method = "POST",
                requestObj.url = "/api/reviews"
            }

            $http(requestObj).success(function (data) {
                updateStars(true, $scope.repo.userReview.starRating);
            });
            
        }

        $scope.repo.contributers = repoData.Contributors.map(function (aContributer) {
            return {
                username: aContributer.Login,
                imageUrl: aContributer.AvatarUrl,
                githubProfileUrl: aContributer.ProfileUrl
            };
        });

        $http.get("/api/reviews/" + repoData.Id).success(function (reviewsData) {
            console.log(reviewsData);
            var sum = reviewsData.reduce(function (previousValue, aReview) {
                return previousValue + aReview.ratingStars;
            }, 0.0);
            var average = sum / reviewsData.length;
            $scope.repo.communityRating = average.toFixed(1);

            $scope.repo.reviews = reviewsData.map(function (aReview) {
                return aReview;
            });

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