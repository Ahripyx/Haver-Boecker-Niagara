let DDLforChosenPO = document.getElementById("selectedPOOptions");

/*function to switch list items from one ddl to another
use the sender param for the DDL from which the user is multi-selecting
use the receiver param for the DDL that gets the options*/
function removePOOptions(event, senderDDL) {
    //find all selected option tags - selectedOptions becomes a nodelist 
    let senderID = senderDDL.id;
    let selectedOptions = senderDDL.querySelectorAll(`#${senderID} option:checked`);
    event.preventDefault();

    if (selectedOptions.length === 0) {
        alert("Nothing selected to remove.");
    }
    else {
        selectedOptions.forEach(function (o, idx) {
            senderDDL.remove(o.index);
        });
    }
}

//create closures so that we can access the event & the 2 parameters
let removeOpts = (event) => removePOOptions(event, DDLforChosenPO);
//assign the closures as the event handlers for each button
document.getElementById("btnDeletePO").addEventListener("click", removeOpts);
document.getElementById("btnSubmit").addEventListener("click", function () {
    DDLforChosenPO.childNodes.forEach(opt => opt.selected = "selected");
})
