function AddFriend(initiatorId, UserInfoId) {
    fetch("https://localhost:44340/Profile/AddFriend?initiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'POST'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not send add request!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            toggleAddRequestToPending(); // Remove add btn & replace it with pending request
            toastr.success('Friend Request Sent!', 'Done');
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function RemoveFriend(initiatorId, UserInfoId) {
    fetch("https://localhost:44340/Profile/RemoveFriend?initiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'PUT'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not remove friend!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            toggleRemoveRequestToPending(); // Remove remove btn & replace it with pending request
            toastr.success('Friend has been Removed!', 'Done');
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}
function rejectRequest(initiatorId, UserInfoId, friendRequestsCount)
{
    console.log(friendRequestsCount);
    fetch("https://localhost:44340/profile/rejectRequest?intiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'DELETE'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not reject the request!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            if (friendRequestsCount == 1)
                deleteRequestContainer();
            else
                deleteRequestRow(initiatorId);
            toastr.success('Friend request rejected!', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function acceptRequest(initiatorId, UserInfoId, friendRequestsCount) {
    fetch("https://localhost:44340/profile/acceptRequest?intiatorId=" + initiatorId + "&deciderId=" + UserInfoId, {
        method: 'PUT'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        debugger
        if (data.statusCode !== 200) {
            toastr.error('You can not accept the request!', 'Validation Error');
        }
        if (data.statusCode === 200) {
            if (friendRequestsCount == 1) 
                deleteRequestContainer();
            else
                deleteRequestRow(initiatorId);
            updateFriendsNumber();
            toastr.success('Friend Request Accepted!', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}


function toggleAddRequestToPending() {
    document.getElementById("addFriendBtn").remove();
    var friendContainer = document.getElementById("addRemoveFriendContainer");
    friendContainer.insertAdjacentHTML("afterbegin", "<p class= text-info>Pending Request</p>")
}
function toggleRemoveRequestToPending() {
    document.getElementById("removeFriendBtn").style.display = "none";
    document.getElementById("addFriendBtn").style.display = "inline-block";
    //var friendContainer = document.getElementById("addRemoveFriendContainer");
    //friendContainer.insertAdjacentHTML("afterbegin", "<button id='addFriendBtn' type='button' class='btn text-white button-addfriend' >Add Friend</button >");
}

function deleteRequestRow(initiatorId) {
    if (initiatorId !== null)
        document.getElementById("Req_" + initiatorId).remove();
}

function updateFriendsNumber() {
    debugger
    var result = document.getElementById("friendsNumber");

    result.innerText++; // Incrementing the number of friends for the user to be shown dynamically 
    console.log(result.innerText);
}

function deleteRequestContainer(){
    document.getElementById("requestsContainer").remove();
}