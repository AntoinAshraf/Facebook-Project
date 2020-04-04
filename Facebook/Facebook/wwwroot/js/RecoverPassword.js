function confirmChangePassword(){
    const code = document.getElementById("code");
    const password = document.getElementById("password");
    const confirmPassword = document.getElementById("confirmPassword");

    const confirmBtn = document.getElementById("confirmBtn");

    if (code.value === "") {
        code.style.borderColor = "red";
        return toastr.error('Code Field Can\'t be Empty.', 'Validation Error');
    }

    if (password.value === "") {
        password.style.borderColor = "red";
        return toastr.error('Password Field Can\'t be Empty.', 'Validation Error');
    }

    if (password.value.length < 5) {
        confirmPassword.style.borderColor = "red";
        return toastr.error('Password Can\'t be less than 5 character.', 'Validation Error');
    }

    if (password.value !== confirmPassword.value) {
        password.style.borderColor = "red";
        confirmPassword.style.borderColor = "red";
        return toastr.error('Password and Confirm Password don\'t match.', 'Validation Error');
    }

    code.style.borderColor = "#e5e5e5";
    password.style.borderColor = "#e5e5e5";
    confirmPassword.style.borderColor = "#e5e5e5";

    confirmBtn.disabled  = true;
    confirmBtn.innerHTML = "Loading";
    confirmBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44340/Account/RecoverPassword/?code=" + code.value, {
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
            toastr.error(data.responseMessage, 'Validation Error');
        }
        if (data.statusCode === 200) {
            toastr.success('Password Changed Successfully.', 'Done');
            window.location.href = "/";
        }
        confirmBtn.disabled = false;
        confirmBtn.innerHTML = "Confirm";
        confirmBtn.style.backgroundColor = "#0e385f";
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
        confirmBtn.disabled = false;
        confirmBtn.innerHTML = "Confirm";
        confirmBtn.style.backgroundColor = "#0e385f";
    });
}