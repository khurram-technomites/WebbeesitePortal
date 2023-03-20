"use strict";
var table1;
var KTDatatablesBasicScrollable = function () {

    var initTable1 = function () {
        var table = $('#kt_datatable1');

        // begin first table
        table1 = table.DataTable({
            //scrollY: '50vh',
            scrollX: true,
            scrollCollapse: true,
            "language": {
                processing: '<i class="spinner spinner-left spinner-dark spinner-sm"></i>'
            },
            "initComplete": function (settings, json) {
                $('#kt_datatable1 tbody').fadeIn();
                if (settings.aoData.length <= 0) {
                    $('.excel-btn').prop('disabled', true);
                }
                else {
                    $('.excel-btn').prop('disabled', false);
                }
            },
            // Pagination settings


            columnDefs: [
                {
                    targets: 0,
                    /*className: "dt-center",*/
                    width: '130px',
                },
                {
                    targets: 1,
                    /* className: "dt-center",*/
                    width: '100px',
                },
                {
                    targets: 2,
                    /*className: "text-left",*/
                    width: '200px',

                    render: function (data, type, full, meta) {
                        
                        var number = 40;
                        var description = data.length > number ? data.substr(0, number - 1) + '&hellip;' : data;
                        return description;
                    },
                },
                {
                    targets: 3,
                    className: "dt-center",
                    width: '75px',

                    render: function (data, type, full, meta) {
                        
                        data = data.toUpperCase();
                        var status = {
                            "LOW": {
                                'title': 'LOW',
                                'class': ' label-light-success'
                            },
                            "MEDIUM": {
                                'title': 'MEDIUM',
                                'class': ' label-light-primary'
                            },
                            "HIGH": {
                                'title': 'HIGH',
                                'class': ' label-light-danger'
                            },
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Medium</span>';
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    },
                },
                {
                    targets: 4,
                    className: "dt-center",
                    width: '75px',

                    render: function (data, type, full, meta) {
                        
                        data = data.toUpperCase();
                        var status = {
                            "OPEN": {
                                'title': 'OPEN',
                                'class': ' label-light-success'
                            },
                            "CLOSED": {
                                'title': 'CLOSED',
                                'class': ' label-light-danger'
                            },
                        };
                        if (typeof status[data] === 'undefined') {

                            return '<span class="label label-lg font-weight-bold label-light-danger label-inline">Close</span>';
                        }
                        return '<span  onclick="OpenModelPopup(this,\'/Restaurant/Ticket/StatusChange/' + full[5] + '\',true)" class="label label-lg font-weight-bold' + status[data].class + ' label-inline ">' + status[data].title + '</span>';
                    },
                },


                {
                    targets: -1,
                    title: 'Actions',
                    orderable: false,
                    width: '100px',
                    className: "dt-center",
                    render: function (data, type, full, meta) {
                        

                        return '<a class="btn btn-bg-secondary  mr-1"  href="/Restaurant/Ticket/Details/' + data + '">' +
                            '<i class="fa fa-eye"></i> View' +
                            ' </a> ';


                    },
                },
            ],
        });
    };

    return {
        //main function to initiate the module
        init: function () {
            initTable1();
        },
    };
}();

jQuery(document).ready(function () {
    KTDatatablesBasicScrollable.init();
});


function callback(dialog, elem, isedit, response) {
    
    if (response.success) {
        toastr.success(response.message);

        if (isedit) {
            table1.row($(elem).closest('tr')).remove().draw();
        }

        addRow(response.data);
        //location.reload()
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);
        jQuery('#myModal').modal('hide');
    }
    else {
        jQuery('form', dialog).closest('.modal').find('button[type=submit]').removeClass('spinner spinner-sm spinner-left').attr('disabled', false);

        toastr.error(response.message);
    }
}

function addRow(row) {
    console.log("Row", row);
    table1.row.add($('<tr>' +
        '<td data-order=' + row.ID + '> ' + row.Date + '</td>' +
        '<td>' + row.TicketNo + '</td>' +
        '<td id="' + Description + '">' + row.Description + '</td>' +
        '<td>' + row.Priority + '</td>' +
        '<td>' + row.Status + '</td>' +
        '<td nowrap="nowrap">' + row.ID + '</td>' +
        '</tr>'
    )).draw();


}