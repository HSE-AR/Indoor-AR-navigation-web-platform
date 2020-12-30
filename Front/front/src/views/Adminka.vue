<template>
  <div >
    <h1> Админка</h1>
    <button @click="openEditor()">Открыть редактор</button>
       <router-view/>
  </div>
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    openEditor(){
      console.log("редактор открыт")
      router.push('/adminka/editor')
    }

  }
}
</script>
