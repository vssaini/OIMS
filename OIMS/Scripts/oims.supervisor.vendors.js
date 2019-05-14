/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Vendors
*/

var showDiv = true;

$(function ()
{
    ApplySelect2();
    
    $("#btnShowNewVendorDiv").click(ShowNHideDiv);
    
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
        $("#newVendorSpan").text('Hide this');
        $("#NewVendorDiv").fadeIn(500);
        showDiv = false;
    } else
    {
        $("#newVendorSpan").text('Add new vendor');
        $("#NewVendorDiv").fadeOut(500);
        showDiv = true;
    }
}

// Set autocomplete feature by typeahead
function SetAutocomplete()
{
    var vName = '#Name';
    var remUrl = "Supervisor/GetVendors?searchTerm=%QUERY";

    var vendorCols = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        hint: true,
        highlight: true,
        items: 5,
        minLength: 3,
        remote: remUrl
    });

    vendorCols.initialize();

    $(vName).typeahead(null, {
        name: 'vendorCols',
        displayKey: 'Name',
        source: vendorCols.ttAdapter()
    });
}

// AJAX FUNCTIONALITIES
function VendorBegin() 
{
    VendorLoader.Show();
}

function VendorSuccess()
{
   VendorLoader.Hide();
    
    if ($("#serverConfirm").length > 0)
    {
        gvVendors.PerformCallback();

        // Reset form back after saving
        $('#frmVendor').each(function ()
        {
            this.reset();
        });
    }
}

function VendorFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}
