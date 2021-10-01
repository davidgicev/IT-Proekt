var prevScroll = window.pageYOffset;
let navbar = document.getElementById("navbar")
if (prevScroll == 0)
    navbar.classList.add("navbar-top")
document.body.style.paddingTop = navbar.offsetHeight + "px"

window.addEventListener("scroll", function () {
    let current = window.pageYOffset;
    if (current == 0)
        navbar.classList.add("navbar-top")
    else
        navbar.classList.remove("navbar-top")

    if (prevScroll > current) {
        document.body.style.paddingTop = navbar.offsetHeight + "px"
        navbar.style.top = "0";
    } else if (prevScroll < current) {
        if (navbar.offsetHeight / document.body.scrollHeight < 0.05) {
            document.body.style.paddingTop = "0"
            navbar.style.top = "-100vh";
        }
    }
    prevScroll = current;
});