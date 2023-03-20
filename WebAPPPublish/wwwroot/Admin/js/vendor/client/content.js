var DomainList = []
var EmailList = []
var NoOfEmail = null;
var filePath = null;
var FileList = [];
$(document).ready(function () {
    debugger;
    $("#YesNewDomain").trigger('change')
        $("#AlreadyDomain").trigger('change')
   
});

$('.btndomain').click(function () {
    var domain = $('#Domain').val()
    var html = ` <div class="m-1 p-0 ">
                                                                    <div class="bg-gray-200 d-flex justify-content-center align-items-center rounded p-1">
                                                                        <span class="font-size-xs font-weight-boldest">${domain}</span>
                                                                        <span><i data-id ="0" class="fa fa-trash text-red font-size-sm ml-3 my-cursor btndeletedomain"></i></span>
                                                                    </div>
                                                                </div>`
    $('.domainsection').append(html)
    var Domain = {
        ClientId: $('#ClientId').val(),
        Domain: domain,
    }
    DomainList.push(Domain)
    $('#Domain').val('')
})

$('.btnemail').click(function () {
    
    if (NoOfEmail == null) {
        $.ajax({
            url: "/Vendor/Client/GetPackageEmail",
            type: "POST",
            async: false,
            data: { 'Id': $('#ClientId').val() },
            success: function (response) {
                if (response.success) {
                    NoOfEmail = response.data;
                }
            },
            error: function (er) {
                toastr.error(er);
            }
        });
    }
    if (EmailList.length != NoOfEmail && NoOfEmail != null) {
        var email = $('#Email').val()
        if (email != '') {
            if (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))) {
                toastr.error("You have entered an invalid email address!")
                return (false)
            } 
           
            var html = ` <div class="m-1 p-0 ">
                                                                    <div class="bg-gray-200 d-flex justify-content-center align-items-center rounded p-1">
                                                                        <span class="font-size-xs font-weight-boldest">${email}</span>
                                                                        <span><i data-id ="0" class="fa fa-trash text-red font-size-sm ml-3 my-cursor btndeleteemail"></i></span>
                                                                    </div>
                                                                </div>`
            $('.emailsection').append(html)
            var Email = {
                Email : email,
                ClientId: $('#ClientId').val()
            }
            EmailList.push(Email)
            $('.countspan').html(`You can add ${NoOfEmail - EmailList.length} more email`) 
            $('#Email').val('')
        }
      
    }
    else {
        toastr.error("Cannot add more Email");
    }
    
})

