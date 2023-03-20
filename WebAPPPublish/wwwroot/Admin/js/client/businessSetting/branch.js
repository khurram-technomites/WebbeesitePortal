"use strict";

var model = {
	GarageBusinessSettingId: null,
	Id: null,
	Name: null,
	ContactPersonName: null,
	Contact: null,
	Contact2: null,
	Email: null,
	Address: null,
	CompleteAddress: null,
	Latitude: null,
	Longitude: null
};



function Save(element) {
	var row = $(element).closest(".list-repeat");

	if (row.find(".Name").val() == "") {
		Swal.fire(
			'Oops!',
			'Branch Name required ...',
			'error'
		)
	}

	else {

		var data = new FormData();
		row.find('.edit-cancel').prop('disabled', true);
		row.find('.save-changes').prop('disabled', true);
		row.find('.edit-profile').prop('disabled', true);
		row.find('.Delete').prop('disabled', true);
		debugger;
		data.append("Id", Number(row.find(".Id").val()));
		data.append("GarageBusinessSettingId", BusinessSettingID);
		data.append("Name", row.find(".Name").val());
		data.append("Contact1", row.find(".Contact1").val());
		data.append("Contact2", row.find(".Contact2").val());
		data.append("ContactPersonName", row.find(".ContactPersonName").val());
		data.append("Latitude", row.find(".Latitude").val());
		data.append("Longitude", row.find(".Longitude").val());
		data.append("Email", row.find(".Email").val());
		data.append("Address", row.find(".StreetAddress").val());
		data.append("CompleteAddress", row.find(".Address").val());

		$.ajax({
			url: "/Client/BusinessSetting/BranchSettings/",
			type: "POST",
			processData: false,
			contentType: false,
			data: data,
			success: function (response) {
				if (response.success) {

					row.find(".BranchName").text(response.data.Name);
					row.find(".Id").val(response.data.Id);
					row.find(".BusinessSettingID").val(response.data.BusinessSettingId);
					row.find(".Delete").attr("onclick", "Delete(this," + response.data.Id + ")");
					row.find(".edit-profile").attr("onclick", "Change(this," + response.data.Id + ")");
					row.find(".edit-cancel").attr("onclick", "Cancel(this," + response.data.Id + ")");
					row.find(".save-changes").attr("onclick", "Save(this," + response.data.Id + ")");
					toastr.success(response.message);

				}
				else {
					toastr.error(response.message);
					console.log(response.error);
				}

				SaveAfter(row);
			},
			error: function (er) {
				toastr.error(er);
			}
		});
		return false;
	}
}

function Delete(element, record) {

	var row = $(element).closest(".list-repeat");

	if (record == 0) {
		setTimeout(function () {
			row.remove();
		}, 2000);
		row.slideUp();
	}
	else {
		swal.fire({
			title: 'Are you sure?',
			text: "You won't be able to revert this!",
			type: 'warning',
			showCancelButton: true,
			confirmButtonText: 'Yes, delete it!'
		}).then(function (result) {
			if (result.value) {
				$.ajax({
					url: '/Client/BusinessSetting/DeleteBranchSetting/' + record,
					type: 'POST',
					data: {
						"__RequestVerificationToken":
							$("input[name=__RequestVerificationToken]").val()
					},
					success: function (response) {
						if (response.success != undefined) {
							if (response.success) {
								row.slideUp();
								setTimeout(function () { row.remove(); }, 250);
								row.find('input').empty();
								row.find('textarea').empty();
								toastr.success(response.message);
							}
							else {
								toastr.error(response.message);
								console.log(response.error);
							}
						} else {
							swal.fire("Your are not authorize to perform this action", "For further details please contact Garageistrator !", "warning").then(function () {
							});
						}
					},
					error: function (xhr, ajaxOptions, thrownError) {
						if (xhr.status == 403) {
							try {
								var response = $.parseJSON(xhr.responseText);
								swal.fire(response.Error, response.Message, "warning").then(function () {
									$('#myModal').modal('hide');
								});
							} catch (ex) {
								swal.fire("Access Denied", "Your are not authorize to perform this action, For further details please contact Garageistrator !", "warning").then(function () {
									$('#myModal').modal('hide');
								});
							}

							$(element).removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
							$(element).find('i').show();
						}
					}
				});
			}
		});
	}
}

