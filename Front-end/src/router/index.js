import { createRouter, createWebHistory } from 'vue-router'

import Login from '../views/Login.vue';
import Overview from '../views/Overview.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/overzicht', component: Overview }
    
  ]
})

export default router
