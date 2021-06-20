function renderMessages(data) {
    $('#messages').empty();

    for (let message of data) {
        $('#messages')
            .append('<div class="message d-flex justify-content-start"><strong>'
                + getUser()
                + '</strong>:'
                + message.content
                + '</div>')
    }
}

function loadMessages() {
    $.get({
        url: APP_SERVICE_URL + 'messages/all',
        success: function success(data){
            renderMessages(data);
        },
        error: function error(error){
            console.log(error);
        }
    });
}

function createMessage() {
    let username = getUser();
    let message = $('#message').val();

    if (username == null){
        alert('You cannot send a message before a choosing an username!');
        return;
    }

    if (message.length === 0){
        alert('You cannot send an empty message!');
        return;
    }

    $.post({
        url: APP_SERVICE_URL + 'messages/create',

        headers: {
            'Content-Type': 'application/json'
        },

        data: JSON.stringify({content:message, user: username}),

        success: function success(data){
            renderMessages(data);
            loadMessages();
        },

        error: function error(error){
            console.log(error);
        }
    })
}