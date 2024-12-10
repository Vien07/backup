// Header stick
$('<div class="header-anchor">&nbsp;</div>').insertBefore(".header");
$('<div class="navigate-anchor d-none d-md-block">&nbsp;</div>').insertBefore("#navigate");
function HeaderStick() {
    var anchorHeader    = $(".header-anchor");
    var header          = $('.header');
    var anchorNavigate  = $('.navigate-anchor');
    if ($(window).scrollTop() > anchorHeader.offset().top) {
        header.addClass('stick');
        anchorHeader.height(header.innerHeight());
    } else {
        header.removeClass('stick');
        anchorHeader.height('');
    }
    // Navigate
    if ($('#navigate').length && $(window).scrollTop() > anchorNavigate.offset().top - 6) {
        $('#navigate').addClass('stick');
        anchorNavigate.height($('#navigate').innerHeight());
    } else {
        $('#navigate').removeClass('stick');
        anchorNavigate.height('');
    }
}
$(window).on("load scroll", function() {
    if($('.header').length) {
        HeaderStick();
    }
});