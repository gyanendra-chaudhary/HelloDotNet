"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable the send button until connection is established.
$("#btnSendMessage").prop('disbled', true);


// start connection
connection.start().then(function () {
    $("#btnSendMessage").prop('disabled', false);
    alert('Connected to ChatHub');
}).catch(function () {
    return console.error(error.toString());
});


$("#btnSendMessage").click(function (e) {
    var user = $("#txtUser").val();
    var message = $("#txtMessage").val();
    connection.invoke("SendMessageToall", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    $("#txtMessage").val('');
    $("#txtMessage").focus();

    e.preventDefault();
});

connection.on("ReceiveMessage", function (user, message) {
    var content = `<b>${user} - </b>${message}`;
    $("#messageList").append(`<li>${content}</li>`);
});


