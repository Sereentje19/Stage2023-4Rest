
import { createApp } from 'vue'
import mitt from 'mitt'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'


const emitter = mitt()
const app = createApp(App)

app.use(createPinia())
app.use(router)

app.config.globalProperties.emitter = emitter
app.mount('#app')
