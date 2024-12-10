$(".languagePicker").click(function (data) {
    var lang = data.target.lang;
    $.ajax({
        url: '/Shared/SetSessionLang',
        method: 'POST',
        data: { lang: lang }
    }).done(function (rs) {
        sessionStorage.setItem("textLang", data.target.innerText);
        window.location.href = rs;
    })
});

$(function () {
    var lang = sessionStorage.getItem("textLang");
    if (lang) {
        $('#text-lang').text(lang);
    }

    if (location.href.includes("en")) {
        $('#text-lang').text("English");
        sessionStorage.setItem("textLang", "English");
    }
});
