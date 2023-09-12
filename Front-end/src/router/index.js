import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import Overview from '../views/Overview.vue';
import Uploaden from '../views/Uploaden.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/overzicht', component: Overview },
    { path: '/uploaden', component: Uploaden },
  ],
});

export default router;
