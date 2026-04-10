document.addEventListener("DOMContentLoaded", function () {
  var base = document.querySelector("base") ? document.querySelector("base").href : "/";
  var depth = (window.location.pathname.match(/\//g) || []).length - 1;
  var rel = depth > 0 ? "../".repeat(depth) : "";
  var link = document.createElement("link");
  link.rel = "stylesheet";
  link.href = rel + "styles/main.css";
  document.head.appendChild(link);
});
