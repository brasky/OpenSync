var tag = document.createElement('script');

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
var player;
function onYouTubeIframeAPIReady(id) {
    console.log("Making player");
    player = new YT.Player('ytplayer', {
            videoId: id,
            events: {
                    'onStateChange': onPlayerStateChange
                }
            });
      }

function onPlayerReady(event) {
    console.log("play video");
                event.target.playVideo();
}

function onPlayerStateChange(event) {
    console.log("State Change - New Player State: " + currentState);
    var currentTimestamp = getTimestamp();
    var duration = getDuration();
    var currentState = player.getPlayerState()
    //-1 is "Unstarted" but there is no YT.PlayerState constant for Unstarted.
    if (currentTimestamp == duration && currentState != -1 && currentState != YT.PlayerState.BUFFERING) {
        DotNet.invokeMethodAsync('OpenSync.Client', 'jsNextVideo');
    }
    else {
        DotNet.invokeMethodAsync('OpenSync.Client', 'jsSync');
    }       
}

function getDuration() {
    return Math.round(player.getDuration());
}

function getTimestamp() {
    return Math.round(player.getCurrentTime());
}

