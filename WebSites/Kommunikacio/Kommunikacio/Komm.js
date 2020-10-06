/// <reference path="Scripts/jquery-3.1.1.js" />

if (!window.WebSocket)
    alert("A WebSocet elérhetetlen!");


$(document).on("click", "#openbutton", openClick);
$(document).on("click", "#closeButton", closeClick);
var socket;

function openClick() {
    var url = "ws://sockets.itfactory.hu/demo/chat.ashx";
    socket = new WebSocket(url);

    socket.onopen = socketOpened;
    socket.onerror = socketError;
    socket.
}

function socketOpened() {
    $("#status").text("csatlakozva");
}

function socketError() {
    $("#status").text("hiba");
}

function closeClick() {
    socket.close();

}