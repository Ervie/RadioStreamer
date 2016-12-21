
// Odświezanie polecanek meta co minutę
$(document).ready(function () {
    // pierwszy raz od razu przy wczytaniu
    refreshSuggestions()

    setInterval(refreshSuggestions, 60000)
});

function refreshSuggestions() {
    $.ajax({
        url: 'sidebar',
        success: function (data) {
            $('#sidebar').html(data);
        }
    })
};

// Pierwsza sugestia
$(document).ready(function () {
    $('body').on('click', '#firstSuggestion', function () {
        var stream = {
            title: $('body').data('firstChannelName'),
            mp3: $('body').data('firstChannelUrl')
        }

        imgSrc = $('body').data('firstImagePath')

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = $('body').data('firstChannelUrl');
        currentChannelName = $('body').data('firstChannelName');

        loadAdditionalInfo();
    });
});

// Druga sugestia
$(document).ready(function () {
    $("body").on('click', '#secondSuggestion', function () {
        var stream = {
            title: $('body').data('secondChannelName'),
            mp3: $('body').data('secondChannelUrl')
        }

        imgSrc = $('body').data('secondImagePath')

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = $('body').data('secondChannelUrl');
        currentChannelName = $('body').data('secondChannelName');

        loadAdditionalInfo();
    });
});

// Trzecia sugestia
$(document).ready(function () {
    $("body").on('click', '#thirdSuggestion', function () {
        var stream = {
            title: $('body').data('thirdChannelName'),
            mp3: $('body').data('thirdChannelUrl')
        }

        imgSrc = $('body').data('thirdImagePath')

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = $('body').data('thirdChannelUrl');
        currentChannelName = $('body').data('thirdChannelName');

        loadAdditionalInfo();
    });
});

// Inicjalizacja ratingu
$(document).ready(function () {
    $('#userStarRating').rating({
        hoverEnabled: false,
        showCaption: false
    });
});

// Zmiana ratingu
$(document).on('ready', function () {
    $("#userStarRating").rating().on("rating.clear", function (event) {
        if (currentChannelName != "") {

            $.ajax({
                url: 'additionalInfo',
                type: "POST",
                data: {
                    'currentChannelName': currentChannelName,
                    'value': "0",
                    'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
                }
            })
        }
    }).on("rating.change", function (event, value) {
        if (currentChannelName != "") {

            $.ajax({
                url: 'additionalInfo',
                type: "POST",
                data: {
                    'currentChannelName': currentChannelName,
                    'value': value,
                    'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
                }
            })
        }
    });
});

// Wczytanie ratingu
function loadAdditionalInfo() {

    $.ajax({
        url: 'additionalInfo',
        type: "GET",
        data: {
            'currentChannelName': currentChannelName,
            'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
        },
        success: function (data, textStatus, jqXHR) {
            $("#userStarRating").rating("update", data.value);
            $("#squaredOne").prop('checked', data.isFavorite);
        }
    })
};

// Wczytanie listy stacji
$(document).on('ready', function () {
	// Wszystkie
	$.ajax({
		url: 'channelList',
		type: "GET",
		success: function (data, textStatus, jqXHR) {
			i = 0;
			$.each(
                data,
                function (i) {
                	$("#station-list").append("<li><a class='channelRef' id='" + data[i] + "'>" + data[i] + "</a></li>");
                }
            );

		}
	})
	// Prywatne
	$.ajax({
		url: 'privateChannelList',
		type: "GET",
		success: function (data, textStatus, jqXHR) {
			i = 0;
			$.each(
                data,
                function (i) {
                	$("#private-station-list").append("<li><a class='privateChannelRef' id='" + data[i] + "'>" + data[i] + "</a></li>");
                }
            );

		}
	})
	//Ulubione
	$.ajax({
		url: 'favoriteList',
		success: function (data, textStatus, jqXHR) {
		    $("#favorite-list").append("<li><a href='#' class='active'>Ulubione stacje</a></li>");
			i = 0;
			$.each(
				data,
				function (i) {
				    $("#favorite-list").append("<li><a class='channelRef site-text fav-channel' id='" + data[i] + "'>" + data[i] + "</a></li>");
				}
			)}
		});
});

// Przedładowywanie stacji ulubionych przy zmianie stanu checkboxa
$(document).on('ready', function () {
    $('#squaredOne').change(function () {

        if (document.getElementById('squaredOne').checked)
        {
            $.ajax({
                url: 'favoriteList',
                type: "POST",
                data: {
                    'currentChannelName': currentChannelName,
                    'operation': "Add",
                    'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
                },
                success: function (data, textStatus, jqXHR) {
                    $("#favorite-list").empty();
                    $("#favorite-list").append("<li><a href='#' class='active'>Ulubione stacje</a></li>");
                    i = 0;
                    $.each(
                        data,
                        function (i) {
                            $("#favorite-list").append("<li class='fav-channel'><a class='channelRef site-text' id='" + data[i] + "'>" + data[i] + "</a></li>");
                        }
                    )}
            })
        }
        else
        {
            $.ajax({
                url: 'favoriteList',
                type: "POST",
                data: {
                    'currentChannelName': currentChannelName,
                    'operation': "Delete",
                    'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
                },
                success: function (data, textStatus, jqXHR) {
                    $("#favorite-list").empty();
                    $("#favorite-list").append("<li><a href='#' class='active'>Ulubione stacje</a></li>");
                    i = 0;
                    $.each(
                        data,
                        function (i) {
                            $("#favorite-list").append("<li class='fav-channel'><a class='channelRef site-text' id='" + data[i] + "'>" + data[i] + "</a></li>");
                        }
                    )
                }
            })
        }

    });

});