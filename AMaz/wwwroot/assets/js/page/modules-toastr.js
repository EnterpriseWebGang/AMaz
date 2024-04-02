"use strict";

$("#toastr-2").click(function () {
    var formData = $("#changeUserRoleForm").serialize();
    $.ajax({
        url: '/Account/ChangeUserRoleAndFaculty?userId=@ViewBag.UserId',
        type: 'POST',
        data: formData,
        success: function (result) {
            iziToast.success({
                title: 'Success!',
                message: 'User role or faculty changed successfully!',
                position: 'topRight'
            });
            window.location.href = '/Account';
        },
        error: function () {
            iziToast.error({
                title: 'Error!',
                message: 'An error occurred while changing user role or faculty.',
                position: 'topRight'
            });
        }
    });
});
