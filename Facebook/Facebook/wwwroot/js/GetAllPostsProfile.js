var pp;
fetch(window.location.origin+"/Profile/GetPosts", {
    method: "get"
}).then((response) => {

    return response.json();
}).then((data) => {

    if (data.statusCode === 404) {
        toastr.error(data.responseMessage, 'Validation Error');
    }
    if (data.statusCode === 200) {
        toastr.success('Successfully', 'Done');
        createPosts(data.responseMessage);

        //window.location.href = "/";
    }
}).catch((err) => {
    debugger;
});