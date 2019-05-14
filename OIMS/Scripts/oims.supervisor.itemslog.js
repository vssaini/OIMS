/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Items Log
*/

var counter;
var showDiv = true;
var itemCols;

$(function ()
{
    $("#btnShowNewLogDiv").click(ShowNHideDiv);

    counter = 1;
    $('#btnNewItemLog').click(addNewItemLog); // add new item log

    $('#btnSaveItemLog').click(saveItemsLog); // save all items log

    $("[data-original-title]").tooltip(); // show tooltip

    // Credit - http://code.webmonkey.uk.com/plugins/
    $('input[type=number]').numeric({ negative: false }); // restrict text instead of numbers

    $('input[type=image]').click(function () { return false; }); // prevent form submission

    ApplySelect2();

});

// Show and hide div accordingly
function ShowNHideDiv($event)
{
    $event.preventDefault();

    if (showDiv)
    {
        $("#newItemSpan").text('Hide this');
        $("#NewLogDiv").fadeIn(500);
        showDiv = false;
    } else
    {
        $("#newItemSpan").text('Update item stock');
        $("#NewLogDiv").fadeOut(500);
        showDiv = true;
    }
}

// Add new log item dynamically
function addNewItemLog()
{
    // Url of image
    var websiteImagePath = $('#website-image-path').val();
    var imgUrl = websiteImagePath + "IconCross.png";

    var itemName = 'Item[' + counter + ']';  // Set item id

    // --------- START - Dropdown ---------

    // 1 -- Open select tag
    var selectItem = '<select id="' + 'Item_' + counter + '" name="' + itemName + '"class="ddlWid">';

    // 2 -- Add option values
    $("#Item_0 > option").each(function ()
    {
        selectItem += '<option value="' + this.value + '">' + this.text + '</option>';
    });

    // 3  -- Close select tag
    selectItem += '</select>';

    // --------- END - Dropdown ---------

    // Create dynamic path
    var label = '<tr><td class="padRight6"><label class="control-label" for="' + itemName + '">Select a item:</label>&nbsp;&nbsp;';

    var logItems = selectItem + '&nbsp;&nbsp;';

    var quantity = '<input type="number" id="Quantity_' + counter + '" name="Quantity[' + counter + ']"  min=".1" step="0.1" required="required" placeholder="Quantity" class="qtyWid"></td>&nbsp;';

    var button = '<td class="inline"><input type="image" src="' + imgUrl + '" name="saveForm" id="btnRemove_' + counter + '" data-original-title="Remove current item" formnovalidate onclick="return false;"></td></tr>';

    $(label + logItems + quantity + button).appendTo('#ItemsLogList');
    $("[data-original-title]").tooltip();
    ApplySelect2();
    removeRow();
    counter++;
    return false;
}

// For removing row from table
function removeRow()
{
    $("input[type=image]").click(function ()
    {
        $(this).tooltip('destroy'); // destroy tooltip. fixed bug

        var row = $(this).parent().parent(); // get row to remove

        row.fadeOut(500, function ()
        {
            row.remove();
            counter--;
        });
    });
}

// Allow saving unique input fields only
function saveItemsLog()
{
    var flag = false;

    $("#frmItemsLog input[type=number]").each(function ()
    {
        // Check if not empty value (another way if( $(this).val().length === 0)
        if ($(this).val())
        {
            flag = true;
        } else
        {
            flag = false;
        }
    });

    if (flag == false)
    {
        alert("Quantity for one of the items is not filled.");
        return false;
    }
    return true;
}

// Run select2 for each select
function ApplySelect2()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });
}

// AJAX FUNCTIONALITIES
function LogBegin()
{
    LogLoader.Show();
}

function LogSuccess()
{
    LogLoader.Hide();

    if ($("#serverConfirm").length > 0)
    {
        gvItemsLog.PerformCallback();

        // Reset form back after saving
        $('#frmItemsLog').each(function ()
        {
            this.reset();
        });
    }
}

function LogFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}
