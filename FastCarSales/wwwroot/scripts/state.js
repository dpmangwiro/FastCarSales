function saveScrollPosition() {
    localStorage.setItem('scrollPosition', window.scrollY);
}

function restoreScrollPosition() {
    const scrollPosition = localStorage.getItem('scrollPosition');
    if (scrollPosition) {
        window.scrollTo(0, parseInt(scrollPosition, 10));
    }
}

function saveSearchData(searchData) {
    localStorage.setItem('searchData', JSON.stringify(searchData));
}

function saveSearchArgs(searchArgs) {
    localStorage.setItem('searchArgs', JSON.stringify(searchArgs));
}

function saveSearchResults(searchResults) {
    localStorage.setItem('searchResults', JSON.stringify(searchResults));
}


function saveAdminResults(adminResults) {
    localStorage.setItem('adminResults', JSON.stringify(adminResults));
}

function getSearchData() {
    return JSON.parse(localStorage.getItem('searchData'));
}

function getSearchArgs() {
    return JSON.parse(localStorage.getItem('searchArgs'));
}

function getSearchResults() {
    return JSON.parse(localStorage.getItem('searchResults'));
}

function getAdminResults() {
    return JSON.parse(localStorage.getItem('adminResults'));
}


// Save the current scroll position before the page unloads
window.addEventListener('beforeunload', saveScrollPosition);

// Restore the scroll position when the page loads
window.addEventListener('load', restoreScrollPosition);
