function SendComment(postId) {
    const commentTextBox = document.getElementById("commentTextBox_" + postId);
    const sendBtn = document.getElementById("sendBtn_" + postId);
    const commentContainer = document.getElementById("commentContainer_" + postId);
    const commentNumber = document.getElementById("commentNumber_" + postId);

    if (commentTextBox.value === "") {
        commentTextBox.style.borderColor = "red";
        return toastr.error('Comment Can\'t be Empty.', 'Validation Error');
    }

    commentTextBox.style.borderColor = "rgb(229, 229, 229)";

    sendBtn.disabled = true;
    sendBtn.innerHTML = "Loading";

    fetch("https://localhost:44340/CommentLike/AddComment", {
        method: "post",
        body: JSON.stringify({
            PostId: postId,
            CommentContent: commentTextBox.value
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
            toastr.success('Comment has been Added Successfuly', 'Done');
            commentContainer.insertAdjacentHTML('afterbegin', '<li id="comment_' + data.responseMessage.commentId + '"><div class= "comment-list" style="color:firebrick"><div class="bg-img"><img class="friendListPic" src="' + data.responseMessage.profilePicUrl + '" alt=""></div><div class="comment"><h3><a href="~/Profile/Profile/' + data.responseMessage.userId + 'title="">' + data.responseMessage.fullName+'</a></h3><span><img src="images/clock.png" alt="">' + data.responseMessage.commentDate + '</span><p>' + data.responseMessage.commentContent + '</p></div><div class="comment"><i onclick = "deleteComment(' + data.responseMessage.commentId + ', ' + data.postId +')" class= "la la-trash" ></i ></div ></div></li>');
            commentTextBox.value = "";
            commentNumber.innerHTML = Number.parseInt(commentNumber.innerHTML) + 1;
        }

        sendBtn.disabled = false;
        sendBtn.innerHTML = "Send";

    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
        sendBtn.disabled = false;
        sendBtn.innerHTML = "Send";
    });
}


function SendLike(postId) {
    const likeNumber = document.getElementById("likeNumber_" + postId);
    const likesContainer = document.getElementById("likesContainer_" + postId);
    const heart = document.getElementById("heart_" + postId);

    fetch("https://localhost:44340/CommentLike/AddLike?postId=" + postId, {
        method: "get"
    }).then((response) => {
        return response.json();
    }).then((data) => {
        if (data.statusCode === 400) {
            toastr.error(data.responseMessage, 'Validation Error');
        }
        if (data.statusCode === 200) {
            if (data.isLike) {
                heart.style.color = "gray";
                toastr.success('Like has been Removed Successfuly', 'Done');
                likeNumber.innerHTML = Number.parseInt(likeNumber.innerHTML) - 1;
                document.getElementById("like_" + data.likeId).remove();
                if (likesContainer.innerHTML.trim() === "")
                    likesContainer.innerHTML = "There is no reactions for this post yet."
            } else {
                heart.style.color = "blue";
                toastr.success('Like has been Added Successfuly', 'Done');
                likeNumber.innerHTML = Number.parseInt(likeNumber.innerHTML) + 1;
                debugger
                var x = likesContainer.innerHTML.trim();
                if (likesContainer.innerHTML.trim() === "There is no reactions for this post yet.")
                    likesContainer.innerHTML = "";
                likesContainer.insertAdjacentHTML('afterbegin', '<div id="like_' + data.responseMessage.likeId + '" class="comment-list"><div class= "bg-img" ><img class="friendListPic" src="' + data.responseMessage.profilePicUrl + '" alt=""></div><div class="comment"><h3><a href="~/Profile/Profile/' + data.responseMessage.userId + '" title="">' + data.responseMessage.fullName + '</a></h3><span><img src="images/clock.png" alt="">' + data.responseMessage.likeDate + '</span></div></div>');
            }
        }
    }).catch((err) => {
        toastr.error("Something went wrong!", 'Validation Error');
    });
}

function deleteComment(commentId, postId) {
    const commentNumber = document.getElementById("commentNumber_" + postId);

    Swal.fire({
        title: 'Are you sure you want to delete this comment?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        allowOutsideClick: false,
    }).then((result) => {
        if (result.value) {
            fetch("https://localhost:44340/CommentLike/DeleteComment?commentId=" + commentId, {
                method: "get"
            }).then((response) => {
                return response.json();
            }).then((data) => {
                if (data.statusCode === 400) {
                    toastr.error("can not delete this comment", 'Validation Error');
                }
                if (data.statusCode === 200) {
                    debugger
                    toastr.success('Comment has been Deleted Successfuly', 'Done');
                    commentNumber.innerHTML = Number.parseInt(commentNumber.innerHTML) - 1;
                    document.getElementById("comment_" + commentId).remove();
                }
            }).catch((err) => {
                toastr.error("Something went wrong!", 'Validation Error');
            });
        }
    })
}