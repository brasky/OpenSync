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
    console.log("state change");
    DotNet.invokeMethodAsync('BlazorApp2.Client', 'jsSync');
      }

function getTimestamp() {
    return Math.round(player.getCurrentTime());
}

