function Register(){
    let password = document.getElementById("password");
    let confirmpassword = document.getElementById("confirmpassword");
    let email = document.getElementById("email");
    let firstName = document.getElementById("firstName");
    let lastName = document.getElementById("lastName");
    let phoneNumber = document.getElementById("phoneNumber");
    let gender = document.getElementById("gender");
    let birthdate = document.getElementById("birthdate");
    
    let RegisterBtn = document.getElementById("registerBtn");

    firstName.style.borderColor = "#e5e5e5";
    lastName.style.borderColor = "#e5e5e5";
    email.style.borderColor = "#e5e5e5";
    password.style.borderColor = "#e5e5e5";
    confirmpassword.style.borderColor = "#e5e5e5";
    phoneNumber.style.borderColor = "#e5e5e5";
    birthdate.style.borderColor = "#e5e5e5";


    if (firstName.value === "") {
        firstName.style.borderColor = "red";
        return toastr.error('First Name Field Can\'t be Empty.', 'Validation Error');
    }

    if (lastName.value === "") {
        lastName.style.borderColor = "red";
        return toastr.error('Last Name Field Can\'t be Empty.', 'Validation Error');
    }

    if (email.value === "") {
        email.style.borderColor = "red";
        return toastr.error('Email Field Can\'t be Empty.', 'Validation Error');
    }

    if (!ValidateEmail(email.value)) {
        email.style.borderColor = "red";
        return toastr.error('This Email is not valid.', 'Validation Error');
    }

    if (phoneNumber.value === "") {
        phoneNumber.style.borderColor = "red";
        return toastr.error('Phone Number Field Can\'t be Empty.', 'Validation Error');
    }

    if (!ValidatePhoneNumber(phoneNumber.value)) {
        phoneNumber.style.borderColor = "red";
        return toastr.error('This Phone Number is not valid.', 'Validation Error');
    }
  
    if (gender.value === "") {
        gender.style.borderColor = "red";
        return toastr.error('Gender Field Can\'t be Empty.', 'Validation Error');
    }

    if (birthdate.value === "") {
        birthdate.style.borderColor = "red";
        return toastr.error('Birth Date Field Can\'t be Empty.', 'Validation Error');
    }

    if (password.value === "") {
        password.style.borderColor = "red";
        return toastr.error('Password Field Can\'t be Empty.', 'Validation Error');
    }

    if (password.value.length < 5) {
        password.style.borderColor = "red";
        return toastr.error('Password Can\'t be less than 5 character.', 'Validation Error');
    }

    if (password.value !== confirmpassword.value) {
        password.style.borderColor = "red";
        confirmpassword.style.borderColor = "red";
        return toastr.error('Password and Confirm Password don\'t match.', 'Validation Error');
    }

    RegisterBtn.disabled  = true;
    RegisterBtn.innerHTML = "Loading";
    //RegisterBtn.style.backgroundColor = "#65a6f9";

    fetch("https://localhost:44340/Account/Register", {
        method: "post",
        body: JSON.stringify({
            Email: email.value,
            FirstName: firstName.value,
            LastName: lastName.value,
            PhoneNumber: phoneNumber.value,
            Password: password.value,
            GenderId: Number.parseInt(gender.value),
            BirthDate: birthdate.value
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
            toastr.success('You have been Registered Successfully', 'Done');
            var tab_id = "tab-1";
            $('.sign-control li').removeClass('current');
            $('.sign_in_sec').removeClass('current');
            $("#signinTab").addClass('current animated fadeIn');
            $("#" + tab_id).addClass('current animated fadeIn');
        }
        RegisterBtn.disabled = false;
        RegisterBtn.innerHTML = "Get Started";
        RegisterBtn.style.backgroundColor = "#0e385f";

    }).catch((err) => {
        RegisterBtn.disabled = false;
        RegisterBtn.innerHTML = "Get Started";
        RegisterBtn.style.backgroundColor = "#0e385f";
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function ValidateEmail(email) 
{
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email))
        return true;
    return false;
}

function ValidatePhoneNumber(number) 
{
    if (/^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/.test(number))
        return true;
    return false;
}





