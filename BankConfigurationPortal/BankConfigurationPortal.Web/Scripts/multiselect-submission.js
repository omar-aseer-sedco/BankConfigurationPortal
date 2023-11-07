$(document).ready(function () {
    $("#servicesMultiSelect").chosen({ width: "100%" });
});

const addedButtonIds = new Set();
const removedButtonIds = new Set();
$("#servicesMultiSelect").on("change", function (event, params) {
    if (params.selected) {
        console.log("selected: " + params.selected);

        if (removedButtonIds.has(parseInt(params.selected))) {
            removedButtonIds.delete(parseInt(params.selected));
        }
        else {
            addedButtonIds.add(parseInt(params.selected));
        }
    }
    else {
        console.log("deselected: " + params.deselected);

        if (addedButtonIds.has(parseInt(params.deselected))) {
            addedButtonIds.delete(parseInt(params.deselected));
        }
        else {
            removedButtonIds.add(parseInt(params.deselected));
        }
    }
});

function handleFormSubmission() {
    var addedButtonIdsArray = Array.from(addedButtonIds);
    var addedButtonIdsJson = JSON.stringify(addedButtonIdsArray);
    var removedButtonIdsArray = Array.from(removedButtonIds);
    var removedButtonIdsJson = JSON.stringify(removedButtonIdsArray);

    var addedButtonsHiddenInput = document.createElement("input");
    addedButtonsHiddenInput.type = "hidden";
    addedButtonsHiddenInput.name = "addedButtonIds";
    addedButtonsHiddenInput.value = addedButtonIdsJson;
    var removedButtonsHiddenInput = document.createElement("input");
    removedButtonsHiddenInput.type = "hidden";
    removedButtonsHiddenInput.name = "removedButtonIds";
    removedButtonsHiddenInput.value = removedButtonIdsJson;

    document.getElementById("editCounterForm").appendChild(addedButtonsHiddenInput);
    document.getElementById("editCounterForm").appendChild(removedButtonsHiddenInput);

    document.getElementById("editCounterForm").submit();
}

document.getElementById("submitButton").addEventListener("click", handleFormSubmission);