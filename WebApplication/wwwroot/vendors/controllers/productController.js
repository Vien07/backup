(function () {
    var url = new URL(window.location.href);
    var param = url.searchParams.get("sortby");
    param ? $(`#cbosort option[value=${param}]`).attr('selected', 'selected') : $(`#cbosort option[value=""]`).attr('selected', 'selected');
})();
$('#cbosort').change(function () {
    var url = new URL(window.location.href);
    url.searchParams.set('sortby', this.value);
    window.location.href = url;
});
function changePrice(optionId, productId) {
    $.ajax({
        url: '/Product/ChangePrice',
        method: 'GET',
        data: {
            optionId: optionId,
            productId: productId,
        },
        success: function (response) {
            try {
                var data = JSON.parse(response);
                var html = ``;

                if (data.Price == 0) {
                    html = `<div class="price-origin"><a href="/lien-he.html" >Liên hệ</a></div>`
                }
                else if (data.PriceDiscount == 0) {
                    html = `<div class="price-origin">${data.PriceString}<u>đ </u></div>`
                }
                else {
                    html = `<div class="price-origin">${data.PriceDiscountString}<u>đ </u></div><del class="price-sale">${data.PriceString}<u>đ </u></del>`
                }

                $('#divPrice').html(html)
            }
            catch (e) {
                console.error(e)
            }

        },
        error: function (err) {
            console.log(err)
        }
    })
}
$('#down').click(function () {
    var quantity = parseInt($('#quantity').val()) - 1;
    if (quantity <= 0) {
        quantity = 1;
    }
    $('#quantity').val(quantity);
})
$('#up').click(function () {
    var quantity = parseInt($('#quantity').val()) + 1;
    $('#quantity').val(quantity);
})
$('.color-item').click(function () {
    $('.color-item').each(function (i, ele) {
        $(ele).removeClass('active');
    })

    $(this).addClass('active')
})

function comment(customerId, productId, message, parentId) {
    $.ajax({
        url: '/Product/InsertComment',
        method: 'POST',
        data: {
            customerId: customerId,
            productId: productId,
            message: message,
            parentId: parentId,
        },
        success: function (response) {
            if (response) {
                AlertToast('Thông báo', 'Cảm ơn bạn. Bình luận đang chờ duyệt!', 'success')
            } else {
                AlertToast('Thông báo', 'Có lỗi xảy ra!', 'error')
            }
        },
        error: function (err) {
            AlertToast('Thông báo', 'Có lỗi xảy ra!', 'error')
            console.log(err)
        }
    })
}