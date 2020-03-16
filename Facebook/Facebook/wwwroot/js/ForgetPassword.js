function ForgetPasswordEmail(){
    let email = document.getElementById("email").value;
    let sendEmailBtn = document.getElementById("sendEmailBtn");

    if(email === ""){
        return toastr.error('Email Field Can\'t be Empty.', 'Validation Error')
    }

    if(!ValidateEmail(email)){
        return toastr.error('This Email is not valid.', 'Validation Error')
    }

    sendEmailBtn.disabled  = true;
    sendEmailBtn.innerHTML = "Loading";
    sendEmailBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44381/api/User/ForgetPassword/?email="+email,{
        method:"get",
    }).then((response) => {
        return response.json();
    }).then((data) => {
        debugger
        if(data.responseStatus === 400){
            data.validationErrors.forEach(element => {
                toastr.error(element.message, 'Validation Error')
            });
        }
        if(data.responseStatus === 200){
            toastr.success('Code has been sent to your Email Successfully', 'Done');
            //window.location.href = "/Account/Signin";
        }
        sendEmailBtn.disabled  = false;
        sendEmailBtn.innerHTML = "Send Email";
        sendEmailBtn.style.backgroundColor = "#1877F2";
        
    }).catch((err) => {
        sendEmailBtn.disabled  = false;
        sendEmailBtn.innerHTML = "Send Email";
        sendEmailBtn.style.backgroundColor = "#1877F2";
        toastr.error("Something went wrong!", 'Validation Error')
    })
}

function ValidateEmail(email) 
{
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
        return true;
    return false
}