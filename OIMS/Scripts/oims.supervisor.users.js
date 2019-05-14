/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Users
*/

var showDiv = true;
var passMatch = false;

$(function ()
{
    ApplySelect2();
    
    $("#btnShowNewUserDiv").click(ShowNHideDiv);
    
    $("#btnSave").click(SubmitForm);

    $("#Password").on('keyup', ResetPassInfo);

    $("#ConfirmPassword").on('keyup', MatchPass);

    SetAutocomplete();
});

// Run select2 for each select
function ApplySelect2()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });
}

// Show and hide div accordingly
function ShowNHideDiv($event)
{
    $event.preventDefault();
    
    if (showDiv) {
        $("#newUserSpan").text('Hide this');
        $("#NewUserDiv").fadeIn(500);
        showDiv = false;
    } else
    {
        $("#newUserSpan").text('Add new user');
        $("#NewUserDiv").fadeOut(500);
        showDiv = true;
    }
}

// Submit form if passwords matches
function SubmitForm()
{
    if (!passMatch)
    {
        return false;
    }
    else
    {
        return true;
    }
}

// Reset pass info div
function ResetPassInfo() {
    passMatch = false;
    $("#pass-info").removeClass().html("");
}

// Match password and confirm password
function MatchPass() {
    var pass = $("#Password").val();
    var cpass = $("#ConfirmPassword").val();

    if (pass !== cpass)
    {
        $("#pass-info").removeClass().addClass('weakpass').html("Passwords do not match!");
        passMatch = false;
    } else
    {
        $("#pass-info").removeClass().addClass('goodpass').html("Passwords matched!");
        passMatch = true;
    }
}

// Set autocomplete feature by typeahead
function SetAutocomplete()
{
    var emailId = '#Email';
    var remUrl = "Supervisor/GetEmails?searchTerm=%QUERY";

    var emailCols = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        hint: true,
        highlight: true,
        items: 5,
        minLength: 3,
        remote: remUrl
    });

    emailCols.initialize();

    $(emailId).typeahead(null, {
        name: 'emailCols',
        displayKey: 'Email',
        source: emailCols.ttAdapter()
    });
}

// AJAX FUNCTIONALITIES
function UserBegin() {
    UserLoader.Show();
}

function UserSuccess()
{
    UserLoader.Hide();
    
    if ($("#serverConfirm").length > 0)
    {
        gvUsers.PerformCallback();

        // Reset form back after saving
        $('#frmUser').each(function ()
        {
            this.reset();
        });
        
        $("#pass-info").removeClass().html("");
    }
}

function UserFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}
