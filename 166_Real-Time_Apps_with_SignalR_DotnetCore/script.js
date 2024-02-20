document.getElementById("joinRoom").addEventListener("click", function () {
    const roomName = document.getElementById("roomName").value;
    const userName = document.getElementById("userName").value;
    connection.invoke("JoinRoom", userName, roomName);
});

document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.key === "Enter") {
        const message = document.getElementById("messageInput").value;
        const roomName = document.getElementById("roomName").value;
        if (message && roomName) {
            connection.invoke("SendMessageToRoom", roomName, message);
        }
    }
});

connection.on("ReceiveMessage", function (msg) {
    const messages = document.getElementById("messages");
    messages.innerHTML += `<p>${msg.user}: ${msg.text}</p>`;
});

connection.on("UserJoined", function (msg) {
    const messages = document.getElementById("messages");
    messages.innerHTML += `<p>${msg} has joined.</p>`;
});

connection.on("UserLeft", function (msg) {
    const messages = document.getElementById("messages");
    messages.innerHTML += `<p>${msg}has left.</p>`;
});