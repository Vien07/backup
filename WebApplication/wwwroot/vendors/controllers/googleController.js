function handleCredentialResponse(response) {
    $.ajax({
        url: '/Customer/SignInGoogle',
        method: 'POST',
        data: { token: response.credential },
        success: function (response) {
            window.location.href = response;
        },
        error: function (err) {
        }
    })
}