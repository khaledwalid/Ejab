
//$(document).ready(function(){
//    $("#btnSubmit").on("click",function()
//    {
//        var emailTxt = $("#txtemail");
//        if (emailTxt.val().length < 0) {
//            alert("Please Enter Your Email");
//        }
//    });

//});


function ImagePreview(input) {
   
    //if (input.files.length == 0) {

    //    $('#ImgPreview').css('display', 'none');
    //}
    if (input.files && input.files[0]) {
        document.getElementById("ImgPreview").style["display"] = "inline";
        var reader = new FileReader();
        $('#ImgPreview').css('display', 'inline-block');
        reader.onload = function (e) {
            $('#ImgPreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function toggleChecked(status) {
    debugger;
    $('input[type=checkbox]').each(function () {
        // Set the checked status of each to match the 
        // checked status of the check all checkbox:
        $(this).prop("checked", status);
    });
}

$(document).ready(function () {  
    debugger;
    // Grab a reference to the check all box:
    var checkAllBox = $("#chkAll");

    //Set the default value of the global checkbox to true:
    checkAllBox.prop('checked', false);

    // Attach the call to toggleChecked to the
    // click event of the global checkbox:
    checkAllBox.click(function () {
        var status = checkAllBox.prop('checked');
        toggleChecked(status);
    });
});








