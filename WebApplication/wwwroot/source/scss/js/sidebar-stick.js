// Stick sidebar
$('[data-wrapper]').each(function() {
    var t           = $(this);
    var sidebar     = $(this).find('[data-sidebar]');
    var mainbody    = $(this).find('[data-mainbody]');
    var screen      = 768;
    var topspace    = 70; // top: 65px // custom

    $('<div class="anchor-top"></div>').prependTo(sidebar);
    $('<div class="anchor-bottom"></div>').appendTo(t);

    var datastick   = $(this).find('[data-stick]');
    var anchortop   = $(this).find('.anchor-top');
    var anchorbot   = $(this).find('.anchor-bottom');
    
    // stick srcoll
    function rollstick(vl) {
        // stick
        if ($(window).scrollTop() > anchortop.offset().top - topspace) {
            datastick.addClass('is-stick');
            ($(window).width() >= screen) ? anchortop.height(datastick.outerHeight()) : anchortop.height('');
        } else {
            datastick.removeClass('is-stick');
            anchortop.height('');
        }
        // stuck
        var anchorwin   = $(window).scrollTop() + datastick.outerHeight(true);
        if (anchorwin + topspace > anchorbot.offset().top) {
            datastick.addClass('is-stuck');
        } else {
            datastick.removeClass('is-stuck');
        }
    }
    // set width div stick
    function setwidth(w) {
        if(w.is('[data-width-fill]')) {
            ($(window).width() >= screen) ? datastick.width(anchortop.width()) : datastick.width('');
        }
    }
    // set height sidebar
    function setheight() {
        var height1 = $('[data-stick]');
        var height2 = $('[data-mainbody]');
        var max = Math.max(height1.height(), height2.height());
        ($(window).width() >= screen) ? sidebar.height(max) : sidebar.height('');
    }
    // initiallize
    $(window).on("load resize", function(){
        rollstick(t);
        setwidth(datastick);
        setheight();
    }).trigger("resize");

    $(window).on("scroll", function(){
        rollstick(t);
        setheight();
    });
});
// End

// News detail - social-stick
if($('.social-stick').length) {
    // $('<div class="s-anchor-t"></div>').prependTo('.content-master');
    $('<div class="s-anchor-b" style="position:absolute;bottom:0;"></div>').appendTo('.content-master');
    var socialstick = $('.social-stick');
    var space       = $('.header').height();
    var screen      = 768;
    function sticksc() {
        var ypos    = $('.social-stick').parent().offset().top - 70;
        var winpos  = $(window).scrollTop();
        // stick
        (winpos > ypos) ? socialstick.addClass('is-stick') : socialstick.removeClass('is-stick');
        // stuck
        var va      = $(window).scrollTop() + socialstick.height();
        var posb    = $('.s-anchor-b').offset().top;
        (posb < va) ? socialstick.addClass('is-stuck') : socialstick.removeClass('is-stuck');
    }
    $(window).on('scroll', sticksc).trigger('scroll');
}