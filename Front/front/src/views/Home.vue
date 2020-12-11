<template>
  <div >
    <button @click="openEditor()">Открыть редактор</button>
  </div>
</template>

<script>

import router from '../router'

export default {
  name: 'Home',
  methods:{

    openEditor(){
      console.log("редактор открыт")
      router.push('/editor')
    }

  }
}
</script>
