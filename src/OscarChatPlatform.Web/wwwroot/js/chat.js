"use strict";


const currentUser = {
    UserId: sessionId,
    Username: 'Pippo'
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.on("ReceiveMessage", function (user, message) {

    console.log("Entro")

    // Display the message on the main screen
    displayMessageBasedOnSender(sessionId, user, message)
})

connection.start().then(function () {
    connection.invoke("RegisterUser", currentUser);
}).catch(function (err) {
    console.error("Errore di connessione: ", err.toString());
    // Mostra un messaggio di errore all'utente
});

$("#sendMessage").on("click", function () {
    const message = $("#textMessage").val();
    if (message.trim() !== "") {
        
        connection.invoke("SendMessage", currentUser, message).catch(function (err) {
            console.error("Errore nell'invio del messaggio: ", err.toString());
            // Mostra un messaggio di errore all'utente
        });
    }
});

function displayMessageBasedOnSender(userId, senderUser, message, timestamp) {
    const time = new Date(timestamp).toLocaleTimeString();
    if (userId === senderUser.userId)
        $(".chat-container").append(`<div class="message mb-3 text-end">
                    <div class="message-sender">Tu</div>
                        <div class="message-content bg-primary text-white p-3 rounded">
                        ${message}
                        </div>
                        <small class="text-muted">${time}</small>
                    </div>`)
    else
        $(".chat-container").append(`<div class="message mb-3">
                    <div class="message-sender">${senderUser.username}</div>
                    <div class="message-content bg-light p-3 rounded">
                        ${message}
                    </div>
                    <small class="text-muted">${time}</small>
                </div>`)
}
