
var mypackage = [];
var SubTotal = 0;
$(document).ready(function () {
    SubTotal = SubTotal + DefaultTotal
    $(".subtotal").text("AED " + DefaultTotal)
    //Package Load in Array on Page Load
    $(".purchases").each(function () {
        var modules = $('.modulesection').find('.' + $(this).find("#moduleid").text() + '')
        var price = parseInt($(this).find(".purchaseprice").text()) 
        $(this).find(".purchaseprice").text(price.toLocaleString('en-US'))
        if (modules.length > 0) {
            modules.text($(this).find("#Quantity").val())
           
        }
        Package = {
            ModuleID: $(this).find("#moduleid").html(),
            Quantity: $(this).find("#Quantity").val(),
            TotalPrice: price,
            ClientModulePurchaseID: $("#Id").val(),
            Id: $(this).find("#PurchaseDetailId").val()
        }
        mypackage.push(Package)
    });
})



function AddPackage(element, PackageId, Max, Min,ManageQuantity = true) {

    servicename = $(element).parent().parent().find('.servicename').text()
    price = $(element).parent().parent().find('#price').text()
    currentquantity = $(element).parent().find('.quantity').text()
    if (currentquantity == Max && Max!=0) {
        toastr.error("Max Number added");
        return false
    }
    updatedquantity = parseInt(currentquantity) + 1
    if (currentquantity == 0) {
        updatedquantity = Min
    }



    $(element).parent().find('.quantity').text(updatedquantity)
    TotalPrice = price * updatedquantity
    var oldpackagedetail = $('#detailsection').find('.' + PackageId + '')
    if (oldpackagedetail.length == 0) {
        var html = `<div class="bg-white w-100">
        <div class="w-100 justify-content-between d-flex ${PackageId}">
<input type="hidden" id="PurchaseDetailId" value="0"/>
            <div class="text-dark servicename">${updatedquantity} x ${servicename}</div>
            <div class="text-dark font-weight-bold ">AED <span>${TotalPrice.toLocaleString('en-US')}</span></div>
        </div>

    </div>`
        Package = {
            ModuleID: PackageId,
            Quantity: updatedquantity,
            TotalPrice: TotalPrice,
            ClientModulePurchaseID: $('#Id').val(),
            Id: 0,
        }
        mypackage.push(Package)
        $('#detailsection').append(html)
        SubTotal = SubTotal + TotalPrice
        $(".subtotal").text("AED " + SubTotal)
    }
    else {
        mypackage.find((o, i) => {
            if (parseInt(o.ModuleID) === PackageId) {
                SubTotal = SubTotal - mypackage[i].TotalPrice
                mypackage[i] = { ModuleID: PackageId, Quantity: updatedquantity, TotalPrice: TotalPrice, ClientModulePurchaseID: $('#Id').val(), Id: mypackage[i].Id };
                return true; // stop searching
            }
        })
        SubTotal = SubTotal + TotalPrice
        $(".subtotal").text("AED " + SubTotal)
        oldpackagedetail.find('span').text(TotalPrice.toLocaleString('en-US'))
        oldpackagedetail.find('.servicename').text(updatedquantity + ' x ' + servicename)


    }
    if (ManageQuantity == false) {
        var html = `<div class="d-flex justify-content-center align-items-center rounded bg-danger font-weight-bold font-size-h3" style="width:40px; height:40px;cursor: pointer" onclick="RemovePackage(this,${PackageId},1,false)"><i class="fa fa-trash"></i></div>`
        $(element).parent().append(html)
        $(element).remove();
    }
}

