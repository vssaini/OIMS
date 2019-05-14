/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Requests
*/

var el = $('#reqSummary');
var busy = false;
var requestId;
var requestorId;

$(function ()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });

    $('#btnGo').on("click", function ($event)
    {
        $event.preventDefault();

        gvRequests.PerformCallback({ 'filter': $('#selectFilters').val() });
    });
});

function ShowDetails(reqId, status, creatorId)
{
    if (reqId != null)
    {
        requestId = reqId;
        requestorId = creatorId;

        var cmd =
        {
            src: el,
            href: el.data("href"),
            data: { 'requestId': requestId, 'status': status, 'filter': $('#selectFilters').val() }
        };

        if (busy)
            return false;

        $.ajax({
            cache: false,
            type: 'POST',
            url: cmd.href,
            data: cmd.data,

            beforeSend: function ()
            {
                busy = true;
                EventBroker.publish("reqSummary.beforeSend", cmd);
            },

            success: function (data)
            {
                // If not defined, assume true
                var isSuccess = data.success === undefined ? true : data.success;

                if (isSuccess)
                {
                    EventBroker.publish("reqSummary.showed", data);
                }
                else
                {
                    EventBroker.publish("reqSummary.error",
                        $.extend({ response: { success: false, message: data.message } }));
                }
            },

            error: function (jqXHR, textStatus, errorThrown)
            {
                EventBroker.publish(
                           "reqSummary.error",
                           $.extend({ response: { success: false, message: errorThrown } }));
            },

            complete: function ()
            {
                busy = false;
                EventBroker.publish("reqSummary.complete", cmd);
            }
        });
    }
    return false;
}

function Select2Misc()
{
    var select = $('#StatusId');
    var btnStatus = $('#btnSaveStatus');

    // Get status for request
    var sid = $('#statusIdSupv').val();

    // Apply select styles
    select.select2();
    $(".select2-search").remove();

    // Set selected value
    select.select2("val", sid);

    // Do something with selected text
    var data = select.select2('data');

    if (data)
    {
        if (data.text === "Approved" || data.text === "Rejected" || data.text === "Manager approval pending")
        {
            select.select2("disable");
            btnStatus.hide();
        }
        else
        {
            // Remove options not required for supervisor
            select.find('option[value=3]').remove(); // Approved
            select.find('option[value=4]').remove(); // Rejected

            if (data.text === "Partial approved")
            {
                select.find('option[value=1]').remove(); // In process
            } else
            {
                select.find('option[value=5]').remove(); // Partial approved
            }

            btnStatus.show();
        }
    }
    else
    {
        console.log('Select selected text is null.');
    }

    select.on("change", function (e)
    {
        // Enable or disable button for 'In Process' or 'Partialapproved'
        if (e.val == 1 || e.val == 5)
        {
            btnStatus.attr('disabled', true);
        } else
        {
            btnStatus.attr('disabled', false);
        }

        // Change the text of button based on option
        if (e.val == 2)
        {
            $('#btnSaveStatus span').text("Save & Recommend");
        } else
        {
            $('#btnSaveStatus span').text("Save");
        }
    });

    btnStatus.on("click", function ($event)
    {
        $event.preventDefault();
        SaveStatus(select.val());
    });
}

function SaveStatus(value)
{
    if (value != null)
    {
        var cmd =
        {
            href: 'Supervisor/SaveStatus',
            data: { 'statusId': value, 'requestId': requestId, 'requestorId': requestorId }
        };

        if (busy)
            return false;

        $.ajax({
            cache: false,
            type: 'POST',
            url: cmd.href,
            data: cmd.data,

            beforeSend: function ()
            {
                busy = true;
                EventBroker.publish("reqSummary.beforeSend", cmd);
            },

            success: function (data)
            {
                var isSuccess = data.success === undefined ? true : data.success;

                if (isSuccess)
                {
                    EventBroker.publish("reqSummary.saved", data);
                }
                else
                {
                    EventBroker.publish("reqSummary.error",
                        $.extend({ response: { success: false, message: data.message } }));
                }
            },

            error: function (jqXHR, textStatus, errorThrown)
            {
                EventBroker.publish("reqSummary.error",
                           $.extend({ response: { success: false, message: errorThrown } }));
            },

            complete: function ()
            {
                busy = false;
                EventBroker.publish("reqSummary.complete", cmd);
            }
        });
    }
    return false;
}