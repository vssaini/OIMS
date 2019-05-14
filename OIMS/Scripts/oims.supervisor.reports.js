/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Reports
*/

$(function ()
{
    ApplySelect2();
    HandleSelectChange();

});

// Apply select2 to all selects
function ApplySelect2()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });
}

// Handle select change event
function HandleSelectChange()
{
    var selectCategory = $('#Category');
    var selectShelters = $('#s2id_ShelterId');
    
    selectCategory.on("change", function ()
    {
        var text = selectCategory.select2('data').text;

        if (text === "Items")
        {
            selectShelters.fadeOut();
            $('#SearchParam').fadeIn();
        }

        if (text === "Shelters")
        {
            $('#SearchParam').fadeOut();
            selectShelters.fadeIn();
        }
    });
}


// AJAX FUNCTIONALITIES
function ReportBegin()
{
    ReportLoader.Show();
}

function ReportSuccess()
{
    ReportLoader.Hide();
}

function ReportFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}