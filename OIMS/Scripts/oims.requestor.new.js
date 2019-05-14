/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Requestor--> New Request
*/

// ----- Variables -----
var radioId = null;
var oldJobChoice = null;
var newJobChoice = null;
var companyName = null;

$(function ()
{
    DisableControls(true);
    ApplySelect2();
    ShowDivInCenter('placeHolder');
    ShelterMisc();

    $('#btnShowPopup').on("click", function ($event)
    {
        $event.preventDefault();
        jobPopup.Show();

        // Bug - This div was moving down automatically. 
        // So fixed by class to show popup in center.
        $('#jobPopup_PW-1').addClass("top163");
    });
});

function SetBtnClicks()
{
    $('#btnSaveJob').on("click", function ($event)
    {
        $event.preventDefault();

        // Set variable's values
        companyName = $("#Company option:selected").text();
        newJobChoice = $('#txtJob').val();

        if (radioId === "oldJob")
        {
            oldJobChoice = $("#Job option:selected").text();
            newJobChoice = null;
        }

        // Send request to save details
        $.ajax({
            type: 'POST',
            url: 'Requestor/SaveJob',
            cache: false,
            async: true,
            data: { 'setDefault': false, 'existingJob': oldJobChoice, 'newJob': newJobChoice, 'company': companyName },
            beforeSend: function ()
            {
                PopLoader.Show();
            },
            success: function (response)
            {
                if (response && response.message)
                {
                    if (response.success)
                    {
                        PopLoader.Hide();
                        jobPopup.Hide();

                        // Enable boxes
                        DisableControls(false);

                        $('#hfJob').val(response.job); // Set job name
                        ShowNotice(response.message, "info", true);
                    }
                    else
                    {
                        $('#hfJob').val("");
                        PopLoader.Hide();
                        $("#PopupContent").html(response.message);
                    }

                }
                else
                {
                    $('#hfJob').val("");
                    PopLoader.Hide();
                    $("#PopupContent").html(response);
                }
            }
        });
    });

    $('#btnDefault').on("click", function ($event)
    {
        $event.preventDefault();
        
        // Get details
        companyName = $("#Company option:selected").text();
        
        $.ajax({
            type: 'POST',
            url: 'Requestor/SaveJob',
            cache: false,
            async: true,
            data: { 'setDefault': true, 'company': companyName },
            beforeSend: function ()
            {
                PopLoader.Show();
            },
            success: function (response)
            {
                if (response && response.message)
                {
                    if (response.success)
                    {
                        PopLoader.Hide();
                        jobPopup.Hide();

                        // Enable boxes
                        DisableControls(false);

                        $('#hfJob').val(response.job); // Set job name
                        ShowNotice(response.message, "info", true);
                    }
                    else
                    {
                        $('#hfJob').val("");
                        PopLoader.Hide();
                        $("#PopupContent").html(response.message);
                    }

                }
                else
                {
                    $('#hfJob').val("");
                    PopLoader.Hide();
                    $("#PopupContent").html(response);
                }
            }
        });
    });

    $('#btnCancel').on("click", function ($event)
    {
        $event.preventDefault();
        jobPopup.Hide();
    });
}

function SetRadioClicks()
{
    $(':radio').change(function ()
    {
        radioId = $(this).data('id');

        if (radioId === "newJob")
        {
            oldJobChoice = null;
            $('#oldJob').fadeOut();
            $('#txtJob').fadeIn();
        }
        else
        {
            newJobChoice = null;
            $('#txtJob').fadeOut();
            $('#oldJob').fadeIn(500);
        }
    });
}

// Run select2 for each select
function ApplySelect2()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });

    $("[data-original-title]").tooltip(); // show tooltip

    $('input[type=number]').numeric({ negative: false }); // restrict text instead of numbers
}

