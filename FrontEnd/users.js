function login() {
    let username = $('#username-login').val();
    let password = $('#password-login').val();

    $('#username-login').val('');
    $('#password-login').val('');

    let requestBody = {
        username: username,
        password: password
    };

    $('#guest-navbar').hide();
    $('#caption').text('Welcome to Chat-Inc!');
    hideLoginAndRegisterAndShowLoggedInData();


    $.post({
        url: APP_SERVICE_URL + 'users/login',
        headers: {'Content-Type': 'application/json'},
        data: JSON.stringify(requestBody),
        success: function (data) {
            hideGuestNavbar();
            $('#caption').text('Welcome to Chat-Inc!');
            let token = data.rawHeader
                + '.'
                + data.rawPayload
                + '.'
                + data.rawSignature;

            saveToken(token);

            $('#username-logged-in').text(getUser());

            hideLoginAndRegisterAndShowLoggedInData();

            loadMessages()
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function register() {
    let username = $('#username-register').val();
    let password = $('#password-register').val();

    $('#username-register').val('');
    $('#password-register').val('');

    let requestBody = {
        username: username,
        password: password
    };

    $.post({
        url: APP_SERVICE_URL + 'users/register',
        headers: {'Content-Type': 'application/json'},
        data: JSON.stringify(requestBody),
        success: function (data) {
            toggleLogin();
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function hideGuestNavbar() {
    $('#guest-navbar')
        .removeClass('d-block')
        .addClass('d-none');
}

function showGuestNavbar() {
    $('#guest-navbar')
        .removeClass('d-none')
        .addClass('d-block');
}

function toggleLogin() {
    $('#login-data').show();
    $('#register-data').hide();
}

function toggleRegister() {
    $('#login-data').hide();
    $('#register-data').show();
}

function hideLoginAndRegisterAndShowLoggedInData() {
    $('#login-data').hide();
    $('#register-data').hide();

    $('#logged-in-data').show();
}

function showLoginAndHideLoggedInData() {
    toggleLogin();

    $('#logged-in-data').hide();
}

function logout() {
    $('#caption').text('Choose your username to begin chatting!');

    showGuestNavbar()

    showLoginAndHideLoggedInData();
}

function saveToken(token) {
    localStorage.setItem('auth_token', token);
}

function evictToken() {
    localStorage.removeItem('auth_token', token);
}

function getUser() {
    let token = localStorage.getItem('auth_token');

    let claims = token.split('.')[1];
    let decodedClaims = atob(claims);
    let parsedClaims = JSON.parse(decodedClaims);

    return parsedClaims.nameid;
}

function isLoggedIn(){
    return localStorage.getItem('auth_token') != null;
}

$('#logged-in-data').hide();
toggleLogin();
