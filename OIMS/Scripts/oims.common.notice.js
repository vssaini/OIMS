/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Notification and Functions extension
*/

(function ($, window, document, undefined)
{
    window.setLocation = function (url)
    {
        window.location.href = url;
    };

    window.displayAjaxLoading = function (display)
    {
        if ($.throbber === undefined)
            return;

        if (display)
        {
            $.throbber.show({ speed: 50, white: true });
        } else
        {
            $.throbber.hide();
        }
    };

    window.displayNotification = function (message, type, sticky, delay)
    {
        if (window.EventBroker === undefined || window._ === undefined)
            return;

        var notify = function (msg)
        {
            EventBroker.publish("message", {
                text: msg,
                type: type,
                delay: delay || 5000,
                hide: !sticky
            });
        };

        if (_.isArray(message))
        {
            $.each(message, function (i, val)
            {
                notify(val);
            });
        } else
        {
            notify(message);
        }
    };

    window.htmlEncode = function (value)
    {
        return $('<div/>').text(value).html();
    };

    window.htmlDecode = function (value)
    {
        return $('<div/>').html(value).text();
    };

    // global notification subscriber
    if (window.EventBroker && window._ && $.pnotify) {
        //var stack_bottomright = { "dir1": "up", "dir2": "left", "firstpos1": 25, "firstpos2": 25 };
        var stack_topright = { "dir1": "down", "dir2": "left", "firstpos1": 60, "firstpos2": 115 };
        EventBroker.subscribe("message", function(message, data) {
            var opts = _.isString(data) ? { text: data } : data;

            opts.stack = stack_topright;
            //opts.addclass = "stack-bottomright";

            $.pnotify(opts);
        });
    } 

    // on document ready
    $(function ()
    {
        if (!Modernizr.csstransitions)
        {
            $.fn.transition = $.fn.animate;
        }

        // adjust pnotify global defaults
        if ($.pnotify)
        {
            $.extend($.pnotify.defaults, {
                history: false,
                animate_speed: "fast",
                styling: 'bootstrap',
                sticker: false
            });

            // intercept window.alert with pnotify
            window.alert = function (message)
            {
                if (message == null || message.length <= 0)
                    return;

                $.pnotify({
                    text: message,
                    type: "info",
                    animate_speed: 'fast',
                    closer_hover: false,
                    stack: false,
                    before_open: function (pnotify)
                    {
                        // Position this notice in the center of the screen.
                        pnotify.css({
                            "top": ($(window).height() / 2) - (pnotify.height() / 2),
                            "left": ($(window).width() / 2) - (pnotify.width() / 2)
                        });
                    }
                });
            };
        }
    });

})(jQuery, this, document);


/* --------------------------------------------------------------
                       jquery.utils.js
-------------------------------------------------------------- */
(function ($)
{

    $.fn.extend({

        /*
            Binds a simple JSON object (no collection) to a set of html elements
            defining the 'data-bind-to' attribute
        */
        bindData: function (data, options)
        {
            var defaults = {
                childrenOnly: false,
                includeSelf: false,
                showFalsy: false,
                fade: false
            };
            var opts = $.extend(defaults, options);

            return this.each(function ()
            {
                var el = $(this);

                var elems = el.find(opts.childrenOnly ? '>[data-bind-to]' : '[data-bind-to]');
                if (opts.includeSelf)
                    elems = elems.andSelf();

                elems.each(function ()
                {
                    var elem = $(this);
                    var val = data[elem.data("bind-to")];
                    if (val !== undefined)
                    {

                        if (opts.fade)
                        {
                            elem.fadeOut(400, function ()
                            {
                                elem.html(val);
                                elem.fadeIn(400);
                            });
                        }
                        else
                        {
                            elem.html(val);
                        }

                        if (!opts.showFalsy && !val)
                        {
                            // it's falsy, so hide it
                            elem.hide();
                        }
                        else
                        {
                            elem.show();
                        }
                    }
                });
            });
        }

    }); // $.fn.extend


})(jQuery);

/* --------------------------------------------------------------
               Events Subscriber For Requests
-------------------------------------------------------------- */

var ReqCol = (function ($)
{
    function notify(resp)
    {
        if (resp && resp.message)
        {
            displayNotification(resp.message, !!(resp.success) ? "success" : "error");
        }
    }

    EventBroker.subscribe("reqSummary.beforeSend", function (msg, data)
    {
        el.throbber({ white: true, small: true });

        var ele = $('#itemBox');
        if (ele)
        {
            ele.throbber({ white: true, small: true });
        }

    });

    EventBroker.subscribe("reqSummary.showed", function (msg, data)
    {
        el.html(data);

        // If function exists
        if ($.isFunction(window.Select2Misc))
        {
            Select2Misc();
        }
    });

    EventBroker.subscribe("reqSummary.saved", function (msg, data)
    {
        el.html("");
        notify(data);
        
        //Select2Misc();
        
        gvRequests.Refresh(); // Refresh grid to show changed status
    });

    EventBroker.subscribe("reqSummary.error", function (msg, data)
    {
        el.html("");

        //ShowNotice(data.response.message, "error", false);
        notify(data.response);
    });

    EventBroker.subscribe("reqSummary.complete", function (msg, data)
    {
        //
    });

})(jQuery);

