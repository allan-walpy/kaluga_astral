/* thnx to https://github.com/taniarascia/sandbox; */
var request = new XMLHttpRequest();

request.open('Get', 'https://localhost:5001/room/', true);

request.onload = function () {
    var data = JSON.parse(this.response);
    console.log("retrived data about roooms:");
    console.log(data);


    var content = makeRoomTable(data);
    document.getElementById("content").innerHTML = content;
    console.log("made content:");
    console.log(content);

};

request.send();
console.log("request sent");

function toCategory(number) {
    if (number === 1)
        return "HalfLuxury";
    if (number === 2)
        return "Luxury";
    return "Standart";
}

function cell(value) {
    return `<td>${value}</td>`;
}

function makeRoomTable(data, isShowId = true)
{
    var output = "<table><tr>";
    if (isShowId)
        output += cell("Id")
    output += cell("Number") + cell("Capacity") + cell("Category") + cell("Occupied")+ "</tr>";

    data.forEach(room => {
        output += "<tr>";
        console.log(room);
        if (isShowId)
            output += cell(room.id)
        output += cell(room.number)
            + cell(room.capacity)
            + cell(toCategory(room.category))
            + cell(room.inhabitantId === null);
        output += "</tr>"
    });
    output += "</table>";
    return output;
}