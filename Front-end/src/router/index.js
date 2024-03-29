import { createRouter, createWebHistory } from 'vue-router';
//Login
import Login from '../views/Login.vue';
import ForgotPassword from '../views/ForgotPassword.vue';
import ChangePassword from '../views/ChangePassword.vue';

//Edit 
import EditDocument from '../views/EditDocument.vue';
import EditEmployee from '../views/EditEmployee.vue';
import EditProduct from '../views/EditProduct.vue';

//Info
import InfoDocument from '../views/infoDocument.vue';
import InfoProduct from '../views/InfoProduct.vue';
import InfoEmployee from '../views/InfoEmployee.vue';

//Uploaden
import UploadenDocument from '../views/UploadenDocument.vue';
import UploadenEmployee from '../views/UploadenEmployee.vue';
import UploadenProduct from '../views/UploadenProduct.vue';

//History
import HistoryProduct from '../views/HistoryProduct.vue';
import HistoryEmployee from '../views/HistoryEmployee.vue';

//Overview
import OverviewDocuments from '../views/OverviewDocuments.vue';
import OverviewEmployees from '../views/OverviewEmployees.vue';
import OverviewProducts from '../views/OverviewProducts.vue';
import OverviewLongValid from '../views/OverviewLongValid.vue';
import OverviewArchiveDocuments from '../views/OverviewArchiveDocuments.vue';
import OverviewArchiveEmployee from '../views/OverviewArchiveEmployee.vue';
import OverviewArchiveProducts from '../views/OverviewArchiveProducts.vue';

//Profile
import ProfilePersonalData from '../views/ProfilePersonalData.vue';
import ProfileChangePassword from '../views/ProfileChangePassword.vue';
import ProfileManageUsers from '../views/ProfileManageUsers.vue';
import ProfileAddUser from '../views/ProfileAddUser.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: Login },
    { path: '/login/wachtwoord-vergeten', component: ForgotPassword },
    { path: '/login/wachtwoord-vergeten/nieuw-wachtwoord/:code', component: ChangePassword, props: true },
    { path: '/profiel/persoonsgegevens', component: ProfilePersonalData },
    { path: '/profiel/wachtwoord-wijzigen', component: ProfileChangePassword },
    { path: '/profiel/gebruikers-beheren', component: ProfileManageUsers },
    { path: '/profiel/gebruikers-toevoegen', component: ProfileAddUser },
    { path: '/edit/document/:type/:id', component: EditDocument, props: true},
    { path: '/edit/medewerker/:type/:id', component: EditEmployee, props: true},
    { path: '/edit/product/:id', component: EditProduct, props: true},
    { path: '/info/document/:type/:id', component: InfoDocument, props: true},
    { path: '/info/bruikleen/:id', component: InfoProduct, props: true },
    { path: '/info/medewerker/:type/:id', component: InfoEmployee, props: true },
    { path: '/geschiedenis/product/:id', component: HistoryProduct, props: true },
    { path: '/geschiedenis/medewerker/:id', component: HistoryEmployee, props: true },
    { path: '/uploaden/document', component: UploadenDocument },
    { path: '/uploaden/medewerker', component: UploadenEmployee },
    { path: '/uploaden/product', component: UploadenProduct },
    { path: '/overzicht/documenten', name: 'OverviewDocuments', component: OverviewDocuments,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/documenten/lang-geldig', name: 'OverviewLongValid', component: OverviewLongValid,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/documenten/archief', name: 'OverviewArchiveDocuments', component: OverviewArchiveDocuments,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/medewerkers', name: 'OverviewEmployees', component: OverviewEmployees,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/medewerkers/archief', name: 'OverviewArchiveEmployees', component: OverviewArchiveEmployee,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/bruikleen', name: 'OverviewProducts', component: OverviewProducts,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
    { path: '/overzicht/bruikleen/archief', name: 'OverviewArchiveProducts', component: OverviewArchiveProducts,
    props: route => ({ popup1: route.query.popup1 === 'true' })},
  ],
});

export default router;
