import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'


// import * as THREE from './js/three/build/three.module.js';
/* eslint-disable */
import * as THREE from '../public/three/build/three.module'
import { Editor } from '../public/three/editor/js/Editor.js';
import { Viewport } from '../public/three/editor/js/Viewport.js';
import { Toolbar } from '../public/three/editor/js/Toolbar.js';
import { Script } from '../public/three/editor/js/Script.js';
import { Player } from '../public/three/editor/js/Player.js';
import { Sidebar } from '../public/three/editor/js/Sidebar.js';
import { Menubar } from '../public/three/editor/js/Menubar.js';
import { Resizer } from '../public/three/editor/js/Resizer.js';



Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
 

export { THREE,Editor,Viewport,Toolbar,Script,Player,Sidebar,Menubar,Resizer};