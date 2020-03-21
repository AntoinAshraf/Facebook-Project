function checkform() {
    var postContent = document.querySelector("#textPost");
    var postBtn = document.querySelector("#submit");
    if (postContent.value.trim().length !== 0) {
        console.log(postContent.value);
        document.querySelector("#submit").disabled = false;
    }
    else
    {
        console.log("jjjj");
         toastr.error('post Field Can\'t be Empty.', 'Validation Error');
        document.querySelector("#submit").disabled = true;
    }
    
}



