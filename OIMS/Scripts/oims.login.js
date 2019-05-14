/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Login
*/

var counter;
var showDiv = true;

$(function ()
{
    $(".user-block").click(processUserSelection);    $(".switchUserLink").click(processSwitchUser);

    $("#frmLogin button:submit").click(submitForm);
});

// Show specific user's credentials
function processUserSelection()
{
    $("#userContent").html(""); // Reset error text
    
    var uc = $(".login-outer");
    var selectedId = this.id;

    if (uc)
    {
        uc.fadeOut(500, function ()
        {
            var selectedUserInfo = $("#user_" + selectedId);
            if (selectedUserInfo)
            {
                selectedUserInfo.fadeIn(500);
                //selectedUserInfo.find("input[type=password]").val('MyPassword');
            }
        });
    }
}

// Switch to users group
function processSwitchUser()
{
    $("#userContent").html(""); // Reset error text

    var parent = $(this).parent();
    if (parent)
        parent.fadeOut(500, function ()
        {
            var uc = $(".login-outer");
            if (uc)
            {
                uc.fadeIn(500);
            }
        });
}

// Check if password fields are not blank
function submitForm(event)
{
    var parent = $(this).parent();
    if (parent)
    {
        var user = $.trim(parent.find("input[type=text]").val());
        var pass = $.trim(parent.find("input[type=password]").val());
    }
    
    if (user == ''|| pass == '') 
    {
        // Cancel form submit
        event.preventDefault();

        // Show alert message
        $().toastmessage('showToast', {
            text: 'Username or Password is blank',
            sticky: false,
            position: 'middle-center',
            type: 'error',
            close: function () { console.log("toast is closed ..."); }
        });
    }
}

