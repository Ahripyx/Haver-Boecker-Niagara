let DDLforChosenMachine = document.getElementById("selectedMachineOptions");
let DDLforAvailableMachine = document.getElementById("availableMachineOptions");

/*function to switch list items from one ddl to another
use the sender param for the DDL from which the user is multi-selecting
use the receiver param for the DDL that gets the options*/
function switchMachineOptions(event, senderMachineDDL, receiverMachineDDL) {
    //find all selected option tags - selectedOptions becomes a nodelist 
    let senderMachineID = senderMachineDDL.id;
    let selectedMachineOptions = document.querySelectorAll(`#${senderMachineID} option:checked`);
    event.preventDefault();

    if (selectedMachineOptions.length === 0) {
        alert("Nothing to move.");
    }
    else {
        selectedMachineOptions.forEach(function (o, idx) {
            senderMachineDDL.remove(o.index);
            receiverMachineDDL.appendChild(o);
        });
    }
}
//create closures so that we can access the event & the 2 parameters
let addMachineOptions = (event) => switchMachineOptions(event, DDLforAvailableMachine, DDLforChosenMachine);
let removeMachineOptions = (event) => switchMachineOptions(event, DDLforChosenMachine, DDLforAvailableMachine);
//assign the closures as the event handlers for each button
document.getElementById("btnLeft").addEventListener("click", removeMachineOptions);
document.getElementById("btnRight").addEventListener("click", addMachineOptions);

document.getElementById("btnSubmit").addEventListener("click", function () {
    DDLforChosenMachine.childNodes.forEach(opt => opt.selected = "selected");
})
