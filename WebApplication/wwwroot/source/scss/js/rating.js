if($('.form-reply').length) {
    $('.reply-action').each(function() {
        var fm = $(this).parent().next('.form-reply');
        $(this).on('click', function() {
            $('.form-reply').not(fm).removeClass('active');
            fm.toggleClass('active');
        });
    });
    $('.review-toggle').click(function() {
        $(this).next('.reply-group').addClass('active');
        $(this).hide();
    })
}