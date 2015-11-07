
var model = {
    numOfAps: 22,

    selectedApButton: -1,
    selectedAp: -1,

    washDuration: -1,
    dryDuration: -1,
};

$(document).ready(function () {

    // Bind ap clicks
    for (var i = 1; i <= model.numOfAps; i++) {
        $('#apb' + i).on('click', null, null, apButtonSelected);
    }

    // Bind login
    $('#login').on('click', null, null, login);

    // Bind enter to input
    $('#password').keydown(function (event) {
        if (event.keyCode == 13) {
            $('#login').click();
        }
    });

    // Bind time selection
    $('#wash30').on('click', null, null, washDuration);
    $('#wash60').on('click', null, null, washDuration);
    $('#wash90').on('click', null, null, washDuration);
    $('#wash120').on('click', null, null, washDuration);
    $('#wash150').on('click', null, null, washDuration);
    $('#wash180').on('click', null, null, washDuration);
    $('#wash210').on('click', null, null, washDuration);

    $('#dry30').on('click', null, null, dryDuration);
    $('#dry60').on('click', null, null, dryDuration);
    $('#dry90').on('click', null, null, dryDuration);
    $('#dry120').on('click', null, null, dryDuration);
    $('#dry150').on('click', null, null, dryDuration);
    $('#dry180').on('click', null, null, dryDuration);
    $('#dry210').on('click', null, null, dryDuration);

    // Bind confirms
    $('#confirmWash').on('click', null, null, doWash);
    $('#confirmDry').on('click', null, null, doDry);
});

function apButtonSelected(event) {
    var apButton = event.target.id;
    var ap = parseInt(event.target.innerText);

    if (model.selectedAp != -1) {
        $('#' + model.selectedApButton).removeClass('btn-info').addClass('btn-default');
    }

    $('#' + apButton).removeClass('btn-default').addClass('btn-info');
    model.selectedAp = ap;
    model.selectedApButton = apButton;
};

function login(event) {

    if (model.selectedAp === -1) {
        alert('Please choose an apertment first');
        return;
    }

    var password = $('#password')[0].value;
    if (password === '') {
        alert('Please enter your password first');
        return;
    }

    $('#loginGif').show();
    $.ajax('api/data/checkpassword?id=' + model.selectedAp + '&password=' + password)
        .done(function (result) {
            $('#loginGif').hide();
            if (result) {
                logifyPage();
            }
            else {
                $('#login').removeClass('btn-info').removeClass('btn-success').addClass('btn-danger');
            }
        })
        .error(function (x, y, z)
        {
            $('#loginGif').hide();
            alert('Something went wrong. Please try again later.');
        });
}

function logifyPage() {

    // Make buttons unclickable
    for (var i = 1; i <= model.numOfAps; i++) {
        $('#apb' + i).attr('disabled', 'disabled');
    }

    $('#login').removeClass('btn-info').removeClass('btn-danger').addClass('btn-success').attr('disabled', 'disabled');
    $('#password').attr('disabled', 'disabled');
    
    showActions();
    //getHistory();
}

function showActions() {
    $('#actions').show();
}

function washDuration(event) {
    var time = event.target.id.substring(4);
    model.washDuration = time;
    $('#washDuration').html(time + ' minutes <span class="caret"></span>');
    $('#washButton').removeAttr('disabled');
    $('#washModalBody').html('Washing machine for the next ' + model.washDuration + ' minutes');
}

function dryDuration(event) {
    var time = event.target.id.substring(3);
    model.dryDuration = time;
    $('#dryDuration').html(time + ' minutes <span class="caret"></span>');
    $('#dryButton').removeAttr('disabled');
    $('#dryModalBody').html('Drying machine for the next ' + model.dryDuration + ' minutes');
}

function doWash() {

    $.ajax('api/data/order?id=' + model.selectedAp + '&duration=' + parseInt(model.washDuration) + '&type=1')
        .done(function (result) {
            if (result) {
                alert('Wee, all done!');
            }
            else {
                alert('Busy machine. Please try again later.');
            }
        })
        .error(function (x, y, z) {
            alert('Something went wrong. Please try again later.');
        });


    //var table = $('#historyTable').html();
    //var len = table.length;
    //var nTable = table.substring(0, len - 8);

    //nTable = nTable + '<tr><td>' + new Date() + ' <span class="label label-info">In progress</span></td><td>' + model.washDuration + '</td><td><span class="glyphicon glyphicon-fire"></span></td></tr></table>';
    //$('#historyTable').html(nTable);
}

function doDry() {

    $.ajax('api/data/order?id=' + model.selectedAp + '&duration=' + parseInt(model.dryDuration) + '&type=2')
        .done(function (result) {
            if (result) {
                alert('Wee, all done!');
            }
            else {
                alert('Busy machine. Please try again later.');
            }
        })
        .error(function (x, y, z) {
            alert('Something went wrong. Please try again later.');
        });
}

function x() {
    /*
    function getHistory() {
        $('#history').show();
    
        $.ajax('api/data/GetHistory?ap=' + model.selectedAp + '&password=' + $('#password')[0].value).done(function (data) {
    
            $('#historyGif').hide();
    
            var table = '<table id="historyTable" class="table"><tr><th>Time</th><th>Duration</th><th>Type</th></tr>';
    
            for (var i in data) {
                var d = data[i];
                if (d.machineType === 0) {
                    var type = '<span class="glyphicon glyphicon-fire"></span>';
                } else {
                    var type = '<span class="glyphicon glyphicon-retweet"></span>';
                }
                table = table + '<tr><td>' + d.historyTime + '</td><td>' + d.duration + '</td><td>' + type + '</td></tr>';
            }
    
            table = table + '</table>';
            $('#history').append($(table));
    
        });
    }
    */
}