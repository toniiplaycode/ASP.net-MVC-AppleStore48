var Detail = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $("btnAddToCart").off('click').on('click', function () {
            window.location.href = "/ShoppingCart/AddToCart"
        });
    }
}
Detail.init();
