var ShoppingCart = [];

var subtotal = 0;
var vat = 0;
var total = 0;
var no = 1;
var supplierCouponId = 0;
var totalAfterCoupon = 0;
var percent = 0;

$(window).scroll(function () {

    var html = '';
    var categoryId = $(".category-swiper").find(".selected-category a").attr("data-attr");


    if ($(window).height() + $(window).scrollTop() > $(document).height() - 0.9) {
        no += 1;
        $.ajax({
            type: "POST",
            url: "/Restaurant/Shop/Pagination",
            data: {
                id: supplierId,
                categoryId: categoryId,
                pgno: no
            },
            success: function (result) {

                if (result.success) {
                    $.each(result.items, function (k, v) {
                        var discount = '';
                        var price = '';
                        var regular = '';
                        

                        if (v.salePrice > 0) {
                            discount = `<a href="javacript:void(0);" class="label label-danger label-inline">${v.discount}% OFF</a>`;
                            price = `<div class="price item-price">AED ${(v.salePrice).toFixed(2)}</div>`
                            regular = ` <lable class="saleprice opacity-70"><del>AED ${(v.regularPrice).toFixed(2)}</del></lable>`
                        }

                        else {
                            discount = `<a href="javacript:void(0);"></a>`
                            price = `<div class="price item-price">AED ${v.regularPrice}</div>`
                        }


                        html += `<div class="col-md-2 mb-lg-5 product-list">
                                                        <div class="product-item" id="item-${v.id}">
                                                            <div class="product-thumb">
                                                                <a onclick="CreateBasket(this, ${v.id})"><img id="Product-Image" src="${v.image}" alt="product"></a>
                                                                <div class="batch" style="display:none">
                                                                    <a href="javascript:void(0)" onclick="dcrQty(this,${v.id})" class="btn btn-icon btn-dark btn-sm">
                                                                        <i class="fa fa-minus fa-sm"></i>
                                                                    </a>
                                                                    <a href="javascript:void(0)" class="btn btn-icon btn-light-dark btn-sm item-quantity">0</a>
                                                                    <a href="javascript:void(0)" onclick="incQty(this,${v.id})" class="btn btn-icon btn-danger btn-sm">
                                                                        <i class="fa fa-plus fa-sm"></i>
                                                                    </a>


                                                                </div>

                                                            </div>
                                                            <div class="product-content">
                                                                <div class="d-flex justify-content-between">
                                                                      ${discount}
                                                                    <a href="javacript:void(0);" onclick="OpenModelPopup(this, '/Restaurant/Shop/AddToCart/${v.id}')" class="btn btn-icon btn-light-facebook btn-circle btn-sm mr-2">
                                                                        <i class="fa fa-info fa-sm"></i>
                                                                    </a>
                                                                </div>

                                                                <p class="shop-name"><strong class="font-size-xs">Sold by </strong><span class="theme-red">${supplierName}</span></p>
                                                                <div class="d-flex justify-content-between">
                                                                    <h6 class="item-title">${v.name}</h6>
                                                                    ${regular}
                                                                </div>

                                                                <div class="d-flex justify-content-between align-items-center">
                                                                    <label class="opacity-70 mb-0 item-packaging">${v.packaging}</label>
                                                                   ${price}



                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>`

                    });

                    $(".item-list").append(html);

                }

            }
        });
    }

});


$(document).ready(function () {
    var cart = localStorage.getItem("cart");
    if (cart) {
        ShoppingCart = JSON.parse(cart);
    }
    UpdateCartCounter();
    FillCart();
});

