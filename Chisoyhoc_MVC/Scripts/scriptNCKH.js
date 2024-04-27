let numColumns = 2; // Default number of columns

// Function to generate table with specified number of columns
function generateTable() {
    let tableContainer = document.getElementById("tableContainer");
    tableContainer.innerHTML = '';

    let table = document.createElement("table");
    table.id = "dataTable";

    let thead = document.createElement("thead");
    let tr = document.createElement("tr");
    for (let i = 0; i < numColumns; i++) {
        let th = document.createElement("th");
        th.textContent = "Column " + (i + 1);
        tr.appendChild(th);
    }
    thead.appendChild(tr);
    table.appendChild(thead);

    let tbody = document.createElement("tbody");
    table.appendChild(tbody);

    tableContainer.appendChild(table);

    // Add initial rows
    for (let i = 0; i < 3; i++) {
        addRow();
    }
}

// Function to add rows to the table
function addRow() {
    let table = document.getElementById("dataTable").getElementsByTagName('tbody')[0];
    let newRow = table.insertRow(table.rows.length);
    for (let i = 0; i < numColumns; i++) {
        let cell = newRow.insertCell(i);
        cell.contentEditable = true; // Allow editing of cells
    }
}

// Function to clear all existing rows
function clearTable() {
    let table = document.getElementById("dataTable").getElementsByTagName('tbody')[0];
    table.innerHTML = '';
}

// Parse Excel data to extract values
function parseExcelData(pastedData) {
    let rows = pastedData.split('\n');
    let values = [];
    rows.forEach(function (row) {
        let cells = row.split('\t');
        let rowValues = [];
        for (let i = 0; i < numColumns && i < cells.length; i++) {
            rowValues.push(cells[i]);
        }
        values.push(rowValues);
    });
    return values;
}

// Event listener for paste event
document.addEventListener('paste', function (e) {
    let pastedData = (e.clipboardData || window.clipboardData).getData('text');
    let values = parseExcelData(pastedData);
    clearTable();
    values.forEach(function (rowValues) {
        let newRow = document.createElement('tr');
        for (let i = 0; i < numColumns; i++) {
            let cell = document.createElement('td');
            cell.textContent = rowValues[i] || ''; // Set cell value or empty string if undefined
            cell.contentEditable = true; // Allow editing of cells
            newRow.appendChild(cell);
        }
        document.getElementById('dataTable').getElementsByTagName('tbody')[0].appendChild(newRow);
    });
    e.preventDefault();
});

// Event listener for Generate Table button
document.getElementById("generateTableButton").addEventListener("click", function () {
    numColumns = parseInt(document.getElementById("numColumns").value);
    if (!isNaN(numColumns) && numColumns > 0) {
        generateTable();
    } else {
        alert("Please enter a valid number of columns (greater than 0).");
    }
});



// Event listener for Clear button
document.getElementById("clearButton").addEventListener("click", function () {
    clearTable();
});

// Function to export table data as CSV
function exportTableToCSV() {
    let csvContent = "data:text/csv;charset=utf-8,";

    let rows = document.querySelectorAll("#dataTable tbody tr");
    rows.forEach(function (row) {
        let rowData = [];
        row.querySelectorAll("td").forEach(function (cell) {
            let cellValue = cell.textContent.trim();
            // Wrap cell value in double quotes and escape existing double quotes
            rowData.push('"' + cellValue.replace(/"/g, '""') + '"');
        });
        csvContent += rowData.join(",") + "\n";
    });

    let encodedUri = encodeURI(csvContent);
    let link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "table_data.csv");
    document.body.appendChild(link);
    link.click();
}

// Event listener for Export CSV button
document.getElementById("exportCSVButton").addEventListener("click", function () {
    exportTableToCSV();
});


// Event listener for Export XLS button
document.getElementById("exportXLSButton").addEventListener("click", function () {
    exportTableToXLS();
});
