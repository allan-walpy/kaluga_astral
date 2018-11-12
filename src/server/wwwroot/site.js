/* thnx to https://github.com/taniarascia/sandbox; */
var request = new XMLHttpRequest();

var data;

request.open('Get', 'https://localhost:5001/room/', true);

request.onload = function () {
    data = JSON.parse(this.response);

    showTable();
};

request.send();
console.log("request sent");

function showTable() {
    console.log("drawing new table");
    var content = makeRoomTable(data);
    document.getElementById("content").innerHTML = content;
}

function makeRoomTable(data, isShowId = true) {
    var output = "<table><tr>";
    if (isShowId)
        output += cell("Id", "onClickSortId")
    output += cell("Number", "onClickSortNumber")
        + cell("Capacity", "onClickSortCapacity")
        + cell("Category", "onClickSortCategory")
        + cell("Occupied", "onClickSortOccupied")
         + "</tr>";

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

function cell(value, onClick) {
    if (onClick !== null)
        return `<td onClick="${onClick}()">${value}</td>`;
    return `<td>${value}</td>`;
}

function toCategory(number) {
    if (number === 1)
        return "HalfLuxury";
    if (number === 2)
        return "Luxury";
    return "Standart";
}

function onClickSortId() {
    sortById();
    showTable();
}

function onClickSortNumber() {
    sortByNumber();
    showTable();
}

function onClickSortCapacity() {
    sortByCapacity();
    showTable();
}

function onClickSortCategory() {
    sortByCategory();
    showTable();
}

function onClickSortOccupied() {
    sortByOccupied();
    showTable();
}

function sortByNumber() {
    data.sort((a, b) => {
        if (a.number > b.number)
            return 1;
        return -1;
    });
}

function sortById() {
    data.sort((a, b) => {
        if (a.id > b.id)
            return 1;
        return -1;
    });
}

function sortByCapacity() {
    data.sort((a, b) => {
        if (a.capacity > b.capacity)
            return 1;
        return -1;
    });
}

function sortByCategory() {
    data.sort((a, b) => {
        if (a.category > b.category)
            return 1;
        return -1;
    });
}

function sortByOccupied() {
    data.sort((a, b) => {
        if (a === null)
            return 1;
        if (b === null)
            return -1;
        return 0;
    });
}