function deletePost(postId) {
    const post = document.getElementById("post_" + postId);

    fetch("https://localhost:44340/Home/DeletePost?postId=" + postId, {
        method: "get"
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 400) {
            toastr.error('You can not delete this post', 'Validation Error');
        }
        if (data.statusCode === 200) {
            post.remove();
            toastr.success('Post has been Deleted Successfuly', 'Done');
        }

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function getPostById(postId) {
    const postContentTextArea = document.getElementById("postContentTextArea");
    const postIdTextBox = document.getElementById("postIdTextBox");

    fetch("https://localhost:44340/Home/GetPostById?postId=" + postId, {
        method: "get"
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 200) {
            postContentTextArea.value = data.responseMessage.postContent;
            postIdTextBox.value = data.responseMessage.postId;
            $('#editModel').modal("show");
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function EditPost() {
    const postContentTextArea = document.getElementById("postContentTextArea");
    const postIdTextBox = document.getElementById("postIdTextBox");
    const postContent = document.getElementById("postContent_" + postIdTextBox.value);

    fetch("https://localhost:44340/Home/EditPost", {
        method: "post",
        body: JSON.stringify({
            PostId: postIdTextBox.value,
            PostContent: postContentTextArea.value
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
            toastr.success('Post Edited Successfuly', 'Done');
            postContent.innerHTML = postContentTextArea.value;
            $('#editModel').modal("hide");
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}