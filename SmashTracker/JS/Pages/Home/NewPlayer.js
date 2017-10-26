app.controller("NewPlayerController", function ($scope, $http, $timeout) {
	$scope.DefaultNewPlayer = {
		Name: "",
		PlayerTag: ""
	};
	$scope.NewPlayer = angular.copy($scope.DefaultNewPlayer);
	$scope.PlayerToEdit = null;
	$scope.EditPlayerOriginalData = null;
	$scope.SavingWaitShow = false;
	$scope.SendingMatchWait = false;
	$scope.ShowSaveSuccess = false;
	$scope.ValidationMessages = [];

	$scope.CurrentPlayers = [];

	$scope.SaveNewPlayer = function () {

		$scope.ValidationMessages = [];

		//Validation
		if ($scope.NewPlayer.Name == null || $scope.NewPlayer.Name.length < 1) {
			$scope.ValidationMessages.push("Player needs a name");
		} else {
			if ($scope.NewPlayer.Name.length > 20) {
				$scope.ValidationMessages.push("Name is too long: max of 20 characters");
			}
			//Check for special characters
		}
		if ($scope.NewPlayer.PlayerTag == null || $scope.NewPlayer.PlayerTag.length < 1) {
			$scope.ValidationMessages.push("Player needs a tag");
		} else {
			if ($scope.NewPlayer.PlayerTag.length > 10) {
				$scope.ValidationMessages.push("Tag is too long: max of 10 characters");
			}
			//check for special characters
		}

		if ($scope.ValidationMessages.length == 0) {

			$scope.SavingWaitShow = true;
			$scope.SendingMatchWait = true;
			$scope.$parent.AjaxErrorMessage = null;
			$scope.ValidationMessages.length = 0;

			$http({
				method: 'POST',
				url: '/api/SmashApi/SaveNewPlayer',
				data: $scope.NewPlayer,
				headers: { 'Content-Type': 'application/json' }
			}).then(function successCallback(response) {
				console.log("Got result", response);
				$scope.CurrentPlayers.push(response.data);
				$scope.NewPlayer = angular.copy($scope.DefaultNewPlayer);
				$scope.SendingMatchWait = false;
				$scope.ShowSaveSuccess = true;
				$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, 5000);
			}, function errorCallback(response) {
				console.log("Got error", response);
				$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
				$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, $scope.$parent.AjaxLoadDelay);
			});
		}
	}

	$scope.GetCurrentPlayers = function () {

		$scope.$parent.AjaxErrorMessage = null;
		$scope.ValidationMessages.length = 0;

		$http({
			method: 'GET',
			url: '/api/SmashApi/GetAllPlayers'
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.CurrentPlayers = response.data;
			$timeout(function () { $scope.$parent.PageLoading = false; }, $scope.$parent.AjaxLoadDelay);
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
			$timeout(function () { $scope.$parent.PageLoading = false; }, $scope.$parent.AjaxLoadDelay);
		});
	}

	$scope.GetCurrentPlayers();
	
	$scope.EnablePlayerEdit = function (index) {
		$scope.PlayerToEdit = index;
		$scope.EditPlayerOriginalData = angular.copy($scope.CurrentPlayers[index]);
	}

	$scope.UndoPlayerEdit = function () {
		$scope.CurrentPlayers[$scope.PlayerToEdit].Name = $scope.EditPlayerOriginalData.Name;
		$scope.CurrentPlayers[$scope.PlayerToEdit].PlayerTag = $scope.EditPlayerOriginalData.PlayerTag;
		$scope.PlayerToEdit = null;
		$scope.EditPlayerOriginalData = null;
	}

	$scope.SavePlayerEdit = function () {

		if ($scope.CurrentPlayers[$scope.PlayerToEdit].Name == null || $scope.CurrentPlayers[$scope.PlayerToEdit].length < 1) {
			$scope.ValidationMessages.push("Player needs a name");
		} else {
			if ($scope.CurrentPlayers[$scope.PlayerToEdit].Name.length > 20) {
				$scope.ValidationMessages.push("Name is too long: max of 20 characters");
			}
			//Check for special characters
		}

		if ($scope.CurrentPlayers[$scope.PlayerToEdit].PlayerTag == null || $scope.CurrentPlayers[$scope.PlayerToEdit].PlayerTag.length < 1) {
			$scope.ValidationMessages.push("Player needs a tag");
		} else {
			if ($scope.CurrentPlayers[$scope.PlayerToEdit].PlayerTag.length > 10) {
				$scope.ValidationMessages.push("Tag is too long: max of 10 characters");
			}
			//check for special characters
		}

		$scope.$parent.AjaxErrorMessage = null;
		$scope.ValidationMessages.length = 0;

		$http({
			method: 'POST',
			url: '/api/SmashApi/EditPlayer',
			data: $scope.CurrentPlayers[$scope.PlayerToEdit],
			headers: { 'Content-Type': 'application/json' }
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.PlayerToEdit = null;
			$scope.EditPlayerOriginalData = null;
			$scope.SendingMatchWait = false;
			$scope.ShowSaveSuccess = true;
			$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, 5000);
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.UndoPlayerEdit();
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
			$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, $scope.$parent.AjaxLoadDelay);
		});
	}
});