function UpdateCartCounter() {
    if (ShoppingCart.length > 0) {

        $('.shop-alert').fadeIn();

        $('.shop-alert .alert-price').html('AED ' + parseFloat(ShoppingCart.sum("Price")).toFixed(2));
        $('.shop-alert .alert-items').html(`${ShoppingCart.sum("Quantity")} Items`);

        $('.amount-tile').after(function () {

            subtotal = ShoppingCart.sum("Price");
            subtotal = parseFloat(subtotal);
            couponprice = 0;
            vat = (subtotal * taxPercent) / 100;
            total = subtotal + deliveryCharges + vat;
            if ($(".coupon-div").is(":visible")) {
                if (percent > 0) {
                    couponprice = (subtotal * percent) / 100;
                }
                else {
                    couponprice = $(".coupon-amount").text().split(" ")[0];
                }

                total = total - parseFloat(couponprice);
                $('.order-total').text((total).toFixed(2) + 'AED');
            }

            else {
                $('.order-total').text((total).toFixed(2) + 'AED');
            }
            $('.order-delivery').text((deliveryCharges).toFixed(2) + ' AED');
            $('.order-subtotal').text((subtotal).toFixed(2) + ' AED');
            $('.order-vat').text((vat).toFixed(2) + ' AED');
            $('.coupon-amount').text((couponprice).toFixed(2) + ' AED');



        });
    } else {
        $('.order-subtotal').text('0.00 AED');
        $('.order-total').text('0.00 AED');
        $('.order-vat').text('0.00 AED');
        $('.order-delivery').text('0.00 AED');
        $('.shop-alert').fadeOut();
    }
}

function FillCart() {

    if (ShoppingCart.length > 0) {
        for (var i = 0; i < ShoppingCart.length; i++) {
            UpdateCart(ShoppingCart[i]);
        }

    }
    else {
        $('.batch').hide();

    }
}

function FillOrderCart() {

    if (ShoppingCart.length > 0) {
        for (var i = 0; i < ShoppingCart.length; i++) {
            BindOrderCart(ShoppingCart[i]);
        }
    }

}


function UpdateTotal() {
    if (ShoppingCart.length > 0) {
        $('.site-cart .total .money').html('AED ' + parseFloat(ShoppingCart.sum("Price")).toFixed(2));

        $('.sitebar-drawar .cart-price').html('AED ' + parseFloat(ShoppingCart.sum("Price")).toFixed(2));
    } else {
        $('.site-cart .total .money').html('AED 0.0');

        $('.sitebar-drawar .cart-price').html('AED 0.0');
    }
}

function UpdateCart(Product) {

    var elem = $("#item-" + Product.ProductId);
    $(elem).find(".item-quantity").text(Product.Quantity);
    $(elem).find(".batch").show();
}


function UpdateProductDirectAddToCartSection(Product) {

    $(`.product-item[product-id=${Product.ProductID}]`).find('.direct-add-to-cart-btn').hide();

    $(`.product-item[product-id=${Product.ProductID}]`).find('.price-increase-decrese-group').addClass('d-flex');

    $(`.product-item[product-id=${Product.ProductID}]`).find('.price-increase-decrese-group input[name=quantity]').val(Product.Quantity);

}

function AddToCart(Product) {

    let TempItemDetails = Object.assign({}, Product);

    var product = ShoppingCart.filter(function (obj) {
        return obj.ProductId == Product.ProductId;
    });

    if (product.length > 0) {
        product[0].Quantity += 1;
        product[0].Price += Product.Price;
        ShoppingCart.map((obj) => product.find((o) => o.ProductId === obj.ProductId) ?? obj);

    }
    else {

        ShoppingCart.push(TempItemDetails);
    }


    localStorage.setItem("cart", JSON.stringify(ShoppingCart));


}

//function RemoveFromCart(elem, id) {


//    $('.CartProduct' + id).fadeOut(300, function () { $(this).remove(); });

//    let item = ShoppingCart.find(function (obj) {
//        return obj.RowId == id;
//    });

//    ShoppingCart = ShoppingCart.filter(function (obj) {
//        return obj.RowId !== id;
//    });

