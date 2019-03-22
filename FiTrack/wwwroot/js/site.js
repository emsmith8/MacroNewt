// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {

    $("#confirmRemove").click(function () {
        
        var selectedRemove = document.querySelector('input[name="currentFoods"]:checked').getAttribute("id");

  //      var list = document.getElementById('selectedFoods').getElementsByTagName("li");
  //      var fList = document.getElementById('currentFoods').getElementsByTagName("label");


        localStorage.removeItem(selectedRemove);
        getSelectedList();
        getCurrentFoods();
  //      location.reload();
        
       /* 
        for (var i = 0; i < list.length; i++) {
            if (list[i].getAttribute('data-ndb') == selectedRemove) {
                list[i].parentNode.removeChild(list[i]);
                fList[i].parentNode.removeChild(fList[i]);
                //         list[i].remove();
       //         fList[i].remove();
            }
        }
       */
        
    })

});

/*
function getFoodSelection() {
    var selection = document.querySelector('input[name="foods"]:checked').value;
    var selectionData = document.querySelector('input[name="foods"]:checked').getAttribute("data-val");

    var sList = document.getElementById('selectedFoods');
    var entry = document.createElement('li');
    entry.setAttribute('data-ndb', selectionData);
    entry.setAttribute('id', selectionData);
    entry.appendChild(document.createTextNode(selection));
    sList.appendChild(entry);
}
*/

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

      //  alert("The local thingy is " + localStorage.getItem(key) + " and the ndb is " + key);
    }


    
}

function getCurrentFoods() {
    const el = document.getElementById('currentFoods');
    while (el.firstChild) el.removeChild(el.firstChild);

    //just trying it here
    for (var i = 0; i < localStorage.length; i++) {
        let key = localStorage.key(i);

        //     alert("I'm going into here");
        var fList = document.getElementById('currentFoods');
        var rElem = document.createElement('input');
        rElem.type = 'radio';
        //    rElem.setAttribute("id", list[i].getAttribute('data-ndb'));
        rElem.setAttribute("id", key)
        rElem.setAttribute("name", "currentFoods");
        //     rElem.setAttribute("name", localStorage.getItem(key))
        var rLabel = document.createElement('label');
        rLabel.appendChild(rElem);
        //   rLabel.append(list[i].textContent);
        rLabel.append(localStorage.getItem(key));
        fList.appendChild(rLabel);
    }
}


    function getFoodSelection() {
        var selection = document.querySelector('input[name="foods"]:checked').value;
        var selectionData = document.querySelector('input[name="foods"]:checked').getAttribute("data-val");


        //   var foodObj = { name: selection, ndbno: selectionData };
        //   var jsonString = JSON.stringify(foodObj);
        localStorage.setItem(selectionData, selection);

        getSelectedList();
        getCurrentFoods();

        /*
        var sList = document.getElementById('selectedFoods');
        var entry = document.createElement('li');
        entry.setAttribute('data-ndb', selectionData);
        entry.setAttribute('id', selectionData);
        entry.appendChild(document.createTextNode(selection));
        sList.appendChild(entry);
        */

        /*
        for (let i = 0; i < localStorage.length; i++) {
            let key = localStorage.key(i);
            alert("The local thingy is " + localStorage.getItem(key) + " and the ndb is " + key);
        }
        */


    }



//var list = document.getElementById('selectedFoods').getElementsByTagName("li");
//var fList = document.getElementById('currentFoods').getElementsByTagName("label");

/*
function getAllSelections() {

 //   for (var i = 0; i < list.length; i++) {
 //       alert('Children item is: ' + list[i].textContent + " and ndb is " + list[i].getAttribute('data-ndb'));
    return list;
}
*/    

/*
function removeSelection(foodNDB) {

    for (var i = 0; i < list.length; i++) {
        if (list[i].getAttribute('data-ndb') == foodNDB) {
            list.splice(i, 1);
        }
    }
    

}
*/

// Get the modal
var modal = document.getElementById('removeModal');
//var modalContent = document.getElementById('modal-content');

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
    

    /*
    for (var i = 0; i < list.length; i++) {
        alert("The list includes " + list[i].textContent);
    }
    */

    /*
    for (var i = 0; i < localStorage.length; i++) {
        let key = localStorage.key(i);

        alert("I'm going into here");
        var fList = document.getElementById('currentFoods');
        var rElem = document.createElement('input');
        rElem.type = 'radio';
    //    rElem.setAttribute("id", list[i].getAttribute('data-ndb'));
        rElem.setAttribute("id", key)
        rElem.setAttribute("name", "currentFoods");
   //     rElem.setAttribute("name", localStorage.getItem(key))
        var rLabel = document.createElement('label');
        rLabel.appendChild(rElem);
     //   rLabel.append(list[i].textContent);
        rLabel.append(localStorage.getItem(key));
        fList.appendChild(rLabel);
        
    } 
    */
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
