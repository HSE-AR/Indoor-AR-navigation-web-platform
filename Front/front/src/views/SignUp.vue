<template>
  <div >
    <h1> форма регистрации</h1>
    <button @click="enterArSession()">зарегистрироваться</button>
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
