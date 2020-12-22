$(document).ready(function () { 
    // Load Messages From Bottom
    $("#messages").scrollTop($("#messages")[0].scrollHeight);

    // Toggle Create Conversation Box
    $("#createConversation").click(function () {
        $("#blurForCreate").fadeIn();
        $("#createConversationBox").fadeIn();
        $("#searchUser").hide();
    });
    $("#closeCreateConversationBox").click(function () {
        $("#blurForCreate").fadeOut();
        $("#createConversationBox").fadeOut();
    });
    // Show Input For Searching User
    $("#singleConversation").click(function () {
        $("#searchUser").fadeIn();
    });
    // Create A New Conversation
    $(".getUserId").click(function (event) {
        // Get UserId That Wanted To Create Conversation With
        event.preventDefault();
        var index = $(".getUserId").index($(this));
        var userId = $(".getUserId").eq(index).attr('href');
        // Create A New Conversation
        let data = {
            'userId': userId
        };
        $.ajax({
            data: data,
            dataType: 'json',
            method: "POST",
            url: "/Messenger/NewConversation"
        }).done(function (response) {
            console.log(response);
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
});

// Auto Reset After 1 second
setInterval(function () {
    getConversations();
    getMessages();
}, 1000);

// GET CURRENT CONVERSATION FROM URL
var currentUrl = window.location.href;
var splittedUrl = currentUrl.split("/");
var currentConversationId = splittedUrl[splittedUrl.length - 1];
// GET CURRENT USER ID
var userID = document.getElementById("userID").value;

// Render Conversations
function getConversations() {
    $(document).ready(function () {
        $.ajax({
            dataType: 'json',
            method: "GET",
            url: "/Messenger/GetConversations"
        }).done(function (response) {
            var conversations = "";
            response.forEach(function (value, index, array) {
                if (value['conversationID'] == currentConversationId) {
                    $("#sendToUser").html(value['userFullName']);
                    conversations += "<a href='./" + value['conversationID'] + "'>"
                        + "<li class='conversation active'>"
                        + "<img src='https://localhost:44341/images/" + value['userAvatar'] + "' alt='avatar' />"
                        + "<span class='username'>" + value['userFullName'] + "</span>"
                        + "<span class='time'>2:09 PM</span>"
                        + "<span class='preview'>I was wondering...</span>"
                        + "</li>"
                        + "</a>";
                } else {
                    conversations += "<a href='./" + value['conversationID'] + "'>"
                        + "<li class='conversation'>"
                        + "<img src='https://localhost:44341/images/" + value['userAvatar'] + "' alt='avatar' />"
                        + "<span class='username'>" + value['userFullName'] + "</span>"
                        + "<span class='time'>2:09 PM</span>"
                        + "<span class='preview'>I was wondering...</span>"
                        + "</li>"
                        + "</a>";
                }
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
            url: "/Messenger/Conversation/" + currentConversationId,
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
                            "<p>" + messObj[j]['grouppedMessages'][x]['senderMessage'] + "</p>"
                            + "<br>";
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
                            "<p>" + messObj[j]['grouppedMessages'][x]['senderMessage'] + "</p>"
                            + "<br>";
                    }
                    messages +=
                        "</div>"
                        + "</div>";
                }
            }
            $("#messages").html(messages);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown);
        });
    });
}

