

$(document).ready(function () {

    // Bind login
    $('#login').on('click', null, null, login);

    // Bind enter to input
    $('#password').keydown(function (event) {
        if (event.keyCode == 13) {
            $('#login').click();
        }
    });

    $('#password').focus();
});


function login(event) {

    var password = $('#password')[0].value;

    if (password === 'eyalsapir' || password === 'roibecker') {
        showTables();
    }
}

function showTables() {
    $('#history').show();
    $('#passwordDiv').hide();
    
    for (var i = 0; i < 12; i++) {

        $.ajax('api/data/GetHistory?month=' + i).done(function (j) {
            return function (data) {

                $('#historyGif' + (j + 1)).hide();
                
                var table = '<tr><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Appertment</th><th>Duration</th><th>Time</th><th>Type</th></tr>';

                for (var i in data) {
                    var d = data[i];
                    if (d.historyTime === '') {
                        var type = '';
                        var s = ' class="success"';
                    } else {
                        var s = '';
                        if (d.machineType === 1) {
                            var type = 'Wash';
                        } else {
                            var type = 'Dry';
                        }
                    }
                    if (s !== '') {
                        d.app = '<strong><span class="glyphicon glyphicon-arrow-right"></span>&nbsp;' + d.app + '</strong>';
                        d.historyTime = '<strong>' + d.historyTime + '</strong>';
                        d.duration = '<strong>' + d.duration + '   (' + (d.duration / 12) + ' Shekels)</strong>';
                        type = '<strong>' + type + '</strong>';
                    } else {
                        d.app = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + d.app;
                    }

                    table = table + '<tr' + s + '><td>' + d.app + '</td><td>' + d.duration + '</td><td>' + d.historyTime + '</td><td>' + type + '</td></tr>';
                }

                $('#hTable' + (j + 1)).append($(table));

            }
        }(i));

    }

    //$.ajax('api/data/GetHistory?month=curr').done(function (data) {
        
    //    $('#historyGif1').hide();

    //    var table = '<tr><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Appertment</th><th>Duration</th><th>Time</th><th>Type</th></tr>';

    //    for (var i in data) {
    //        var d = data[i];
    //        if (d.historyTime === '') {
    //            var type = '';
    //            var s = ' class="success"';
    //        } else {
    //            var s = '';
    //            if (d.machineType === 1) {
    //                var type = 'Wash';
    //            } else {
    //                var type = 'Dry';
    //            }
    //        }
    //        if (s !== '') {
    //            d.app = '<strong><span class="glyphicon glyphicon-arrow-right"></span>&nbsp;' + d.app + '</strong>';
    //            d.historyTime = '<strong>' + d.historyTime + '</strong>';
    //            d.duration = '<strong>' + d.duration + '   (' + (d.duration / 12) + ' Shekels)</strong>';
    //            type = '<strong>' + type + '</strong>';
    //        } else {
    //            d.app = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + d.app;
    //        }

    //        table = table + '<tr' + s + '><td>' + d.app + '</td><td>' + d.duration + '</td><td>' + d.historyTime + '</td><td>' + type + '</td></tr>';
    //    }

    //    $('#hTable1').append($(table));

    //});

    //$.ajax('api/data/GetHistory?month=prev').done(function (data) {

    //    $('#historyGif2').hide();

    //    var table = '<tr><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Appertment</th><th>Duration</th><th>Time</th><th>Type</th></tr>';

    //    for (var i in data) {
    //        var d = data[i];
    //        if (d.historyTime === '') {
    //            var type = '';
    //            var s = ' class="success"';
    //        } else {
    //            var s = '';
    //            if (d.machineType === 1) {
    //                var type = 'Wash';
    //            } else {
    //                var type = 'Dry';
    //            }
    //        }
    //        if (s !== '') {
    //            d.app = '<strong><span class="glyphicon glyphicon-arrow-right"></span>&nbsp;' + d.app + '</strong>';
    //            d.historyTime = '<strong>' + d.historyTime + '</strong>';
    //            d.duration = '<strong>' + d.duration + '   (' + (d.duration / 12) + ' Shekels)</strong>';
    //            type = '<strong>' + type + '</strong>';
    //        } else {
    //            d.app = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + d.app;
    //        }

    //        table = table + '<tr' + s + '><td>' + d.app + '</td><td>' + d.duration + '</td><td>' + d.historyTime + '</td><td>' + type + '</td></tr>';
    //    }

    //    $('#hTable2').append($(table));

    //});

    //$.ajax('api/data/GetHistory?month=prevprev').done(function (data) {

    //    $('#historyGif3').hide();

    //    var table = '<tr><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Appertment</th><th>Duration</th><th>Time</th><th>Type</th></tr>';

    //    for (var i in data) {
    //        var d = data[i];
    //        if (d.historyTime === '') {
    //            var type = '';
    //            var s = ' class="success"';
    //        } else {
    //            var s = '';
    //            if (d.machineType === 1) {
    //                var type = 'Wash';
    //            } else {
    //                var type = 'Dry';
    //            }
    //        }
    //        if (s !== '') {
    //            d.app = '<strong><span class="glyphicon glyphicon-arrow-right"></span>&nbsp;' + d.app + '</strong>';
    //            d.historyTime = '<strong>' + d.historyTime + '</strong>';
    //            d.duration = '<strong>' + d.duration + '   (' + (d.duration / 12) + ' Shekels)</strong>';
    //            type = '<strong>' + type + '</strong>';
    //        } else {
    //            d.app = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + d.app;
    //        }

    //        table = table + '<tr' + s + '><td>' + d.app + '</td><td>' + d.duration + '</td><td>' + d.historyTime + '</td><td>' + type + '</td></tr>';
    //    }

    //    $('#hTable3').append($(table));

    //});
}
