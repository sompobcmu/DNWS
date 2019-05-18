'use strict';

angular.module('followingList', ['ngRoute'])
  .component('followingList', {
    templateUrl: 'following/following.html',
    controller: ['$http', '$rootScope', function TweetListController($http, $rootScope) {
      var self = this;

      const requestOptions = {
          headers: { 'X-session': $rootScope.x_session }
      };

      $http.get('http://localhost:8080/twitterapi/following/', requestOptions).then(function (response) {
        self.followings = response.data;
		});
		//create action_add for use in html
		self.sendFollow = function sendFollow(followingname) {
			$http.post('http://localhost:8080/twitterapi/following/', "followingname=" + encodeURIComponent(followingname), requestOptions);
		}

		//create action_delete for use in html
		self.sendUnFollow = function sendUnFollow(followingname) {
			$http.defaults.headers.delete = { 'X-session': $rootScope.x_session };
			$http.delete('http://localhost:8080/twitterapi/following/?' + "followingname=" + encodeURIComponent(followingname));
		}
    }]
});