app.controller("BeltMatchController", function ($scope, $http, $timeout) {
	$scope.BeltMatch = null;
	
	$scope.GetLatestBeltMatchStats = function () {
		$http({
			method: 'GET',
			url: '/api/SmashApi/GetLatestBeltMatchStats'
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.BeltMatch = response.data;
			$timeout(function () { $scope.$parent.PageLoading = false; console.log("matches", $scope.BeltMatch); }, $scope.$parent.AjaxLoadDelay);
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
			$timeout(function () { $scope.$parent.PageLoading = false; }, $scope.$parent.AjaxLoadDelay);
		});
	}

	$scope.GetLatestBeltMatchStats();
});