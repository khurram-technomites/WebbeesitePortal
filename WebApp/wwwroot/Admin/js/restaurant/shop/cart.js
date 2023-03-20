$(document).ready(function () {
	//$('body').removeClass('template-index').removeClass('home2-default').addClass('page-template').addClass('belle');

	if (ShoppingCart.length <= 0) {
		BindEmptyCart();
	} else {
		$('#Subtotal').html('AED ' + parseFloat(ShoppingCart.sum("Price")));
	}
})

function BindEmptyCart() {
	var EmptyCartTemplate = `<div class="row">
            	<div class="col-12 col-sm-12 col-md-12 col-lg-12">
        			<div class="empty-page-content text-center">
                        <h1>Your cart is empty</h1>
                        <p><a href="/" class="btn btn--has-icon-after">Continue shopping <i class="fa fa-caret-right" aria-hidden="true"></i></a></p>
                      </div>
        		</div>
        	</div>`;

	$("#shopping-cart-container").html(EmptyCartTemplate);
}

function BindCart(Product) {
	console.log(Product);
	var tr = '<tr class="cart__row border-bottom line1 cart-flex border-top" id=' + Product.RowId + '>';
	tr += '	<td class="cart__image-wrapper cart-flex-item">';
	tr += '		<a href="/product/' + Product.Slug + '" target="_blank">';
	tr += '			<img class="cart__image img-lazyload" src="' + Product.Thumbnail + '" alt="' + Product.Title + '">';
	tr += '		</a>';
	tr += '	</td>';
	tr += '	<td class="cart__meta small--text-left cart-flex-item pt-3">';
	tr += '		<div class="list-view-item__title">';
	tr += '			<a href="/product/' + Product.Slug + '">' + Product.Title + ' </a>';
	tr += '		</div>';
	if (Product.Attributes.length > 0) {
		tr += '		<div class="cart__meta-text" id="attributes_' + Product.RowId + '">';
		tr += '			' + GetAttributeString(Product.Attributes) + '<br>';
		tr += '		</div>';
	}

	if (Product.CustomNote) {
		tr += '	<hr class="m-0" />';
		tr += '	<div class="cart__meta-text">';
		tr += '		<strong>Custom Note :</strong> <textarea id="CustomNote" name="CustomNote" rows="2" onchange="ChangeCustomNote(this,\'' + Product.RowId + '\')">' + Product.CustomNote + '</textarea>';
		tr += '	</div>';
	}
	tr += '	</td>';
	tr += '	<td class="cart-flex-item text-center pt-3">';
	tr += '		<span class="money">AED ' + Product.UnitPrice + '</span>';
	tr += '	</td>';
	tr += '	<td class="cart-flex-item text-center pt-3">';
	tr += '		<div class="cart__qty offset-3 text-center">';
	tr += '			<div class="qtyField" id="qtyField-' + Product.RowId + '">';
	tr += '				<a class="qtyBtn minus" href="javascript:void(0);" onclick="ChangeQuantity(this,-1)">';
	tr += '					<i class="fa fa-minus" aria-hidden="true"></i>';
	tr += '				</a>';
	tr += '				<input class="cart__qty-input qty ProductQuantity_' + Product.RowId + '" style="width:60px;" type="number" min="1" name="updates[]" id="qty" value="' + Product.Quantity + '"  pattern="[0-9]*" max="2147483647" onkeyup="InputQuantity(this,\'' + Product.RowId + '\')">';
	tr += '				<a class="qtyBtn plus" href="javascript:void(0);" onclick="ChangeQuantity(this,1)">';
	tr += '					<i class="fa fa-plus" aria-hidden="true"></i>';
	tr += '				</a>';
	tr += '			</div>';
	tr += '			<p class="text-danger one-item-' + Product.RowId + '" style="display:none;">Only 1 item per order</p>';
	tr += '		</div>';
	tr += '	</td>';
	tr += '	<td class="cart-flex-item td-cart-price text-right pt-3 ">';
	tr += '		<div>';
	tr += '			<span class="money">AED ' + Product.Price + '</span>';
	tr += '		</div>';
	tr += '	</td>';
	tr += '	<td class="cart-flex-item text-center">';
	tr += '		<a href="javascript:;" class="btn" title="Remove tem" onclick="RemoveFromCart(this,\'' + Product.RowId + '\')">';
	tr += '			<i class="fa fa-trash"></i>';
	tr += '		</a>';
	tr += '	</td>';
	tr += '</tr>'
	MaxQty(Product);



	$('#CartProducts').append(tr);

	$("#CartProducts .cart__qty-input.qty").keydown(function (event) {
		// Prevent shift key since its not needed
		if (event.shiftKey == true) {
			event.preventDefault();
		}
		// Allow Only: keyboard 0-9, numpad 0-9, backspace, tab, left arrow, right arrow, delete
		if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
			// Allow normal operation
		} else {
			// Prevent the rest
			event.preventDefault();
		}
	});

	if (!Product.ProductVaraiationID && Product.IsSoldIndividually) {
		$('#qtyField-' + Product.RowId + '').hide();
		$('.one-item-' + Product.RowId + '').show();
	}
	else if (Product.ProductVaraiationID && Product.ProductVaraiationSoldIndividual) {
		$('#qtyField-' + Product.RowId + '').hide();
		$('.one-item-' + Product.RowId + '').show();
	}

	setTimeout(function () { OnErrorImage(); }, 3000);

}

