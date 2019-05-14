/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Supervisor
*/

$(function ()
{
    var shopBar = $("#shopbar");

    if (shopBar.length)
    {
        var shopBarTop = shopBar.offset().top - parseFloat(shopBar.css('margin-top').replace(/auto/, 0));
        $(window).on("scroll resize", function (e)
        {
            var y = $(this).scrollTop();
            shopBar.toggleClass("sticky", y >= 155 /*shopBarTop*/);
        });
    }
});

// Loader for Tabs
function ShowLoader()
{
    LoadingPanel.Show();
}

function HideLoader()
{
    LoadingPanel.Hide();
}

function HandleError(jqXHR, textStatus, errorThrown)
{
    // var err = JSON.parse(xhr.responseText);

    ShowNotice(errorThrown, "error", true);
}

// Show pNotify for different notices.
function ShowNotice(nMsg,nType,nHide)
{
    var stackTopright = { "dir1": "down", "dir2": "left", "firstpos1": 60, "firstpos2": 115 };

    $.pnotify({
        text:nMsg,
        type: nType,
        animate_speed: 'fast',
        closer_hover: false,
        hide: nHide,
        sticker: false,
        stack: stackTopright,
    });
}