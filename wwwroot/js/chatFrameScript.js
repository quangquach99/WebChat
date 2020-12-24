$(document).ready(function () {
    // Close Notification
    $("#closeNotification").click(function () {
        $("#notification").fadeOut();
    });

    // Toggle Create Conversation Box
    $("#createConversation").click(function () {
        $("#blurForCreate").fadeIn();
        $("#createConversationBox").fadeIn();
        $("#searchUser").hide();
    });
    $("#closeCreateConversationBox").click(function () {
        $("#blurForCreate").fadeOut();
        $("#createConversationBox").fadeOut();
        $("#searchUserResults").fadeOut();
    });

    // Show Input For Searching User
    $("#singleConversation").click(function () {
        $("#searchUser").fadeIn();
    });

    // When User Click Send Message
    $("#sendMessageBtn").click(function () {
        newMessage();
    });

    //When User Press Enter
    $("#sendInput").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            newMessage();
        }
        //Stop the event from propogation to other handlers
        //If this line will be removed, then keypress event handler attached 
        //at document level will also be triggered
        event.stopPropagation();
    });

    // Search Users For Creating New Conversation
    $("#searchUser").keyup(function () {
        // GET THE HINT STRING
        var searchHint = $(this).val();
        console.log(searchHint);
        // CALLING AJAX TO GET MATCHED RECORDS
        $.ajax({
            dataType: 'json',
            method: "GET",
            url: "/Messenger/SearchUsers?searchHint=" + searchHint
        }).done(function (response) {
            var searchUserResult = "";
            if (response == null) {
                searchUserResult = "No Record!!!";
                $("#searchUserResults").fadeOut();
            } else {
                response.forEach(function (value, index, array) {
                    searchUserResult +=
                        "<div class='searchResult'>"
                        + "<img src='https://localhost:44341/images/" + value['userAvatar'] + "' alt='avatar'>"
                        + "<span class='conversationName'>" + value['userFullname'] + "</span>"
                        + "<a href='#'><i class='fas fa-user-circle'></i></a>"
                        + "<a class='getUserId' href='https://localhost:44341/Messenger/NewConversation?userId=" + value['userID'] + "'>"
                        + "<i class='fab fa-facebook-messenger'></i>"
                        + "</a>"
                        + "</div>";
                });
                $("#searchUserResults").fadeIn();
                $("#searchUserResult").html(searchUserResult);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ': ' + errorThrown);
        });
    });

    //Search conversation current user
    $("#search").keyup(function () {
        // Checks value the input
        if (this.value.length != 0) {
            var NameUser = this.value;
            let data = {
                'nameUser': NameUser
            }
            $.ajax({
                data: data,
                dataType: "Json",
                method: "POST",
                url: "/Messenger/SearchConversation"
            }).done(function (response) {
                if (response != NameUser) {
                    var conversations = "";
                    response.forEach(function (value, index, array) {
                        conversations += "<div class='searchResult'>"
                            + "<img src='https://localhost:44341/images/ " + value['userAvatar'] + "' alt='avatar'>"
                            + "<span class='conversationName'>" + value['userFullName'] + "</span > "
                            + "<a href='./" + value['conversationID'] + "'>"
                            + "<i class='fas fa-user-circle'></i>"
                            + "</a>"
                            + "<a class='getUserId' href='./'" + value['UserID'] + ">"
                            + "<i class='fab fa-facebook-messenger'></i>"
                            + "</a>"
                            + "</div>";
                    });
                } else {
                    conversations = "No matching results were found";
                }
                //$(".searchResults").css({ 'display': ''});
                $("#searchResults").html(conversations);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            });
        } else {
            $("#searchResults").empty();
        }     
    });

    // Initial Messages For Chat Frame
    getMessages();
    getConversations();
});

// Auto Reset After 1 second
setInterval(function () {
    checkConversation();
    checkNewMessage();
}, 1000);

// GET CURRENT CONVERSATION FROM URL
var currentUrl = window.location.href;
var splittedUrl = currentUrl.split("/");
var currentConversationId = splittedUrl[splittedUrl.length - 1];
// GET CURRENT USER ID
var userID = document.getElementById("userID").value;

// Temporaryly saved conversation status
var conversationsArray = [];

// Render Conversations
function getConversations() {
    $(document).ready(function () {
        $.ajax({
            dataType: 'json',
            method: "GET",
            url: "/Messenger/GetConversations"
        }).done(function (response) {
            var conversations = "";
            conversationsArray = [];
            response.forEach(function (value, index, array) {
                if (value['conversationID'] == currentConversationId) {
                    $("#sendToUser").html(value['userFullName']);
                    conversations += "<a href='./" + value['conversationID'] + "'>";
                    if (value['isSeen'] == 0) {
                        conversations += "<li class='conversation new active'>";
                    } else {
                        conversations += "<li class='conversation active'>";
                    }
                    conversations +=                    
                        "<img src='https://localhost:44341/images/" + value['userAvatar'] + "' alt='avatar' />"
                        + "<span class='username'>" + value['userFullName'] + "</span>"
                        + "<span class='time'>" + value['lastMessageTime'] + "</span>"
                        + "<span class='preview'>" + value['lastMessage'] + "</span>"
                        + "</li>"
                        + "</a>";
                } else {
                    conversations += "<a href='./" + value['conversationID'] + "'>";
                    if (value['isSeen'] == 0) {
                        conversations += "<li class='conversation new'>";
                    } else {
                        conversations += "<li class='conversation'>";
                    }
                    conversations +=
                        "<img src='https://localhost:44341/images/" + value['userAvatar'] + "' alt='avatar' />"
                        + "<span class='username'>" + value['userFullName'] + "</span>"
                        + "<span class='time'>" + value['lastMessageTime'] + "</span>"
                        + "<span class='preview'>" + value['lastMessage'] + "</span>"
                        + "</li>"
                        + "</a>";
                }
                var conversationStatus = {
                    'conversationID': value['conversationID'],
                    'isSeen' : value['isSeen']
                };
                conversationsArray.push(conversationStatus);
            });
            $("#conversationList").html(conversations);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown);
        });
    });
}

