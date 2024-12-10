function Search(div) {
    var searchString = $("#" + div).val().trim();
    if (searchString === "") {
        Swal.fire({ position: 'center', type: 'error', title: enterSearchKeyword, showConfirmButton: false, timer: 1500 });
        return;
    } else {
        var encodedSearchString = encodeURI(searchString).replaceAll('%20', '+');
        window.location.href = searchUrl + '?q=' + encodedSearchString;
    }
}

function SearchTag(tag) {
    var searchString = encodeURI(tag).replaceAll('%20', '+');
    window.location.href = searchUrl + '?option=tags' + '&q=' + searchString;
}

$('input.search-biz').on('keyup', function (e) {
    e.preventDefault();
    if (e.keyCode == 13) {
        Search(this.id)
    }
})

$("#search-button").on('click', function (e) {
    e.preventDefault();
    Search("search-desktop")
})
