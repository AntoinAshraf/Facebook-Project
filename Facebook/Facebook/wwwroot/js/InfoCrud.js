function editInfo(userId) {
    const name = document.getElementById("infoName");
    const bd = document.getElementById("infoBd");
    const phone = document.getElementById("infoPhone");
    const gender = document.getElementById("infoGender");

    const nameOld = document.getElementById("infoNameCurrent");
    const bdOld = document.getElementById("infoBdCurrent");
    const phoneOld = document.getElementById("infoPhoneCurrent");
    const genderOld = document.getElementById("infoGenderCurrent");
    // update user full name in profile page
    const fullNameOld = document.getElementsByClassName("fullNameCurrent");

    const regex = /^01[0-2]{1}[0-9]{8}$/;

    if (name.value == null)
        return toastr.error("Name cannot be empty!", 'Validation Error');
    if (!regex.test(phone.value))
        return toastr.error("Phone number should have 11 digits!", 'Validation Error');


    fetch("https://localhost:44340/Profile/EditInfo", {
        method: "put",
        body: JSON.stringify({
            FullName: name.value,
            BirthDate: bd.value,
            GenderName: gender.options[gender.selectedIndex].text,
            PhoneNumber: phone.value,
            id: userId
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 400) {
            toastr.error(data.responseMessage, 'Validation Error');
        }
        if (data.statusCode === 200) {
            toastr.success('Info Edited!', 'Done');

            nameOld.innerHTML = name.value;
            bdOld.innerHTML = bd.value;
            phoneOld.innerHTML = phone.value;
            genderOld.innerHTML = gender.options[gender.selectedIndex].text;

            for (var i = 0; i < fullNameOld.length; i++) {
                fullNameOld[i].innerHTML = name.value;
            }
            

            $('#editInfoModal').modal("hide");
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}


function showInfo() {
    document.getElementById("infoContainer").style.display = "block";
}
