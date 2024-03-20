"use strict";

$("#toastr-2").click(function (event) {
    // Prevent the default behavior of the element
    event.preventDefault();

    // Check if the form is valid
    if ($("#academicYearForm")[0].checkValidity()) {
        // Your code to handle the form submission
        iziToast.success({
            title: 'Success!',
            message: 'Academic year created successfully!',
            position: 'topRight'
        });

        // Redirect to admin.html after 3000 milliseconds (3 seconds)
        setTimeout(function () {
            window.location.href = 'admin.html';
        }, 3000);
    } else {
        // If the form is not valid, trigger form validation
        $("#academicYearForm").addClass('was-validated');
    }
});
