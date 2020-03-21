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
        postTopBar.insertAdjacentHTML("afterbegin","<div class='usy-dt col-10' id='usy-dt'></div>");

        var usyDt = document.querySelector("#usy-dt");


        usyDt.insertAdjacentHTML("afterbegin", "<img id='pImg'></img>");
        var imgSrc = document.querySelector("#pImg");
        imgSrc.src = "/images/person.png";
        imgSrc.style.width = "10%";
        imgSrc.style.height = "auto";
           
        if (item.urlUser!==null) {
           
            console.log(imgSrc);
            imgSrc.src =Context.AddFileVersionToPath("/images/" + item.urluser);
            
            alert(imgSrc.src);
        }

        usyDt.insertAdjacentHTML("beforeend", "<div class='usy-name' id='usy-name'></div>");
        var usyName = document.querySelector("#usy-name");
        usyName.insertAdjacentHTML("beforeend", "<h3>" + item.fullName + "</h3>");
        var data = new Date( item.createdAt);
        console.log(typeof data);
        console.log(data.getDate() + "-" + data.getMonth() + "-" + data.getFullYear());
        usyName.insertAdjacentHTML("beforeend", " <span> <i class='far fa-clock'></i>" + data.getDate() + "-" + data.getMonth() + "-" + data.getFullYear() + " </span>");
        var id = item.id;
        if (item.owner === true) {
            postTopBar.insertAdjacentHTML("beforeend",
                "<div class='ed-opts'><a  href = ''  class= 'ed-opts-open' > <i class='la la-ellipsis-v'> </i> </a><ul class='ed-options'><li><a data-target='EditModal' href='/Home/EditPost/" + id + "'>Edit Post</a></li><li><a href='/Home/DeletePost/" + id + "' > Delete Post</a ></li ></ul ></div > ");
        }
            postBar.insertAdjacentHTML("beforeend", "<div class='job_descp'><p>" + item.postContent + "</p></div >");

        var AllImagDiv = document.createElement("div");
        postBar.insertAdjacentElement("beforeend", AllImagDiv);
        for (var i = 0; i < item.urlPost.length; i++) {
            AllImagDiv.insertAdjacentHTML("beforeend", "<img src=" + "/images/" + item.urlPost[i] + " >");
            var AllImage = AllImagDiv.lastChild;
            AllImage.style.width = "15%";
            AllImage.style.margin = "5px";
        }
       
       
        //AllImage.forEach(function (item) {
        //    item.style.width = "21%";
        //    item.style.height = "auto";
        //});

        
    });

    

  

}
