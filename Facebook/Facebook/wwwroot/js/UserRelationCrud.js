function rejectRequest(initiatorId, UserInfoId)
{
    fetch("https://localhost:44340/profile/rejectRequest?intiatorId=" + initiatorId + "&DeciderId=" + UserInfoId, {
        method: 'DELETE'
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode !== 200) {
            toastr.error('You can not reject', 'Validation Error');
        }
        if (data.statusCode === 200) {
            deleteRequestRow(initiatorId);
            toastr.success('Post has been Deleted Successfuly', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });


}

function deleteRequestRow(initiatorId) {
    if (initiatorId !== null)
        document.getElementById("Req_" + initiatorId).remove();
}