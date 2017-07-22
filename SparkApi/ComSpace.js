

process.env['public_address'] = 'https://example.ngrok.io';
process.env['access_token'] = 'User access_token';
process.env['TCP_PORT'] = '8000';
process.env['HTTP_PORT'] = '3000';
process.env['BOT_ADRESS'] = '__Name@sparkbot.io';
process.env['webhock_id'] = '';
process.env['event_name'] = '';

var net = require('net');
var http = require('http')  
var express = require('express')
var bodyParser = require('body-parser')
var CiscoSparkClient = require('./node_modules/node-sparkclient/lib/CiscoSparkClient')


var accessToken = process.env.access_token;
var sparkClient = new CiscoSparkClient(accessToken);
// Create a new instance of express
var app = express();

/* 
this is not neccessery if the script is 
runing in web server whith an adress web ex: www.example.com.
The webhock is created in online CiscoSpark developper and we get the id and name to update the public adrees 
each time we restart the [ngrok http 3000].
process.env['webhock_id'] = 'Y2lzY29zcGFyazovL3VzL1dFQkhPT0svNjVkZTQ4NzEtNTg0MS00NTVkLWEzNzYtNjg3ZmQwMTMxNThl';
process.env['event_name'] = 'messages_created';
*/
sparkClient.updateWebhook(process.env.webhock_id, process.env.event_name, process.env.public_address, function (err, webhook) {
    if (!err) {
        console.log("update Webhook is done !!");
    }
})


var clientCsharp;
var tcp_server = net.createServer(function (socket) {

    clientCsharp = socket;
    clientCsharp.nickname = "Game";
    socket.on('data', function (data) {
       SendRequest(data);
    });

    socket.on('close', function (data) {
        console.log('CLOSED: ' + clientCsharp.remoteAddress + ' ' + clientCsharp.remotePort);
    });
});

var SendRequest = function (data) {
    // Send message to the BOT_ADRESS
    sparkClient.createMessage(process.env.BOT_ADRESS, data.toString(), function (err, response) {
        if (err)
            console.dir(err);
    });
}

var SendResponse = function (message) {
    // Send reponse to the ClientCsharp. The '\r\n' specifiy the end of message
    clientCsharp.write(message + '\r\n');
}

tcp_server.listen(process.env.TCP_PORT);


// support json encoded bodies
app.use(bodyParser.json());
// Tell express to use the body-parser middleware and to not parse extended bodies
app.use(bodyParser.urlencoded({ extended: false }))

// Route that receives a POST request to / from Cisco spark
app.post('/', function (req, res) {
    var body = req.body;
    sparkClient.getMessage(body.data.id, function (err, message) {

        /*
        message.text structure
        {
        "msg_id":"0YF2Afw32mdXBJhsZ",
        "_text":"Message send to the BOT",
        "message":"Message receive from the BOT",
        "entities":
        {
        "actor":[{"confidence":0.97605987241175,"value":"Actor1","type":"value"}],
        "position":[{"confidence":0.90240428601974,"value":"A","type":"value"}],
        "intent":[{"confidence":0.78524527113668,"value":"Moving"}],
        "greetings":[{"confidence":0.99271398783805,"value":"true"}]
        }
        }
        */
        if (!err) {

            /*
            normalised the messageFormated to be consumed from ClientCsharp 
            message value#Actor value, ... #position value, ...#intent value.
            */
            var messageFormated = "";
            // check only message that comme from the BOT
            if (message.personEmail == process.env.BOT_ADRESS) {
                var messageJson = JSON.parse(message.text);
                // console.log(messageJson._text);

                if (typeof messageJson.message != "undefined") {
                    messageFormated = messageJson.message.replace(/\n/g, '. ')
                    messageFormated += "#"
                } else {
                    messageFormated += "NaN#";
                }
                // actor is an array 
                if (typeof messageJson.entities.actor != "undefined") {
                    for (var i = 0; i < messageJson.entities.actor.length; ++i) {
                        // console.log(' A ' + messageJson.entities.actor[i].value);
                        messageFormated += messageJson.entities.actor[i].value;
                        if (i < messageJson.entities.actor.length - 1)
                            messageFormated += ",";
                    }
                    messageFormated += "#";
                } else {
                    messageFormated += "NaN#";
                }
                // position is an array
                if (typeof messageJson.entities.position != "undefined") {
                    for (var i = 0; i < messageJson.entities.position.length; ++i) {
                        //  console.log(' P ' + messageJson.entities.position[i].value);
                        messageFormated += messageJson.entities.position[i].value
                        if (i < messageJson.entities.position.length - 1)
                            messageFormated += ",";
                    }
                    messageFormated += "#";
                } else {
                    messageFormated += "NaN#";
                }
                // intent is an array
                // This example is limited to one intent Moving
                if (typeof messageJson.entities.intent != "undefined") {
                    // do traitment to manage several intent such as Hide, MovingLef, MovingRight, Attak ...
                    for (var i = 0; i < messageJson.entities.intent.length; ++i) {
                        //  console.log(' P ' + messageJson.entities.position[i].value);
                        messageFormated += messageJson.entities.intent[i].value
                        if (i < messageJson.entities.intent.length - 1)
                            messageFormated += ",";
                    }
                    //messageFormated += messageJson.entities.intent;
                } else {
                    messageFormated += "NaN";
                }
                console.log(messageJson);
                console.log('#######################');
                console.log(messageFormated);
                //send normalised data to ClientCsharp
                SendResponse(messageFormated);
            }
        }
    });
});

// Tell our app to listen on port 3000
app.listen(process.env.HTTP_PORT, function (err) {
  if (err) {
    throw err
  }
    console.log('Web Server started on port ' + process.env.HTTP_PORT)
})