//    localStorage.setItem("cart", JSON.stringify(ShoppingCart));
//    UpdateCartCounter();
//    UpdateTotal();

//    //if (typeof RemoveCartItem != "undefined") {
//    //    $('.cart__row[id=' + id + ']').fadeOut(300, function () { $(this).remove(); });
//    //    $('#Subtotal').html('AED ' + parseFloat(ShoppingCart.sum("Price")));
//    //}


//    //if (typeof RemoveCheckoutCartItem != "undefined") {
//    //    RemoveCheckoutCartItem(id);
//    //}

//    //var url = window.location.href;
//    //var arrurl = url.split("/");
//    //var lastpath = arrurl.pop();
//    //var restpath = arrurl.join("/");
//    //if (restpath.includes("product")) {
//    //    $.ajax({
//    //        type: 'GET',
//    //        url: '/en/products/' + GetURLParameter(),
//    //        success: function (response) {
//    //            if (response.success) {
//    //                console.log(response.data);
//    //                details = response.data;
//    //                Product.ProductID = details.ID;
//    //                Product.Title = details.Title;
//    //                Product.Slug = details.SKU
//    //                Product.Thumbnail = details.Thumbnail;
//    //                Product.VendorID = details.Vendor.ID;

//    //                if (details.Type == '1' || details.Type == 'Simple') {
//    //                    BindSimpleProduct(details);
//    //                }
//    //                else {
//    //                    $('#productAttributes .swatch input').change(function () {
//    //                        selectedAttributes = [];

//    //                        $('#productAttributes .swatch input:checked').each(function () {
//    //                            selectedAttributes.push($(this).attr('product-attribute-id'));
//    //                        });

//    //                        var variation = details.Variations.find(function (obj) {
//    //                            return isEqual(obj.Attributes, selectedAttributes);
//    //                        });

//    //                        if (variation) {
//    //                            BindVariableProduct(variation);

//    //                            if (variation.Stock) {
//    //                                $('#btnAddToCart').prop('disabled', false);
//    //                                $('#btnBuyNow').prop('disabled', false);
//    //                            }

//    //                        } else {
//    //                            $('#btnAddToCart').prop('disabled', true);
//    //                            $('#btnBuyNow').prop('disabled', true);
//    //                        }
//    //                    });

//    //                    $('.swatch').each(function () { $(this).find('input:first').prop('checked', true) });
//    //                    //$('.swatch').each(function () { $(this).find('input:first').trigger('click') });
//    //                    $('.swatch input:first').trigger('change');
//    //                }
//    //            } else { }
//    //        }
//    //    });
//    //    var CheckCart = ShoppingCart.find(function (obj) {
//    //        return obj.ProductID == Product.ProductID
//    //            && obj.ProductVaraiationID == Product.ProductVaraiationID
//    //            && isEqual(obj.Attributes, Product.Attributes);
//    //    });
//    //    if (CheckCart) {
//    //        if (CheckCart.Quantity) {
//    //            variation.Stock = variation.Stock - CheckCart.Quantity;
//    //            $('#cartStock').html(CheckCart.Quantity + ' in Cart').show();
//    //        }
//    //    }
//    //}

//    //if (typeof CheckIsSoldIndividually != 'undefined') {
//    //    CheckIsSoldIndividually(ShoppingCart.ProductID, ShoppingCart.ProductVaraiationID);
//    //}

//    //if (item) {
//    //    $(`.product-item[product-id=${item.ProductID}]`).find('.direct-add-to-cart-btn').show();

//    //    $(`.product-item[product-id=${item.ProductID}]`).find('.price-increase-decrese-group').addClass('d-none').removeClass('d-flex');

//    //    $(`.product-item[product-id=${item.ProductID}]`).find('.price-increase-decrese-group input[name=quantity]').val('');
//    //}
//}

