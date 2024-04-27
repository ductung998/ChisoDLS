$(document).ready(function () {
    $('#addRowBtn').click(function () {
        addEmptyRow();
    });

    $('#productTable').on('blur', 'td.editable', function () {
        var $cell = $(this);
        var $row = $cell.closest('tr');
        var isLastRow = $row.is(':last-child');

        // If the edited cell is in the last row and the cell is not empty, add a new empty row
        if (isLastRow && $cell.text().trim() !== '') {
            addEmptyRow();
        }

        var productId = $row.find('td:first').text();
        var columnName = $cell.index() === 1 ? 'Name' : 'Price';
        var updatedValue = $cell.text();

        updateProduct(productId, columnName, updatedValue);
    });

    document.getElementById('pasteBtn').addEventListener('click', function () {
        pasteFromClipboard();
    });

    // Add event listener for Ctrl + V keyboard shortcut
    document.addEventListener('keydown', function (event) {
        if (event.ctrlKey && event.key === 'v') {
            pasteFromClipboard();
        }
    });
});

function addEmptyRow() {
    var newRowId = parseInt($('#productTable tr:last td:first').text()) + 1;
    var newRow = '<tr><td>' + newRowId + '</td>';

    // Add editable cells based on the number of columns
    for (var i = 0; i < $('#productTable tr:first th').length - 1; i++) {
        newRow += '<td class="editable" contenteditable="true"></td>';
    }

    newRow += '</tr>';
    $('#productTable').append(newRow);
}

function updateProduct(productId, columnName, updatedValue) {
    // Implement AJAX request to update the product (similar to the previous example)
}

function addProductToTable(name, price) {
    // Generate ID (e.g., incrementing the last ID)
    var lastId = $('#productTable tr:last td:first').text();
    var newId = lastId !== '' ? parseInt(lastId) + 1 : 1;

    // Add row to the table
    var newRow = '<tr><td>' + newId + '</td><td>' + name + '</td><td>' + price + '</td></tr>';
    $('#productTable').append(newRow);
}

function pasteFromClipboard() {
    navigator.clipboard.readText()
        .then(function (clipboardText) {
            var rows = clipboardText.split('\n');
            var numberOfColumnsInTable = $('#productTable tr:first td').length - 1; // Exclude the ID column
            var lastRow = rows[rows.length - 1].split('\t'); // Get the last row from clipboard data

            // Check if the table is empty
            var isNewTable = $('#productTable tbody').children().length === 0;

            // Initialize ID based on the last ID in the table or 0 if the table is empty
            var newRowId = isNewTable ? 0 : parseInt($('#productTable tr:not(:first) td:first').last().text()) || 0;

            // Check if the last row is empty
            if (lastRow.every(function (value) { return value.trim() === ''; })) {
                rows.pop(); // Remove the last row if it's empty
            }

            rows.forEach(function (row, rowIndex) {
                var columns = row.split('\t'); // Assuming tab-separated values

                // Trim excess columns if needed
                columns = columns.slice(0, numberOfColumnsInTable);

                // Increment the ID for each row
                newRowId++;

                var newRow = '<tr>';
                newRow += '<td>' + newRowId + '</td>';

                // Append the data columns from the pasted data
                columns.forEach(function (column) {
                    newRow += '<td class="editable" contenteditable="true">' + column.trim() + '</td>';
                });

                // Add empty cells for any remaining columns (if fewer columns pasted than expected)
                for (var i = columns.length; i < numberOfColumnsInTable; i++) {
                    newRow += '<td class="editable" contenteditable="true"></td>';
                }

                newRow += '</tr>';
                $('#productTable').append(newRow);
            });
        })
        .catch(function (err) {
            console.error('Failed to read clipboard contents: ', err);
        });
}


$('#clearBtn').click(function () {
    $('#productTable tr:gt(0)').remove(); // Remove all rows except the first one (header row)
});


$('#exportCsvBtn').click(function () {
    var csvContent = 'data:text/csv;charset=utf-8,';
    var rows = $('#productTable tr');

    // Loop through each row and append its data to the CSV content
    rows.each(function () {
        var rowData = [];
        $(this).find('td').each(function () {
            var cellData = $(this).text();
            // If the cell data contains a comma, enclose it in double quotes
            if (cellData.includes(',')) {
                cellData = '"' + cellData + '"';
            }
            rowData.push(cellData);
        });
        csvContent += rowData.join(',') + '\n';
    });

    // Create a temporary anchor element to trigger the download
    var encodedUri = encodeURI(csvContent);
    var link = document.createElement('a');
    link.setAttribute('href', encodedUri);
    link.setAttribute('download', 'products.csv');
    document.body.appendChild(link);

    // Trigger the download
    link.click();

    // Clean up
    document.body.removeChild(link);
});