import './js/flickity-install';
import './js/header.js';
import './js/menu.js';
import './js/footer.js';
import './js/sidebar-stick.js';
import './js/rating.js';

$('.preloader').addClass('active');
$(window).load(function() {
    $('.preloader, .wrapper').addClass('already');
    var wow = new WOW({
        boxClass:     'wow',
        animateClass: 'animated',
        offset:       0,
    });
    wow.init();
});

$(".menu, .thumb-res").bind("touchstart touchend", function () { });
$('<div/>', { class: 'bg-overlay' }).appendTo('.header');

$('.bg-overlay').on('click', function() {
    $(this).fadeOut(150);
    $('html, body').removeClass('overflow-hidden');
    $('#navigate, .form-search').removeClass('active');
});
// Back to top
$('.btt').click(function(){
    $("html, body").animate({scrollTop: 0}, 0);
});
// social stick
var ss = $('#social-stick'), ssH = $('#social-stick').height();
$('.social-stick-toggle').click(function(){
    ss.slideToggle(400).toggleClass('active');
})
// line - d-width
$('.d-width').each(function () {
    var dw = $(this).attr('width');
    var dh = $(this).attr('height');
    $(this).css({
        'width': dw,
        'height': dh,
    });
});
// Form search
if ($('.toggle-form-search').length) {
    $('.toggle-form-search').click(function () {
        $('.form-search').toggleClass('active');
        $('.bg-overlay').fadeToggle(200);
    });
    $('.close-form-search').click(function () {
        $('.toggle-form-search').trigger('click');
        $('.bg-overlay').fadeOut(200);
    });
}
$('#txtkeysearch').focus(function(e){
    $('.form-search').addClass('open-sugg');
});
$(document).on("click",function(e){
    var $fm_search = $('.form-search');
    if (!$fm_search.is(e.target) && $fm_search.has(e.target).length === 0) {
      $fm_search.removeClass('open-sugg');
    }
});

// click to active victim
$('[data-clickjs]').on('click', function () {
    var this_atr = $(this).attr('data-active');
    $('#' + this_atr + '').toggleClass('active');
    $(this).toggleClass('active');
});
// content detail
function contentEllips() {
    $('.content-ellips').each(function () {
        var $this = $(this);
        var x = 500;
        $this.height() > x ? $this.addClass('textover') : $this.removeClass('textover');
    });
}
contentEllips();
//

// Form material
function inputFocusCheck(element) {
    //var $label = $(element).siblings('label');
    if ($(element).val().length > 0) {
        $(element).addClass('has-value');
    } else {
        $(element).removeClass('has-value');
    }
}
// The lines below are executed on page load
$('.material-form .form-control').each(function () {
    inputFocusCheck(this);
});
// The lines below (inside) are executed on change & keyup
$('.material-form .form-control').on('change keyup', function () {
    inputFocusCheck(this);
});
// show hide password
$('.passw-toggle-view').click(function() {
    var control = $(this).parent().find('.form-control');
    (control.attr('type') == 'text' ? control.attr('type', 'password') + $(this).removeClass('active') : control.attr('type', 'text') + $(this).addClass('active'));
})

// nice-select2 install
document.addEventListener("DOMContentLoaded", function (e) {
    // default
    var els = document.querySelectorAll(".form-select");
    els.forEach(function (select) {
        NiceSelect.bind(select);
    });
    // // seachable
    // var elss = document.querySelectorAll(".nice-select-search");
    // var options = { searchable: true };
    // elss.forEach(function (select) {
    //     NiceSelect.bind(select, options);
    // });
});

$('.add-wishlist').click(function() {
    $(this).toggleClass('active');
})
// // ul - ol - list
// $('.order-list-icon').each(function () {
//     var $data = $(this).attr('data-list-icon');
//     $('<div class="icon ' + $data + '"></div>').prependTo($(this).find('>*'));
// });
// $(window).on('load', function() {
//     $('.order-list-icon').each(function () {
//         var $di = $(this).find('.icon');
//         $(this).find('>*').css('padding-left', $di.width() + 8);
//     });
// });