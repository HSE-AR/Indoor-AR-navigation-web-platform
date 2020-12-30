<template>
  <div >
           <router-view/>
  </div>
</template>

<script>

import router from '../router'

export default {
  methods:{

    enterArSession()
    {
        router.push('/arweb/main')
    },
    enterSignUp()
    {
        router.push('/arweb/signup')
    },
    enterSignIn()
    {
        router.push('/arweb/signin')
    }

  }
}
</script>
