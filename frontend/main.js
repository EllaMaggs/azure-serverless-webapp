window.addEventListener('DOMContentLoaded', () => {
    console.log("Page loaded. Starting getVisitCount...");
    getVisitCount();
});

const functionApiUrl = 'https://emresumeproject.azurewebsites.net/api/GetResumeCounter?code=jY2weazXxO3ucEYh33_ntl3kix1tdmxcRmMfRXIS9r8TAzFuljJDtw%3D%3D'
const localhostfunctionApi = 'http://localhost:7071/api/GetResumeCounter'; // Replace with live API URL when deployed

const getVisitCount = async () => {
    const counterElement = document.getElementById("counter");

    // Check if the counter element exists in the DOM
    if (!counterElement) {
        console.error("Element with id 'counter' not found in the DOM!");
        return;
    }

    // Set a loading message while fetching the data
    counterElement.innerText = "Loading...";

    try {
        // Fetch the data from the Azure Function API
        const response = await fetch(functionApiUrl);
        console.log("API Response Status:", response.status);

        // Check if the response is successful
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        // Parse the JSON response
        const data = await response.json();
        console.log("Data received from API:", data);

        // Update the counter in the DOM using the correct property name 'Count'
        if (data && typeof data.Count === "number") {
            counterElement.innerText = data.Count;
        } else {
            console.error("API response is missing 'Count' or is not a number:", data);
            counterElement.innerText = "Error loading count";
        }
    } catch (error) {
        // Log any errors to the console and update the counter with an error message
        console.error("Error fetching visitor count:", error);
        counterElement.innerText = "Error loading count";
    }
};
