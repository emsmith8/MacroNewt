﻿function handleAddFoodsNextClick() {
    var currentFoods = document.getElementById('currentFoods').getElementsByTagName('tbody')[0];
    if (currentFoods.rows.length == 1) {
        $("#addFoodPageMessage").text("You must add foods to the meal before proceeding");
        return;
    }
    else {
        $('#getDetailWaitSpinnerModal').modal('toggle');
        handleSubmitMealClick();
    }
}

function handleMealDetailsPreviousClick() {
    $("#smartwizard").smartWizard('goToStep', 0);
}

function handleMealDetailsNextClick() {
    $('#confirmWaitSpinnerModal').modal('toggle');
    handleSubmitMealLogClick();
}

function handleSearchFoodsClick(e) {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var food = $("#searchString").val();
    var database = $("#targetDatabase").children('option:selected').val();
    if (food.length == 0) {
        return;
    }
    else {
        var container = $("#searchContainer").empty();
        $('#waitSpinnerModal').modal('toggle');
        
        $.get(baseUrl + 'Logger/SearchFoods', { foodName: food, targetDatabase: database }, function (data) {
            container.html(data);
            var addFoodButtons = document.querySelectorAll(".foodAddButton");
            [].forEach.call(addFoodButtons, function (elem) {
                elem.addEventListener('click', addFoodToMeal, false);
            });
            $("#foodSearchResults").on("draw.dt", function () {
                hideFoodsAlreadyInMeal();
                showFoodsNoLongerInMeal();
            });
            $("#foodSearchResults").DataTable({
                "pageLength": 25,
                "pagingType": "numbers",
                "dom": '<"row" <"col-md-12 col-lg-8 d-flex justify-content-start" p><"col-md-12 col-lg-4 d-flex justify-content-end findBox" f>><"clear" i><"clear">rt<"bottom"<"actions">l<"col-md-12 col-lg-8 d-flex justify-content-start" p><"clear">>',
                "order": [[1, 'asc']],
                "columnDefs": [
                    {"width": "10%", "targets": 0}
                ],
                columns: [
                    { orderable: false },
                    null
                ]
            });
            $('#waitSpinnerModal').modal('hide');
            $("html, body").animate({ scrollTop: "500px" }, 1000);
        });
        
    }
    
}

function showFoodsNoLongerInMeal() {
    var stagingList = document.querySelectorAll(".stagingItems");
    [].forEach.call(stagingList, function (elem) {
        var x = document.getElementById(elem.value);
        if (typeof(x) != 'undefined' && x != null) {
            document.getElementById(elem.value).style.display = "table-row";
        }
    });
    
}

function hideFoodsAlreadyInMeal() {
    var currentNdbnos = document.getElementsByClassName("removeFoodButton");

    var x = document.getElementById("foodSearchResults").rows.length;

    for (var i = 0; i < currentNdbnos.length; i++) {
        if (document.getElementById("searchResult-" + currentNdbnos[i].dataset.ndbno)) {
            document.getElementById("searchResult-" + currentNdbnos[i].dataset.ndbno).style.display = "none";
        }
        
    }
}

