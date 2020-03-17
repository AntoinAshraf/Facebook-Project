function saveSettings(){
    let oldPassword = document.getElementById("oldPassword");
    let newPassword = document.getElementById("newPassword");
    let confirmPassword = document.getElementById("confirmPassword");

    let saveSettingsBtn = document.getElementById("saveSettingsBtn");

    oldPassword.style.borderColor = "#e5e5e5";
    newPassword.style.borderColor = "#e5e5e5";
    confirmPassword.style.borderColor = "#e5e5e5";

    if (oldPassword.value === "") {
        oldPassword.style.borderColor = "red";
        return toastr.error('Old Password Field Can\'t be Empty.', 'Validation Error');
    }

    if (newPassword.value === "") {
        newPassword.style.borderColor = "red";
        return toastr.error('New Password Field Can\'t be Empty.', 'Validation Error');
    }

    if (newPassword.value.length < 5) {
        newPassword.style.borderColor = "red";
        return toastr.error('New Password Can\'t be less than 5 character.', 'Validation Error');
    }

    if (newPassword.value !== confirmPassword.value) {
        newPassword.style.borderColor = "red";
        confirmPassword.style.borderColor = "red";
        return toastr.error('New Password and Confirm Password don\'t match.', 'Validation Error');
    }

    saveSettingsBtn.disabled  = true;
    saveSettingsBtn.innerHTML = "Loading";
    saveSettingsBtn.style.backgroundColor = "#65a6f9";
    debugger
    fetch("https://localhost:44340/Account/ChangePassword", {
        method: "post",
        body: JSON.stringify({
            OldPassword: oldPassword.value,
            NewPassword: newPassword.value
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    }).then((response) => {
        return response.json();
    }).then((data) => {
        debugger
        if (data.statusCode === 400) {
            toastr.error(data.responseMessage, 'Validation Error');
        }
        if (data.statusCode === 200) {
            toastr.success('Password has been Chaanged Successfully', 'Done');
        }
        saveSettingsBtn.disabled = false;
        saveSettingsBtn.innerHTML = "Save Setting";
        saveSettingsBtn.style.backgroundColor = "#1877F2";

    }).catch((err) => {
        saveSettingsBtn.disabled = false;
        saveSettingsBtn.innerHTML = "Save Setting";
        saveSettingsBtn.style.backgroundColor = "#1877F2";
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function ValidateEmail(email) 
{
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
        return true;
    return false;
}