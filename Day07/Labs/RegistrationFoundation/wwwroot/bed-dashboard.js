// ===================================================
// Hospital Bed Availability Dashboard
// ===================================================

// BED DATA
let beds = [
    { bedNumber: 1, isOccupied: false },
    { bedNumber: 2, isOccupied: true },
    { bedNumber: 3, isOccupied: false },
    { bedNumber: 4, isOccupied: true },
    { bedNumber: 5, isOccupied: false },
    { bedNumber: 6, isOccupied: false },
    { bedNumber: 7, isOccupied: true },
    { bedNumber: 8, isOccupied: false },
    { bedNumber: 9, isOccupied: true },
    { bedNumber: 10, isOccupied: false },
    { bedNumber: 11, isOccupied: true },
    { bedNumber: 12, isOccupied: false }
];

// FUNCTION TO RENDER BEDS
function renderBeds() {

    let container = document.getElementById("bedContainer");
    let count = 0;

    // Clear old data
    container.innerHTML = "";

    // Loop through beds
    for (let i = 0; i < beds.length; i++) {

        let bed = beds[i];

        let bedDiv = document.createElement("div");
        bedDiv.classList.add("bed");

        if (bed.isOccupied) {

            count++;

            bedDiv.classList.add("occupied");
            bedDiv.innerText =
                "Bed " + bed.bedNumber + "\nOccupied";

        } else {

            bedDiv.classList.add("available");
            bedDiv.innerText =
                "Bed " + bed.bedNumber + "\nAvailable";
            bedDiv.onclick = function () {

                bed.isOccupied = true;

                renderBeds();
            };
        }

        container.appendChild(bedDiv);
    }
    console.log(document.getElementById("totalbeds"));
    console.log(document.getElementById("bedCount"));

    document.getElementById("totalBeds").innerText = "Total Beds: " + beds.length;

    document.getElementById("bedCount").innerText = "Occupied Beds: " + count;
}

// INITIAL LOAD
console.log("renderBeds called");
renderBeds();