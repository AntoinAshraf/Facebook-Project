function signin(){
    let email = document.getElementById("signinEmail");
    let password = document.getElementById("signinPassword");
    let signinBtn = document.getElementById("signinBtn");

    email.style.borderColor = "#e5e5e5";
    password.style.borderColor = "#e5e5e5";

    if (email.value === "") {
        email.style.borderColor = "red";
        return toastr.error('Email Field Can\'t be Empty.', 'Validation Error');
    }

    if (!ValidateEmail(email.value)) {
        email.style.borderColor = "red";
        return toastr.error('This Email is not valid.', 'Validation Error');
    }

    if (password.value === "") {
        password.style.borderColor = "red";
        return toastr.error('Password Field Can\'t be Empty.', 'Validation Error');
    }

    if (password.value.length < 5) {
        email.style.borderColor = "red";
        return toastr.error('Password Can\'t be less than 5 character.', 'Validation Error');
    }
    
    signinBtn.disabled  = true;
    signinBtn.innerHTML = "Loading";
    signinBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44340/Account/Login", {
        method: "post",
        body: JSON.stringify({
            Email: email.value,
            Password: password.value
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 400) {
            data.responseMessage.forEach(element => {
                toastr.error(element.errorMessage, 'Validation Error');
            });
        }
        if (data.statusCode === 200) {
            toastr.success('Sign in complete.', 'Done');
            window.location.href = "/Home/Index";
        }
        signinBtn.disabled = false;
        signinBtn.innerHTML = "Sign in";
        signinBtn.style.backgroundColor = "#1877F2";
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
        signinBtn.disabled = false;
        signinBtn.innerHTML = "Sign in";
        signinBtn.style.backgroundColor = "#1877F2";
    });
}
