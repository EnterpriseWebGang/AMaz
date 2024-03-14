document.addEventListener("DOMContentLoaded", function () {
    var userProfile = document.querySelector(".user-profile");

    userProfile.addEventListener("click", function () {
        userProfile.classList.toggle("active");
    });
});
