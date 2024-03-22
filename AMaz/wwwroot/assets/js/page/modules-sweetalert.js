"use strict";

$("#swal-2").click(function() {
  // Trigger the form submission when the button is clicked
  $(".need-validation").submit();
});

$("#swal-2").click(function () {
    // Check if any radio button is selected
    if ($('input[name="value"]:checked').length === 0) {
        // If no radio button is selected, show the validation message
        $('#roleValidationMessage').show();
    } else {
        // If a radio button is selected, check if the form is valid
        if ($('.wizard-content')[0].checkValidity()) {
            // Check password validity
            var password = $('input[name="password"]').val();
            if (!isValidPassword(password)) {
                // Password does not meet requirements, show validation message
                $('#passwordValidationMessage').show();
                return;
            } else {
                // Hide password validation message if password is valid
                $('#passwordValidationMessage').hide();
            }

            // Display sweet alert if the form is valid
            $('#roleValidationMessage').hide();
            swal('Account Created', 'Your account has been created successfully!', 'success')
                .then((value) => {
                    if (value) {
                        // If the user clicks OK on the alert, redirect to admin.html
                        var redirectUrl = "/Account"
                        window.location.href = redirectUrl;
                    }
                });
        } else {
            // If the form is not valid, trigger form validation
            $('.wizard-content').addClass('was-validated');
        }
    }
});

// Function to validate password format
// Function to validate the password format
function isValidPassword(password) {
    // Regular expression for password validation
    // Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one non-alphanumeric character
    var passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$/;
    return passwordPattern.test(password);
}

$(".need-validation").submit(function (event) {
    event.preventDefault();
    event.stopPropagation();

    var form = $(this); // Capture the form element for later use

    // Validate the form
    if (form[0].checkValidity() === false) {
        // Add the was-validated class to trigger Bootstrap validation styles
        form.addClass("was-validated");
    } else {
        // Check if a role is selected
        var selectedRole = $('input[name="Role"]:checked').val();
        if (!selectedRole) {
            // If no role is selected, show validation message and return
            $('#roleValidationMessage').show();
            return;
        }

        // Hide all validation messages if validation passes
        $('.invalid-feedback').hide();

        // Check password validity
        var password = $('input[name="password"]').val();
        if (!isValidPassword(password)) {
            // Password does not meet requirements, show validation message
            $('#passwordValidationMessage').show();
            return;
        }

        // Check email existence
        var email = $('input[name="email"]').val();
        isEmailExist(email).then(function (exists) {
            // Hide all email-related validation messages
            $('.email-validation-feedback').hide();
            
            if (exists) {
                // Email already exists, show validation message
                $('#emailExistsValidationMessage').show();
            } else {
                // Email does not exist, submit the form data asynchronously
                $.ajax({
                    url: form.attr('action'),
                    method: form.attr('method'),
                    data: form.serialize(),
                    success: function (response) {
                        // Display success message
                        swal({
                            title: 'Account Created',
                            text: 'Your account has been created successfully!',
                            icon: 'success',
                        }).then((value) => {
                            // Redirect to the appropriate page after success message
                            var redirectUrl = "/Account";
                            window.location.href = redirectUrl;
                        });
                    },
                    error: function (xhr, status, error) {
                        // Handle error case
                        console.error(error);
                        // Display error message if needed
                    }
                });
            }
        }).catch(function (error) {
            console.error('Error checking email existence:', error);
        });
    }
});

// Function to check if the email exists
function isEmailExist(email) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Account/CheckEmailExists', // Endpoint to check email existence
            method: 'POST',
            data: { email: email },
            success: function (response) {
                resolve(response.exists); // Resolve with true or false based on the response
            },
            error: function (xhr, status, error) {
                reject(error); // Reject with the error message
            }
        });
    });
}




$(".needs-validation-profile").submit(function(event) {
  event.preventDefault();
  event.stopPropagation();

  // Validate the form
  if (this.checkValidity() === false) {
    // Add the was-validated class to trigger Bootstrap validation styles
    $(this).addClass("was-validated");
  } else {
    // If the form is valid, show success message
    swal({
      title: 'Changes Saved',
      text: 'Your profile information has been updated successfully!',
      icon: 'success',
    })
    .then((value) => {
        if (value) {
            // If the user clicks OK on the alert, redirect to admin.html
            window.location.href = '../UserUI/user-profile.html';
        }
    });
  }
});

$("#swal-4").click(function() {
  // Trigger the form submission when the button is clicked
  $(".needs-validation-profile").submit();
});


$(".needs-validation").submit(function(event) {
  event.preventDefault();
  event.stopPropagation();

  // Validate the form
  if (this.checkValidity() === false) {
    // Add the was-validated class to trigger Bootstrap validation styles
    $(this).addClass("was-validated");
  } else {
    // If the form is valid, show success message
    swal({
      title: 'Changes Saved',
      text: 'Your profile information has been updated successfully!',
      icon: 'success',
    })
    .then((value) => {
        if (value) {
            // If the user clicks OK on the alert, redirect to admin.html
            window.location.href = '#';
        }
    });
  }
});

$("#swal-3").click(function() {
  // Trigger the form submission when the button is clicked
  $(".needs-validation").submit();
});


$("#swal-6").click(function (event) {
  // Prevent the default form submission
  event.preventDefault();

  // Get the values of password and confirm password fields
  var password = $('input[name="password"]').val();
  var confirmPassword = $('input[name="confirmPassword"]').val();

  // Check if the passwords match
  if (password !== confirmPassword) {
      // Passwords don't match, show an alert
      swal('Error', 'Passwords do not match. Please make sure your passwords match.', 'error');
      return; // Stop further processing
  }

  // Check if the form is valid
  if ($('.wizard-content')[0].checkValidity()) {
      // Display sweet alert if the form is valid
      swal({
        title: 'Are you sure?',
        text: 'Once Reseted, This account will log in with the new password!',
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    }).then((willReset) => {
        if (willReset) {
            swal('Reset Password Successfully!', {
                icon: 'success',
            }).then(() => {
                // If the user clicks OK on the alert, redirect to admin.html
                window.location.href = '#';
            });
        } else {
            swal('The password remains unchanged.');
        }
      });
  } else {
      // If the form is not valid, trigger form validation
      $('.wizard-content').addClass('was-validated');
  }
});

$(document).ready(function() {
  $("#homeworkForm").submit(function(event) {
      // Check if the checkbox is checked
      if (!$("#agree").prop("checked")) {
          // Prevent the default form submission
          event.preventDefault();
          // Display SweetAlert warning
          swal('Warning', 'You have not checked the box to agree to terms and conditions!', 'warning');
      }
  });
});


