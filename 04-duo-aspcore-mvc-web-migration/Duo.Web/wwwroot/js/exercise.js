/**
 * Shared JavaScript functions for exercise views
 */

// Handle exercise form submission
function handleExerciseSubmit(formId, validator, successCallback, errorCallback) {
    $(document).ready(function() {
        $('#' + formId).on('submit', function(event) {
            event.preventDefault();
            
            // Call the validator function
            var result = validator(this);
            
            if (result.isValid) {
                if (successCallback) {
                    successCallback(result);
                } else {
                    alert("Correct answer!");
                }
            } else {
                if (errorCallback) {
                    errorCallback(result);
                } else {
                    alert("Incorrect answer. Please try again.");
                }
            }
        });
    });
}

// Show a simple result alert
function showResultAlert(isCorrect, correctAnswer) {
    if (isCorrect) {
        alert("Correct! Well done.");
    } else {
        alert("Incorrect. The correct answer is: " + correctAnswer);
    }
}

// Reset a form
function resetForm(formId) {
    $('#' + formId)[0].reset();
}

// Toggle visibility of elements
function toggleVisibility(selector) {
    $(selector).toggleClass('d-none');
}

// Scroll to element
function scrollToElement(selector, offset) {
    offset = offset || 0;
    $('html, body').animate({
        scrollTop: $(selector).offset().top + offset
    }, 300);
} 