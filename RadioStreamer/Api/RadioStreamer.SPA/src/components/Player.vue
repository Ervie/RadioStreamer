<template>
  <audio class="mejs_player" controls autoplay>
	<source type="audio/mpeg" src="http://redir.atmcdn.pl/sc/o2/Eurozet/live/antyradio.livx?audio=5" />
    <p>Your browser does not support the audio element.</p>
</audio>
</template>

<<script>
export default {
  name: 'Player',
  methods: {
    preparePlayer () {
      this.$('video, audio').mediaelementplayer({
        pluginPath: 'https://cdnjs.com/libraries/mediaelement/',
        shimScriptAccess: 'always'
      })
    }
  },

  mounted () {
    this.preparePlayer()
  }
}
</script>

