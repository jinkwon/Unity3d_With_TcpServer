net = require('net');

// Keep track of the chat clients
var clients = [];

var x = 0;
var y = 0;
var z = 0;

// Start a TCP Server
net.createServer(function (socket) {

    // Identify this client
    socket.name = socket.remoteAddress + ":" + socket.remotePort;

    // Put this new client in the list
    clients.push(socket);

    // Send a nice welcome message and announce
    socket.write("Welcome " + socket.name + "\n");
    broadcast(socket.name + " joined the chat\n", socket);

    // Handle incoming messages from clients.
    socket.on('data', function (data) {

        var sData = data.toString().trim();

        if(sData == 'get_status'){
            var vectorPos = [x, y, z];
            broadcast( vectorPos.join(',') + "\n");
        }

    });

    // Remove the client from the list when it leaves
    socket.on('end', function () {
        clients.splice(clients.indexOf(socket), 1);
        broadcast(socket.name + " left the chat.\n");
    });


    // Send a message to all clients
    function broadcast(message, sender) {

        clients.forEach(function (client) {
            // Don't want to send it to sender
            if (client === sender) return;
            client.write(message);
        });

        // Log it to the server output too
        //process.stdout.write(message);
    }

}).listen(1337);

// Put a friendly message on the terminal of the server.
console.log("Chat server running at port 1337\n");

process.on('uncaughtException', function (err) {
    console.log('Caught exception: ' + err);
    clients = [];
});


var http = require('http');
var fs = require('fs');

http.createServer(function(req, res){

    var filename = __dirname + req.url;

    readStream = fs.createReadStream(filename);

    readStream.on('open', function () {
        // This just pipes the read stream to the response object (which goes to the client)
        readStream.pipe(res);
    });



}).listen(3131);

var io = require('socket.io').listen(3232);

io.sockets.on('connection', function (socket) {
    socket.on('set_pos', function (data) {
        x = parseFloat(data.x);
        y = parseFloat(data.y);
        z = parseFloat(data.z);
    });
});