function AddBranch(e) {
	debugger;
	var html = `  <div class="row form-group list-repeat align-items-center m-0 mt-5 mb-5 p-8 rounded">

                <div hidden>
                    <input type="number" class="form-control hidden ID" value="" hidden />
                </div>
                <div class="col-md-12 col-sm-12 text-center" hidden>
                    <div class="flex-shrink-0 mr-7">
                        <div class="symbol symbol-50 symbol-lg-120">
                            <input type="number" class="form-control  BusinessSettingID" value="${BusinessSettingID}" />
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <h4 class="card-label BranchName">
                        Branch Name
                    </h4>
                    <hr />
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label">Branch Name</label>
                        <div class="input-group mb-2">
                            <input type="text" class="form-control Name" value="" placeholder="Enter branch name here ..." />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-comment-alt "></i></span></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <label class="control-label">Contact Person Name</label>
                        <div class="input-group mb-2">
                            <input type="text" class="form-control ContactPersonName" value="" placeholder="Enter contact person name here ..."  />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-user "></i></span></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">Contact 1</label>
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <span class="input-group-text">+971</span>
                            </div>
                            <input type="number" onblur="validate(this.value,this)" class="form-control Contact1" value="" placeholder="Enter contact 1 here ..."  />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-phone"></i></span></div>
                        </div>
                        <span class="form-text text-danger confPassError" style="font-size: 9px;"></span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">Contact 2</label>
                        <div class="input-group mb-2">
                            <div class="input-group-prepend">
                                <span class="input-group-text">+971</span>
                            </div>
                            <input type="number" onblur="validate(this.value,this)" class="form-control Contact2" value="" placeholder="Enter contact 2 here ..."  />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-phone"></i></span></div>
                        </div>
                        <span class="form-text text-danger confPassError" style="font-size: 9px;"></span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">Fax No</label>
                        <div class="input-group mb-2">
                            <input type="tel" class="form-control Fax" value="" placeholder="Enter fax here ..."  />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-fax"></i></span></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label class="control-label">Email</label>
                        <div class="input-group mb-2">
                            <input type="email" class="form-control Email" value="" placeholder="Enter email here ..."  />
                            <div class="input-group-append"><span class="input-group-text"><i class="fa fa-envelope"></i></span></div>
                        </div>
                    </div>
                </div>


                    <div class="col-md-12">
                        <div class="form-group">
                            <label class="control-label">Address</label>
                            <div class="input-group mb-2">
                                <textarea class="form-control StreetAddress" rows="4" placeholder="Enter street address here..."></textarea>
                                <div class="input-group-append"><span class="input-group-text"><i class="fa fa-home"></i></span></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 col-sm-12 location-div">
                        <div class="form-group">
                            <label class="wo-titleinput mb-0">Search Location</label>
                            <div class="wo-form-icon">
                                <input type="text" class="form-control branch-Address  show-map-input Address" id="Address" value="" placeholder="Choose location" required>
                                <a href="javascript:void(0);" class="wo-right-icon get-current-location" onclick="getLocation(this)">
                                    <i class="flaticon2-map"></i>
                                </a>
                                <a href="javascript:void(0);" class="wo-right-icon pin-on-map"  onclick="openMap(this)">
                                    Pin On Map
                                </a>
                            </div>
                        </div>
                        <div class="MapSearchResult" style="display:none">
                        </div>
                        <input type="hidden" name="Latitude" class="Latitude" value="" />
                        <input type="hidden" name="Longitude" class="Longitude" value="" />
                    </div>
                <div class="col-md-12 text-right">
                    <div class="card-toolbar">
                        <button type="button" class="btn btn-danger font-weight-bolder font-weight-bolder Delete mr-2" onclick="Delete(this, 0)">
                            <i class="la la-trash-o"></i>Delete
                        </button>
                        <div class="btn-group">
                            <button type="submit" class="btn btn-success font-weight-bolder save-changes" onclick="Save(this, 0)">
                                <i class="ki ki-check icon-sm"></i> Save Changes
                            </button>
                        </div>
                    </div>
                </div>
            </div>`;
	$("#repeatList").append(html);
	Change(e , "");
}

function Change(element, record) {
	debugger;
	var row = $(element).closest(".list-repeat");
	row.find('input').prop('disabled', false);
	row.find('textarea').prop('disabled', false);
	row.find('.edit-cancel').fadeIn();
	row.find('.save-changes').fadeIn();
	row.find('.edit-profile').hide();
}

function Cancel(element, record) {
	var row = $(element).closest(".list-repeat");
	row.find('input').prop('disabled', true);
	row.find('textarea').prop('disabled', true);
	row.find('.edit-cancel').hide();
	row.find('.save-changes').hide();
	row.find('.edit-profile').fadeIn();
}

function SaveAfter(row) {
	row.find('.edit-cancel').prop('disabled', false);
	row.find('.save-changes').prop('disabled', false);
	row.find('.edit-profile').prop('disabled', false);
	row.find('.Delete').prop('disabled', false);

	row.find('input').prop('disabled', true);
	row.find('textarea').prop('disabled', true);

	row.find('.edit-cancel').hide();
	row.find('.save-changes').hide();
	row.find('.edit-profile').fadeIn();

}