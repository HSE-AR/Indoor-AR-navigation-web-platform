<template>
  <div >
    
    <h1>Лендинг</h1>
    <div>
        <button class="" @click="signInAdmin()">Войти в административную панель</button>
    </div>
    <hr>
    <div>
        <button class="" @click="signUpAdmin()">Стать админом</button>
    </div>
    <hr>
    <div>
        <button class="" @click="getARWeb()">Перейти на страницу с AR</button>
    </div>
  </div>
  
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    signInAdmin(){
      router.push('/signin/admin')
    },

    signUpAdmin(){
      router.push('/signup/admin')
    },

    getARWeb(){
        router.push('/arweb/') 
    }

  }
}
</script>
