/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini
Author: Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Requestor -- Ajax cart
*/
var AjaxCart = (function ($, window, document, undefined)
{
    $(function ()
    {
        // GLOBAL event handler
        $(document).on("click", ".ajax-cart-link", function ()
        {
            // Call ajax if values are not empty
            var callAjax = true;
            var subType = $(this).data('sub-type');

            switch (subType)
            {
                case 'product':
                    var pel = $('#divProduct').find('input[type=number]');

                    if (!pel.val()) {
                        callAjax = false;
                        EventBroker.publish(
                            "ajaxcart.error",
                            $.extend({ response: { success: false, message: "Quantity for product can't be blank." } }));
                    }
                    break;
                case 'item':
                    var iel = $('#divItem').find('input[type=number]');

                    if (!iel.val())
                    {
                        callAjax = false;
                        EventBroker.publish(
                           "ajaxcart.error",
                           $.extend({ response: { success: false, message: "Quantity for item can't be blank." } }));
                    }
                    break;
            }

            if (callAjax)
            {
                return AjaxCart.executeRequest(this);
            }
            else
            {
                return false;
            }
        });

        $("body").on("click", ".save-cart-link", function ()
        {
            return AjaxCart.executeRequest(this);
        });
    });

    function createMessageObj(el)
    {
        return {
            success: { title: el.data("msg-success-title"), text: el.data("msg-success-text") },
            error: { title: el.data("msg-error-title"), text: el.data("msg-error-text") }
        };
    }

    /*
        attr("href") > href
    */
    function createCommand(el)
    {
        el = $(el);

        var cmd = {
            src: el,
            type: el.data("type") || "cart",
            action: el.data("action") || "add", // or "remove" or "save"
            href: el.data("href") || el.attr("href"),
            data: $('#form-NewRequest').serialize()
        };

        return cmd;
    }

    function verifyCommand(cmd)
    {
        return !!(cmd.href);
    }

    var busy = false;

    return {
        executeRequest: function (cmd)
        {
            if (busy)
                return false;

            if (!$.isPlainObject(cmd))
            {
                cmd = createCommand(cmd);
            }
            if (!cmd || !verifyCommand(cmd)) return false;

            busy = true;

            if (cmd.action === "add")
            {
                EventBroker.publish("ajaxcart.item.adding", cmd);
            }
            else if (cmd.action === "remove")
            {
                EventBroker.publish("ajaxcart.item.removing", cmd);
            }

            $.ajax({
                cache: false,
                url: cmd.href,
                data: cmd.data,
                type: 'POST',

                beforeSend: function ()
                {
                    if (cmd.action === "add")
                    {
                        EventBroker.publish("ajaxcart.beforeSend", cmd);
                    }
                    
                    if (cmd.action === "save")
                    {
                        EventBroker.publish("ajaxcart.save.beforeSend", cmd);
                    }
                },

                success: function (response)
                {
                    if (response.redirect)
                    {
                        // when the controller sets the "redirect"
                        // property (either to cart, product page etc.), 
                        // it's mandatory to do so and useless to do ajax stuff.
                        location.href = response.redirect;
                        return false;
                    }

                    // success is optional and therefore true by default
                    var isSuccess = response.success === undefined ? true : response.success;

                    var msg;

                    if (cmd.action === "add")
                    {
                        msg = "ajaxcart.item.added";
                    }
                    else if (cmd.action === "remove")
                    {
                        msg = "ajaxcart.item.removed";
                    } else if (cmd.action === "save")
                    {
                        msg = "ajaxcart.item.saved";
                    }

                    //var msg = cmd.action === "add" ? "ajaxcart.item.added" : "ajaxcart.item.removed";

                    EventBroker.publish(
                        isSuccess
                            ? msg
                            : "ajaxcart.error",
                        $.extend(cmd, { response: response })
                    );

                    return false;
                },

                error: function (jqXHR, textStatus, errorThrown)
                {
                    EventBroker.publish(
                        "ajaxcart.error",
                        $.extend(cmd, { response: { success: false, message: errorThrown } })
                    );
                },

                complete: function ()
                {
                    busy = false;
                    EventBroker.publish("ajaxcart.complete", cmd);
                }
            });

            // for stopping event propagation
            return false;
        }
    };

})(jQuery, this, document);
