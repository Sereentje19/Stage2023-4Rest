import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import Overview from '../views/Overview.vue';
import Uploaden from '../views/Uploaden.vue';
import infopage from '../views/infopage.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/infopage', component: infopage},
    { path: '/uploaden', component: Uploaden },
    {path: '/overzicht',
    name: 'overview',
    component: Overview,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
  ],
});

export default router;
