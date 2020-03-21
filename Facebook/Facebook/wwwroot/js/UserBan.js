function UserBan(userId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            let Pan;
            const thisButton = document.getElementById("BanBtn_" + userId);
            const content = document.getElementsByClassName("companies-info")[0];
            const BanButtons = content.getElementsByTagName("button");

            const thisButtonText = thisButton.innerHTML;
            const thisButtonClasses = thisButton.classList[1];
            if (thisButtonClasses === "btn-danger")
                Pan = true;
            else
                Pan = false;

            thisButton.disabled = true;
            thisButton.innerHTML = "Loading";
            thisButton.style.opacity = .7;
            for (let i = 0; i < BanButtons.length; i++) {
                BanButtons[i].disabled = true;
                BanButtons[i].style.opacity = .7;
            }


            fetch("https://localhost:44340/Account/UserBan/" + userId + "?ban=" + Pan, {
                method: "get"
            }).then((response) => {
                return response.json();
            }).then((data) => {
                if (data.statusCode === 200) {
                    if (Pan)
                        toastr.success('User has been Baned Successfully', 'Done');
                    else
                        toastr.success('User has been UnBaned Successfully', 'Done');
                    thisButton.classList.remove("btn-danger", "btn-primary");
                    thisButtonClasses === "btn-danger" ? thisButton.classList.add("btn-primary") : thisButton.classList.add("btn-danger");
                }
                thisButton.innerHTML = thisButtonText === "Ban" ? "UnBan" : "Ban";
                for (let i = 0; i < BanButtons.length; i++) {
                    BanButtons[i].disabled = false;
                    BanButtons[i].style.opacity = 1;
                }
            }).catch((err) => {
                toastr.error("Something went wrong!", 'Validation Error');
                thisButton.innerHTML = thisButtonText;
                for (let i = 0; i < BanButtons.length; i++) {
                    BanButtons[i].disabled = false;
                    BanButtons[i].style.opacity = 1;
                }
            });
        }
    });
}

function changeRole(userId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            const sel = document.getElementById('Select_' + userId);
            const roleId = sel.options[sel.selectedIndex].value;
            const content = document.getElementsByClassName("companies-info")[0];
            const BanButtons = content.getElementsByTagName("button");
            const RoleSelects = content.getElementsByTagName("select");

            for (let i = 0; i < BanButtons.length; i++) {
                BanButtons[i].disabled = true;
                BanButtons[i].style.opacity = .7;
                RoleSelects[i].disabled = true;
                RoleSelects[i].style.opacity = .7;
            }

            debugger
            fetch("https://localhost:44340/Account/ChangeRole/" + userId + "?roleId=" + roleId, {
                method: "get"
            }).then((response) => {
                return response.json();
            }).then((data) => {
                if (data.statusCode === 200) {
                    toastr.success('Role has been Changed Successfully', 'Done');
                }
                for (let i = 0; i < BanButtons.length; i++) {
                    BanButtons[i].disabled = false;
                    BanButtons[i].style.opacity = 1;
                    RoleSelects[i].disabled = false;
                    RoleSelects[i].style.opacity = 1;
                }
            }).catch((err) => {
                toastr.error("Something went wrong!", 'Validation Error');
                for (let i = 0; i < BanButtons.length; i++) {
                    BanButtons[i].disabled = false;
                    BanButtons[i].style.opacity = 1;
                    RoleSelects[i].disabled = false;
                    RoleSelects[i].style.opacity = 1;
                }
            });
        }
    });
}


