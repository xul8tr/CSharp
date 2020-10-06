var sys = require("util"),
    http = require("http"),
    querystring = require('querystring')
url = require('url')
fs = require('fs');

var myObject = { id: 1, name: 'Lali', egyeb: 'hoppa' };

http.createServer(function (request, response) {

    request.setEncoding('utf-8');

    var requestUrl = url.parse(request.url, true);

    console.info(requestUrl.pathname);

    if (requestUrl.pathname.toLowerCase() == "/akcio") {
        response.setHeader("200", { "Content-Type": "text/plain" });
        response.write("Köszönöm, hogy megnyomta a gombot!<br/>Ez egy önmegsemmisítő rendszer.");
        response.end();
        return;
    }

    if (requestUrl.pathname.toLowerCase() == "/adatok") {
        response.setHeader("200", { "Content-Type": "application/json" });

        var adatok = [];
        for (var i = 0; i < 10; i++) {
            adatok.push({ id: i, ido: new Date() });
        }

        response.write(JSON.stringify(adatok));
        response.end();
        return;
    }


    var fileExists = false;
    var filename = "." + requestUrl.pathname;
    try {
        var fileStat = fs.lstatSync(filename);
        fileExists = fileStat.isFile();
        console.info(fileStat.isFile());
    }
    catch (e) {
        console.warn(e);
    }

    if (fileExists) {

        var file = fs.readFileSync(filename, "binary");
        response.write(file, "binary");
        response.end();
        return;
    }




    request.on('data', function (chunk) { myObject.name = querystring.parse(chunk); }); //ez csak postnál
    request.on('end', function () {
        response.setHeader("200", { "Content-Type": "application/json" });

        myObject.egyeb = url.parse(request.url, true);  //GET, POST

        response.write(JSON.stringify(myObject));
        response.end();

    });

}).listen(8080);

sys.puts("Server running at http://localhost:8080/");