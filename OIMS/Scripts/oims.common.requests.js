/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Common - Requests
*/

var el = $('#reqSummary');
var busy = false;

function ShowDetails(value)
{
    if (value != null)
    {
        var cmd =
        {
            src: el,
            href: el.data("href"),
            data: { 'rowId': value }
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