/// <reference path="Scripts/jquery-1.6.2-vsdoc.js" />

//document.getElementById("cucc").addEventListener("dragenter", folemhuztak);

//$("#cucc").live("dragstart", megrantottak);

//$("#doboz").live("dragenter", folemhuztak);


function iehack(event) {

    if (window.event.button == 1) {
        event.target.dragDrop();
    }

}



//dragstart
function megrantottak(event) {
    // event.target
    event.dataTransfer.setData("Text", $(event.target).attr("id"));
    event.dataTransfer.effectsAllowed = "all";
}

function elejtettek(event) {
    //alert('oops');
}

function huznak(event) {
}

//dragenter
function folemhuztak(event) {
    event.dataTransfer.dropEffect = "copy";
    var adat = event.dataTransfer.getData("Text");
    event.target.innerHTML = adat;
}

//dragleave
function elvittek(event) {
    event.target.innerHTML = "ide hozd";
}

function folottemvan(event) {
    event.preventDefault();
}

function ramdobtak(event) {
    var adat = event.dataTransfer.getData("Text");
    event.target.innerHTML = adat;
}
