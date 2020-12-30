<template>
  <div >
    <h1> форма для регистрации админа</h1>
    <button @click="enterAdminka()">зарегистрироваться</button>
  </div>
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    enterAdminka(){
      router.push('/adminka/')
    }

  }
}
</script>
