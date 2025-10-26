let nav = document.getElementById("collapse");
let barIcon = document.getElementById("open");
let closeIcon = document.getElementById("close");

barIcon.onclick = () => {
  nav.style.transform = "translateX(0%)";
};
closeIcon.onclick = () => {
  nav.style.transform = "translateX(-100%)";
};
