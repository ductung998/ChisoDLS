$(document).ready(function () {
    var formSelector = $('#formSelector');
    var selectedFormId = formSelector.val();
    showSelectedForm(selectedFormId);

    $('#getAnswersButton').on('click', function () {
        var selectedFormId = formSelector.val();
        var answers = getAnswers(selectedFormId);
        alert('Answers: ' + answers);
    });
});

function launchForm() {
    var formSelector = document.getElementById('formSelector');
    var selectedFormId = formSelector.value;
    showSelectedForm(selectedFormId);
}

function showSelectedForm(selectedFormId) {
    // Hide all forms
    $('.form').hide();

    // Show the selected form
    $('#' + selectedFormId).show();
}

function getAnswers(selectedFormId) {
    var answers = [];

    // Include the selected form ID
    //answers.push('selectedFormId:' + selectedFormId);

    // Get all form elements of the selected form
    var formElements = $('#' + selectedFormId).find(':input');

    // Iterate through all form elements of the selected form using a for loop
    for (var i = 0; i < formElements.length; i++) {
        var name = $(formElements[i]).attr('name');
        var type = $(formElements[i]).attr('type');
        var value = $(formElements[i]).val();

        // Check if the element has a name attribute
        if (name) {
            // Check if the question is an MCQ
            var isMCQ = type == "radio";
            if (isMCQ && $(formElements[i]).prop('checked')) {
                // Include only the selected option for MCQ
                answers.push(value);
            } else if (value && !isMCQ) {
                // Include values for non-MCQ questions
                answers.push(value);
            }
        }
    }

    // Join the answers array into a single string separated by underscores
    return answers.join('_');
}

