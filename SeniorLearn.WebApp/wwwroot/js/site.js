// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const hamburger = document.querySelector('#toggle-btn');

hamburger.addEventListener('click', () => {
    document.querySelector('#sidebar').classList.toggle('expand');
})

//dark theme toggle
//document.querySelector('.theme-toggle').addEventListener('click', () => {
//    toggleLocalStorage();
//    toggleRootClass();
//})

//function toggleRootClass(){
//    const current = document.documentElement.getAttribute('data-bs-theme');
//    const inverted = current == 'dark' ? 'light' : 'dark';
//    document.documentElement.setAttribute('data-bs-theme', inverted);
//}

//function toggleLocalStorage() {
//    if (isLight()) {
//        localStorage.removeItem("light");
//    } else {
//        localStorage.setItem("light", "set")
//    }
//}

//function isLight() {
//    return localStorage.getItem('light');
//}

//if (isLight()) {
//    toggleRootClass();
//}

//edit icon
function toggleEdit(fieldId, icon) {
    const field = document.getElementById(fieldId);
    const isReadOnly = field.hasAttribute('readonly');

    if (isReadOnly) {
        field.removeAttribute('readonly');
        field.focus();
        icon.classList.remove('bi-pencil-square');
        icon.classList.add('bi-check-square');
    } else {
        field.setAttribute('readonly', true);
        icon.classList.remove('bi-check-square');
        icon.classList.remove('bi-pencil-square');

    }
}

//Modal for Manage page

    $(document).ready(function(){
        $('#editFirstName').on('click', function () {
            $('#editFirstNameModal').modal('show');
        });

    $('#saveFirstName').on('click', function() {
            const newFirstName = $('#FirstNameEditField').val();
    $('#FirstNameField').val(newFirstName);
    $('#editFirstNameModal').modal('hide');
        });
    });