// Render Messages
function getMessages() {
    $(document).ready(function () {
        // GET ALL MESSAGES FROM THE CONVERSATION SHAPE OF MessagesViewModel
        $.ajax({
            dataType: 'json',
            method: "GET",
            url: "/Messenger/Conversation/" + currentConversationId
        }).done(function (response) {
            // INITIAL THE FIRST RECORD
            var messObj = [
                {
                    conversationID: response[0]['conversationID'],
                    senderID: response[0]['senderID'],
                    senderAvatar: response[0]['senderAvatar'],
                    grouppedMessages: [
                        {
                            senderMessage: response[0]['senderMessage'],
                            messagedTime: response[0]['messagedTime']
                        }
                    ]
                }
            ];
            // SAVE ALL DATA RESPONSE INTO AN OBJECT
            var count = 0;
            for (var i = 0; i < response.length - 1; i++) {
                if (response[i + 1]['senderID'] == response[i]['senderID']) {
                    var tempObj = {
                        senderMessage: response[i + 1]['senderMessage'],
                        messagedTime: response[i + 1]['messagedTime']
                    };
                    messObj[count]['grouppedMessages'].push(tempObj);
                } else {
                    var tempObj = {
                        conversationID: response[i + 1]['conversationID'],
                        senderID: response[i + 1]['senderID'],
                        senderAvatar: response[i + 1]['senderAvatar'],
                        grouppedMessages: [
                            {
                                senderMessage: response[i + 1]['senderMessage'],
                                messagedTime: response[i + 1]['messagedTime']
                            }
                        ]
                    }
                    messObj.push(tempObj);
                    count++;
                }
            }
            // RENDER TO HTML
            var messages = "";
            for (var j = 0; j < messObj.length; j++) {
                if (messObj[j]['senderID'] == userID) {
                    messages +=
                        "<div class='messageRow right-message'>"
                        + "<div class='message'>";
                    for (var x = 0; x < messObj[j]['grouppedMessages'].length; x++) {
                        messages +=
                            "<div class='detail'>"
                            + "<p>" + messObj[j]['grouppedMessages'][x]['senderMessage'] + "</p>"
                            + "</div>";
                    }
                    messages +=
                        "</div>"
                        + "</div>";
                } else {
                    messages +=
                        "<div class='messageRow left-message'>"
                            + "<a href='#'>"
                                + "<img src='https://localhost:44341/images/" + messObj[j]['senderAvatar'] + "'>"
                            + "</a>"
                        + "<div class='message'>";
                    for (var x = 0; x < messObj[j]['grouppedMessages'].length; x++) {
                        messages +=
                            "<div class='detail'>"
                            + "<p>" + messObj[j]['grouppedMessages'][x]['senderMessage'] + "</p>"
                            + "</div>";
                    }
                    messages +=
                        "</div>"
                        + "</div>";
                }
            }
            $("#messages").html(messages);
            // Load Messages From Bottom
            $("#messages").scrollTop($("#messages")[0].scrollHeight);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown);
        });
    });
}

// Send New Message
function newMessage() {
    $(document).ready(function () {
        // Get Message Value And Check For Validations
        var message = $("#sendInput").val();
        if (message == "") {
            $("#notificationMessage").addClass("text-danger");
            $("#notificationMessage").html("Message Can Not Be Empty!");
            $("#notification").fadeIn();
        } else if (message.length > 8000) {
            $("#notificationMessage").addClass("text-danger");
            $("#notificationMessage").html("You Have Exceeded The Limit Characters!");
            $("#notification").fadeIn();
        } else {
            // Encode
            var encodedMessage = message.replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;");
            console.log(encodedMessage);
            // Ajax For Calling MessengerController To Save New Message
            var data = {
                'message': encodedMessage,
                'conversationID': currentConversationId
            }
            $.ajax({
                data: data,
                dataType: "Json",
                method: "POST",
                url: "/Messenger/NewMessage"
            }).done(function (response) {
                $("#sendInput").val("");
                getMessages();
                getConversations();
            }).fail(function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus + ': ' + errorThrown);
            });
        } 
    });
}

// Check New Message
function checkNewMessage() {
    $(document).ready(function () {
        $.ajax({
            dataType: "Json",
            method: "GET",
            url: "/Messenger/CheckNewMessage?conversationID=" + currentConversationId
        }).done(function (response) {
            if (response == 1) {
                getMessages();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown);
        });;
    });
}

// Check Status For Conversation
function checkConversation() {
    $.ajax({
        dataType: 'json',
        method: "GET",
        url: "/Messenger/GetConversations"
    }).done(function (response) {
        console.log("response: ");
        console.log(response);
        console.log("status: ");
        console.log(conversationsArray);
        var refesh = 0;
        if (response.length != conversationsArray.length) {
            refesh = 1;
        } else {
            for (var i = 0; i < response.length; i++) {
                if (response[i]['conversationID'] != conversationsArray[i]['conversationID']) {
                    refresh = 1;
                } else {
                    if (response[i]['isSeen'] != conversationsArray[i]['isSeen']) {
                        refesh = 1;
                        break;
                    }
                }
            }
        }
        if (refesh == 1) {
            getConversations();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        alert(textStatus + ': ' + errorThrown);
    });
}