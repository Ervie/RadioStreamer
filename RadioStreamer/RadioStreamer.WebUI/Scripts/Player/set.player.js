// zmienna globalna zawierająca adres radia
var currentChannelUrl;
// zmienna globalna zawierająca adres radia
var currentChannelName;
// zmienna globalna przechowująca moment rozpoczęcia słuchania nowego utworu
var startDate;

// Ustawianie playera
$(document).ready(function () {

    var stream = {
        title: "",
        mp3: ""
    },
            ready = false;

    $("#jquery_jplayer_1").jPlayer({
        ready: function (event) {
        	ready = true;
            $(this).jPlayer("setMedia", stream);
        },
        pause: function () {
        	logListeningTime();
        },
        play: function () {
        	startDate = new Date();
        },
        error: function (event) {
            if (ready && event.jPlayer.error.type === $.jPlayer.error.URL_NOT_SET) {
                // Setup the media stream again and play it.
                $(this).jPlayer("setMedia", stream).jPlayer("play");
            }
        },
        cssSelectorAncestor: "#jp_container_1",
        swfPath: "../player",
        solution: 'html, flash',
        supplied: "mp3, oga",
        useStateClassSkin: true,
        autoBlur: false,
        smoothPlayBar: true,
        keyEnabled: true
    });

    $("#currentChannelLogo").attr('src', "static/app/image/icons/300px/placeholder.png");

    startDate = new Date();
    currentChannelUrl = ""
    currentChannelName = ""
});

// Reset Stacji
$(document).ready(function () {
    $("#MediaResetBtn").click(function () {
        $('#jquery_jplayer_1').jPlayer('clearMedia');
        $("#currentChannelLogo").attr('src', "static/app/image/icons/300px/placeholder.png");
        logListeningTime();
        currentChannelUrl = ""
        currentChannelName = ""
    });
});
    
// Losowa stacja
$(document).ready(function () {
	$("#ChangeChannelBtn").click(function () {

	    $.ajax({
	        url: 'randomChannel',
	        type: "GET",
	        success: function (data, textStatus, jqXHR){


	            var stream = {
	                title: data.channelName,
	                mp3: data.channelUrl
	            };

	            imgSrc = data.imagePath;

	            $('#jquery_jplayer_1').jPlayer('setMedia', stream);
	            $("#currentChannelLogo").attr('src', imgSrc);

	            logListeningTime();
	            currentChannelName = data.channelName;
	            currentChannelUrl = data.channelUrl;

	            loadAdditionalInfo();
	        }
	    })

	});
});

// Stacja wybrana z dropdown
$(document).ready(function () {
    $("#station-list").on("click", ".channelRef", function () {
        $.ajax({
            url: 'requestedChannel',
            type: "GET",
            data: {
                'channelName': event.target.id,
                'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
            },
            success: function (data, textStatus, jqXHR) {

                var stream = {
                    title: data.channelName,
                    mp3: data.channelUrl
                };

                imgSrc = data.imagePath;

                $('#jquery_jplayer_1').jPlayer('setMedia', stream);
                $("#currentChannelLogo").attr('src', imgSrc);

                logListeningTime();
                currentChannelName = data.channelName;
                currentChannelUrl = data.channelUrl;

                startDate = new Date();
                loadAdditionalInfo();
            }
        })

    });
});

// Wybrana prywatna stacja z drugiego dropdown
$(document).ready(function () {
	$("#station-list").on("click", ".privateChannelRef", function () {
		$.ajax({
			url: 'requestedPrivateChannel',
			type: "GET",
			data: {
				'channelName': event.target.id,
				'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
			},
			success: function (data, textStatus, jqXHR) {

				var stream = {
					title: data.channelName,
					mp3: data.channelUrl
				};

				$('#jquery_jplayer_1').jPlayer('setMedia', stream);
				$("#currentChannelLogo").attr('src', "static/app/image/icons/300px/placeholder.png");

				currentChannelName = data.channelName;
				currentChannelUrl = data.channelUrl;

				startDate = new Date();
			}
		})

	});
});

// Stacja wybrana z ulubionych
$(document).ready(function () {
	$("#favorite-list").on("click", ".channelRef", function () {
		$.ajax({
			url: 'requestedChannel',
			type: "GET",
			data: {
				'channelName': event.target.id,
				'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
			},
			success: function (data, textStatus, jqXHR) {

				var stream = {
					title: data.channelName,
					mp3: data.channelUrl
				};

				imgSrc = data.imagePath;

				$('#jquery_jplayer_1').jPlayer('setMedia', stream);
				$("#currentChannelLogo").attr('src', imgSrc);

				logListeningTime();
				currentChannelName = data.channelName;
				currentChannelUrl = data.channelUrl;

				startDate = new Date();
				loadAdditionalInfo();
			}
		})

	});
});

// Wczytywanie meta co 3 sekund
$(document).ready(function () {

    setInterval(refreshMeta, 3000)

});

function refreshMeta() {
    $.ajax({
        url: 'metadata',
        type: "GET",
        data: {
            'currentChannelUrl': currentChannelUrl,
            'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()},
        success: function (data) {
            $('#jp-meta').html(data);
        }
    })
};

// Logowanie odsłuchanego czasu
function logListeningTime() {

	endDate = new Date();
	endDateISO = endDate.toISOString();
	startDateISO = startDate.toISOString();

	$.ajax({
		url: 'logTime',
		type: "POST",
		data: {
			'currentChannelName': currentChannelName,
			'startTimestamp': startDateISO,
			'endTimestamp': endDateISO,
			'csrfmiddlewaretoken': $('input[name="csrfmiddlewaretoken"]').val()
		},
		success: function (data) {
			$('#jp-meta').html(data);
		}
	})
}