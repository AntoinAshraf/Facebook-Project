function rejectRequest(initiatorId, UserInfoId)
{
    fetch("https://localhost:44340/profile/rejectRequest?intiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'DELETE'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not reject the request!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            deleteRequestRow(initiatorId);
            toastr.success('Friend request rejected!', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });


}

function acceptRequest(initiatorId, UserInfoId) {
    fetch("https://localhost:44340/profile/acceptRequest?intiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'PUT'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not accept the request!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            deleteRequestRow(initiatorId);
            updateFriendsNumber();
            toastr.success('Friend Request Accepted!', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });


}

function deleteRequestRow(initiatorId) {
    if (initiatorId !== null)
        document.getElementById("Req_" + initiatorId).remove();
}

function updateFriendsNumber() {
    var result = document.getElementById("friendsNumber");

    result.innerText++; // Incrementing the number of friends for the user to be shown dynamically 
    console.log(result.innerText);
}