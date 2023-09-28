import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import Overview from '../views/Overview.vue';
import Uploaden from '../views/Uploaden.vue';
import infopage from '../views/infopage.vue';
import archief from '../views/Archive.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/infopage/:id', component: infopage, props: true},
    { path: '/uploaden', component: Uploaden },
    { path: '/archief', component: archief },
    { path: '/overzicht', component: Overview ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
  ],
});

export default router;
