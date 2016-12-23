// zmienne do przetrzymywania informacji o polecankach
var firstChannelName;
var firstChannelUrl;
var secondChannelName;
var secondChannelUrl;
var thirdChannelName;
var thirdChannelUrl;


// Odświezanie polecanek  co minutę
$(document).ready(function () {
    // pierwszy raz od razu przy wczytaniu
    refreshSidebar()

    setInterval(refreshSidebar, 60000)
});

function refreshSidebar() {
    $.ajax({
        url: 'Home/GetSuggestions',
        success: function (data) {
            $('#sidebar').html(data);
        },
        success: function (data, textStatus, jqXHR) {
            firstChannelName = data.FirstChannelName;
            firstChannelUrl = data.FirstChannelUrl;
            secondChannelName = data.SecondChannelName;
            secondChannelUrl = data.SecondChannelUrl;
            thirdChannelName = data.ThirdChannelName;
            thirdChannelUrl = data.ThirdChannelUrl;

            $.ajax({
                url: 'Home/Sidebar',
                type: "GET",
                data: {
                    'firstChannelName': firstChannelName,
                    'secondChannelName': secondChannelName,
                    'thirdChannelName' : thirdChannelName,
                    'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
                },
                success: function (data) {
                    $('#sidebar').html(data);
                }
            })
        }
    });
};

// Pierwsza sugestia
$(document).ready(function () {
	$('body').on('click', '#firstSuggestion', function () {
		var stream = {
			title: firstChannelName,
			mp3: firstChannelUrl
	}

		imgSrc = "Images/Icons/300px/" + firstChannelName + ".png";

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = firstChannelUrl;
        currentChannelName = firstChannelName;

        loadAdditionalInfo();
    });
});

// Druga sugestia
$(document).ready(function () {
    $("body").on('click', '#secondSuggestion', function () {
        var stream = {
            title: secondChannelName,
            mp3: secondChannelUrl
        }

        imgSrc = "Images/Icons/300px/" + secondChannelName + ".png";

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = secondChannelUrl;
        currentChannelName = secondChannelName;

        loadAdditionalInfo();
    });
});

// Trzecia sugestia
$(document).ready(function () {
    $("body").on('click', '#thirdSuggestion', function () {
        var stream = {
            title: thirdChannelName,
            mp3: thirdChannelUrl
        }

        imgSrc = "Images/Icons/300px/" + thirdChannelName + ".png";

        $('#jquery_jplayer_1').jPlayer('setMedia', stream);
        $("#currentChannelLogo").attr('src', imgSrc);

        logListeningTime();
        currentChannelUrl = thirdChannelUrl;
        currentChannelName = thirdChannelName;

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
                url: 'Home/AdditionalInfo',
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
                url: 'Home/AdditionalInfo',
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

// Wczytanie ratingu i wartości checkboxa
function loadAdditionalInfo() {

    $.ajax({
        url: 'Home/AdditionalInfo',
        type: "GET",
        data: {
            'currentChannelName': currentChannelName,
            'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
        },
        success: function (data, textStatus, jqXHR) {
            $("#userStarRating").rating("update", data.Value);
            $("#squaredOne").prop('checked', data.IsFavorite);
        }
    })
};

// Wczytanie listy stacji
$(document).on('ready', function () {
	// Wszystkie
	$.ajax({
		url: 'Home/GetChannelList',
		type: "GET",
		success: function (data, textStatus, jqXHR) {
			var parsedData = $.parseJSON(data);
			i = 0;
			$.each(
                parsedData,
                function (i) {
                	$("#station-list").append("<li><a class='channelRef' id='" + parsedData[i] + "'>" + parsedData[i] + "</a></li>");
                }
            );

		}
	})
	//Ulubione
	$.ajax({
		url: 'favoriteList',
		success: function (data, textStatus, jqXHR) {
			var parsedData = $.parseJSON(data);
		    $("#favorite-list").append("<li><a href='#' class='active'>Ulubione stacje</a></li>");
			i = 0;
			$.each(
				parsedData,
				function (i) {
					$("#favorite-list").append("<li><a class='channelRef site-text fav-channel' id='" + parsedData[i] + "'>" + parsedData[i] + "</a></li>");
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