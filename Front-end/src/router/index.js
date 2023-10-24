import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import Overview from '../views/OverviewDocument.vue';
import Uploaden from '../views/UploadenDocument.vue';
import infopage from '../views/infoDocument.vue';
import Edit from '../views/EditDocument.vue';
import OverviewEmployees from '../views/OverviewEmployees.vue';
import OverviewLoan from '../views/OverviewLoan.vue';
import LoanHistory from '../views/LoanHistory.vue';
import InfoLoan from '../views/InfoLoan.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/info/document/:id', component: infopage, props: true},
    { path: '/Edit/:route/:id', component: Edit, props: true},
    { path: '/uploaden/document', component: Uploaden },
    { path: '/geschiedenis/product/:id', component: LoanHistory, props: true },
    { path: '/info/bruikleen/:id', component: InfoLoan, props: true },
    { path: '/overzicht/document', name: 'Overview', component: Overview ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/medewerkers', name: 'OverviewMedewerkers', component: OverviewEmployees ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/bruikleen', name: 'OverviewBruikleen', component: OverviewLoan ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
  ],
});

export default router;
