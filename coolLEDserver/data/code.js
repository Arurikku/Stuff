window.onload = load;
var sliderR;
var sliderG;
var sliderB;
var selected;
var btns;
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
function load() {
    sliderR = document.getElementById("redSlider");
    sliderG = document.getElementById("greenSlider");
    sliderB = document.getElementById("blueSlider");
    sliderR.oninput = (() => {
        selected.style.backgroundColor = `rgb(${sliderR.value}, ${sliderG.value}, ${sliderB.value})`;
    });
    sliderG.oninput = (() => {
        selected.style.backgroundColor = `rgb(${sliderR.value}, ${sliderG.value}, ${sliderB.value})`;
    });
    sliderB.oninput = (() => {
        selected.style.backgroundColor = `rgb(${sliderR.value}, ${sliderG.value}, ${sliderB.value})`;
    });
    btns = document.getElementsByClassName("button");
    selected = btns[0];
    selected.style.backgroundColor = `rgb(${sliderR.value}, ${sliderG.value}, ${sliderB.value})`;
    let angle = 360 - 90, dangle = 360 / (btns.length - 1);
    for (i = 0; i < btns.length - 1; i++) {
        btns[i].style.position = "absolute";
        btns[i].style.display = "block";
        angle += dangle;
        btns[i].style.transform = `rotate(${angle}deg) translate(300px) rotate(-${angle}deg) translateX(69em) translateY(28em)`
    }
    btns[i].style.position = "absolute";
    btns[16].style.left = "450px";
    btns[16].style.top = "920px";
}

function click1() {
    selected = btns[0];
}
function click2() {
    selected = btns[1];
}
function click3() {
    selected = btns[2];
}
function click4() {
    selected = btns[3];
}
function click5() {
    selected = btns[4];
}
function click6() {
    selected = btns[5];
}
function click7() {
    selected = btns[6];
}
function click8() {
    selected = btns[7];
}
function click9() {
    selected = btns[8];
}
function click10() {
    selected = btns[9];
}
function click11() {
    selected = btns[10];
}
function click12() {
    selected = btns[11];
}
function click13() {
    selected = btns[12];
}
function click14() {
    selected = btns[13];
}
function click15() {
    selected = btns[14];
}
function click16() {
    selected = btns[15];
}
async function send() {
    let x = "";
    for(i = 0; i < 16; i++)
    {
        var bg_color = window.getComputedStyle(btns[i], null).backgroundColor;
        bg_color = bg_color.match(/\d+/g);
        x += bg_color + ";";
    }
    var xhr = new XMLHttpRequest();
xhr.open("POST", "sendData", true);
xhr.setRequestHeader('Content-Type', 'text/plain');
xhr.send(x);
}