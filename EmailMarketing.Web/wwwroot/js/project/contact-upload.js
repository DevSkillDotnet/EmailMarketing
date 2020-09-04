﻿function loadDatatable(url) {

    if (!$().DataTable) {
        console.warn('Warning - datatables.min.js is not loaded.');
        return;
    }

    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        columnDefs: [{
            orderable: false,
            width: 100,
            targets: [8]
        }],
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            searchPlaceholder: 'Type to filter...',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': $('html').attr('dir') == 'rtl' ? '&larr;' : '&rarr;', 'previous': $('html').attr('dir') == 'rtl' ? '&rarr;' : '&larr;' }
        }
    });

    $('#contact-upload-table').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": url,
        "order": [[1, "desc"]],
        "columnDefs": [
            {
                "targets": [0],
                'sortable': true,
                'searchable': true,
                "orderData": [0]
            },
            {
                "targets": [1],
                'sortable': true,
                'searchable': false,
                "orderData": [1]
            }, 
            {
                "targets": [2],
                'sortable': false,
                'searchable': false,
                "orderData": [2],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },   
            {
                "targets": [3],
                'sortable': false,
                'searchable': false,
                "orderData": [3],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },    
            {
                "targets": [4],
                'sortable': false,
                'searchable': false,
                "orderData": [4],
                "render": function (data, type, row, meta) {
                    var lbl = data == "Processing" ? "badge-warning" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },    
            {
                "targets": [5],
                'sortable': false,
                'searchable': false,
                "orderData": [5],
                "render": function (data, type, row, meta) {
                    var lbl = data == "No" ? "badge-danger" : "badge-success";
                    return '<span class="badge  ' + lbl + '">' + data + '</span>';
                }
            },    
            {
                "targets": [6],
                'sortable': false,
                'searchable': false,
                "orderData": [6]
            },       
            {
                "targets": [7],
                'sortable': false,
                'searchable': false,
                "width": "15%",
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    
                    var activeButton = '<a class="text-danger" data-toggle="modal" data-target="#modal-activeField" data-id="' + data + '" data-title="' + row[4] + '" href="#" title="Finish/Process">' +
                        '<i class="icon-blocked"></i></a>';

                    return  activeButton;
                }
            }
        ]
    });
}