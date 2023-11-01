import { createRouter, createWebHistory } from 'vue-router';
//Login
import Login from '../views/Login.vue';

//Edit 
import EditDocument from '../views/EditDocument.vue';
import EditEmployee from '../views/EditEmployee.vue';
import EditProduct from '../views/EditProduct.vue';

//Info
import InfoDocument from '../views/InfoDocument.vue';
import InfoProduct from '../views/InfoProduct.vue';
import InfoEmployee from '../views/InfoEmployee.vue';

//Uploaden
import UploadenDocument from '../views/UploadenDocument.vue';

//History
import HistoryProduct from '../views/HistoryProduct.vue';
import HistoryEmployee from '../views/HistoryEmployee.vue';

//Overview
import OverviewDocuments from '../views/OverviewDocuments.vue';
import OverviewEmployees from '../views/OverviewEmployees.vue';
import OverviewProducts from '../views/OverviewProducts.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/edit/document/:id', component: EditDocument, props: true},
    { path: '/edit/medewerker/:id', component: EditEmployee, props: true},
    { path: '/edit/product/:id', component: EditProduct, props: true},
    { path: '/info/document/:id', component: InfoDocument, props: true},
    { path: '/info/bruikleen/:id', component: InfoProduct, props: true },
    { path: '/info/medewerker/:id', component: InfoEmployee, props: true },
    { path: '/uploaden/document', component: UploadenDocument },
    { path: '/geschiedenis/product/:id', component: HistoryProduct, props: true },
    { path: '/geschiedenis/medewerker/:id', component: HistoryEmployee, props: true },
    { path: '/overzicht/documenten', name: 'Overview', component: OverviewDocuments ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/medewerkers', name: 'OverviewMedewerkers', component: OverviewEmployees ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/bruikleen', name: 'OverviewBruikleen', component: OverviewProducts ,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
  ],
});

export default router;