// Show div in center
function ShowDivInCenter(divName)
{
    try
    {
        divWidth = 100;
        divHeight = 100;
        divId = divName; // id of the div that to show in center

        // Get the x and y coordinates of the center in output browser's window 
        var centerX, centerY;
        if (self.innerHeight)
        {
            centerX = self.innerWidth;
            centerY = self.innerHeight;
        }
        else if (document.documentElement && document.documentElement.clientHeight)
        {
            centerX = document.documentElement.clientWidth;
            centerY = document.documentElement.clientHeight;
        }
        else if (document.body)
        {
            centerX = document.body.clientWidth;
            centerY = document.body.clientHeight;
        }

        var offsetLeft = (centerX - divWidth) / 2;
        var offsetTop = (centerY - divHeight) / 2;

        // The initial width and height of the div can be set in the
        // style sheet with display:none; divid is passed as an argument to // the function
        var ojbDiv = document.getElementById(divId);

        ojbDiv.style.position = 'absolute';
        ojbDiv.style.top = offsetTop + 'px';
        ojbDiv.style.left = offsetLeft + 'px';
        ojbDiv.style.display = "block";
    }
    catch (e) { }
}

// Disable or enable controls
function DisableControls(value)
{
    // Disable boxes initially
    $('#prodItemBox').attr('disabled', value);
    $('input[type=number]').attr('disabled', value);
    $("#prodItemBox :button").attr('disabled', true);
    $('#divShelterItems').fadeOut();

    if (value)
    {
        $("#prodItemBox select").prop('disabled', true);
    } else
    {
        $("#prodItemBox select").prop('disabled', false); // Required otherwise form doesn't include it in serialize
        $("#prodItemBox select").select2("enable");
    }
}

// Miscellaneous things related to shelter
function ShelterMisc()
{
    var shelterId;
    var text = $('select').select2('data').text;
    var number = $('input[type=number]').val();

    $('select').on("change", function ()
    {
        text = $('select').select2('data').text;
        shelterId = $('select').select2('data').id;
        ShowShelterItems(text, number, shelterId);

        //if (text != "-- Select Shelter --" && number)
        //{
        //    $("#prodItemBox :button").attr('disabled', false);
        //    gvShelterItems.PerformCallback({ 'shelterNqty': shelterId + ',' + number });
            
        //    $('#divShelterItems').fadeIn();
        //    $('#divButton').addClass('padTop10');
        //} else
        //{
        //    $("#prodItemBox :button").attr('disabled', true);
        //    $('#divShelterItems').fadeOut();
        //    $('#divButton').removeClass('padTop10');
        //}
    });

    $('input[type=number]').keyup(function ()
    {
        number = $('input[type=number]').val();
        ShowShelterItems(text, number,shelterId);

        //if (text != "-- Select Shelter --" && number)
        //{
        //    $("#prodItemBox :button").attr('disabled', false);
        //    gvShelterItems.PerformCallback({ 'shelterNqty': shelterId + ',' + number });
            
        //    $('#divShelterItems').fadeIn();
        //    $('#divButton').addClass('padTop10');
        //} else
        //{
        //    $("#prodItemBox :button").attr('disabled', true);
        //    $('#divShelterItems').fadeOut();
        //    $('#divButton').removeClass('padTop10');
        //}
    });

    $('input[type=number]').change(function ()
    {
        number = $('input[type=number]').val();
        ShowShelterItems(text, number, shelterId);
    });
}

function ShowShelterItems(text, number, shelterId)
{
    if (text != "-- Select Shelter --" && number)
    {
        $("#prodItemBox :button").attr('disabled', false);
        gvShelterItems.PerformCallback({ 'shelterNqty': shelterId + ',' + number });

        $('#divShelterItems').fadeIn();
        $('#divButton').addClass('padTop10');
    } else
    {
        $("#prodItemBox :button").attr('disabled', true);
        $('#divShelterItems').fadeOut();
        $('#divButton').removeClass('padTop10');
    }
}