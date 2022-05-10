
//$(document).ready(function () {

//    var updateOutput = function (e) {
//        var list = e.length ? e : $(e.target),
//                output = list.data('output');
//        if (output != undefined) {
//            if (window.JSON) {
//                output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
//            } else {
//                output.val('JSON browser support required for this demo.');
//            }
//        }
//    };
//    // activate Nestable for list 1
//    $('#nestable2').nestable({
//        group: 1
//    }).on('change', updateOutput);

//    // activate Nestable for list 2
//    $('#nestable2').nestable({
//        group: 1
//    }).on('change', updateOutput);

//    // output initial serialised data
//    updateOutput($('#nestable').data('output', $('#nestable-output')));
//    updateOutput($('#nestable2').data('output', $('#nestable2-output')));

//    $('#nestable2-menu').on('click', function (e) {
//        var target = $(e.target),
//                action = target.data('action');
//        if (action === 'expand-all') {
//            $('.dd').nestable('expandAll');
//        }
//        if (action === 'collapse-all') {
//            $('.dd').nestable('collapseAll');
//        }
//    });



    //////////////////////
    $('#tblData').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        //to select and search from grid
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });

});