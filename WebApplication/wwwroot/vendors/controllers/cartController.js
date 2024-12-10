const CART_HEADER = "#cart-topnav";
const CART_BODY = ".list-order";
const CART_BODY_CATE = ".list-order-cate";
const CART_TABLE = "#cart-table";
var ProductCatePid = 0;
var ProductPid = 0;
var data = [];
function getCartTopNav(langKey) {
    $.ajax({
        url: '/Cart/GetCart',
        method: 'GET',
        data: {
            'cartStr': localStorage.getItem('cart'),
            'lang': langKey,
        },
        success: function (response) {
            try {
                var data = JSON.parse(response);
                if (data.items) {
                    //        $(CART_HEADER).html('');
                    //        var html = ``;

                    //        var topNavHtml = ``;
                    //        $.ajax({
                    //            url: '/template/cart/topnav.html',
                    //            type: 'GET',
                    //            async: false,
                    //            cache: false,
                    //            success: function (res) {
                    //                topNavHtml = res;
                    //            },
                    //            error: function (e) { console.error(e) }
                    //        })

                    //        for (var item of data.items) {
                    //            var itemHtml = topNavHtml;
                    //            var htmlPrice = ``
                    //            if (item.priceDiscount == 0) {
                    //                htmlPrice = `<div class="price lh-base">
                    //    <div class="price-sale">${item.priceString}<u>đ</u></div>
                    //</div>`
                    //            } else {
                    //                htmlPrice = `<div class="price lh-base">
                    //    <div class="price-sale">${item.priceDiscountString}<u>đ</u></div>
                    //    <del class="price-origin">${item.priceString}<u>đ</u></del>
                    //</div>`
                    //            }
                    //            var model = { id: item.id, picture: item.picture, title: item.title, quantity: item.quantity, slug: item.slug, htmlPrice: htmlPrice }

                    //            for (var ele of Object.keys(model)) {
                    //                itemHtml = itemHtml.replaceAll("{{" + ele + "}}", model[ele])
                    //            }

                    //            html += itemHtml;
                    //        }

                    //        $(CART_HEADER).html(html);

                    //        $('#totalHeaderCart').text(`${data.items.length} sản phẩm`)
                    $('#numberItemInCart').text(`${data.items.length}`)
                }

            } catch (e) {
                $(CART_HEADER).html(`<div class="text-note">0 sản phẩm trong giỏ hàng</div>`);
            }
        },
        error: function (err) {
            console.log(err)
        }
    })
}
function loadCart(langKey) {
    $.ajax({
        url: '/Cart/GetCart',
        method: 'GET',
        data: {
            'cartStr': localStorage.getItem('cart'),
            'lang': langKey,
        },
        success: function (response) {
            try {
                let data = JSON.parse(response);
                if (data.items) {
                    $(CART_BODY).html('');
                    let html = ``;
                    let htmlCate = ``;
                    var bodyHtml = ``;
                    var bodyCateHtml = ``;
                    $.ajax({
                        url: '/template/cart/body.html',
                        type: 'GET',
                        async: false,
                        cache: false,
                        success: function (res) {
                            bodyHtml = res;
                        },
                        error: function (e) { console.error(e) }
                    })
                    $.ajax({
                        url: '/template/cart/bodyCate.html',
                        type: 'GET',
                        async: false,
                        cache: false,
                        success: function (res) {
                            bodyCateHtml = res;
                        },
                        error: function (e) { console.error(e) }
                    })
                    for (let item of data.items) {

                        let priceHtml = ``;

                        priceHtml += `<span class="price-sale">${item.priceDiscountString} <u>đ</u></span>
                                                  <del class="price-origin">${item.priceString} <u>đ</u></del>`;


                        var itemHtml = bodyHtml;
                        var itemCateHtml = bodyCateHtml;

                        var model = {
                            code: item.code,
                            id: item.id,
                            title: item.title,
                            quantity: item.quantity,
                            priceString: item.priceString,
                            productCateId: item.productCateId,
                            priceOriginalString: item.priceOriginalString,
                            months: item.months,
                        }

                        for (var ele of Object.keys(model)) {
                            itemHtml = itemHtml.replaceAll("{{" + ele + "}}", model[ele])
                            itemCateHtml = itemCateHtml.replaceAll("{{" + ele + "}}", model[ele])
                        }

                        html += itemHtml;
                        htmlCate += itemCateHtml;
                        ProductCatePid = item.productCateId;
                        ProductPid = item.productId;
                        GetSelectProductCate("#payment-select", lang, ProductPid);
                    }

                    $('#total').text(data.totalPriceString);
                    $(CART_BODY).html(html);
                    $(CART_BODY_CATE).html(htmlCate);
                }
            } catch (e) {
                $(CART_BODY).html('');
                $(CART_BODY_CATE).html('');
                console.log(e)
            }
        },
        error: function (err) {
            console.log(err)
        }
    })
}
function loadTableCart(langKey) {
    $.ajax({
        url: '/Cart/GetCart',
        method: 'GET',
        data: {
            'cartStr': localStorage.getItem('cart'),
            'lang': langKey,
        },
        success: function (response) {
            try {
                let data = JSON.parse(response);
                if (data.items) {
                    $(CART_BODY).html('');
                    let html = ``;

                    var bodyHtml = ``;
                    $.ajax({
                        url: '/template/cart/table.html',
                        type: 'GET',
                        async: false,
                        cache: false,
                        success: function (res) {
                            bodyHtml = res;
                        },
                        error: function (e) { console.error(e) }
                    })

                    for (let item of data.items) {
                        var itemHtml = bodyHtml;

                        var htmlPrice = ``;
                        if (item.priceDiscount != 0) {
                            htmlPrice = ` <div class="cell cell-price"><div class="price-origin">${item.priceDiscountString}<u>đ</u></div><del class="price-sale">${item.priceString}<u>đ</u></del></div>`;
                        } else {
                            htmlPrice = ` <div class="cell cell-price"><div class="price-origin">${item.priceString}<u>đ</u></div></div>`;
                        }

                        var model = {
                            id: item.id,
                            picture: item.picture,
                            code: item.code,
                            title: item.title,
                            quantity: item.quantity,
                            slug: item.slug,
                            color: item.colorTitle,
                            colorCode: item.colorCode,
                            option: item.optionTitle,
                            htmlPrice: htmlPrice
                        }

                        for (var ele of Object.keys(model)) {
                            itemHtml = itemHtml.replaceAll("{{" + ele + "}}", model[ele])
                        }

                        html += itemHtml;
                    }
                    $(CART_TABLE).html(html);
                    $('#totalItem').text(`${data.items.length}`)
                    $('#totalTableCart').text(data.totalPriceString);
                }
            } catch (e) {
                $(CART_TABLE).html('');
                console.log(e)
            }
        },
        error: function (err) {
            console.log(err)
        }
    })
}


