var Table;

$(document).ready(function () {

    BindDataTable();

});

function BindDataTable() {

    Table = $('Table').DataTable({
        "processing": true,
        "serverSide": false,
        "filter": true,
        "columns": [
            { "data": "userId", "visible": false },
            { "data": "email" },
            { "data": "firstName" },
            { "data": "lastName" },
            {
                "data": "isActive",
                "render": function (data, type, row) {
                    return SetActiveStatus(data.toString().toUpperCase())
                }
            },
            {
                'data': null,
                'width': '300px',
                "render": function (data) {
                    var isActive = data.isActive.toString().toUpperCase();

                    var status = {
                        "TRUE": {
                            'title': 'Deactivate',
                            'icon': 'fa-times-circle',
                            'class': ' btn-outline-danger'
                        },
                        "FALSE": {
                            'title': 'Activate',
                            'icon': 'fa-check-circle',
                            'class': ' btn-outline-success'
                        },
                    };

                    var actions = '';

                    actions += '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/User/Edit?UserId=' + data.userId + '\',true)">' +
                        '<i class="fa fa-pen"></i>' +
                        '</button> ' +
                        '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="OpenModelPopup(this,\'/User/Detail?UserId=' + data.userId + '\')" title="View">' +
                        '<i class="fa fa-folder-open"></i>' +
                        '</button> ' +
                        '<button type="button" class="btn btn-bg-secondary btn-icon btn-sm mr-1" onclick="Delete(this,\'/User/Delete?UserId=' + data.userId + '\')"><i class="fa fa-trash"></i></button>';

                    actions += '<button type="button" class="btn btn-sm mr-1' + status[isActive].class + '" onclick="Activate(this,\'/User/ToggleActiveStatus?UserId=' + data.userId + '\')">' +
                        '<i class="fa ' + status[isActive].icon + '" aria-hidden="true"></i> ' + status[isActive].title +
                        '</button>';

                    return actions;
                }
            },
        ],
    });
}

function addRow(row) {

    Table.row
        .add({
            'userId': row.userId,
            'email': row.email,
            'firstName': row.firstName,
            'lastName': row.lastName,
            'isActive': row.isActive,
            '': ''
        })
        .draw(true);
}