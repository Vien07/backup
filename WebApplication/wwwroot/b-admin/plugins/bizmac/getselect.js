function GetSelectGroupUser(divID, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/GroupUser",
        method: "POST"

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            html = `<option selected value="">—Chose Categories</option>`;
        } else {
            html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value === parseInt(selected)) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }

        }
        $(divID).html(html);
        $(divID).niceSelect();
    });
}
function GetSelectLocal(divID, selected) {
    $.getJSON('/b-admin/vendors/bizmac-customjs/local.json', function (data) {
        var rs = data;
        var html = "";
        if (selected === "") {
            html = `<option selected value="">-Chọn địa điểm</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (rs[i].name === selected) {
                html += `<option selected value="${rs[i].name}">${rs[i].name}</option>`;
            }
            else {
                html += `<option value="${rs[i].name}">${rs[i].name}</option>`;
            }

        }
        $(divID).html(html);
        $(divID).select2();
    });
}

function GetSelectPageMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/Page",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {

            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;


        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' '],
            dropdownParent: $('#inputModal .modal-content')
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectPage(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/Page",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            html = `<option selected value="">—Chose Categories</option>`;
        } else {
            html = `<option  value="">—Chose Categories</option>`;

        }
        if (selected !== null && selected !== "") {
            var kt = 0;
            for (let j = 0; j < selected.length; j++) {
                for (let i = 0; i < rs.length; i++) {
                    if (rs[i].Value === selected[j]) {
                        html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
                        break;
                    }
                }

            }
            for (let i = 0; i < rs.length; i++) {
                for (let j = 0; j < selected.length; j++) {
                    if (rs[i].Value === selected[j]) {
                        kt = 1;
                        break;
                    }
                }
                if (kt === 0) {
                    html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

                }
                kt = 0;
            }
        } else {
            for (let i = 0; i < rs.length; i++) {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }

        }
        $(divID).html(html);
        //$(divID).niceSelect();
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).on("select2:select", function (evt) {
            var element = evt.params.data.element;
            var $element = $(element);
            $element.detach();
            $(this).append($element);
            $(this).trigger("change");
        });
    });
}
function GetSelectPageSearch(divID, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/Page",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);

        var html = "";
        html = `<option selected value="">—All Page</option>`;
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}

function GetSelectProductTypeMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/ProductType",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlProductCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectProductColorMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/ProductColor",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlProductCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectProductCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/ProductCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlProductCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectProductCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/ProductCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlProductCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentProduct(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/ProductCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlProductCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        //$(divID).niceSelect();
        $(divID).select2({
            dropdownParent: $('#inputModal .modal-content')
        });
    });
}
function getHtmlProductCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlProductCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectNewsCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/NewsCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlNewsCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectNewsCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/NewsCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlNewsCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentNews(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/NewsCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlNewsCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        //$(divID).niceSelect();
        $(divID).select2({
            dropdownParent: $('#inputModal .modal-content')
        });
    });
}
function getHtmlNewsCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlNewsCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectGalleryCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/GalleryCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlGalleryCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectGalleryCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/GalleryCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlGalleryCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentGallery(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/GalleryCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlGalleryCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        //$(divID).niceSelect();
        $(divID).select2({
            dropdownParent: $('#inputModal .modal-content')
        });
    });
}
function getHtmlGalleryCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlGalleryCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectDiscountCodeCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/DiscountCodeCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlDiscountCodeCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectDiscountCodeCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/DiscountCodeCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlDiscountCodeCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentDiscountCode(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/DiscountCodeCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlDiscountCodeCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect();
    });
}
function getHtmlDiscountCodeCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlDiscountCodeCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectFAQCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/FAQCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlFAQCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectFAQCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/FAQCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlFAQCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentFAQ(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/FAQCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlFAQCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        $(divID).select2({
            dropdownParent: $('#inputModal .modal-content')
        });
    });
}
function getHtmlFAQCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlFAQCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectFeatureCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/FeatureCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlFeatureCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectFeatureCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/FeatureCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlFeatureCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentFeature(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/FeatureCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlFeatureCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        $(divID).select2({
            dropdownParent: $('#inputModal .modal-content')
        });
    });
}
function getHtmlFeatureCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlFeatureCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectPromotionCateMulti(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/PromotionCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected === "") {
            //html = `<option selected value="">—Chose Categories</option>`;
        } else {
            //html = `<option  value="">—Chose Categories</option>`;

        }
        for (let i = 0; i < rs.length; i++) {
            html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            if (rs[i].Children && rs[i].Children.length > 0) {
                if (rs[i].Children && rs[i].Children.length > 0) {
                    html += getHtmlPromotionCate(rs[i].Children, selected, "–");
                }
            }
        }
        $(divID).html(html);
        $(divID).select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });
        $(divID).val(selected).trigger('change');
    });

}
function GetSelectPromotionCateSearch(divID, lang, selected) {
    $.ajax({
        url: "/b-admin/GetSelect/PromotionCate",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected != "") {
            html = `<option value="0">—All Categories</option>`;

        } else {
            html = `<option selected value="0">—All Categories</option>`;
        }
        for (let i = 0; i < rs.length; i++) {
            if (selected == rs[i].Value) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;

            } else {

                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;

            }
            if (rs[i].Children && rs[i].Children.length > 0) {
                html += getHtmlPromotionCate(rs[i].Children, selected, "–");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect('update');
    });
}
function GetSelectParentPromotion(divID, selected, lang) {
    $.ajax({
        url: "/b-admin/GetSelect/PromotionCateParent",
        method: "POST",
        data: { lang: lang }

    }).done(function (data) {
        var rs = JSON.parse(data.jsData);
        var html = "";
        if (selected) {
            html += `<option selected value="0">—Vui lòng chọn</option>`;
        } else {
            html += `<option value="0">—Vui lòng chọn</option>`;
        }

        for (let i = 0; i < rs.length; i++) {
            if (rs[i].Value == selected) {
                html += `<option selected value="${rs[i].Value}">${rs[i].Name}</option>`;
            } else {
                html += `<option value="${rs[i].Value}">${rs[i].Name}</option>`;
            }
            if (rs[i].Children.length > 0) {
                html += getHtmlPromotionCate(rs[i].Children, selected, "");
            }
        }
        $(divID).html(html);
        $(divID).niceSelect();
    });
}
function getHtmlPromotionCate(rs, selected, prefix) {
    prefix += "–";
    let html = '';
    for (let i = 0; i < rs.length; i++) {
        if (rs[i].Value == selected) {
            html += `<option selected value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        } else {
            html += `<option value="${rs[i].Value}">${prefix + rs[i].Name}</option>`;
        }
        if (rs[i].Children.length > 0) {
            html += getHtmlPromotionCate(rs[i].Children, selected, prefix);
        }
    }
    return html;
}

function GetSelectProvince(divProvinceId, divDistrictId, divWardId, provinceValue, districtValue, wardValue) {
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