function handleSubmitMealClick() {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var foodItems = [];
    var servings = [];
    var portion = [];
    var portionIndex = [];
    var selectedPortion = [];

    var selectedProtein;
    var selectedFat;
    var selectedCarb;
    

    var mealTitle = document.getElementById("currentFoods");
    var mealDate = document.getElementById("currentFoods");
    
    var mID = document.getElementById("currentFoods").dataset.mealid;

    var reLogged = document.getElementById("reLogged");

    if (reLogged != null) {
        reLogged = reLogged.value;
    }

    var mealFoodButtons = document.getElementsByClassName("removeFoodButton");
    [].forEach.call(mealFoodButtons, function (elem, i) {
        if (elem.hasAttribute("data-servings")) {
            servings.push(elem.dataset.servings);
            portion.push(elem.dataset.portion);
            portionIndex.push(elem.dataset.pindex);
            selectedPortion.push(elem.dataset.chosenPortion);
        }
        var foodItem = {
            "Ndbno": elem.dataset.ndbno,
            "Servings": elem.dataset.servings,
            "Portion": elem.dataset.portion,
            "PortionIndex": portionIndex[i],
            "SelectedPortionLabel": selectedPortion[i]
        }
        foodItems.push(foodItem);
    });
    var container = $("#componentContainer");
    $.get(baseUrl + 'Logger/GetMealDetailViewComponent', { formData: JSON.stringify(foodItems), mId: mID, reLogged: reLogged }, function (result) {
        container.html(result);

        var md = new Date();

        $('#datetimepicker1').datetimepicker({
            sideBySide: true,
            maxDate: md,
            buttons: {
                showToday: true
            }
        });

        if (servings.length > 0) {
            if (mealTitle != null) {
                document.getElementById("mealTitleInput").value = mealTitle.dataset.mealtitle;
            }
            $('#datetimepicker1').data("datetimepicker").date(mealDate.dataset.mealdate);
        }
        
        var servingSizeInputs = document.getElementsByClassName('servingSize');
        [].forEach.call(servingSizeInputs, function (elem, i) {
            elem.addEventListener('change', handleServingSizeChange, false);
            if (i >= servings.length) {
                servingSizeInputs[i].value = 1;
            }
            else {
                servingSizeInputs[i].value = parseFloat(servings[i]);
            }
        });
        var portionBoxes = document.getElementsByClassName('portionBox');
        [].forEach.call(portionBoxes, function (elem, i) {
            elem.addEventListener('change', handlePortionChange, false);
            portionBoxes[i].selectedIndex = portionIndex[i];
            
            selectedProtein = $('#portionSelectBox_' + i).children('option:selected').attr('data-proteinVal');
            selectedFat = $('#portionSelectBox_' + i).children('option:selected').attr('data-fatVal');
            selectedCarb = $('#portionSelectBox_' + i).children('option:selected').attr('data-carbVal');

            document.getElementById("selectedPortionProtein" + i).value = selectedProtein * servings[i];
            document.getElementById("selectedPortionFat" + i).value = selectedFat * servings[i];
            document.getElementById("selectedPortionCarb" + i).value = selectedCarb * servings[i];


            var foodCalorieTotalBox = document.getElementById("Total_" + i);
            foodCalorieTotalBox.value = parseFloat(servings[i] * portion[i]);
            
        });
        RecalculateMealTotal();
        

        var mealTotalProtein = 0;
        var mealTotalFat = 0;
        var mealTotalCarb = 0;

        var selectedProteins = document.getElementsByClassName('selectedPortionProtein');
        [].forEach.call(selectedProteins, function (elem) {
            mealTotalProtein += parseInt(elem.value);
        });
        var selectedFats = document.getElementsByClassName('selectedPortionFat');
        [].forEach.call(selectedFats, function (elem) {
            mealTotalFat += parseInt(elem.value);
        });
        var selectedCarbs = document.getElementsByClassName('selectedPortionCarb');
        [].forEach.call(selectedCarbs, function (elem) {
            mealTotalCarb += parseInt(elem.value);
        });

        document.getElementById("mealTotalProtein").value = mealTotalProtein;
        document.getElementById("mealTotalFat").value = mealTotalFat;
        document.getElementById("mealTotalCarb").value = mealTotalCarb;


        var detailPrevButton = document.querySelector("#mealDetailsPrevious");
        var detailNextButton = document.querySelector("#mealDetailsNext");
        detailPrevButton.addEventListener('click', handleMealDetailsPreviousClick, false);
        $("#smartwizard").smartWizard('goToStep', 1);

        $('#getDetailWaitSpinnerModal').modal('hide');
    });

}

function handleServingSizeChange(e) {
    var servingSize = parseFloat(e.target.value);
    var foodIndex = e.target.dataset.foodindex;
    var portionValue = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('value');
    var foodCalorieTotalBox = document.getElementById("Total_" + foodIndex);

    var selectedProtein = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-proteinVal');
    var selectedFat = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-fatVal');
    var selectedCarb = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-carbVal');

    document.getElementById("selectedPortionProtein" + foodIndex).value = selectedProtein * servingSize;
    document.getElementById("selectedPortionFat" + foodIndex).value = selectedFat * servingSize;
    document.getElementById("selectedPortionCarb" + foodIndex).value = selectedCarb * servingSize;

    foodCalorieTotalBox.value = parseFloat(servingSize * parseFloat(portionValue));
    var blah = parseFloat(servingSize * parseFloat(portionValue));
    RecalculateMealTotal();

}

