/* Project: Online Inventory Management System  
Copyright (c) Vikram Singh Saini, Freelancer <vs00saini@gmail.com>
Tab: Supervisor--> Shelters
*/

var counter;
var showDiv = true;

$(function ()
{
    $("#btnShowNewProductDiv").click(ShowNHideDiv);
    
    $('#btnNewProductItem').click(addNewProductItem); // add new product item

    $('#btnSaveItem').click(saveProducts); // save all Products

    $("[data-original-title]").tooltip(); // show tooltip

    // Credit - http://code.webmonkey.uk.com/plugins/
    $('input[type=number]').numeric({ negative: false }); // restrict text instead of numbers

    $('input[type=image]').click(function () { return false; }); // prevent form submission

    SetAutocomplete();

});

// Run select2 for each select
function ApplySelect2()
{
    $('select').each(function ()
    {
        $(this).select2();
        $(".select2-search").remove();
    });
}

// Show and hide div accordingly
function ShowNHideDiv($event)
{
    $event.preventDefault();

    if (showDiv)
    {
        $("#newProductSpan").text('Hide this');
        $("#NewProductDiv").fadeIn(500);
        ApplySelect2();
        showDiv = false;
    } else
    {
        $("#newProductSpan").text('Add new shelter');
        $("#NewProductDiv").fadeOut(500);
        showDiv = true;
    }
}

// Add new product item dynamically
function addNewProductItem($event)
{
    $event.preventDefault();

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

    var productItems = selectItem + '&nbsp;&nbsp;';

    var quantity = '<input type="number" id="Quantity_' + counter + '" name="Quantity[' + counter + ']"  min="1" required="required" placeholder="Quantity" class="qtyWid"></td>&nbsp;';

    var button = '<td class="inline"><input type="image" src="' + imgUrl + '" name="saveForm" id="btnRemove_' + counter + '" data-original-title="Remove current item" formnovalidate onclick="return false;"></td></tr>';

    $(label + productItems + quantity + button).appendTo('#ProductsList');
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
function saveProducts()
{
    var inputs = [], flag = false;

    // Check if not empty value (another way if( $(this).val().length === 0)
    if ($(this).val())
    {
        $("#frmProduct select").each(function ()
        {
            if ($.inArray(this.value, inputs) != -1)
            {
                flag = true;
            }

            inputs.push(this.value);
        });
    }

    if (flag == true)
    {
        alert("Products with similar items cannot be added to database.");
        return false;
    }
    return true;
}

// Set autocomplete feature by typeahead
function SetAutocomplete()
{
    var shelterId = '#Product_0'; // Get id of textbox in which results to be shown
    var remUrl = "Supervisor/GetShelters?searchTerm=%QUERY";

    var shelterCols = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        hint: true,
        highlight: true,
        items: 5,
        minLength: 3,
        remote: remUrl
    });

    shelterCols.initialize();

    $(shelterId).typeahead(null, {
        name: 'shelterCols',
        displayKey: 'ShelterName',
        source: shelterCols.ttAdapter()
    });
}

// AJAX FUNCTIONALITIES
function ProductBegin()
{
    ProductLoader.Show();
}

function ProductSuccess()
{
    ProductLoader.Hide();

    if ($("#serverConfirm").length > 0)
    {
        gvShelters.PerformCallback();

        // Reset form back after saving
        this.form.reset();
    }
}

function ProductFailure(request, status, error)
{
    alert("Error occured while processing ajax request : " + request + "\n\nError Status: " + status + "\nError Detail: " + error);
}
