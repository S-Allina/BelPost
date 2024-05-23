"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var user = document.getElementById("IdReceiver").value;

const chatHub = connection.start().then(() => connection.invoke('LoadHistory', user));
connection.on('LoadHistory', (messages) => {
    console.log(messages)
    const messagesList = document.getElementById('messagesList');
    messages.forEach(message => {
        const li = document.createElement("li");
        li.classList.add("message-box");
        if (you[0] === message.senderName || you[1] === message.senderName) li.classList.add("left");
        else li.classList.add("right");
        li.textContent = `${message.text} `;
        messagesList.appendChild(li);
    });
   preventDefault();
});

var you = document.querySelector("#You").textContent.split(' ');





connection.on("ReceiveMessage", function (user, message, time) {
    var li = document.createElement("li");
    li.classList.add("message-box");
    if (you[0] === user || you[1] === user) li.classList.add("left");
    else li.classList.add("right");
    document.getElementById("messageInput").value = "";
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${message} `;
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("IdReceiver").value;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})




