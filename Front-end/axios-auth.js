import axios from 'axios';

const instance = axios.create({
    baseURL: 'https://document-trace-manager.azurewebsites.net',
    withCredentials: true, 
});

export default instance;