function GetAttributeString(array) {
    var Attributes = '';
    array.forEach(function (k, v) {
        if (k.Name.toUpperCase() == "COLOR") {
            Attributes += k.Name + ' : <div class="dot" style="background:' + k.Value + '"></div> /';
        } else {
            Attributes += k.Name + ' : <div class="">' + k.Value + '</div> /';
        }
    });

    return Attributes.slice(0, -1);
}

Array.prototype.sum = function (prop) {
    var total = 0
    for (var i = 0, _len = this.length; i < _len; i++) {
        total += this[i][prop]
    }
    return total
}

var isEqual = function (value, other) {

    // Get the value type
    var type = Object.prototype.toString.call(value);

    // If the two objects are not the same type, return false
    if (type !== Object.prototype.toString.call(other)) return false;

    // If items are not an object or array, return false
    if (['[object Array]', '[object Object]'].indexOf(type) < 0) return false;

    // Compare the length of the length of the two items
    var valueLen = type === '[object Array]' ? value.length : Object.keys(value).length;
    var otherLen = type === '[object Array]' ? other.length : Object.keys(other).length;
    if (valueLen !== otherLen) return false;

    // Compare two items
    var compare = function (item1, item2) {

        // Get the object type
        var itemType = Object.prototype.toString.call(item1);

        // If an object or array, compare recursively
        if (['[object Array]', '[object Object]'].indexOf(itemType) >= 0) {
            if (!isEqual(item1, item2)) return false;
        }

        // Otherwise, do a simple comparison
        else {

            // If the two items are not the same type, return false
            if (itemType !== Object.prototype.toString.call(item2)) return false;

            // Else if it's a function, convert to a string and compare
            // Otherwise, just compare
            if (itemType === '[object Function]') {
                if (item1.toString() !== item2.toString()) return false;
            } else {
                if (item1 !== item2) return false;
            }

        }
    };

    // Compare properties
    if (type === '[object Array]') {
        for (var i = 0; i < valueLen; i++) {
            if (compare(value[i], other[i]) === false) return false;
        }
    } else {
        for (var key in value) {
            if (value.hasOwnProperty(key)) {
                if (compare(value[key], other[key]) === false) return false;
            }
        }
    }

    // If nothing failed, return true
    return true;

};


function ProductsByName(e) {

    $("#notFound").remove();
    $(".product-list").show();
    var text = $(e).val();
    if (text && text !== "") {

        $('.product-list').hide();

        $(".product-list").find('.item-title:contains("' + text + '")').closest('.product-list').fadeIn();
        if ($(".product-list").find('.item-title:contains("' + text + '")').length == 0) {
            $(".item-list").append(`  <div class="col-md-12 " id="notFound">
                                                <div id="notAvalaible" class="text-center">
                                                    <img style="width:10%" src="/Images/Icons/NotFound.png" />
                                                    <h4><em>Item <span style="color: #f55050">not</span> found</em> </h4>
                                                </div>
                                            </div>`);
        }

    }

    else {

        $('.product-list').show();
    }
};

function RemoveCoupon(e) {

    var couponprice = $(".coupon-amount").text();
    total = total + parseInt(couponprice);
    $(".order-total").text(total.toFixed(2));
    $(".coupon-div").hide();
    $('#remove-coupon').hide();
    $('#couponLabel').show();
    supplierCouponId = 0;
}



