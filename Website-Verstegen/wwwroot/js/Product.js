function addProductDetailsInputField() {
    var details_counter = $('.product_detail').length + 1;
    
    $(".product_details").append('<label class="product_detail control-label">Add product details to your product.</label>');
    $(".product_details").append('<input class="form-control" type="text" name="product_detail_' + details_counter + '" placeholder="Keep in the fridge.." />');
    
    $("#product_details_counter").val(details_counter);
}