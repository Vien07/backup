$('.footer-col').each(function() {
    var col = $(this), ftt = col.find('.footer-tt');
    ftt.click(function(){
        $('.footer-col').not(col).removeClass('active');
        col.toggleClass('active');
    })
})