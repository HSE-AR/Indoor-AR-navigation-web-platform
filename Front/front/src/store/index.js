import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    port:'https://192.168.0.103:5555', //ip адрес компа, можно просто https://localhost:5555
    sceneTestId: '5fea51e7c4c61e2e98e97794' //id можно посмотреть в монге
  },
  mutations: {
  },
  actions: {
  },
  modules: {
  }
})
