//$(document).ready(function () {
       
//    $('.demo3').click(function () {
//        debugger;
//        var id = $(this).attr('data-id');
//        alert(id);
//        swal({
//            title: 'areyou sure',//@Resources.Global.CheckDeleting,
//            text: 'are you sure',//@Resources.Global.DeletetingMessage,
//            type: "warning",
//            showCancelButton: true,
//            confirmButtonColor: "#DD6B55",
//            confirmButtonText: "Yes, delete it!",
//            closeOnConfirm: false
//        }, function(isConfirm)   
//        {  
//            if (isConfirm)  
//            {  
//                $.ajax({
//                    url: '/Region/DeleteRegion',
//                    dataType: "json",
//                    type: "DELETE",
//                    contentType: 'application/json; charset=utf-8',
//                    data: { id: 20},
//                    async: true,
//                    processData: false,
//                    cache: false,
//                    success: function (data) {
//                        alert(data);
//                    },
//                    error: function (xhr) {
//                        alert('error');
//                    }
//                });
//            } else   
//            {  
//                swal("Cancelled", "You have Cancelled Form Submission!", "Cancel");  
//            }  
//        }); 
//    });


//});
