var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {

        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    _cart_quantity: $(item).val(),
                    _shopping_product: {
                        IdProduct: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { CartModels: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/Cart/ShowToCart";
                    }
                }
            })
        });

    }
}
cart.init();