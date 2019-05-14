/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Requestor -- shopbar
*/

$(function ()
{
    var shopBar = $("#shopbar");

    $(document).on("click", ShopBar.closeFlyouts);
    $(document).on("click", ".shopbar-flyout, .shopbar-flyout-container", function (e) { e.stopPropagation(); });

    shopBar.find(".shopbar-button").on("click", function ()
    {
        var el = $(this);

        // For logout button
        var type = el.data("type");
        if (type === "logout")
        {
            return true;
        }

        // For cart button
        if (el.hasClass("no-drop"))
        {
            return true;
        }

        var tool = el.parent();

        // If already open, close it and return
        var wasOpened = tool.hasClass("open");
        ShopBar.closeFlyouts();
        if (wasOpened)
        {
            return false;
        }

        // Else open it
        tool.addClass("open");

        // Not opened list
        if (!tool.hasClass("loaded") && !tool.hasClass("loading"))
        {
            // load it!
            ShopBar.loadHtml(tool, false /* keepOpen */);
        }

        return false;
    });

});

// ShopBar OBJECT - ENCAPSULATING DIFFERENT FUNCTIONS
var ShopBar = (function ($)
{
    var shopBar = $("#shopbar");
    var tools = { "cart": $("#shopbar-cart") };

    var wasSticky = false;

    /* data:
            type: cart
            action: add | remove
            url: string,
            data: additional routeData
            src: <Element> --> the srcElement which kicked off the action, can be null
            response: { <jq ajax response> }
    */

    function notify(resp)
    {
        if (resp && resp.message)
        {
            displayNotification(resp.message, !!(resp.success) ? "success" : "error");
        }
    }

    EventBroker.subscribe("ajaxcart.beforeSend", function (msg, data)
    {
        RequestLoader.Show();
    });

    EventBroker.subscribe("ajaxcart.save.beforeSend", function (msg, data)
    {
        $(".save-cart-link").attr('disabled', 'disabled');
    });
    
    EventBroker.subscribe("ajaxcart.item.adding", function (msg, data)
    {
        // show transfer effect
        var tool = tools[data.type];

        ShopBar.closeFlyouts();

        if (data.src)
        {
            // "guess" the closest transferrable element
            var transferSrc = $(data.src).closest(".item-box, [data-transfer-src]");
            if (!transferSrc.length)
            {
                // ... couldn't find any? then take the src itself (could be a bit small though)
                transferSrc = data.src.parent() || data.src;
            }

            wasSticky = shopBar.hasClass("sticky");
            shopBar.removeClass("sticky");

            transferSrc.stop(true, true).effect("transfer", { to: tool.find(".shopbar-button-icon"), easing: "easeOutQuad", className: "transfer" }, 800, function ()
            {
                if (wasSticky)
                {
                    _.delay(function () { shopBar.addClass("sticky"); }, 1000);
                }
            });
        }
    });

    EventBroker.subscribe("ajaxcart.item.added", function (msg, data)
    {
        var tool = tools[data.type];

        if (!tool.hasClass("loading"))
        { // DON'T do UI stuff during loading!
            tool.removeClass("loaded");

            ShopBar.loadSummary(tool, true /*fade*/, function (resultData)
            {
                //// update the badge in AccountDropdown also
                //updateQtyBadge($("#topcartlink .cart-qty"), resultData.TotalProducts);
            });
        }

        // Reset form
        $('#form-NewRequest')[0].reset();

        $('select').each(function ()
        {
            $(this).select2('val', $('.ddlWid option:first-child').val());
        });
        
        notify(data.response);
    });

    EventBroker.subscribe("ajaxcart.item.removed", function (msg, data)
    {
        var tool = tools[data.type];
        var items = $(data.src).closest(".items");
        if (items.length)
        {
            // deletion occuured within the dropdown itself, so reload html!
            items.throbber({ white: true, small: true });
            ShopBar.loadHtml(tool, true, function ()
            {
                //alert(data.response.message);
            });
        }
        else
        {
            tool.removeClass("loaded loading");
        }
        ShopBar.loadSummary(tool, false /*fade*/, function (resultData)
        {
            /*// update the badge in AccountDropdown also
            updateQtyBadge($("#topcartlink .cart-qty"), resultData.TotalProducts);*/
        });
    });

    EventBroker.subscribe("ajaxcart.item.saved", function (msg, data)
    {
        var tool = tools[data.type];
        var items = $('.shopbar-flyout-inner').find("ul");
        
        if (items.length)
        {
            // reload html after saving and deleting cart items
            items.throbber({ white: true, small: true });
            ShopBar.loadHtml(tool, true, function ()
            {
                notify(data.response); // Show saved notice
                ShopBar.closeFlyouts(); // Close flyouts
                $(".save-cart-link").removeAttr('disabled'); // Re-enable button
            });
        }
        else
        {
            tool.removeClass("loaded loading");
        }

        ShopBar.loadSummary(tool, false /*fade*/, function (resultData)
        {
            /*// update the badge in AccountDropdown also
            updateQtyBadge($("#topcartlink .cart-qty"), resultData.TotalProducts);*/
        });
    });

    EventBroker.subscribe("ajaxcart.error", function (msg, data) {
        
        notify(data.response);
    });

    EventBroker.subscribe("ajaxcart.complete", function (msg, data)
    {
        RequestLoader.Hide();
    });

    return {
        init: function (opts)
        {
            // [...]
        },

        closeFlyouts: function ()
        {
            $.each(tools, function (name, val)
            {
                val.removeClass("open");
            });
        },

        loadSummary: function (type, fade, fn /* successCallBack */)
        {
            var tool = _.isString(type) ? tools[type] : type;
            if (!tool) return;

            var button = tool.find(".shopbar-button");
            if (button.data("summary-href"))
            {
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: button.data("summary-href"),
                    success: function (data)
                    {
                        button.bindData(data, { fade: fade });
                        if (_.isFunction(fn))
                        {
                            fn.call(this, data);
                        }
                    },
                    complete: function (jqXHR, textStatus)
                    {
                        // TODO
                    }
                });
            }
        },

        loadHtml: function (type, keepOpen, fn /* completeCallback */)
        {
            var tool = _.isString(type) ? tools[type] : type;
            if (!tool) return;

            if (!keepOpen)
            {
                tool.removeClass("loaded").addClass("loading");
            }
            var cnt = tool.find('.shopbar-flyout');

            $.ajax({
                cache: false,
                type: "POST",
                url: cnt.data("href"),
                success: function (data)
                {
                    cnt.empty().html(data);
                },
                complete: function (jqXHR, textStatus)
                {
                    tool.removeClass("loading").addClass("loaded");
                    if (_.isFunction(fn))
                    {
                        fn.apply(this);
                    }
                }
            });
        }

        // ...
    };

})(jQuery)