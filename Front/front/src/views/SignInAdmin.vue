<template>
  <div >
    <h1> форма для входа в админку</h1>
    <button @click="enterArSession()">войти в админку</button>
  </div>
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    enterArSession(){
      router.push('/adminka/')
    }

  }
}
</script>
