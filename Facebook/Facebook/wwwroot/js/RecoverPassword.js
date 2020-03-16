const urlParams = new URLSearchParams(window.location.search);
const token = urlParams.get('token');
const sendCodeBtn = document.getElementById("sendCodeBtn");
let email;

fetch("https://localhost:44381/api/User/RecoverPassword/?token="+token,{
    method:"get",
}).then((response) => {
    return response.json();
}).then((data) => {
    if(data.responseStatus === 200){
        email = data.result;
    }
})

function sendCode(){
    const code = document.getElementById("code").value;
    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;

    if(code === ""){
        return toastr.error('Code Field Can\'t be Empty.', 'Validation Error')
    }

    if(password === ""){
        return toastr.error('Password Field Can\'t be Empty.', 'Validation Error')
    }

    if(password !== confirmPassword){
        return toastr.error('Password and Confirm Password don\'t match.', 'Validation Error')
    }

    if(password.length < 5){
        return toastr.error('Password Can\'t be less than 5 character.', 'Validation Error')
    }

    sendCodeBtn.disabled  = true;
    sendCodeBtn.innerHTML = "Loading";
    sendCodeBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44381/api/User/ConfirmRecoverPassword/?code="+code,{
        method:"post",
        body: JSON.stringify({
            Email: email,
            Password: password,
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if(data.responseStatus === 400){
            data.validationErrors.forEach(element => {
                toastr.error(element.message, 'Validation Error')
            });
        }
        if(data.responseStatus === 200){
            toastr.success('Password Changed Successfully.', 'Done');
            window.location.href = "/Account/Signin";
        }
        sendCodeBtn.disabled  = false;
        sendCodeBtn.innerHTML = "Sign in";
        sendCodeBtn.style.backgroundColor = "#1877F2";
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
        sendCodeBtn.disabled  = false;
        sendCodeBtn.innerHTML = "Sign in";
        sendCodeBtn.style.backgroundColor = "#1877F2";
    })
}