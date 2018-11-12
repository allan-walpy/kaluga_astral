
/* thnx to https://github.com/taniarascia/sandbox; */
var request = new XMLHttpRequest();

var data;
var currentData;

request.open('Get', 'https://localhost:5001/room/', true);

request.onload = function () {
    data = JSON.parse(this.response);
    currentData = data;

    showTable();
};

request.send();
console.log("request sent");

function showTable(newData) {
    console.log("drawing new table");
    var content = makeRoomTable(newData ? newData : data);
    document.getElementById("content").innerHTML = content;
}

function makeRoomTable(data, isShowId = true, isFilter = true) {
    var output = "<table><tr class='table-header'>";
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
            + cell(room.inhabitantId !== null);
        output += "</tr>"
    });
    output += "</table>";
    return output;
}

function cell(value, onClick, isHeader) {
    var tag = isHeader ? "th": "td";
    var onClick = onClick ? ` onClick="${onClick}()"` : "";
    return `<${tag}${onClick}>${value}</${tag}>`;
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

function selectId() {
    var select = document.getElementById("idInput").value.toString();
    if (select === "")
        return;
    currentData = currentData.filter((r) => r.id.toString().includes(select));
}

function selectNumber() {
    var select = document.getElementById("numberInput").value.toString();
    if (select === "")
        return;
    currentData = currentData.filter((r) => r.number.toString().includes(select));
}

function selectCapacity() {
    var select = document.getElementById("capacityInput").value.toString();
    if (select === "")
        return;
    currentData = currentData.filter((r) => r.capacity.toString() === select);
}

function selectCategory() {
    var select = document.getElementById("categoryInput").value.toString();
    if (select === "")
        return;
    currentData = currentData.filter((r) => toCategory(r.category).toString().includes(select));
}

function selectOccupied() {
    var e = document.getElementById("occupiedInput");
    var select = e.options[e.selectedIndex].value;
    console.log('-------', select);
    if (select == "false")
        currentData = currentData.filter((r) => r.inhabitantId == null);
    if (select == "true")
        currentData = currentData.filter((r) => r.inhabitantId != null);
}

function recalculate() {
    selectId();
    selectNumber();
    selectCapacity();
    selectCategory();
    selectOccupied();

    showTable(currentData);
    currentData = data;
}

function onTyping() {
    recalculate();
}

function onClickClear() {
    console.log("all clear");
    document.getElementById("numberInput").value = "";
    currentData = data;
    showTable();
}