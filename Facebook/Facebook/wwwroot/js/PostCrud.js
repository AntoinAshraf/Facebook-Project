function deletePost(postId) {
    Swal.fire({
        title: 'Are you sure you want to delete this post?',
        text: "",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        allowOutsideClick: false,
    }).then((result) => {
        if (result.value) {
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
    })
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

    if (postContentTextArea.value === "") {
        postContentTextArea.style.borderColor = "red";
        return toastr.error("Post content can't be Empty.", 'Validation Error');
    }

    postContentTextArea.style.borderColor = "rgb(229, 229, 229)";

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

function createPost() {
    const postTextArea = document.getElementById("postTextArea");
    const postImage = document.getElementById("postImage");
    const postsContainer = document.getElementById("posts");

    if (postTextArea.value === "") {
        postTextArea.style.borderColor = "red";
        return toastr.error("Post content can't be Empty.", 'Validation Error');
    }

    postTextArea.style.borderColor = "rgb(229, 229, 229)";

    if (postImage.value !== "") {
        const file_ext = postImage.value.substr(postImage.value.lastIndexOf('.') + 1, postImage.value.length);
        if (file_ext !== "jpg" && file_ext !== "png") {
            return toastr.error("Image Extention must be jpg or png.", 'Validation Error');
        }
    }

    var formData = new FormData();
    var file = postImage.files[0];
    formData.append("postImage", file);
    formData.append("postText", postTextArea.value);

    $.ajax({
        url: 'https://localhost:44340/Home/CreatePost',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.statusCode === 400) {
                toastr.error(result.responseMessage, 'Validation Error');
            }
            if (result.statusCode === 200) {
                toastr.success('Post Added Successfuly', 'Done');
                $('#myModal').modal("hide");
                window.location.href = "/Home/Index";
                //postsContainer.insertAdjacentHTML('afterbegin', '<div id="post_' + result.responseMessage.postId + '" class="posty"><div class= "post-bar no-margin"><div class="post_topbar"><div class="usy-dt"><img class="friendListPic" src="' + result.responseMessage.profilePic + '" alt=""><div class="usy-name"><h3>' + result.responseMessage.fullName + '</h3><span><img src="images/clock.png" alt="">' + result.responseMessage.postDate + '</span></div></div><div class="ed-opts"><a title="" href="#" class="ed-opts-open"><i class="la la-ellipsis-v"></i></a><ul class="ed-options"><li><a onclick="getPostById(' + result.responseMessage.postId + ')" title="">Edit Post</a></li><li><a onclick="deletePost(' + result.responseMessage.postId + ')" title="">Delete Post</a></li></ul></div></div><div class="job_descp"><p id="postContent_' + result.responseMessage.postId + '">' + result.responseMessage.postContent + '</p><img style="margin-left:23%; width:250px" src="' + result.responseMessage.postPicUrl + '" /></div><div class="job-status-bar"><ul class="like-com"><li><a><i id="heart_' + result.responseMessage.postId + '" onclick="SendLike(' + result.responseMessage.postId + ')" style="color:gray" class="la la-heart"></i><button style="color:#076bec; border:none; background-color:white;" type="button" data-toggle="modal" data-target="#exampleModal_' + result.responseMessage.postId + '">Like</button></a><img src="images/liked-img.png" alt=""><span id="likeNumber_' + result.responseMessage.postId + '" style="margin-left:1px">0</span></li><li><a style="margin-top:16px" title="" class="com"><img src="images/com.png" alt=""> Comment </a><span id="commentNumber_' + result.responseMessage.postId + '" style="margin-left:1px">0</span></li></ul></div></div><!--post-bar end--><div class="comment-section"><div class="comment-sec"><ul id="commentContainer_' + result.responseMessage.postId + '"></ul></div><!--comment-sec end--><div class="post-comment"><div class="comment_box"><form><input id="commentTextBox_' + result.responseMessage.postId + '" type="text" placeholder="Post a comment"><button id="sendBtn_' + result.responseMessage.postId + '" type="button" onclick="SendComment(' + result.responseMessage.postId + ')">Send</button></form></div></div><!--post-comment end--></div><!--comment-section end--></div><div class="modal fade" id="exampleModal_' + result.responseMessage.postId + '" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true"><div class="modal-dialog" role="document"><div class="modal-content"><div class="modal-header"><h5 class="modal-title" id="exampleModalLabel">Likes</h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button></div><div id="likesContainer_' + result.responseMessage.postId+'" class="modal-body">There is no reactions for this post yet .</div><div class="modal-footer"><button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button></div></div></div></div>');
            }
        },
        error: function () {
            toastr.error("Something went wrong!", 'Validation Error');
        }
    });
}