function handlePortionChange(e) {
    var portionValue = e.target.value;
    var foodIndex = e.target.dataset.foodindex;
    var portionIndex = e.target.selectedIndex;
    var servingSize = document.getElementById("Foods_" + foodIndex + "__NumberOfServings").value;
    document.getElementById("portionChoiceIndex" + foodIndex).value = portionIndex;
    

    var selectedProtein = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-proteinVal');
    var selectedFat = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-fatVal');
    var selectedCarb = $('#portionSelectBox_' + foodIndex).children('option:selected').attr('data-carbVal');


    document.getElementById("selectedPortionProtein" + foodIndex).value = selectedProtein * servingSize;
    document.getElementById("selectedPortionFat" + foodIndex).value = selectedFat * servingSize;
    document.getElementById("selectedPortionCarb" + foodIndex).value = selectedCarb * servingSize;

    $(servingSize).attr('portionIndex', portionIndex);
    var foodCalorieTotalBox = document.getElementById("Total_" + foodIndex);
    foodCalorieTotalBox.value = parseFloat(servingSize * parseFloat(portionValue));
    RecalculateMealTotal();
}

function RecalculateMealTotal() {
    var foodTotals = document.getElementsByClassName("FoodValue");
    var mealTotal = 0.0;
    for (var i = 0; i < foodTotals.length; i++) {
        if (!isNaN(foodTotals[i].value)) {
            mealTotal += Math.round(foodTotals[i].value);
        }
    }
    document.getElementById("CalorieTotal").value = mealTotal;
}

function addFoodToMeal(e) {
    var ndbno = e.target.dataset.ndbno;
    var foodName = e.target.dataset.foodname;
    var tableRef = document.getElementById('currentFoods').getElementsByTagName('tbody')[0];

    var stagingList = document.querySelectorAll(".stagingItems");

    if (typeof(stagingList) != 'undefined') {
        [].forEach.call(stagingList, function (elem) {

            var x = document.getElementById(elem.value);
            if (elem.value == "searchResult-" + ndbno) {
                elem.parentNode.removeChild(elem);
            }
        });
    }
    
    var newRow = tableRef.insertRow(tableRef.rows.length);
    newRow.classList.add("foodItemRow");
    newRow.setAttribute("id", "mealItem-" + ndbno);
    var buttonCell = newRow.insertCell(0);
    var nameCell = newRow.insertCell(1);

    var removeButton = document.createElement('input');
    removeButton.type = "button";
    removeButton.classList.add('btn');
    removeButton.classList.add('btn-sm');
    removeButton.classList.add('btn-myBlueDark');
    removeButton.classList.add('removeFoodButton');
    removeButton.value = "Remove";
    removeButton.dataset.foodname = foodName;
    removeButton.dataset.ndbno = ndbno;
    removeButton.addEventListener('click', removeFoodFromMeal, false);
    buttonCell.appendChild(removeButton)
    buttonCell.classList.add("currentMealTableButtonCell");
    var nameText = document.createTextNode(foodName);
    nameCell.appendChild(nameText);

    if (document.getElementById("NoFoodsRow").style.display = "block") {
        document.getElementById("NoFoodsRow").style.display = "none"
        document.getElementById("addFoodsNext").style.display = "block";

        var relogStatus = document.getElementById("relogPreviousMeal");
        if (relogStatus) {
            document.getElementById("relogPreviousMeal").style.display = "none";
        }
    }
    document.getElementById("searchResult-" + ndbno).style.display = "none";

    $('#foodAddedModal').modal({ backdrop: 'static', keyboard: false });
    $('#foodAddedModal').modal('toggle');
}

