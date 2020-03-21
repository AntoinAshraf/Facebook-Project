function upload(e) {

    console.log(e.target.value);
    var img = document.querySelector("#post-image");
    img.src = e.target.value;
    
}