function CouponValidate(e, id) {

    var days = $(e).find("#remainingDays").attr("data-attr");
    if (parseInt(days) > 0) {
        $.ajax({
            url: '/Restaurant/Shop/CouponValidate/' + id,
            type: 'Get',
            success: function (response) {
                if (response.success) {
                    
                    if (total <= response.maxAmount || response.percent == false) {
                        totalAfterCoupon = 0;

                        if (response.percent) {
                            totalAfterCoupon = subtotal * response.amount / 100;
                            percent = response.amount;
                        }
                        else {
                            totalAfterCoupon = response.amount;
                        }

                        if (total < totalAfterCoupon) {
                            toastr.error("Order amount is lower for this coupon !")
                            return false;
                        }

                        total = total - totalAfterCoupon;
                        $(".order-total").text(total.toFixed(2));
                        $(".coupon-amount").text(totalAfterCoupon.toFixed(2) + " AED");
                        $(".coupon-basket").find(".coupon-code").text("Coupon (" + response.couponCode + ")");
                        $(".coupon-div").show();
                        $('#myModal').modal('hide');
                        $('#couponLabel').hide();
                        $('#remove-coupon').show();
                        supplierCouponId = id;
                        toastr.success("Coupon is redeemed!")
                    }
                    else {
                        toastr.error("Order amount is higher for this coupon !")
                    }


                }

                else {
                    toastr.error("Somthing went wrong")
                }

            },

        });
    }
    else {
        toastr.error("Validity of this coupon is expired !");
    }


}

function ProductsByCategory(e, elem) {

    
    var html = '';

    $.ajax({
        url: '/Restaurant/Shop/GetProductByCategory',
        type: 'POST',
        data: {
            "SupplierId": supplierId,
            "CategoryId": elem
        },
        success: function (result) {

            if (result.success) {
                no = 1;
                $(".category-swiper").find(".selected-category").removeClass('selected-category');
                $(e).parent().addClass("selected-category");

                $(".item-list").empty();
                $.each(result.items, function (k, v) {
                    var discount = '';
                    var price = '';
                    var regular = '';
                    

                    if (v.salePrice > 0) {
                        discount = `<a href="javacript:void(0);" class="label label-danger label-inline">${v.discount}% OFF</a>`;
                        price = `<div class="price item-price">AED ${(v.salePrice).toFixed(2)}</div>`
                        regular = ` <lable class="saleprice opacity-70"><del>AED ${(v.regularPrice).toFixed(2)}</del></lable>`
                    }

                    else {
                        discount = `<a href="javacript:void(0);"></a>`
                        price = `<div class="price item-price">AED ${v.regularPrice}</div>`
                    }


                    html += `<div class="col-md-2 mb-lg-5 product-list">
                                                        <div class="product-item" id="item-${v.id}">
                                                            <div class="product-thumb">
                                                                <a onclick="CreateBasket(this, ${v.id})"><img id="Product-Image" src="${v.image}" alt="product"></a>
                                                                <div class="batch" style="display:none">
                                                                    <a href="javascript:void(0)" onclick="dcrQty(this,${v.id})" class="btn btn-icon btn-dark btn-sm">
                                                                        <i class="fa fa-minus fa-sm"></i>
                                                                    </a>
                                                                    <a href="javascript:void(0)" class="btn btn-icon btn-light-dark btn-sm item-quantity">0</a>
                                                                    <a href="javascript:void(0)" onclick="incQty(this,${v.id})" class="btn btn-icon btn-danger btn-sm">
                                                                        <i class="fa fa-plus fa-sm"></i>
                                                                    </a>


                                                                </div>

                                                            </div>
                                                            <div class="product-content">
                                                                <div class="d-flex justify-content-between">
                                                                      ${discount}
                                                                    <a href="javacript:void(0);" onclick="OpenModelPopup(this, '/Restaurant/Shop/AddToCart/${v.id}')" class="btn btn-icon btn-light-facebook btn-circle btn-sm mr-2">
                                                                        <i class="fa fa-info fa-sm"></i>
                                                                    </a>
                                                                </div>

                                                                <p class="shop-name"><strong class="font-size-xs">Sold by </strong><span class="theme-red">${supplierName}</span></p>
                                                                <div class="d-flex justify-content-between">
                                                                    <h6 class="item-title">${v.name}</h6>
                                                                    ${regular}
                                                                </div>

                                                                <div class="d-flex justify-content-between align-items-center">
                                                                    <label class="opacity-70 mb-0 item-packaging">${v.packaging}</label>
                                                                   ${price}



                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>`

                });

                $(".item-list").append(html);


            }
        },

    });

}