function removeFoodFromMeal(e) {
    var ndbno = e.target.dataset.ndbno;
    var foodName = e.target.dataset.foodname;
    var rowToRemove = document.getElementById("mealItem-" + ndbno);

    var input = document.createElement("input");
    input.setAttribute("type", "hidden");
    input.setAttribute("class", "stagingItems");
    input.setAttribute("name", "searchResult-" + ndbno);
    input.setAttribute("value", "searchResult-" + ndbno);

    document.getElementById("currentFoods").appendChild(input);
    

    if (document.getElementById("searchResult-" + ndbno)) {
        document.getElementById("searchResult-" + ndbno).style.display = "table-row";
    }
    
    rowToRemove.parentNode.removeChild(rowToRemove);
    var tableRef = document.getElementById('currentFoods').getElementsByTagName('tbody')[0];
    if (tableRef.rows.length == 1) {
        document.getElementById("NoFoodsRow").style.display = "table-row";
        document.getElementById("addFoodsNext").style.display = "none";
        if ($('#relogPreviousMeal').length) {
            document.getElementById("relogPreviousMeal").style.display = "block";
        }
    }
}

function handleSubmitMealLogClick() {
    
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var form = $("#ConfirmMealForm");
    var title = $('#mealTitleInput').val();

    var mealTotalProtein = 0;
    var mealTotalFat = 0;
    var mealTotalCarb = 0;

    var selectedProteins = document.getElementsByClassName('selectedPortionProtein');
    [].forEach.call(selectedProteins, function (elem) {
        mealTotalProtein += parseInt(elem.value);
    });
    var selectedFats = document.getElementsByClassName('selectedPortionFat');
    [].forEach.call(selectedFats, function (elem) {
        mealTotalFat += parseInt(elem.value);
    });
    var selectedCarbs = document.getElementsByClassName('selectedPortionCarb');
    [].forEach.call(selectedCarbs, function (elem) {
        mealTotalCarb += parseInt(elem.value);
    });

    document.getElementById("mealTotalProtein").value = mealTotalProtein;
    document.getElementById("mealTotalFat").value = mealTotalFat;
    document.getElementById("mealTotalCarb").value = mealTotalCarb;
    

    var servings = [];
    var portion = [];
    $.ajax({
        url: baseUrl + 'Logger/ConfirmMeal',
        type: 'POST',
        data: form.serialize(),
        success: function (data, status, xhr) {
            $('#confirmWaitSpinnerModal').modal('hide');
            if (xhr.getResponseHeader("vstatus") == "pass") {
                var container = $("#confirmContainer").empty();
                container.html(data);
                $('#confirmMealModal').modal({ backdrop: 'static', keyboard: false });
                $('#confirmMealModal').modal('toggle');
                document.getElementById("confirmMealLog").addEventListener('click', confirmAndLogMeal, false);
            }
            else if (xhr.getResponseHeader("vstatus") == "fail") {
                var container = $("#componentContainer").empty();
                container.html(data);
                $('#datetimepicker1').datetimepicker({
                    sideBySide: true,
                    buttons: {
                        showToday: true
                    }
                });
                var servingSizeInputs = document.getElementsByClassName('servingSize');
                [].forEach.call(servingSizeInputs, function (elem, i) {
                    elem.addEventListener('change', handleServingSizeChange, false);
                    servings.push(servingSizeInputs[i].value);

                    $('#finishFoodDetailsButton_' + i).text("Edit");
                    $('#finishFoodDetailsButton_' + i).addClass('btn-myBlueDark').removeClass('btn-myGreen');
                });
                var portionBoxes = document.getElementsByClassName('portionBox');
                [].forEach.call(portionBoxes, function (elem, i) {
                    elem.addEventListener('change', handlePortionChange, false);
                    portion.push(portionBoxes[i].value);
                    var foodCalorieTotalBox = document.getElementById("Total_" + i);
                    foodCalorieTotalBox.value = parseInt(servings[i] * portion[i]);
                });
                
                if ($('#mealTitleInput').val() == "Auto") {
                    $('#autoNameCheckbox').prop("checked", true);
                    $('#mealTitleInput').prop("readonly", true);
                }


                $('#mealDetailsNext').prop("disabled", false);
                $('#mealDetailsNext').addClass('btn-myGreen').removeClass('btn-myDisabledGreen');
                
                var detailPrevButton = document.querySelector("#mealDetailsPrevious");
                var detailNextButton = document.querySelector("#mealDetailsNext");
                detailPrevButton.addEventListener('click', handleMealDetailsPreviousClick, false);
                detailNextButton.addEventListener('click', handleMealDetailsNextClick, false);
                
                RecalculateMealTotal();

            }
        }
    });
}

