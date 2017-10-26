$(document).ready(function () {
	$(".LayoutHeaderMenuButton").click(function () {
		$(".LayoutHeaderMenuButton").toggleClass("LayoutHeaderMenuButton_Expanded");
		$(".LayoutHeaderPulloutMenu").toggleClass("LayoutHeaderPulloutMenu_Expanded");
	});
});

var app = angular.module("SmashApp", [])

app.controller("MainController", function ($scope, $timeout) {
	$scope.PageLoading = true;
	$scope.AjaxErrorMessage = "";
	$scope.AjaxErrorMessageShow = true;
	$scope.AjaxLoadDelay = 200;
	$scope.Characters = EnumCharacters;
	$scope.RuleSets = EnumRuleSets;

	$scope.ComputeMatchMetaData = function (matches) {
		for (var i = 0; i < matches.length; i++) {
			var match = matches[i];
			var teamcount = match.Teams.length;
			var teamplayercounts = [];
			var totalplayers = 0;
			for (var j = 0; j < match.Teams.length; j++) {
				var team = match.Teams[j];
				teamplayercounts.push(team.Players.length);
				totalplayers += team.Players.length;
			}
			var teamsareeven = false;
			for (var j = 0; j < teamplayercounts.length; j++) {
				if (teamplayercounts[0] == teamplayercounts[j]) {
					teamsareeven = true;
				}
			}

			var teamtype = "";
			if (teamsareeven) {
				switch (teamplayercounts[0]) {
					case 1://One person per team
						switch (match.Teams.length) {
							case 2://Two teams total
								teamtype = "1v1";
								break;
							default:
								teamtype = "Free For All";
								break;
						}
						break;
					case 2://Two people per team
						switch (match.Teams.length) {
							case 2://Two teams total
								teamtype = "2v2";
								break;
							case 3:
								teamtype = "2v2v2";
								break;
							case 4:
								teamtype = "2v2v2v2";
								break;
							default:
								teamtype = "Custom";
								break;
						}
						break;
					case 3:
						switch (match.Teams.length) {
							case 2:
								teamtype = "3v3";
								break;
							case 3:
								teamtype = "3v3v3";
								break;
							default:
								teamtype = "Custom";
								break;
						}
						break;
					case 4:
						switch (match.Teams.length) {
							case 2:
								teamtype = "4v4";
								break;
							default:
								teamtype = "Custom";
								break;
						}
						break;
					default:
						teamtype = "Custom";
						break;
				}
			} else {
				teamtype = "Custom";
			}

			matches[i].TeamType = teamtype;
		}
		return matches;
	}
});
	