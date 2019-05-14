/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Manager--> Requests
*/

var requestId;
var requestorId;

function Select2Misc()
{
    var select = $('select');
    var btnStatus = $('#btnSaveStatusMgr');

    var sid = $('#statusId').val(); // Status selected value

    // Apply select styles
    select.select2();
    $(".select2-search").remove();
    
    if (sid)
    {
        // Set selected value
        select.select2("val", sid);
        console.log(sid);
    }
    else
    {
        select.select2("val", -1);
    }

    btnStatus.on("click", function ()
    {
        //ShowNotice(select.val(), "info", true);
        SaveStatus(select.val());
    });
    
    $("[data-original-title]").tooltip(); // Set tooltip
}

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
            data: { 'requestId': requestId, 'status': status}
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

function SaveStatus(value)
{
    if (value != null)
    {
        var cmd =
        {
            href: 'Manager/SaveStatus',
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