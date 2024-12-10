function CustomGoBack() {
    if (window.history && window.history.pushState) {

        window.history.pushState('forward', null, '');

        $(window).on('popstate', function () {
            location.reload();
        });

    }
}