function deleteItemInCart(id) {
    try {
        let cart = JSON.parse(localStorage.getItem('cart'))
        cart = cart.filter(x => x.id != id);
        AlertToast('Thông báo', 'Xóa sản phẩm khỏi giỏ hàng thành công!', 'success')

        localStorage.setItem('cart', JSON.stringify(cart));
        refesh();
    } catch (e) {
        AlertToast('Thông báo', 'Xóa sản phẩm khỏi giỏ hàng thất bại!', 'error')
        console.log(e)
    }
}
function addToCart(pid, colorId, optionId, quantity) {
    try {
        if (!colorId) {
            colorId = 1;
        }

        if (!optionId) {
            optionId = 1;
        }

        localStorage.removeItem('cart');

        var cart = JSON.parse(localStorage.getItem('cart'));

        if (!cart) {
            cart = [];
        }

        var newUUID = uuidv4();
        cart.push({ id: newUUID, productId: pid, quantity: quantity, colorId: colorId, optionId: optionId, productCateId: 0 });
        localStorage.setItem('cart', JSON.stringify(cart));


        window.location.href = checkoutUrl;
        //AlertToast('Thông báo', 'Thêm vào giỏ hàng thành công!', 'success')
        //getCartTopNav(lang);

    } catch (e) {
        //AlertToast('Thông báo', 'Thêm vào giỏ hàng thất bại!', 'error')
        console.error(e);
    }
}

