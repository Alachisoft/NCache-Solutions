//  Copyright (c) 2018 Alachisoft
//  
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//     http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License

var MatchName = "Team A vs Team B"; // name of match. This is the topic to be created on pub/sub

var MatchStartDate = new Date(); // time at which the match is started

var connection = new signalR.HubConnectionBuilder().withUrl("/signalr").build(); // SignalR connection

var timeHandler; // this handles time increment when a match starts
var matchTimeHandler; // this handles the remaining time when a match is ongoing
var MatchEndTimeInMinutes = 1; // time after which a match must end

// init event for web page
document.addEventListener("DOMContentLoaded", function () {

    // fetch match details
    // this also tells if a match is happening or not
    XMLRequest(
        "?handler=MatchDetails",
        "POST",
        {
            "matchName": MatchName, // name of match
        },
        {
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value // CSRF token
        },
        function (result) {
            if (result === false) {
                // if this request has returned false, this means there are no updates to be fetched since the match is not happening yet
                document.getElementById("alert").style.display = "block";
                document.getElementById("alert").innerHTML = 'Match has not been started yet. Refresh after match has started.';
                return;
            }
            result = JSON.parse(result); // parse match updates

            // iterate on each match update
            result.forEach(function (element) {
                HandleMatchEvents(element, true); // pass match update to match events handler
            });
        },
        function (failure) {
            // if this request has failed, this means there has been an unexpected error
            document.getElementById("alert").style.display = "block";
            document.getElementById("alert").innerHTML = 'An error has occurred. Please refresh.';
        }
    );

    // accept match updates through SignalR
    connection.on("MatchUpdate", function (matchUpdate) {
        HandleMatchEvents(matchUpdate.value);
    });

    function HandleMatchEvents(matchUpdate, init) {
        init = init === null ? false : init;
        console.log(matchUpdate);

        // reinitialize time if match start
        if (matchUpdate.updateType === "match_start" && init) {
            MatchStartDate = new Date(matchUpdate.datetime);
        }
        else if (matchUpdate.updateType === "match_start") {
            MatchStartDate = new Date();
        }
        // extract current datetime
        var d;
        if (init) {
            d = new Date(new Date(matchUpdate.datetime) - new Date(MatchStartDate));
            matchTimeHandler = new Date(new Date(matchUpdate.datetime) - new Date(MatchStartDate));
        }
        else {
            d = new Date(new Date() - MatchStartDate);
        }

        switch (matchUpdate.updateType) {
            case "match_start":
                printAll(d, matchUpdate);
                startUpdatedTime();
                break;
            case "match_end":
                XMLRequest(
                    "?handler=StopMatch",
                    "POST",
                    {
                        "matchName": MatchName,
                    },
                    {
                        "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
                    },
                    function (result) {
                        printAll(d, matchUpdate);
                        stopUpdatedTime();
                        document.getElementById("alert").style.display = "block";
                        document.getElementById("alert").innerHTML = 'Match Stopped. Please Refresh after starting a new match.';
                    },
                    function (failure) {
                        console.log(failure);
                    }
                );
                break;
            case "goal":
                document.getElementById(matchUpdate.entity + "Score").innerHTML = parseInt(document.getElementById(matchUpdate.entity + "Score").innerHTML) + 1;
                print(d, "<strong>" + matchUpdate.entity + "</strong> has a new <strong>Goal</strong>!");
                break;
            case "yellow_card":
                print(d, "<strong>Player " + matchUpdate.data + "</strong> has a <strong>Yellow Card</strong>");
                break;
            case "red_card":
                print(d, "<strong>Player " + matchUpdate.data + "</strong> has a <strong>Red Card</strong>");
                break;
            default:
                printAll(d, matchUpdate);
                break;
        }

        if (init) {
            matchTimeHandler = new Date(matchUpdate.datetime);
        }

        function print(date, message) {
            document.getElementById("MatchCommentary").innerHTML = '<p><em>' + zeroFill(date.getMinutes(), 2) + ':' + zeroFill(date.getSeconds(), 2) + '</em><br />' + message + '</p>' + document.getElementById("MatchCommentary").innerHTML;
        }

        function printAll(date, receivedMatchUpdate) {
            document.getElementById("MatchCommentary").innerHTML = '<p><em>' + zeroFill(date.getMinutes(), 2) + ':' + zeroFill(date.getSeconds(), 2) + '</em><br /><strong>' + receivedMatchUpdate.entity + '</strong>: ' + receivedMatchUpdate.data + '</p>' + document.getElementById("MatchCommentary").innerHTML;
        }
    }

    connection.start().then(function () {

    }).catch(function (err) {
        return console.error(err.toString());
        });

    function startUpdatedTime() {
        timeHandler = setInterval(function () {
            matchTimeHandler = new Date(new Date() - MatchStartDate);
            document.getElementById("MatchTime").innerHTML = zeroFill(matchTimeHandler.getMinutes(), 2) + ":" + zeroFill(matchTimeHandler.getSeconds(), 2);
        }, 1000);
    }

    function stopUpdatedTime() {
        clearInterval(timeHandler);
    }

});

function XMLRequest(url, method, data, headers, successCallback, errorCallback) {
    var xmlhttp = new XMLHttpRequest();

    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState === XMLHttpRequest.DONE) {
            if (xmlhttp.status === 200) {
                successCallback(xmlhttp.responseText);
            }
            else {
                if (errorCallback) {
                    errorCallback(xmlhttp.responseText);
                }
                console.log(xmlhttp);
            }
        }
    };

    var formData = new FormData();
    for (i in data) {
        formData.append(i, data[i]);
    }

    xmlhttp.open(method, url, true);

    for (i in headers) {
        xmlhttp.setRequestHeader(i, headers[i]);
    }

    xmlhttp.send(formData);
}

function zeroFill(number, width) {
    width -= number.toString().length;
    if (width > 0) {
        return new Array(width + (/\./.test(number) ? 2 : 1)).join('0') + number;
    }
    return number + ""; // always return a string
}