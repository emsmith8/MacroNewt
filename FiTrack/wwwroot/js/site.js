
$(document).ready(function () {

    $("#confirmRemove").click(function () {
        var selectedRemove = document.querySelector('input[name="currentFoods"]:checked').getAttribute("id");
        localStorage.removeItem(selectedRemove);
        getSelectedList();
        getCurrentFoods();
    })

});


function getSelectedList() {
    const el = document.getElementById('selectedFoods');
    while (el.firstChild) el.removeChild(el.firstChild);
    for (let i = 0; i < localStorage.length; i++) {
        let key = localStorage.key(i);

        var sList = document.getElementById('selectedFoods');
        var entry = document.createElement('li');
        entry.setAttribute('data-ndb', key);
        entry.setAttribute('id', key);
        entry.appendChild(document.createTextNode(localStorage.getItem(key)));
        sList.appendChild(entry);
    }
}

function getCurrentFoods() {
    const el = document.getElementById('currentFoods');
    while (el.firstChild) el.removeChild(el.firstChild);
    
    for (var i = 0; i < localStorage.length; i++) {
        let key = localStorage.key(i);
        var fList = document.getElementById('currentFoods');
        var rElem = document.createElement('input');
        rElem.type = 'radio';
        rElem.setAttribute("id", key)
        rElem.setAttribute("name", "currentFoods");
        var rLabel = document.createElement('label');
        rLabel.appendChild(rElem);
        rLabel.append(localStorage.getItem(key));
        fList.appendChild(rLabel);
    }
}


    function getFoodSelection() {
        var selection = document.querySelector('input[name="foods"]:checked').value;
        var selectionData = document.querySelector('input[name="foods"]:checked').getAttribute("data-val");
        
        localStorage.setItem(selectionData, selection);

        getSelectedList();
        getCurrentFoods();
    }

// Get the modal
var modal = document.getElementById('removeModal');

// Get the button that opens the modal
var btn = document.getElementById("removeSelection");

var confirmBtn = document.getElementById("confirmRemove");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal 
btn.onclick = function () {
    modal.style.display = "block";
    if (localStorage.length == 0) {
        document.getElementById('currentFoods').empty();
    }
}

confirmBtn.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
