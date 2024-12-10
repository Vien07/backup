function SetToggle(div, value) {
    $(div).toggles({
        drag: true,
        click: true,
        text: {
            on: 'Publish',
            off: 'Off'
        },
        on: value,
        animate: 250,
        easing: 'swing',
        checkbox: null,
        clicker: null,
        width: 70,
        height: 26,
        textIndent: 5,
        type: 'compact'
    }).on('toggle', function (e, active) {
        obj.Enabled = active;
        if (active) {
            $(div).attr('value', 'true');
        } else {
            $(div).attr('value', 'false');
        }

    });
}