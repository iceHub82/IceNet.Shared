export function toggleDarkmode(isDarkMode) {

    var links = document.getElementsByTagName('link');
    var lastLink = links[links.length - 1];

    lastLink.disabled = isDarkMode ? false : true;
}