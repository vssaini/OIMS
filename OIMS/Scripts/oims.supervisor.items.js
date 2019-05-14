/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Items
*/

var counter;
var showDiv = true;
var itemCols;

$(function ()
{
    $("#btnShowNewItemDiv").click(ShowNHideDiv);

    counter = 1;
    $('#btnNewItem').click(addNewItem); // add new item

    $('#btnSaveItem').click(saveItems); // save all items

    $("[data-original-title]").tooltip(); // show tooltip

    // Credit - http://code.webmonkey.uk.com/plugins/
    $('input[type=number]').numeric({ negative: false }); // restrict text instead of numbers

    $('input[type=image]').click(function () { return false; }); // prevent form submission

    SetAutocomplete();

});

// Show and hide div accordingly
function ShowNHideDiv($event)
{
    $event.preventDefault();

    if (showDiv)
    {
        $("#newItemSpan").text('Hide this');
        $("#NewItemDiv").fadeIn(500);
        showDiv = false;
    } else
    {
        $("#newItemSpan").text('Add new item');
        $("#NewItemDiv").fadeOut(500);
        showDiv = true;
    }
}

// Add new item dynamically
function addNewItem()
{
    // Url of image
    var websiteImagePath = $('#website-image-path').val();
    var imgUrl = websiteImagePath + "IconCross.png";

    var itemId = "ItemName_" + counter;

    // Create dynamic path
    var item = '<tr><td class="padRight6"><input id="' + itemId + '" name="ItemName[' + counter + ']" type="text" required="required" placeholder="Item name" autofocus="autofocus"></td>';

    var size = '<td class="padRight6"><input id="Size_' + counter + '" name="Size[' + counter + ']" type="text" required="required" placeholder="Item size"></td>';

    var marking = '<td class="padRight6"><input id="Marking_' + counter + '" name="Marking[' + counter + ']" type="text" required="required" placeholder="Item marking"></td>';

    var quantity = '<td class="padRight6"><input id="Quantity_' + counter + '" name="Quantity[' + counter + ']" type="number" min="1" required="required" placeholder="Quantity"></td>';

    var button = '<td class="inline"><input type="image" src="' + imgUrl + '" name="saveForm" id="btnRemove_' + counter + '" data-original-title="Remove current item" formnovalidate onclick="return false;"></td></tr>';

    $(item + size + marking + quantity + button).appendTo('#itemsList');
    $("[data-original-title]").tooltip();

    // Set search facility for each fur
    itemId = "#" + itemId;
    $(itemId).typeahead(null, {
        name: 'itemCols',
        displayKey: 'ItemName',
        source: itemCols.ttAdapter()
    });

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
function saveItems()
{
    var inputs = [], flag = false;

    // Get each input element whose name starts with 'ItemName'
    $('#frmItem input[name^=ItemName]').each(function ()
    {
        // Check if not empty value (another way if( $(this).val().length === 0)
        if ($(this).val())
        {
            // If value match with those in inputs
            if ($.inArray(this.value, inputs) != -1)
            {
                flag = true;
            }

            // add value to array
            inputs.push(this.value);
        }
    });

    if (flag == true)
    {
        alert("Items with same name cannot be added to database.");
        return false;
    }
    return true;
}

// Set autocomplete feature by typeahead
function SetAutocomplete()
{
    var itemId = '#ItemName_0';
    var remUrl = "Supervisor/GetItems?searchTerm=%QUERY";

    itemCols = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        hint: true,
        highlight: true,
        items: 5,
        minLength: 3,
        remote: remUrl
    });

    itemCols.initialize();

    $(itemId).typeahead(null, {
        name: 'itemCols',
        displayKey: 'ItemName',
        source: itemCols.ttAdapter()
    });
}

// AJAX FUNCTIONALITIES
function ItemBegin()
{
    ItemLoader.Show();
}

function ItemSuccess()
{
    ItemLoader.Hide();

    if ($("#serverConfirm").length > 0)
    {
        gvItems.PerformCallback();

        // Reset form back after saving
        $('#frmItem').each(function ()
        {
            this.reset();
        });
    }
}

function ItemFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}
