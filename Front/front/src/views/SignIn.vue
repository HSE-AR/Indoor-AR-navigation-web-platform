<template>
  <div >
    <h1> форма для входа в ар сессию</h1>
    <button @click="enterArSession()">войти</button>
  </div>
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    enterArSession(){
      router.push('/arweb/main')
    }

  }
}
</script>