$(document).on("click", ".btndeleteemail", function () {
    var id = $(this).data("id");
    if (id != 0) {
        $.ajax({
            url: "/Vendor/Client/DeleteEmail",
            type: "POST",
            data: { 'Id': id },
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
    var email = this.parentElement.parentElement.children[0].textContent
    EmailList.find((o, i) => {
        if (o.Email == email) {
            EmailList.splice(i, 1)
            return true
        }
    })
    $('.countspan').html(`You can add ${NoOfEmail - EmailList.length} more email`) 
    this.parentElement.parentElement.parentElement.remove()
})
$(document).on("click", ".btndeletedomain", function () {
    var id = $(this).data("id");
    if (id != 0) {
        $.ajax({
            url: "/Vendor/Client/DeleteDomain",
            type: "POST",
            data: { 'Id': id },
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
    var domain = this.parentElement.parentElement.children[0].textContent
    DomainList.find((o, i) => {
        if (o.Domain == domain) {
            DomainList.splice(i,1)
            return true
        }
    })
    this.parentElement.parentElement.parentElement.remove()
})
$("#YesNewDomain").change(function () {

    if (this.checked) {
        $("#AlreadyDomain").prop('checked', false); 
        $(".AlreadyDomainSection").attr('hidden', 'hidden');
        $(".newdomainsection").removeAttr('hidden', 'hidden');
    }
    else {
        $("#YesNewDomain").prop('checked', false); 
        $(".newdomainsection").attr('hidden', 'hidden');
        $(".AlreadyDomainSection").removeAttr('hidden', 'hidden');
    }
})
$("#AlreadyDomain").change(function () {

    if (this.checked) {
        $("#YesNewDomain").prop('checked', false);
        $(".newdomainsection").attr('hidden', 'hidden');
        $(".AlreadyDomainSection").removeAttr('hidden', 'hidden');
    }
    else {
        $("#AlreadyDomain").prop('checked', false);
        $(".AlreadyDomainSection").attr('hidden', 'hidden');
        $(".newdomainsection").removeAttr('hidden', 'hidden');

       
    }
})


$(document).on("click", ".content-upload", function () {
    $(this).find(`input[type="file"]`)[0].click();
});
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

function DeleteDocument(element, type, Id = 0) {
    if (Id != 0) {
        $.ajax({
            url: "/Vendor/Client/DeleteDocument",
            type: "POST",
            data: { 'Id': Id },
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
    FileList.find((o, i) => {
        if (o.DocumentType == type) {
            FileList.splice(i, 1)
            return true; // stop searching
        }
    })
    var DocumentHTML = `   <div class="bg-dark-o-15 w-100 p-5 rounded d-flex justify-content-center align-items-center content-upload" style=" border: #c1c1c1 2px dashed;">
                                                                <p class="m-0 pb-2 pt-2 text-dark-50 font-weight-bold">Upload</p>
                                                                   <input type="file" data-type="${type}" class="fileUpload" accept=".doc,.pdf,.rar" hidden/>
                                                            </div>`
    var div = element.parentElement.parentElement.parentElement

    $(div).append(DocumentHTML);
    element.parentElement.parentElement.remove()
}


$(".fileUpload").change(function (e) {
    var Count = 0
    var currentdate = new Date(); 
    filePath = UploadImageToDraft(this.files[0]).responseText;
    var type = this.dataset.type;
 
        Content = {
            DocumentType : type,
            DocumentPath: filePath,
            ClientID: $('#ClientId').val(),
            CreatedOn: currentdate.toLocaleDateString() + " " + currentdate.toLocaleTimeString()
        }
        FileList.push(Content)
    var DocumentHTML = `<div class="form-group mb-3">
                                                                    <div class="bg-dark-o-15 w-100 p-4 rounded d-flex justify-content-between align-items-center">
                                                                        <div class="float-left d-flex align-items-center">
                                                                            <div class="bg-dark-o-20 rounded d-flex justify-content-center align-items-center" style="height: 60px; width: 60px;">
                                                                                <a href=" ${filePath}" target="_blank"><i class="fa fa-download font-size-h1 text-black-50"></i></a>
                                                                            </div>
                                                                            <div class="pl-1">
                                                                                <p class="m-0 text-black-50 line-height-md font-size-xs">
                                                                                    <span > ${formatDate(currentdate)}</span><br />
                                                                                    <span>${type}.${this.files[0].type}</span>
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="float-right">
                                                                            <i class="fa fa-trash text-danger my-cursor" onclick="DeleteDocument(this,'${type}')"></i>
                                                                        </div>
                                                                    </div>
                                                           </div>`
    var div = this.parentElement.parentElement
   
    $(div).append(DocumentHTML);
    this.parentElement.remove()
  

});

function saveContent(element) {
    debugger
    $(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
    var data = {
        ClientID: $('#ClientId').val(),
        IsDomainRequired: $("#YesNewDomain").is(":checked"),
        DomainPassword: $('input[name=DomainPassword]').val(),
        Website: $('input[name=Website]').val(),
        DomainUserId: $('input[name=DomainUserId]').val(),
        DomainProvider: $('input[name=DomainProvider]').val(),
        ClientContentMediaViewModels: FileList,
        ClientDomainSuggestionsViewModels : DomainList,
        clientEmailsViewModels: EmailList
    }
    $.ajax({
        url: "/Vendor/Client/Content",
        type: "POST",
        data: { 'clientContentView': data },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                //For Edit Layout
                $(element).removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
                if (Layout != "ClientEdit") {
                    LoadPage('step4', '/Vendor/Client/Finalize')
                }
                
            }
            else {
                toastr.error(response.message);
                $(element).addClass('spinner spinner-sm spinner-left').attr('disabled', true);
                return false;
            }
        },
        error: function (er) {
            toastr.error(er);
        }
    });
}