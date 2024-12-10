window.fbAsyncInit = function () {
    FB.init({
        appId: facebookAppId,
        cookie: true,
        xfbml: true,
        version: 'v12.0'
    });
    FB.AppEvents.logPageView();
};

function fb_login() {
    FB.login(function (response) {
        if (response.authResponse) {
            var id_token = response.authResponse.accessToken;
            var id_user = response.authResponse.userID;
            if (response.status === 'connected') {
                $.ajax({
                    url: '/Customer/SignInFB',
                    method: 'POST',
                    data: { token: id_token },
                    success: function (response) {
                        FB.logout();
                        window.location.href = response;
                    },
                    error: function (err) {
                        FB.logout();
                    }
                })
            }
        }
        else {

        }
    }, { scope: 'email,public_profile' });
}
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
