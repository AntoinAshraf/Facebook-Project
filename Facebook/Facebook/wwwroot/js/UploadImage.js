
var flag = true;
var imgTag = null;
function upload(e) {
    if (flag) {
        console.log(e.target.value);

        var imgCategory = document.querySelector("#image-category");

        var createDiv = document.createElement("div");
        createDiv.classList.add("col-3");

        imgTag = document.createElement("img");
        imgTag.src = URL.createObjectURL(e.target.files[0]);
        imgTag.style.width = "50px";
        imgTag.style.height = "50px";
        var iTag = document.createElement("i");
        iTag.classList.add("la-times-circle-o");
        iTag.classList.add("la");
        iTag.classList.add("icon-img");

        createDiv.appendChild(imgTag);
        createDiv.insertAdjacentElement("beforeend", iTag);
        imgCategory.append(createDiv);
        //var img = document.querySelector("#post-image");
        //img.src = URL.createObjectURL( e.target.files[0]);
        flag = false;
    }
    else {
        imgTag.src = URL.createObjectURL(e.target.files[0]);
    }
    
}