function RemoveCartItem(elem, id) {

	$(elem).closest('tr').fadeOut(300, function () { $(this).remove(); });

	$('.CartProduct' + id).find('.remove').trigger('click');
	$('#Subtotal').html('AED ' + parseFloat(ShoppingCart.sum("Price")));

	RemoveFromCart(elem, id);
}

function ChangeQuantity(element, quantity) {
	let qty = Number($(element).closest('.qtyField').find('#qty').val());
	qty += quantity;
	$(element).closest('.qtyField').find('#qty').val(qty);
	$(element).closest('.qtyField').find('#qty').trigger('onkeyup');
}

function InputQuantity(element, id) {
	Item = ShoppingCart.find(function (obj) {
		return obj.RowId == id;
	});

	if (Item) {
		var max = $('.ProductQuantity_' + id).attr('max');
		var newVal = Number($(element).val());
		if (newVal > max) {
			newVal = max;
			$('#message').html('<span class="fa fa-check"></span> <span class="message">Product reach maximum quantity!</span>')
			alertMsg();
		}
		if (!newVal) {
			newVal = 1;
		}
		if (newVal <= 0) {
			newVal = 1;
		}
		$(element).val(newVal);
		ShoppingCart = ShoppingCart.filter(function (obj) {
			return obj.RowId !== Item.RowId;
		});

		Item.Quantity = Number(newVal);
		Item.Price = Item.Quantity * Item.UnitPrice;

		$(element).closest('tr').find('.td-cart-price .money').text('AED ' + Item.Price);
		AddToCart(Item);

		$('#Subtotal').html('AED ' + parseFloat(ShoppingCart.sum("Price")));

	}
}

function ChangeCustomNote(element, id) {
	Item = ShoppingCart.find(function (obj) {
		return obj.RowId == id;
	});

	if (Item) {

		ShoppingCart = ShoppingCart.filter(function (obj) {
			return obj.RowId !== Item.RowId;
		});

		Item.CustomNote = $(element).val();

		AddToCart(Item);
	}
}

function MaxQty(Product) {
	$.ajax({
		type: 'GET',
		url: `/en/products/${Product.ProductID}`,
		success: function (response) {
			if (response.success) {

				console.log(response.data);
				details = response.data;

				if (details.Type == '1' || details.Type == 'Simple') {
					if (details.IsManageStock == true) {
						$('.ProductQuantity_' + Product.RowId).attr("max", details.Stock);
					}
					else { }
				}
				else {
					var variation = details.Variations;
					var v = variation.find(function (obj) {
						return obj.ID == Product.ProductVaraiationID;
					});
					if (v) {
						if (v.IsManageStock == true) {
							$('.ProductQuantity_' + Product.RowId).attr("max", v.Stock);
						}
					}
					else { }
				}
			}
			else { }
		}
	});
}

function alertMsg() {
	$('#message').slideDown();
	setTimeout(function () { $('#message').slideUp(); }, 4000);
}