function confirmAndLogMeal() {
    $('#confirmMealModal').modal("hide");
    $('#logWaitSpinnerModal').modal('toggle');

    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var form = $("#ConfirmMealForm");
    var title = $('#mealTitleInput').val();

    var edited = $('#edited').val();

    $.ajax({
        url: baseUrl + 'Logger/LogMeal',
        type: 'POST',
        data: form.serialize(),
        success: function (response) {
            $('#logWaitSpinnerModal').modal('hide');
            if (response.success == true) {
                $("#smartwizard").smartWizard('goToStep', 2);

                refreshUserInfo();

                $.get(baseUrl + 'Logger/GetMealReviewViewComponent', { mealTitle: title, mealID: response.mealID, edited: edited }, function (response) {
                    $('#confirmMealModal').modal('hide');
                    $("#reviewComponentContainer").html(response);
                });
            }
            else {
                
                alert("It didn't work. Darn.");
            }
        }
    });

}

function handleExploreSearchFoods(e) {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var food = $("#searchString").val();
    var database = $("#targetDatabase").children('option:selected').val();
    if (food.length == 0) {
        return;
    }
    else {
        var container = $("#searchContainer").empty();
        $('#waitSpinnerModal').modal('toggle');
        $.get(baseUrl + 'Logger/SearchFoods', { foodName: food, targetDatabase: database }, function (data) {
            container.html(data);
            var exploreFoodButtons = document.querySelectorAll(".foodAddButton");
            [].forEach.call(exploreFoodButtons, function (elem, i) {
                exploreFoodButtons[i].value = "Explore";
                elem.addEventListener('click', function () { exploreFood(exploreFoodButtons[i].dataset.ndbno) }, false);
            });
            $("#foodSearchResults").DataTable({
                "pageLength": 25,
                "pagingType": "numbers",
                "dom": '<"row" <"col-md-12 col-lg-8 d-flex justify-content-start" p><"col-md-12 col-lg-4 d-flex justify-content-end findBox" f>><"clear" i><"clear">rt<"bottom"<"actions">l<"clear">>',
                "order": [[1, 'asc']],
                "columnDefs": [
                    { "width": "10%", "targets": 0 }
                ],
                columns: [
                    { orderable: false },
                    null
                ]
            });
            $('#waitSpinnerModal').modal('hide');
            $("html, body").animate({ scrollTop: "350px" }, 1000);
        });
    }

}

function exploreFood(targetNdbno) {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    var container = $("#modalArea");

    $.get(baseUrl + 'Meals/BuildExploreNutritionLabelModal', { ndbno: targetNdbno, portionIndex: 1 }, function (response) {
        container.html(response);

        $('#exploreNutritionModal').modal({ backdrop: 'static', keyboard: false });
        $('#exploreNutritionModal').modal('toggle');
    });

}

function refreshUserInfo() {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    $.ajax({
        type: 'GET',
        url: baseUrl + 'Home/RefreshUserInfo',
        success: function (result) {
            var loggedInContainer = $('#loggedInUserInfoContainer').empty();
            loggedInContainer.html(result);
        }
    });
}

function scrollFunction() {

    var myBacktoTopButton = document.getElementById("backToTopButton");
    var myNewSearchButton = document.getElementById("newSearchFloatingButton");

    if (document.body.scrollTop > 480 || document.documentElement.scrollTop > 480) {
        myBacktoTopButton.style.display = "block";
        myNewSearchButton.style.display = "block";
    }
    else {
        myBacktoTopButton.style.display = "none";
        myNewSearchButton.style.display = "none";
    }
}

function backToTop() {
    $(window).scrollTop(0);
}

function newSearch() {
    $('#searchString').val('');
    $('#searchString').attr("placeholder", "type food here")
    $('#searchString').focus();
    $('#searchContainer').empty();
    backToTop();
}

function deleteMeal(itemId) {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

    $.get(baseUrl + 'Meals/Delete', { id: itemId }, function (response) {
        var container = $("#deleteMealContainer").empty();
        container.html(response);
    });

}

function triggerSpinner() {
    $('#waitSpinnerModal').modal('toggle');
}

function triggerDelete() {
    $('#waitSpinnerModal').modal('toggle');
    $('#calendarDaysMealsModal').modal('hide');
}