function upQuant(id) {
    try {
        let cart = JSON.parse(localStorage.getItem('cart'))
        for (let i = 0; i < cart.length; i++) {
            if (cart[i].id == id) {
                cart[i].quantity += 1
            }
        }
        localStorage.setItem('cart', JSON.stringify(cart));
        refesh()
    } catch (e) {
        AlertToast('Thông báo', 'Thao tác thất bại!', 'error')
        console.log(e)
    }
}
function downQuant(id) {
    try {
        let cart = JSON.parse(localStorage.getItem('cart'))
        for (let i = 0; i < cart.length; i++) {
            if (cart[i].id == id && cart[i].quantity > 1) {
                cart[i].quantity -= 1
            }
        }
        localStorage.setItem('cart', JSON.stringify(cart));
        refesh()
    } catch (e) {
        AlertToast('Thông báo', 'Thao tác thất bại!', 'error')
        console.log(e)
    }
}
function changeQuant(id, value) {
    try {
        let cart = JSON.parse(localStorage.getItem('cart'))
        for (let i = 0; i < cart.length; i++) {
            if (cart[i].id == id && value >= 1) {
                cart[i].quantity = value
            }
        }
        localStorage.setItem('cart', JSON.stringify(cart));
    } catch (e) {
        AlertToast('Thông báo', 'Thao tác thất bại!', 'error')
        console.log(e)
    }
}

function refesh() {
    getCartTopNav(lang);
    loadCart(lang);
    loadTableCart(lang);
}