function RemovePackage(element, PackageId, Min, ManageQuantity = true, IsDefault = false) {
    debugger
    //If qunatity is not manage
    if (ManageQuantity == false) {
        var oldpackagedetail = $('#detailsection').find('.' + PackageId + '')
        mypackage.find((o, i) => {
            if (parseInt(o.ModuleID) === PackageId) {
                
                DeletePackage(o.Id, o.ClientModulePurchaseID)
               
                
                SubTotal = SubTotal - mypackage[i].TotalPrice
                $(".subtotal").text("AED " + SubTotal)
                mypackage.splice(i,1)
                return true; // stop searching
            }
        })
        oldpackagedetail.remove()
        var html = `<div class="d-flex justify-content-center align-items-center rounded bg-warning font-weight-bold font-size-h3" style="width:40px; height:40px;cursor: pointer" onclick="AddPackage(this,${PackageId},1,1,false)">+</div>`
        $(element).parent().append(html)
        $(element).remove();
        return false
    }
    servicename = $(element).parent().parent().find('.servicename').text()
    price = $(element).parent().parent().find('#price').text()
    currentquantity = $(element).parent().find('.quantity').text()
    if (currentquantity == 0) {
        return false
    }
    var oldpackagedetail = $('#detailsection').find('.' + PackageId + '')

    //If Module is Default
    if (currentquantity == Min && IsDefault == true && ManageQuantity == true) {
        return false
    }
    if (currentquantity == Min) {
        mypackage.find((o, i) => {
            if (parseInt(o.ModuleID) === PackageId) {
                DeletePackage(o.Id, o.ClientModulePurchaseID)
                SubTotal = SubTotal - mypackage[i].TotalPrice
                mypackage.splice(i,1)
                $(".subtotal").text("AED " + SubTotal)
                return true; // stop searching
            }
        })
        oldpackagedetail.remove()
        $(element).parent().find('.quantity').text(0)
        return false
    }
    updatedquantity = parseInt(currentquantity) - 1
    $(element).parent().find('.quantity').text(updatedquantity)
    TotalPrice = price * updatedquantity

    if (oldpackagedetail.length == 0) {
        var html = `<div class="bg-white w-100">
        <div class="w-100 justify-content-between d-flex ${PackageId}">
<input type="hidden" id="PurchaseDetailId" value="0"/>
            <div class="text-dark servicename">${updatedquantity} x ${servicename}</div>
            <div class="text-dark font-weight-bold ">AED <span>${TotalPrice.toLocaleString('en-US')}</span></div>
        </div>

    </div>`
        Package = {
            ModuleID: PackageId,
            Quantity: updatedquantity,
            TotalPrice: TotalPrice,
            ClientModulePurchaseID: $('#Id').val(),
             Id: 0
        }
        mypackage.push(Package)
        SubTotal = SubTotal - TotalPrice
        $(".subtotal").text("AED " + SubTotal)
        $('#detailsection').append(html)
    }
    else {
        mypackage.find((o, i) => {
            if (parseInt(o.ModuleID) === PackageId) {
                SubTotal = SubTotal - mypackage[i].TotalPrice
                mypackage[i] = { ModuleID: PackageId, Quantity: updatedquantity, TotalPrice: TotalPrice, ClientModulePurchaseID: $('#Id').val(), Id: mypackage[i].Id };
                return true; // stop searching
            }
        })
        SubTotal = SubTotal + TotalPrice
        $(".subtotal").text("AED " + SubTotal)
        oldpackagedetail.find('span').text(TotalPrice.toLocaleString('en-US'))
        oldpackagedetail.find('.servicename').text(updatedquantity + ' x ' + servicename)


    }
}

function savePackage(element){
    debugger
    $(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);

    var data = {

        ModulePurchaseDetails: mypackage,
        ClientModulePurchases: {
            Total: SubTotal,
            SubTotal: SubTotal,
            AmountToBePaid: SubTotal,
            Id: $('#Id').val(),
            GarageID: $('#ClientId').val()
        }
    }


    $.ajax({
        url: "/Vendor/Client/Package",
        type: "POST",
        data: { 'ClientPackageViewModel': data },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                $('#Id').val(response.purchaseid)
                //For Edit Layout
                if (Layout == "PackageEdit") {
                    LoadPage('step4', '/Vendor/Client/Finalize')
                }//For Main Layout
                else {
                    LoadPage('step3', '/Vendor/Client/Content')
                }
                
            }
            else {
                toastr.error(response.message);
                $(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
            }
        },
        error: function (er) {
            toastr.error(er);
        }
    });
    return false;
}

function DeletePackage(Id, ClientModulePurchaseID) {
    $.ajax({
        url: "/Vendor/Client/DeletePackage",
        type: "POST",
        data: { 'Id': Id, 'ClientModulePurchaseID': ClientModulePurchaseID },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
            }
            else {
                toastr.error(response.message);
                return false;
            }
        },
        error: function (er) {
            toastr.error(er);
        }
    });
    
}
