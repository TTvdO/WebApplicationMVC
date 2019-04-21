$(document).ready(function () {
    $('#ingredients').chosen();
});

function removeIngredient() {

    var id = $('.derp').find(":selected").val();
    
    var api = '/api/Ingredients/' + id;

    jQuery.ajax({
        type: 'Delete',
        url: api,
        data: JSON.stringify({
            "Name": id,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            $(".ingredients option[value='"+id+"']").remove();
            $('.ingredients').trigger("chosen:updated");
           
        },
        error: function (XMLHttpReq, status, errorThrown) {

            if (XMLHttpReq.responseJSON.error == 'inUse') {
                alert(XMLHttpReq.responseJSON.message);
            }

            jQuery('#errorMelding').html(errorThrown);
        }
    });

}

function addIngredient() {

    var api = '/api/ingredients';

    var Name = $("#Name").val();
    var Unit = $("#Unit").val();
    var Amount = $("#Amount").val();

    jQuery.ajax({
        type: 'POST',
        url: api,
        data: JSON.stringify({
            "Name": Name,
            "Unit": Unit,
            "Amount": Amount
        }),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {

            $(".ingredients").append(new Option(data.name + ' ' + data.amount + '' + data.unit, data.id, true, true));
            $('.ingredients').trigger("chosen:updated");

            $("#Name").val(null);
            $("#Unit").val(null);
            $("#Amount").val(null);
        },
        error: function (XMLHttpReq, status, errorThrown) {
            jQuery('#errorMelding').html(errorThrown);
        }
    });
}

function addPreparationStep() {
    var details_counter = $('.product_detail').length + 1;

    $(".product_details").append('<label class="product_detail control-label">Add preparation steps to your recipe</label>');
    $(".product_details").append('<input class="form-control" type="text" name="product_detail_' + details_counter + '" placeholder="Keep in the fridge.." />');

    $("#product_details_counter").val(details_counter);
}