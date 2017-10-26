app.controller("NewMatchController", function ($scope, $http, $timeout) {
	$scope.DefaultMatch = {
		Asterisk: false,
		RuleSet: "3",
		TeamType: "",
		Teams: [
			{
				KillCount: null,
				Placement: 1,
				Players: []
			}
		]
	};
	$scope.Match = angular.copy($scope.DefaultMatch);
	$scope.Players = [];

	$scope.ChangePlacementShow = false;
	$scope.TeamToChangePlacement = 0;

	$scope.ChangePlayerShow = false;
	$scope.PlayerTeamToChangePlayer = 0;
	$scope.PlayerToChangePlayer = 0;

	$scope.SavingWaitShow = false;
	$scope.SendingMatchWait = false;
	$scope.ShowSaveSuccess = false;

	$scope.ValidationMessages = [];

	$scope.$watch('Match', function (newVal, oldVal) {
		console.log('changed', newVal);
	}, true);

	$scope.GetAllPlayers = function () {
		$http({
			method: 'GET',
			url: '/api/SmashApi/GetAllPlayers'
		}).then(function successCallback(response) {
			console.log("Got result", response);
			$scope.Players = response.data;
			$timeout(function () { $scope.$parent.PageLoading = false; console.log("players", $scope.Players); }, $scope.$parent.AjaxLoadDelay);
		}, function errorCallback(response) {
			console.log("Got error", response);
			$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
			$timeout(function () { $scope.$parent.PageLoading = false; }, $scope.$parent.AjaxLoadDelay);
		});
	}

	$scope.SelectTeamPlacement = function (teamindex) {
		$scope.TeamToChangePlacement = teamindex;
		$scope.ChangePlacementShow = true;
		console.log("ChangeTeamPlacement");
	}
	$scope.ChangeTeamPlacement = function (teamindex, place) {
		console.log("changeing team for", teamindex, place);
		$scope.ChangePlacementShow = false;
		$scope.Match.Teams[teamindex].Placement = place;
	}

	$scope.SelectPlayerAndCharacter = function (teamindex, playerindex) {
		console.log("SHowingf options for change player", teamindex, playerindex);
		$scope.PlayerTeamToChangePlayer = teamindex;
		$scope.PlayerToChangePlayer = playerindex;
		$scope.ChangePlayerShow = true;
	}

	$scope.ChangePlayerCharacter = function (teamindex, playerindex, characterindex) {
		console.log("changing player character", teamindex, playerindex, characterindex);
		$scope.Match.Teams[teamindex].Players[playerindex].Character = characterindex;
	}

	$scope.AddNewPlayerAndCharacter = function (teamindex) {
		console.log("adding new player", teamindex);
		$scope.Match.Teams[teamindex].Players.push({
			Character: 0,
			PlayerId: 0
		});
		$scope.SelectPlayerAndCharacter(teamindex, $scope.Match.Teams[teamindex].Players.length - 1);
	}

	$scope.AddNewTeam = function () {
		console.log("adding new team");
		$scope.Match.Teams.push({
			KillCount: null,
			Placement: $scope.Match.Teams.length + 1,
			Players: []
		});
	}

	$scope.DeleteNewPlayer = function (teamindex, playerindex) {
		$scope.Match.Teams[teamindex].Players.splice(playerindex, 1);
	}

	$scope.DeleteNewTeam = function (teamindex) {
		$scope.Match.Teams.splice(teamindex, 1);
	}

	$scope.SaveNewMatch = function () {
		console.log("saving the match");
		scroll(0, 0);//Scroll top

		$scope.ValidationMessages = [];
		var singlewinningteamfound = false;

		if ($scope.Match.RuleSet < 0 || $scope.Match.RuleSet > $scope.$parent.RuleSets.length - 1) {
			$scope.ValidationMessages.push("Invalid rule set");
		}
		if ($scope.Match.Teams.length < 2) {
			$scope.ValidationMessages.push("Need at least 2 teams");
		}
		if ($scope.Match.Teams.length > 8) {
			$scope.ValidationMessages.push("Can only have 8 team max");
		}

		//Validate
		for (var i = 0; i < $scope.Match.Teams.length; i++) {

			if ($scope.Match.Teams[i].Players.length < 1) {
				$scope.ValidationMessages.push("Teams need at least 1 player")
			}
			if ($scope.Match.Teams[i].Players.length > 7) {
				$scope.ValidationMessages.push("Teams cannot exceed 7 players");
			}

			for (var j = 0; j < $scope.Match.Teams[i].Players.length; j++) {
				if ($scope.Match.Teams[i].Players[j].Character < 1) {
					$scope.ValidationMessages.push("Players must have a character selected");
				}
				if ($scope.Match.Teams[i].Players[j].Character > $scope.$parent.Characters.length) {
					$scope.ValidationMessages.push("Invalid character selected");
				}
				if ($scope.Match.Teams[i].Players[j].Player == null || $scope.Match.Teams[i].Players[j].Player.Id < 1) {
					$scope.ValidationMessages.push("Invalid player name");
				}
			}

			if ($scope.Match.Teams[i].Placement == 1) {
				if (singlewinningteamfound) {
					$scope.ValidationMessages.push("Only one 1st place allowed");
				} else {
					singlewinningteamfound = true;
				}
			}

			if ($scope.Match.Teams[i].Placement < 1 || $scope.Match.Teams[i].Placement > 8) {
				$scope.ValidationMessages.push("Invalid placement for team");
			}
		}

		if (!singlewinningteamfound) {
			$scope.ValidationMessages.push("One team must be in 1st place");
		}

		if ($scope.ValidationMessages.length == 0) {

			$scope.MatchToSend = angular.copy($scope.Match);
			for (var i = 0; i < $scope.MatchToSend.Teams.length; i++) {
				for (var j = 0; j < $scope.MatchToSend.Teams[i].Players.length; j++) {
					$scope.MatchToSend.Teams[i].Players[j].PlayerId = $scope.MatchToSend.Teams[i].Players[j].Player.Id;
					$scope.MatchToSend.Teams[i].Players[j].Player = null;
				}
			}

			$scope.SavingWaitShow = true;
			$scope.SendingMatchWait = true;

			$http({
				method: 'POST',
				url: '/api/SmashApi/SaveNewMatch',
				data: $scope.MatchToSend,
				headers: { 'Content-Type': 'application/json' }
			}).then(function successCallback(response) {
				console.log("Got result", response);
				$scope.SendingMatchWait = false;
				$scope.ShowSaveSuccess = true;
				$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, 5000);
				//window.location = "/";
			}, function errorCallback(response) {
				console.log("Got error", response);
				$scope.$parent.AjaxErrorMessage = response.data.Message + " ~ " + response.data.MessageDetail;
				$timeout(function () { $scope.ShowSaveSuccess = false; $scope.SavingWaitShow = false; }, $scope.$parent.AjaxLoadDelay);
			});
		} else {
			console.log("Validation failed for save", $scope.ValidationMessages);
		}
	}

	$scope.ResetMatch = function () {
		$scope.Match = angular.copy($scope.DefaultMatch);
		$scope.ChangePlacementShow = false;
		$scope.TeamToChangePlacement = 0;
		$scope.ChangePlayerShow = false;
		$scope.PlayerTeamToChangePlayer = 0;
		$scope.PlayerToChangePlayer = 0;
	}

	console.log("characters", $scope.$parent.Characters);
	$scope.GetAllPlayers();
});