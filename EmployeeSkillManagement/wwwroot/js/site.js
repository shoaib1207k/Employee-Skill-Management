// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function ($) {
    var path = window.location.pathname;
    // Get the navigation items
    var navItems = document.querySelectorAll(".navbar-nav a");
    console.log("path: ", path);
    // Loop through each navigation item
    navItems.forEach(function (item) {
        // Check if the item's href matches the current path
        let href = item.getAttribute("href");

        if (path == "/" && href === path) {
            item.classList.add("active");
        } else if (href.includes("Employee") && path.includes("Employee")) {
            item.classList.add("active");
        } else if (href.includes("Skill") && path.includes("Skill")) {
            item.classList.add("active");
        }
    });

    checkNavbarState();

    $("#navbar-half-hide-toggle").on("click", function () {
        if ($(this).hasClass("nav-is-half-hide")) {
            $(".logo-name").fadeIn();
            $(".side-navbar-header").removeClass("half-hide");
        } else {
            $(".logo-name").hide();
            $(".side-navbar-header").addClass("half-hide");
        }

        const isNavbarSmall = $(".side-navbar-header").hasClass("half-hide");
        localStorage.setItem("isNavbarSmall", JSON.stringify(isNavbarSmall));
        console.log(isNavbarSmall);
        $(this).toggleClass("nav-is-half-hide");
    });

    function checkNavbarState() {
        const isNavbarSmall =
            JSON.parse(localStorage.getItem("isNavbarSmall")) || false;
        console.log(isNavbarSmall);
        if (isNavbarSmall) {
            $(".logo-name").hide();
            $(".side-navbar-header").addClass("half-hide");
            $("#navbar-half-hide-toggle").addClass("nav-is-half-hide");
        }
    }
});
