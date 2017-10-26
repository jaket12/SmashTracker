app.controller("HomeController", function ($scope, $http, $timeout) {
	$scope.Matches = [];
	$scope.BeltMatch = null;

	$scope.GetLatestMatches = function () {
		$http({
			method: 'GET',
			url: '/api/SmashApi/GetMatches?Top=500&OrderByDesc=Id'
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.Matches = response.data;
			$scope.Matches = $scope.$parent.ComputeMatchMetaData($scope.Matches);
			$timeout(function () { $scope.$parent.PageLoading = false; console.log("matches", $scope.Matches); }, $scope.$parent.AjaxLoadDelay);
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
			$timeout(function () { $scope.$parent.PageLoading = false; }, $scope.$parent.AjaxLoadDelay);
		});
	}

	$scope.GetLatestBeltMatchStats = function () {
		$http({
			method: 'GET',
			url: '/api/SmashApi/GetLatestBeltMatchStats'
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.BeltMatch = response.data;
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
		});
	}
	
	$scope.PostTest = function () {
		$http({
			method: 'POST',
			url: '/api/SmashApi/PostTest',
			data: "{Name: 'Rich', Id: 1}",
			headers: { 'Content-Type': 'application/json' }
		}).then(function successCallback(response) {
			console.log("Got POST result", response);
		}, function errorCallback(response) {
			console.log("Got POST error", response);
		});
	}

	$scope.GetLatestMatches();
	$scope.GetLatestBeltMatchStats();
	$scope.PostTest();
});