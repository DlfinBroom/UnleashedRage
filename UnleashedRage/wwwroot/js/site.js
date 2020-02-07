// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/* Set the width of the sidebar to 250px and the left margin of the page content to 250px */
function openNav() {
    document.getElementById("Archive").style.width = "250px";
    // document.getElementById("ArchiveOpen").style.visibility = "hidden"
}

function closeNav() {
    document.getElementById("Archive").style.width = "0";
    // document.getElementById("ArchiveOpen").style.visibility = "visible"
}

function changeImage(image) {
    document.getElementById("CurrentPage").src =
        String.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(image));
}