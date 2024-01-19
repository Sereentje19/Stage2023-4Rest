import axios from 'axios';

const instance = axios.create({
    baseURL: 'https://document-trace-manager-dtm.azurewebsites.net/',
    withCredentials: true, 
});

export default instance;