function getSelectProvince(divProvinceId, divDistrictId, divWardId, provinceValue, districtValue, wardValue) {
    if (!provinceValue) {
        provinceValue = 79;
    }
    if (!districtValue) {
        districtValue = 760;
    }
    if (!wardValue) {
        wardValue = 26740;
    }
    $.ajax({
        url: '/data/VietNam/province.json',
        method: 'GET',
        success: function (data) {
            let rs = [];

            Object.keys(data).forEach((i) => {
                rs.push(data[i])
            });

            rs = rs.sort(function (a, b) {
                return ('' + a.name_with_type).localeCompare(b.name_with_type);
            });

            data = {};

            for (let i in rs) {
                data[i] = rs[i]
            }


            let html = ``;
            Object.keys(data).forEach((i) => {
                html += `<option ${provinceValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
            });

            $('#' + divProvinceId).html(html);

            $('#' + divProvinceId).change((e) => {
                let code = e.target.value;
                $.ajax({
                    url: `/data/VietNam/district/${code}.json`,
                    method: 'GET',
                    success: function (data) {
                        let html = ``;
                        Object.keys(data).forEach((i) => {
                            html += `<option ${districtValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
                        });

                        $('#' + divDistrictId).html(html);

                        $('#' + divDistrictId).change((e) => {
                            let code = e.target.value;
                            $.ajax({
                                url: `/data/VietNam/ward/${code}.json`,
                                method: 'GET',
                                success: function (data) {
                                    let html = ``;
                                    Object.keys(data).forEach((i) => {
                                        html += `<option ${wardValue == data[i].code ? 'selected' : ''} value="${data[i].code}">${data[i].name_with_type}</option>`
                                    });

                                    $('#' + divWardId).html(html);
                                    $('#' + divWardId).trigger('change');
                                },
                                error: function (err) {
                                    console.log(err)
                                }
                            })

                        })

                        $('#' + divDistrictId).trigger('change');

                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            })

            $('#' + divProvinceId).trigger('change');

        },
        error: function (err) {
            console.log(err)
        }
    })
}
function validateRecaptcha() {
    if (recapchaStatus == "on") {
        var recaptcha = $("#g-recaptcha-response").val();
        if (recaptcha === "") {
            $("#parsley-id-recCatcha").removeAttr("hidden")
            return false;

        } else {
            $("#parsley-id-recCatcha").attr("hidden", "true")
            return true;
        }
    } else {
        return true;
    }
}
function saveOrder(e) {
    try {
        let cart = JSON.parse(localStorage.getItem("cart"));
        if (!cart || cart.length <= 0) {
            AlertToast('Thông báo', 'Không có sản phẩm!', 'warning')
            return;
        }
        // Check if at least one PaymentMethod radio button is checked
        if (!$("input[name='PaymentMethod']:checked").val()) {
            AlertToast('Thông báo', 'Vui lòng chọn phương thức thanh toán!', 'warning');
            return;
        }
        var $selector = $('#frmCart'),
            form = $selector.parsley();
        var validate = form.validate();
        if (validate == true) {
            if (validateRecaptcha()) {
                let formData = new FormData($selector.get(0));
                formData.append('cartStr', localStorage.getItem('cart'))
                formData.append('lang', lang)
                formData.append('VAT', $('#VAT').is(":checked"));
                $.ajax({
                    url: '/Cart/SaveOrder',
                    method: 'POST',
                    data: formData,
                    beforeSend: function () {
                        $(e).prop('disabled', true);
                        Swal({
                            title: "Checking...",
                            text: "Please wait",
                            imageUrl: "/img/configuration/Ajax-loader.gif",
                            showConfirmButton: false,
                            allowOutsideClick: false
                        });
                    },
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.isError) {
                            localStorage.setItem('cart', "[]")
                            AlertToast('Thông báo', 'Đặt hàng thành công!', 'success')
                            window.location.href = checkoutSuccessUrl + "?orderId=" + response.orderId
                        } else {
                            AlertToast('Thông báo', 'Đặt hàng thất bại!', 'error')
                        }

                        Swal.close();
                        $(e).prop('disabled', false);
                    },
                    error: function (err) {
                        console.log(err)
                        $(e).prop('disabled', false);
                        Swal.close();

                        AlertToast('Thông báo', 'Đặt hàng thất bại!', 'error')
                    }
                })
            }
        }
    } catch (e) {
        AlertToast('Thông báo', 'Đặt hàng thất bại! Hãy kiểm tra lại', 'warning')
        console.error(e)
    }
}
function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}
function CheckUser() {
    Swal.fire({ position: 'center', type: 'info', title: "Vui lòng đăng nhập để tiếp tục", showConfirmButton: false, timer: 1500 });
}
function GetSelectProductCate(divID, lang, productId) {
    $.ajax({
        url: "/b-admin/GetSelect/GetProductCateWithPrice",
        data: {
            lang: lang,
            productId: productId
        },
        method: "POST"
    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        var monthsMapping = {};
        data = [];
        for (let i = 0; i < rs.length; i++) {
            monthsMapping[rs[i].Value] = rs[i].Months;
            if (rs[i].Value === ProductCatePid) {
                html += `<option selected value="${rs[i].Value}" title="${rs[i].Name}"><div>${rs[i].Name}</div></option>`;
                data.push({
                    id: rs[i].Value,
                    text: "<div>" + rs[i].Name + "</div>",
                    html: '<div class="sub">' + rs[i].Name + '</div><div class="price">' + rs[i].PriceString + '<span class="unit">₫</span></div>',
                    title: rs[i].Name,
                });

            } else {
                html += `<option value="${rs[i].Value}" title="${rs[i].Name}"><div>${rs[i].Name}</div></option>`;
                data.push({
                    id: rs[i].Value,
                    text: "<div>" + rs[i].Name + "</div>",
                    html: '<div class="sub">' + rs[i].Name + '</div><div class="price">' + rs[i].PriceString + '<span class="unit">₫</span></div>',
                    title: rs[i].Name,
                });
            }


        }
        $(divID).html(html);
        $(divID).length && $(divID).select2({
            data: data,
            minimumResultsForSearch: -1,
            width: "auto",
            escapeMarkup: function (markup) {
                return markup;
            },
            templateResult: function (data) {
                return data.html;
            },
            templateSelection: function (data) {
                return data.html;
            },
        });
        // Store the monthsMapping in the element's data attribute for later use
        $(divID).data('monthsMapping', monthsMapping);
    });
}
function updateMonth() {
    var selectedProductCate = $('#payment-select').val();
    var monthsMapping = $('#payment-select').data('monthsMapping');
    var cart = JSON.parse(localStorage.getItem('cart'));
    if (selectedProductCate && monthsMapping) {
        if (cart) {
            for (let item of cart) {
                item.productCateId = selectedProductCate;
            }
            localStorage.setItem('cart', JSON.stringify(cart))
        }
        loadCart(lang)
    }
}
function InitSelect(divId) {
    $(divId).on('change', function () {
        updateMonth();
    });
}