function BindOrderCart(Item) {


    var html = `  <div class="col-md-12 mt-8 basket-product-tile">

                                                <div class="product-item">
                                                    <div class="d-flex">
                                                        <div class="flex-shrink-0 mr-7">
                                                            <div class="basket-product-image">
                                                                <img alt="Pic" src="${Item.Image}" />
                                                            </div>
                                                        </div>
                                                        <div class="flex-grow-1">

                                                            <div class="row align-items-center justify-content-between flex-wrap">
                                                                <!--begin::User-->

                                                                <div class="col-md-9 col-sm-8">
                                                                    <a href="javascript:void(0);" class="d-flex align-items-center text-dark text-hover-primary font-size-h6-sm font-weight-bold mr-3">${Item.Name}</a>
                                                                 
                                                                    <a href="javascript:void(0);" class="opacity-70 text-dark">${Item.Packaging}</a>
                                                                    <div class="d-flex flex-wrap">
                                                                        <div class="font-weight-bolder mt-8 item-price">AED ${Item.ActualPrice}</div>
                                                                    </div>
                                                                    
                                                                   
                                                                </div>



                                                                <div class="col-md-3 col-sm-4">
                                                                    <div class="d-flex">
                                                                        <a href="javascript:void(0)" onclick="dcrQty(this,${Item.ProductId})" class="btn btn-icon btn-dark mr-1">
                                                                            <i class="fa fa-minus "></i>
                                                                        </a>
                                                                        <a href="javascript:void(0)" class="btn btn-icon btn-light-dark mr-1 item-quantity">${Item.Quantity}</a>
                                                                        <a href="javascript:void(0)" onclick="incQty(this,${Item.ProductId})" class="btn btn-icon btn-danger">
                                                                            <i class="fa fa-plus"></i>
                                                                        </a>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="separator separator-solid my-1"></div>

                                            </div>`;

    $(".order-cart").append(html);
};


function CreateBasket(e, id, modal = false) {

    var productTile = $(e).closest('#item-' + id);
    if ($(productTile).find(".batch-stock").length > 0) {
        toastr.error("Sorry! this product is out of stock ...")
    }

    else {
        var checkquantity = $(productTile).find(".item-quantity").text();
        if (parseInt(checkquantity) == 0) {

            var sub = $(productTile).find(".item-price").html();
            var price = sub.toString().split(" ")[1];

            Item.ProductId = id;
            Item.Name = $(productTile).find(".item-title").text();
            Item.Packaging = $(productTile).find(".item-packaging").text();
            Item.Price = parseFloat(price);
            Item.Quantity = 1;
            Item.ActualPrice = parseFloat(price).toFixed(2);
            Item.Image = $(productTile).find("#Product-Image").attr("src");
            AddToCart(Item);
            FillCart();
            UpdateCartCounter();
            if (modal) {

                $(".modal-cart-btn").hide();
                $(".modal-cart-inc").show();
                $(productTile).find(".item-quantity").text(1);

            }
        };

    }


}



function incQty(e, id, modal = false) {
    
    var elem = $(e).closest(".product-item");

    var quantity = $(elem).find('.item-quantity').text();
    var newPrice = $(elem).find('.item-price').text();
    var product = ShoppingCart.filter(function (obj) {
        return obj.ProductId == id;
    });

    product[0].Quantity += 1;
    product[0].Price += parseFloat(newPrice.split(" ")[1]);

    ShoppingCart.map((obj) => product.find((o) => o.ProductId === obj.ProductId) ?? obj);
    localStorage.setItem("cart", JSON.stringify(ShoppingCart));
    UpdateCartCounter();
    $(elem).find('.item-quantity').text(parseInt(quantity) + 1);
    if (modal) {
        $("#item-" + id).after(function () {

            $(this).find('.item-quantity').text(parseInt(quantity) + 1);

        });
    }
}

