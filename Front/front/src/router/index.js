import Vue from 'vue'
import VueRouter from 'vue-router'
import Editor from '../views/Editor.vue'
import Lending from '../views/Lending.vue'
import Adminka from '../views/Adminka.vue'
import SignInAdmin from '../views/SignInAdmin.vue'
import SignUpAdmin from '../views/SignUpAdmin.vue'
import SignUp from '../views/SignUp.vue'
import SignIn from '../views/SignIn.vue'
import ARWeb from '../views/ARWeb.vue'
import MainARWeb from '../views/MainARWeb.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Lending',
    component: Lending
  },
  {
    path: '/lending',
    name: 'Lending',
    component: Lending
  },
  {
    path: '/adminka/',
    component:Adminka,
    children : [
      {
        path: 'editor',
        component: Editor,
      }
    ]
  },
  {
    path: '/signin/admin',
    name: 'SignInAdmin',
    component: SignInAdmin
  },
  {
    path: '/signup/admin',
    name: 'SignUpAdmin',
    component: SignUpAdmin
  },
  {
    path: '/arweb/',
    name: 'ARWeb',
    component: ARWeb,
    children: [
      {
        path: '',
        name: 'MainARWeb',
        component: MainARWeb
      },
      {
        path: 'main',
        name: 'MainARWeb',
        component: MainARWeb
      },
      {
        path: 'signup',
        name: 'SignUp',
        component: SignUp
      },
      {
        path: 'signin',
        name: 'SignIn',
        component: SignIn
      }
    ]
  },



]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
