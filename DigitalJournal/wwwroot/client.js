const username = "master";
const password = "123";
let token;

window.addEventListener("DOMContentLoaded", () => {
    const controlDiv = document.getElementById("controls");
    createButton(controlDiv, "Get Data", getData);
    createButton(controlDiv, "Get One Data", getOneData);
    createButton(controlDiv, "Log In", login);
    createButton(controlDiv, "Log Out", logout);
});

async function login() {
    let response = await fetch("/api/account/token", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username: username, password: password })
    });
    if (response.ok) {
        token = (await response.json()).token;
        displayData("Logged in", token);
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
}

async function logout() {
    token = "";
    displayData("Logged out");
}

async function getData() {
    let response = await fetch("/api/profile", {
        headers: { 'Content-Type': 'application/json', "Authorization": `Bearer ${token}` }
    });
    if (response.ok) {
        let jsonData = await response.json();
        displayData(...jsonData.map(item => `${item.surName}, ${item.firstName}`));
    } else {
        displayData(`Error: ${response.status}: ${response.statusText}`);
    }
}

async function getOneData() {
    let response = await fetch("/api/profile/1", {
        headers: { 'Content-Type': 'application/json', "Authorization": `Bearer ${token}` }
    });
    if (response.ok) {
        let jsonData = await response.json();
        displayData(`${jsonData.surName}, ${jsonData.firstName}`);
    } else {
        let text = await response.text();
        displayData(`Error: ${response.status}: ${response.statusText}, text: ${text}`);
    }
}

function displayData(...items) {
    const dataDiv = document.getElementById("data");
    dataDiv.innerHTML = "";
    items.forEach(item => {
        const itemDiv = document.createElement("div");
        itemDiv.innerText = item;
        itemDiv.style.wordWrap = "break-word";
        dataDiv.appendChild(itemDiv);
    })
}

function createButton(parent, label, handler) {
    const button = document.createElement("button");
    button.classList.add("btn", "btn-primary", "m-2");
    button.innerText = label;
    button.onclick = handler;
    parent.appendChild(button);
}
