"use strict";

const SignalR_URL = "/chatHub";


var connection = new signalR.HubConnectionBuilder()
    .withUrl(SignalR_URL)
    .build();


function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).ready(function () {
    const userId = getCookie("UserId");

    connection.start().then(function () {
        connection.invoke("AppendConnectionIdToUser", userId)
    }).catch(function (err) {
        console.error("Errore di connessione: ", err.toString());
    });
    $(".start-chat-btn").on("click", function () {
        $("#loadingPopup").css("display", "flex");
        connection.invoke("JoinChat", userId).catch(function (err) {
            console.error("Errore JoinChat", err.toString());

        });
    })

    connection.on("JoinRoom", function (chatId) {
        window.location.href = "/chat/" + chatId;
    })

    $("#sendButton").on("click", function () {
        const message = $("#chatInput").val();

        if (message.trim() !== "") {
            const chatId = window.location.pathname.split('/')[2] // Gets the chatId
            connection.invoke("SendMessage", chatId, message);

            $("#chatInput").val(""); // Empties the message text input
        }
    });

    // Event when current user or another user send a message
    connection.on("ReceiveMessage", function (message, receivedUserId) {

        // If the ids corrispon I'm the one who sent the message
        const amISender = receivedUserId == userId

        const messageDivClass = amISender ? "message sent" : "message received"

        let $messageDiv = $("<div>", { "class": messageDivClass });
        let $messageText = $("<p>").text(message);
        let $messageTime = $("<span>", { "class": "message-time" });
        $messageTime.text(new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }));

        console.log($messageTime)


        $("#chatMessages").append($messageDiv.append($messageText).append($messageTime));
    })

    connection.onclose(function (e) {
        console.log(e)
        connection.start().then(function () {
            connection.invoke("AppendConnectionIdToUser", userId)
        }).catch(function (err) {
            console.error("Errore di connessione: ", err.toString());
        });
    });

    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#sendButton').click();
            return false;
        }
    });   

});

