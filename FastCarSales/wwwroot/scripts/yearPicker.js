window.setupYearOnlyPicker = function (element) {
    element.type = 'number';
    element.min = '1900';
    element.max = new Date().getFullYear(); element.step = '1';
};