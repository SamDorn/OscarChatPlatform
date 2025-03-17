$(document).ready(function () {

    // When the continue as guest button is pressed
    // it sends a POST request to create a guest user.
    // After that a cookie with UserId is created
    $("#Continua").on("click", function () {
        $.ajax({
            type: "POST",
            url: CREATE_GUEST_URL, // defined in the home index view
            success: function (response) {
                if (response.redirectUrl) {
                    window.location.href = response.redirectUrl; // Redirect the user to the home page
                }
            }
        });
    });
});