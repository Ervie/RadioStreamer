<template>
  <div id="app">
     <section class="hero is-primary">
      <div class="hero-body">
        <div class="container">
          <h1 class="title">
            Temporary header
          </h1>
          <h2 class="subtitle">
            Under construction
          </h2>
        </div>
      </div>
    </section>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'app'
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
