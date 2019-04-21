$(document).ready(function () {
    ShowContent();
})

function ShowContent() {
    //Gets The Total Amount Of Collabsible Items
    var collabsibleItems = document.getElementsByClassName("collapsible");

    for (var i = 0; i < collabsibleItems.length; i++) {
        //On Click Event, This Way We Don't Need OnClick On Every Item
        collabsibleItems[i].addEventListener("click", function () {
            this.classList.toggle("active");
            //Gets The Next Element In This Div, In This Case The Content Div
            var content = this.nextElementSibling;
            if (content.style.maxHeight) {
                content.style.maxHeight = null;
            } else {
                content.style.maxHeight = content.scrollHeight + "px";
            }
        });
    }
}