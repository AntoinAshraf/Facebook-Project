    var pp;
           fetch("https://localhost:44340/Home/GetPosts", {
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




function createPosts(rr) {
        console.log(rr);
    var postsDiv = document.querySelector("#posts");
    rr.forEach(function (item) {
        console.log(item);
        var posty = postsDiv.insertAdjacentHTML("afterbegin", "<div class='posty posty-edit' id='posty'></div>");

        var postyTag = document.querySelector("#posty");
        postyTag.insertAdjacentHTML("afterbegin", "<div class='post-bar no-margin' id='post-bar'></div>");

        var postBar = document.querySelector("#post-bar");
        postBar.insertAdjacentHTML("afterbegin", "<div class='post_topbar' id='post_topbar'></div>");

        var postTopBar = document.querySelector("#post_topbar");
        postTopBar.insertAdjacentHTML("afterbegin","<div class='usy-dt' id='usy-dt'></div>");

        var usyDt = document.querySelector("#usy-dt");
       
     
        if (item.url!==null) {
            usyDt.insertAdjacentHTML("afterbegin", "<img id='pImg'></img>");
            imgSrc = document.querySelector("#pImg");
            imgSrc.src = item.Url;
        }

        usyDt.insertAdjacentHTML("beforeend", "<div class='usy-name' id='usy-name'></div>");
        var usyName = document.querySelector("#usy-name");
        usyName.insertAdjacentHTML("beforeend", "<h3>" + item.fullName + "</h3>");
        usyName.insertAdjacentHTML("beforeend", " <span> <i class='far fa-clock'></i>" + Date.parse(item.createdAt) + " ago </span>");
        var id = item.id;
        postTopBar.insertAdjacentHTML("beforeend",
            "<div class='ed-opts'><a id='' href = '#' title = '' class= 'ed-opts-open' > <i class='la la-ellipsis-v'> </i> </a><ul class='ed-options'><li><a data-target='EditModal' href='/Home/EditPost/" + id + "'>Edit Post</a></li><li><a href='/Home/DeletePost/" + id + "' > Delete Post</a ></li ></ul ></div > ");
       postBar.insertAdjacentHTML("beforeend", "<div class='job_descp'><p>"+item.postContent+"</p></div >");

        
    });
  

}
