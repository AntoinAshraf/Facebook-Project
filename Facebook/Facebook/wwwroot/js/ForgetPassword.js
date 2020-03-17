function sendCode(){
    let email = document.getElementById("email");
    let sendCodeBtn = document.getElementById("sendCodeBtn");

    email.style.borderColor = "#e5e5e5";

    if (email.value === "") {
        email.style.borderColor = "red";
        return toastr.error('Email Field Can\'t be Empty.', 'Validation Error');
    }

    if (!ValidateEmail(email.value)) {
        email.style.borderColor = "red";
        return toastr.error('This Email is not valid.', 'Validation Error');
    }

    sendCodeBtn.disabled  = true;
    sendCodeBtn.innerHTML = "Loading";
    sendCodeBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44340/Account/SendForgetPasswordCode/?Email=" + email.value, {
        method: "get"
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 400) {
            toastr.error(data.responseMessage, 'Validation Error');
        }
        if (data.statusCode === 200) {
            toastr.success('Code has been sent to your Email Successfully', 'Done');
            //window.location.href = "/";
        }
        sendCodeBtn.disabled = false;
        sendCodeBtn.innerHTML = "Send Email";
        sendCodeBtn.style.backgroundColor = "#1877F2";

    }).catch((err) => {
        sendCodeBtn.disabled = false;
        sendCodeBtn.innerHTML = "Send Email";
        sendCodeBtn.style.backgroundColor = "#1877F2";
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function ValidateEmail(email) 
{
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
        return true;
    return false;
}