function dcrQty(e, id, modal = false) {


    var elem = $(e).closest(".product-item");

    var quantity = $(elem).find('.item-quantity').text();
    var newPrice = $(elem).find('.item-price').text();
    var product = ShoppingCart.filter(function (obj) {
        return obj.ProductId == id;
    });

    product[0].Quantity -= 1;
    product[0].Price -= parseFloat(newPrice.split(" ")[1]);

    if (product[0].Quantity == 0) {

        var index = ShoppingCart.findIndex(function (o) {
            return o.ProductId === product[0].ProductId;
        })


        if (index !== -1) ShoppingCart.splice(index, 1);
        localStorage.setItem("cart", JSON.stringify(ShoppingCart));
        $(elem).find('.item-quantity').text(parseInt(quantity) - 1);


        if (modal) {
            $("#item-" + id).after(function () {

                $(this).find('.item-quantity').text(parseInt(quantity) - 1);
                $(this).find(".batch").hide();

            });
        }
        else {
            $(elem).find('.batch').hide();
        }

        $(e).closest('.basket-product-tile').remove();
        $(".modal-cart-btn").show();
        $(".modal-cart-inc").hide();
        UpdateCartCounter();

        if (ShoppingCart.length == 0) {
            var url = window.location.href;
            var path = url.split("/")[5];
            if (path == 'Basket') {
                window.location = "/Restaurant/Shop/Index"
            }
        }

    }

    else {

        ShoppingCart.map((obj) => product.find((o) => o.ProductId === obj.ProductId) ?? obj);
        localStorage.setItem("cart", JSON.stringify(ShoppingCart));

        $(elem).find('.item-quantity').text(parseInt(quantity) - 1);
        if (modal) {
            $("#item-" + id).after(function () {

                $(this).find('.item-quantity').text(parseInt(quantity) - 1);

            });
        }
        UpdateCartCounter();
    }

}

jQuery.expr[':'].contains = function (a, i, m) {
    return jQuery(a).text().toUpperCase()
        .indexOf(m[3].toUpperCase()) >= 0;
};

function PlaceOrder(e) {

    $(e).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
    var orderSupplier = [];
    var time = null;
    var date = null;
    var orderDetails = {};
    for (var i = 0; i <= ShoppingCart.length - 1; i++) {
        orderSupplier.push({
            SupplierItemId: ShoppingCart[i].ProductId,
            Quantity: ShoppingCart[i].Quantity,
        });
    };

    var time = $("#Stime").val();
    var date = $("#Sdate").val();


    orderDetails = {
        SupplierId: $('#supplierId').val(),
        SupplierCouponId: supplierCouponId,
        SupplierCouponDiscountAmount: totalAfterCoupon,
        DeliveryAddress: $('#Address').val(),
        StreetAddress: $('#StreetAddress').val(),
        Contact: $('#ContactNo').val(),
        Latitude: $('#Latitude').val(),
        Longitude: $('#Longitude').val(),
        NoteToRider: $('#Note').val(),
        OrderRequiredTime: time,
        OrderRequiredDate: date,
        SupplierOrderDetails: orderSupplier,
    };

    $.ajax({
        url: '/Restaurant/Shop/OrderNow',
        type: 'POST',
        data: {
            order: orderDetails,
            supplierOrderId: supplierCouponId,
        },
        success: function (result) {

            if (result.success) {

                SupplierCouponId = 0;
                totalAfterCoupon = 0;
                window.location = result.url;

            }
        }


    });

};

function ClearAll(e) {

    localStorage.removeItem("cart");
    $(".order-cart").empty();
    $(".order-delivery").text(0);
    $(".order-subtotal").text(0);
    $(".order-total").text(0);
    $(".order-vat").text(0);
    $(".Address").val("");
    $("#Note").val("");

};



