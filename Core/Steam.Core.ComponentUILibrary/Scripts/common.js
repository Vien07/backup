
function Toastify(mess,type)
{
    var backgroundColor = "linear-gradient(to right, #00b09b, #96c93d)";
    if (type == "error") {
        backgroundColor = "linear-gradient(#e66465, #9198e5)";
    }
    Toastify({
        text: mess,
        duration: 3000,
        backgroundColor: backgroundColor,
    }).showToast();
   
}